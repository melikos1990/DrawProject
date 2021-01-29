import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { CaseAssignGroupDetailViewModel, CaseAssignGroupUserListViewModel, CaseAssignGroupListViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromCaseAssignGroupActions from '../../store/actions/case-assign-group.actions';
import { takeUntil, skip } from 'rxjs/operators';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AllNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';
import { CaseAssignGroupUserModalComponent } from 'src/app/shared/component/modal/case-assign-group-user-modal/case-assign-group-user-modal.component';
import { Guid } from 'guid-typescript';
import { UserListViewModel } from 'src/app/model/organization.model';

const PREFIX = 'CaseAssignGroupComponent';

@Component({
  selector: 'app-case-assign-group-detail',
  templateUrl: './case-assign-group-detail.component.html',
  styleUrls: ['./case-assign-group-detail.component.scss']
})
export class CaseAssignGroupDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild('selector') selector: TemplateRef<any>;

  public options = {};
  public form: FormGroup;

  public uiActionType: ActionType;
  public model: CaseAssignGroupDetailViewModel = new CaseAssignGroupDetailViewModel();

  popupUserRef: NgbModalRef;
  popupGroupRef: NgbModalRef;

  columns = [];
  isUserEnabled?: boolean = true; //人員清單是否篩選啟用人員
  titleTypeString: string = "";

  constructor(
    private modalService: NgbModal,
    private store: Store<fromMasterReducer>,
    private active: ActivatedRoute,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.subscription();
    this.initializeForm();
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.store
      .select((state: fromMasterReducer) => state.master.caseAssignGroup.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(group => {
        this.model = { ...group };

      });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.model.BuID = parseInt(params['BuID']);
        this.store.dispatch(new fromCaseAssignGroupActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        this.columns = this.appendOperator(this.columns);
        break;
      case ActionType.Update:
        this.store.dispatch(new fromCaseAssignGroupActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        this.columns = this.appendOperator(this.columns);
        break;
      case ActionType.Read:
        this.store.dispatch(new fromCaseAssignGroupActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    if (this.vaildNotificationRemark() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_ASSIGN_GROUP.NOTIFICATION_REMARK_ERROR'))));
      return;
    }

    if (this.vaildEmail() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_ASSIGN_GROUP.EMAIL_ERROR'))));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromCaseAssignGroupActions.addAction(this.model));
      }
    )));

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  btnEdit($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    if (this.vaildNotificationRemark() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_ASSIGN_GROUP.NOTIFICATION_REMARK_ERROR'))));
      return;
    }

    if (this.vaildEmail() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_ASSIGN_GROUP.EMAIL_ERROR'))));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromCaseAssignGroupActions.editAction(this.model));
      }
    )));

  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  closeModel(){
    this.popupUserRef.dismiss();
  }

  /*---start 群組拷貝---*/
  btnCopyGroupModal($event) {
    this.popupGroupRef = this.modalService.open(CaseAssignGroupUserModalComponent,
      { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

    this.popupGroupRef.componentInstance.btnAddUser = this.onGroupSelected.bind(this);
    this.popupGroupRef.componentInstance.nodeID = this.model.BuID;
  }

  onGroupSelected(data: CaseAssignGroupListViewModel[]) {

    const payloads = this.generatorPayloads(data);
    this.model.Users = this.union(this.model.Users, payloads);
    this.popupGroupRef.dismiss();
  }
  /*---end 群組拷貝---*/

  /*---start 組織選人---*/
  btnSelectUserModal($event) {
    this.popupUserRef = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

  }

  onUserSelected(instance: AllNodeTreeUserSelectorComponent) {
    const users: any[] = instance.getValue();

    if (!users || users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇使用者")));
      return;
    }
    const payloads = this.generatorPayloads(instance.getValue());
    this.model.Users = this.union(this.model.Users, payloads);
    this.popupUserRef.dismiss();
  }
  /*---end 組織選人---*/

  findIndex = (userID) => this.model.Users.findIndex(x => x.UserID === userID);


  btnDeleteUser($event: UserListViewModel) {
    const index = this.findIndex($event.UserID);
    this.model.Users.splice(index, 1);
    this.model.Users = [...this.model.Users]; // clone ref to refresh view....
  }


  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('NODE_DEFINITION.ORGANIZATION_TYPE'),
        name: 'OrganizationTypeName'
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('USER.EMAIL'),
        name: 'Email',
        customer: true,
      },
      {
        text: this.translateService.instant('CASE_ASSIGN_GROUP.NOTIFICATION_REMARK'),
        name: 'NotificationRemark',
        customer: true,
      }
    ];
  }

  union(existUsers: CaseAssignGroupUserListViewModel[], newUsers: CaseAssignGroupUserListViewModel[]): CaseAssignGroupUserListViewModel[] {

    const result: CaseAssignGroupUserListViewModel[] = [];

    existUsers.forEach(x => {
      result.push(x);
    });

    newUsers.forEach(x => {
      if (result.filter(g => g.UserID === x.UserID).length === 0) {
        result.push(x);
      }
    });

    const resultOrder = result.sort(function (a, b) {
      return a.NotificationRemark > b.NotificationRemark ? 1 : -1;
    });

    return resultOrder;

  }

  getUserIDIfMock(user) {
    return user.UserID || Guid.create().toString();
  }

  generatorPayloads(users: any[]): CaseAssignGroupUserListViewModel[] {
    return users.map((user: any | CaseAssignGroupUserListViewModel) => {
      const data = new CaseAssignGroupUserListViewModel();
      data.BUID = user.BUID;
      data.BUName = user.BUName;
      data.UserID = this.getUserIDIfMock(user);
      data.UserName = user.UserName;
      data.Email = user.Email;
      data.JobName = user.JobName;
      data.JobID = user.JobID;
      data.Mobile = user.Mobile;
      data.NodeID = user.NodeID;
      data.NodeName = user.NodeName;
      data.Address = user.Address;
      data.Email = user.Email;
      data.Telephone = user.Telephone;
      data.OrganizationType = user.OrganizationType;
      data.OrganizationTypeName = user.OrganizationTypeName;
      data.UnitType = this.unitType.Organization;
      data.Mobile = user.Mobile;
      data.NotificationBehavior = this.notificationType.Email.toString();
      data.NotificationRemark = !(user.NotificationRemark) ? user.NotificationRemark = "0" : user.NotificationRemark;
      data.GroupID = this.model.ID;
      return data;
    });
  }

  initializeForm() {
    this.form = new FormGroup({
      BuID: new FormControl(this.model.BuID, [
        Validators.required,
      ]),
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      CaseAssignGroupType: new FormControl(this.model.CaseAssignGroupType, [
        Validators.required,
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  vaildNotificationRemark() {
    console.log(this.model.Users);
    if (!this.model.Users) return true;

    if (this.model.Users.some(x => !x.NotificationRemark)) {
      return false;
    }

    return true;
  }

  vaildEmail() {
    console.log(this.model.Users);
    if (!this.model.Users) return true;

    if (this.model.Users.some(x => !x.Email)) {
      return false;
    }

    return true;
  }

}

