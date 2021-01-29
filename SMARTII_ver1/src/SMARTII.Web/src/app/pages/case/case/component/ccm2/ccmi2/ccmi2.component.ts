import { Component, OnInit, Injector, Input, ViewChild, TemplateRef, Inject } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseAssignmentBaseViewModel, CaseAssignmentConcatUserViewModel, CaseComplainedUserViewModel, CaseViewModel, CaseAssignmentViewModel, CaseAssignmentComplaintInvoiceViewModel, CaseAssignmentComplaintNoticeViewModel, CaseAssignmentComplaintNoticeUserViewModel, CaseAssignmentComplaintInvoiceUserViewModel, CaseAssignmentCommunicateViewModel, CaseAssignmentProcessType } from 'src/app/model/case.model';
import { ConcatableUserViewModel, OrganizationDataRangeSearchViewModel, OrganizationType, BusinesssUnitParameters } from 'src/app/model/organization.model';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AcuModalComponent } from '../../../modal/acu-modal/acu-modal.component';
import { CaseAssignGroupUserModalComponent } from 'src/app/shared/component/modal/case-assign-group-user-modal/case-assign-group-user-modal.component';
import { CaseAssignGroupUserListViewModel, CaseTemplateSearchViewModel, CaseTemplateListViewModel, CaseTemplateParseViewModel, CaseTemplateType } from 'src/app/model/master.model';
import { Guid } from 'guid-typescript';
import { CaseTemplateSelectorComponent } from 'src/app/shared/component/modal/case-template-selector/case-template-selector.component';
import { CaseService } from 'src/app/shared/service/case.service';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { caseTemplateKey } from 'src/global';
import { CASE_FACTORY, ICaseFactory } from 'src/app/business-unit';
import { AllNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';


@Component({
  selector: 'app-ccmi2',
  templateUrl: './ccmi2.component.html',
  styleUrls: ['./ccmi2.component.scss']
})
export class Ccmi2Component extends FormBaseComponent implements OnInit {
  @Input() focusId: string;

  businessParameter: BusinesssUnitParameters;

  @ViewChild('allNodeTreeSelector') allNodeTreeSelector: TemplateRef<any>;
  treemodel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();


  organizationRef: NgbModalRef;

  fileOpts = {};
  columns = [];

  loading: boolean = false;

  operatorInfo: {
    input: any,
    startPos: number,
    endPos: number
  };

  private _model: CaseAssignmentBaseViewModel = new CaseAssignmentBaseViewModel();

  @Input() public set model(v: CaseAssignmentBaseViewModel) {
    this._model = v;
  }
  public get model(): CaseAssignmentBaseViewModel {
    this.refillPayload();
    return this._model
  }

  @Input() sourcekey: string;
  @Input() users: ConcatableUserViewModel[] = [];
  @Input() case: CaseViewModel;


  @Input() form: FormGroup;

  constructor(
    @Inject(CASE_FACTORY) public factories: ICaseFactory[],
    private store: Store<any>,
    private caseService: CaseService,
    private modalService: NgbModal,
    public injector: Injector) {
    super(injector);


  }

  ngOnInit() {
    this.initializeTable();
    this.initailizeForm();
    this.initProp();
  }

  initailizeForm() {
    this.form.addControl('CaseAssignmentProcessType', new FormControl());
    this.form.addControl('NotificationDateTime', new FormControl(this.model.NotificationDateTime, [
      Validators.required
    ]));
    this.form.addControl('Content', new FormControl(this.model.Content, [
      Validators.maxLength(1024)
    ]));
  }

  initProp() {
    if (!this._model.NotificationDateTime)
      this._model.NotificationDateTime = new Date(this.defaultDateTime());

    if (!this._model.NotificationBehaviors)
      this._model.NotificationBehaviors = [];

  }

  btnNotificationUser() {
    this.loading = true;

    this.caseService
      .getBUParameters(this.model.NodeID)
      .subscribe(x => {
        this.businessParameter = x;
        const nodeKey = x.NodeKey;
        const service = this.factories.find(x => x.key == nodeKey);
        if (service) {

          this.model.NotificationUsers = service.getNotificationUserName(this.case.CaseComplainedUsers);

          if (this.case.CaseComplainedUsers.length > 0 && this.model.CaseAssignmentProcessType == CaseAssignmentProcessType.Invoice ) {
            //將反應者資訊帶入通知清單中
            let addUser: CaseAssignGroupUserListViewModel[] = [];
            addUser = service.getNotificationUserMail(this.case.CaseComplainedUsers);

            if (addUser.length > 0) {
              const newUsers = addUser.map(c => this.toConcatUserFromGroupUser(c));
              this.users = this.union(this.users, newUsers);
            }

          }

        }
        this.loading = false;
      })
  }

  onCaseAssignmentProcessTypeChange($event) {
    this.initProp();
    if ($event) {
      this.btnNotificationUser();
    }
  }

  btnCaseTemplateModal() {

    const ref = this.modalService.open(CaseTemplateSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<CaseTemplateSelectorComponent>ref.componentInstance);
    const model = new CaseTemplateSearchViewModel();
    model.BuID = this.model.NodeID;
    model.ClassificKey = this.getClassificKeyString();
    instance.model = model;
    instance.btnAdd = (data: CaseTemplateListViewModel) => {
      this.loading = true;
      this.caseService
        .parseTemplateUseExist(data.Content, this.model.CaseID)
        .subscribe(x => {
          if (x.isSuccess) {
            this.model.Content = (this.model.Content) ? !!(this.operatorInfo) ? this.insertTag(x.element, this.operatorInfo) : `${this.model.Content}${String.prototype.newLine}${x.element}` : x.element;
          } else {
            this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
          }
          this.loading = false;
        })

      ref.dismiss();
    };
    setTimeout(() => {
      instance.table.render();
    }, 1000);
  }

  btnDefaultCaseTemplateModal() {

    if (this.model.CaseAssignmentProcessType == null || this.model.CaseAssignmentProcessType == undefined) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇歷程模式")));
      return;
    }

    const data = new CaseTemplateParseViewModel();
    data.NodeID = this.model.NodeID;
    data.CaseID = this.model.CaseID;
    data.IsDefault = true;
    data.ClassificKey = this.getClassificKeyString();
    this.loading = true;
    this.caseService
      .parseTemplate(data)
      .subscribe(x => {
        if (x.isSuccess) {
          this.model.Content = (this.model.Content) ? !!(this.operatorInfo) ? this.insertTag(x.element.Content, this.operatorInfo) : `${this.model.Content}${String.prototype.newLine}${x.element.Content}` : x.element.Content;
        } else {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
        }
        this.loading = false;
      })

  }
  btnDeleteUser(user: ConcatableUserViewModel) {
    const index = this.users.findIndex(x => x.key === user.key);
    this.users.splice(index, 1);
    this.users = [...this.users];
  }

  btnAddGroupModal() {
    const ref = this.modalService.open(CaseAssignGroupUserModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<CaseAssignGroupUserModalComponent>ref.componentInstance);
    instance.nodeID = this.model.NodeID;
    instance.type = this.caseAssignGroupType.Normal;
    instance.btnAddUser = (users: CaseAssignGroupUserListViewModel[]) => {
      const newUsers = users.map(c => this.toConcatUserFromGroupUser(c));
      this.users = this.union(this.users, newUsers);
      ref.dismiss();
    }
  }
  btnAddUserModal() {
    var ref = this.modalService.open(AcuModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' })
    const instance = (<AcuModalComponent>ref.componentInstance);
    instance.btnAddUser = (user: ConcatableUserViewModel) => {
      this.users = this.union(this.users, [user]);
      ref.dismiss();
    }
  }

  btnAddUnitModal() {
    this.organizationRef = this.modalService.open(this.allNodeTreeSelector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
  }

  addOrganization(data: AllNodeTreeUserSelectorComponent) {
    const selectUsers: any[] = data.getValue();

    if (!selectUsers || selectUsers.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇人員")));
      return;
    }
    const newUsers = selectUsers.map(c => this.toConcatUserFromComplainedUser(c));
    this.users = this.union(this.users, newUsers);

    this.organizationRef.dismiss();
  }

  dismissOrganizationModel() {
    this.organizationRef && this.organizationRef.dismiss();
  }

  onSelecteChange($event) {

    if (!$event) return;

    this.treemodel = new OrganizationDataRangeSearchViewModel();
    this.treemodel.IsEnabled = true;
    switch ($event.toString()) {
      case OrganizationType.CallCenter.toString():
        this.treemodel.IsSelf = true;
        this.treemodel.Goal = OrganizationType.CallCenter;
        break;

      case OrganizationType.HeaderQuarter.toString():
        this.treemodel.NodeID = this.case.NodeID;
        this.treemodel.IsStretch = true;
        this.treemodel.Goal = OrganizationType.HeaderQuarter;
        break;

      case OrganizationType.Vendor.toString():
        this.treemodel.NodeID = this.case.NodeID;
        this.treemodel.Goal = OrganizationType.Vendor;
        break;

      default:
        break;
    }
  }

  toConcatUserFromComplainedUser(user: CaseComplainedUserViewModel) {
    const result = new ConcatableUserViewModel();
    result.UserID = user.UserID;
    result.BUID = user.BUID;
    result.BUName = user.BUName;
    result.Email = user.Email;
    result.NodeID = user.NodeID;
    result.NodeName = user.NodeName;
    result.OrganizationType = user.OrganizationType;
    result.UnitType = user.UnitType;
    result.UserName = user.UserName;
    result.Telephone = user.Telephone;
    result.TelephoneBak = user.TelephoneBak;
    result.JobID = user.JobID;
    result.JobName = user.JobName;
    result.NotificationBehavior = this.notificationType.Email.toString();
    result.NotificationRemark = this.emailReceiveType.Recipient.toString();
    result.key = Guid.create().toString();
    return result;
  }

  toConcatUserFromGroupUser(user: CaseAssignGroupUserListViewModel) {
    const result = new ConcatableUserViewModel();
    result.UserID = user.UserID;
    result.BUID = user.BUID;
    result.BUName = user.BUName;
    result.Email = user.Email;
    result.Gender = user.Gender;
    result.NodeID = user.NodeID;
    result.NodeName = user.NodeName;
    result.OrganizationType = user.OrganizationType;
    result.UnitType = user.UnitType;
    result.UserName = user.UserName;
    result.Telephone = user.Telephone;
    result.TelephoneBak = user.TelephoneBak;
    result.JobID = user.JobID;
    result.JobName = user.JobName;
    result.NotificationBehavior = user.NotificationBehavior;
    result.NotificationRemark = user.NotificationRemark;
    result.key = Guid.create().toString();
    return result;
  }

  refeshPayload() {
    this.model.Content = '';
    this.model.Files = [];
    this.model.NotificationBehaviors = [];
    this.model.NoticeUsers = [];
    this.model.NotificationUsers = '';
    this.model.CaseAssignmentProcessType = null;
    this.model.NotificationDateTime = null;

    this.users = []
  }

  refillPayload() {

    this._model.CaseID = this.case.CaseID;
    this._model.NodeID = this.case.NodeID;
    this._model.OrganizationType = this.case.OrganizationType;

    switch (+this._model.CaseAssignmentProcessType) {
      case this.caseAssignmentProcessType.Assignment:
        (<CaseAssignmentViewModel>this._model).CaseAssignmentConcatUsers = (this.users as CaseAssignmentConcatUserViewModel[]);
        break;
      case this.caseAssignmentProcessType.Invoice:
        (<CaseAssignmentComplaintNoticeViewModel>this._model).Users = (this.users as CaseAssignmentComplaintNoticeUserViewModel[]);
        break;
      case this.caseAssignmentProcessType.Notice:
        (<CaseAssignmentComplaintInvoiceViewModel>this._model).Users = (this.users as CaseAssignmentComplaintInvoiceUserViewModel[]);
        break;
    }
  }

  getClassificKeyString() {
    let classificKey = "";
    switch (+this.model.CaseAssignmentProcessType) {
      case this.caseAssignmentProcessType.Notice:
        classificKey = caseTemplateKey.NOTICE;
        break;
      case this.caseAssignmentProcessType.Invoice:
        classificKey = caseTemplateKey.COMPLAINT;
        break;
      case this.caseAssignmentProcessType.Assignment:
        classificKey = caseTemplateKey.ASSIGNMENT;
        break;
      case this.caseAssignmentProcessType.Communication:
        classificKey = caseTemplateKey.COMMUNICATION;
        break;
      default:
        classificKey = caseTemplateKey.EMAIL;
        break
    }

    return classificKey;
  }

  getASSIGNMENTUserList($event: CaseComplainedUserViewModel[]) {
    const newUsers = $event.map(c => this.toConcatUserFromComplainedUser(c));
    this.users = this.union(this.users, newUsers);
  }
  // 過濾重複通知對象
  union(existUsers: ConcatableUserViewModel[], newUsers: ConcatableUserViewModel[]): ConcatableUserViewModel[] {

    const result: ConcatableUserViewModel[] = [];

    newUsers.forEach(x => {
      result.push(x);
    });

    existUsers.forEach(x => {
      if (result.filter(g => g.UserID === x.UserID).length === 0) {
        result.push(x);
      }
    });

    return result;

  }

  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CUSTOMER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.TYPE'),
        name: 'NotificationRemark',
        customer: true,
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.EMAIL'),
        name: 'Email',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
      }
    ];

  }

  clickInput = (el) => {
    this.operatorInfo = this.resetInputInfo(el);
  }

  insertTag(tag: string, el: any) {
    if (!el) return;

    let { input, startPos } = el;

    let oldValue: string = input.value;

    let newValue = oldValue.slice(0, startPos) + tag + oldValue.slice(startPos);

    return newValue;

  }

  private resetInputInfo = (el) => ({
    input: el,
    startPos: el.selectionStart,
    endPos: el.selectionEnd
  })

}
