import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { Store } from '@ngrx/store';
import { CaseAssignmentCallCenterSearchViewModel, CaseAssigmentState, CaseAssignmentModeType } from 'src/app/model/search.model';
import { PtcAjaxOptions } from 'ptc-server-table';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { State as fromSearchReducer } from '../../store/reducers';
import { EntrancePayload } from 'src/app/model/common.model';
import * as CallCenterCaseAssignmentSearchActions from '../../store/actions/call-center-assignment-search.actions';
import * as fromRootActions from 'src/app/store/actions';
import { SearchBaseComponent } from '../../base/search-base.component';
import { filter, takeUntil } from 'rxjs/operators';
import { CaseAssignmentProcessType } from 'src/app/model/case.model';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { ObjectService } from 'src/app/shared/service/object.service';
import { BuNodeDefinitionLevelSelectorComponent } from 'src/app/shared/component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { OrganizationNodeViewModel, OrganizationType, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { environment } from 'src/environments/environment';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';


const PREFIX = 'CallCenterAssignmentSearchComponent';

@Component({
  selector: 'app-call-center-assignment-search',
  templateUrl: './call-center-assignment-search.component.html',
  styleUrls: ['./call-center-assignment-search.component.scss']
})
export class CallCenterAssignmentSearchComponent extends SearchBaseComponent implements OnInit {

  treemodel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  type: OrganizationType;

  @ViewChild('buSelect') buSelect: BuSelectComponent;
  @ViewChild('dynamicSeletor') dynamicSeletor: DynamicQuestionSelectComponent;
  @ViewChild('allNodeTreeSelector') selectorRef: TemplateRef<any>;

  @ViewChild('allNodeSelector') allNodeSelector: BuNodeDefinitionLevelSelectorComponent;
  @ViewChild('allNodeSelectorComplained') allNodeSelectorComplained: BuNodeDefinitionLevelSelectorComponent;

  organizationRef: NgbModalRef;

  form: FormGroup;
  model: CaseAssignmentCallCenterSearchViewModel = {} as CaseAssignmentCallCenterSearchViewModel;

  columns: any[] = [];

  isEnable: boolean = true;
  tableLoading: boolean = false;
  assignmentList: any = [];

  assignmentState: string;
  assignmentComplainedUsers: any[] = [];
  assignmentProcessType: any = CaseAssignmentProcessType;

  showAdvancedFlag: boolean = false;

  constructor(
    public injector: Injector,
    public modalService: NgbModal,
    public store: Store<fromSearchReducer>,
    public objectService: ObjectService,
  ) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.initFormGroup();
    this.subscription();
    this.setTimeRange();
  }

  setTimeRange() {

    this.model.CreateTimeRange = this.objectService.setDateTimeRange(new Date(), -7);
  }

  subscription() {
    this.store.select(x => x.mySearch.callCenterCaseAssignmentSearch.caseAssignmentList)
      .pipe(filter(x => !!(x)), takeUntil(this.destroy$))
      .subscribe(datas => {
        console.log("data s =>", datas);
        this.assignmentList = datas;
      })
  }

  btnRender($event) {

    if (!this.validForm(this.form)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()))
      return;
    }



    this.fillback();

    this.tableLoading = true;
    this.isEnable = null;

    let hendle = function () {
      this.tableLoading = false;
    }

    let payload = new EntrancePayload(this.model);
    payload.success = hendle.bind(this);
    payload.failed = hendle.bind(this);

    this.store.dispatch(new CallCenterCaseAssignmentSearchActions.callCenterGetList(payload));

  }

  btnReport() {
    this.fillback();
    this.store.dispatch(new CallCenterCaseAssignmentSearchActions.callCenterReport(this.model));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event) {
    const url = `${environment.webHostPrefix}/pages/case/case-assignment-detail`.toCustomerUrl({
      actionType: this.actionType.Read,
      caseID: $event.CaseID,
      ID: $event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment ? $event.SN : $event.IdentityID,
      type: $event.CaseAssignmentProcessType
    })
    window.open(url, '_blank');
  }


  onBtnEdit($event) {
    // const url = `${environment.webHostPrefix}/pages/case/case-assignment-detail`.toCustomerUrl({
    //   actionType: this.actionType.Update,
    //   caseID: $event.CaseID,
    //   ID: $event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment ? $event.SN : $event.IdentityID,
    //   type: $event.CaseAssignmentProcessType
    // })
    // window.open(url, '_blank');
  }


  btnAddUnitModal() {

    if (!this.validForm(this.form)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()))
      return;
    }

    this.organizationRef = this.modalService.open(this.selectorRef, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
  }

  btnAddUser(user) {

    this.assignmentComplainedUsers.push(user);
    this.dismissOrganizationModel();

  }

  btnDeleteUser(user) {

    let idx = this.assignmentComplainedUsers.findIndex(x => user.ID == x.ID);
    this.assignmentComplainedUsers.splice(idx, 1);

  }

  dismissOrganizationModel() {
    this.organizationRef.dismiss();
  }

  assignmentTypeChange(event: any) {

    let target = event ? parseInt(event) : null;
    this.resetRelatedValue();

    switch (target) {
      case CaseAssignmentProcessType.Notice:
        this.assignmentState = "CaseAssignmentComplaintNoticeType"
        break;

      case CaseAssignmentProcessType.Invoice:
        this.assignmentState = "CaseAssignmentComplaintInvoiceType"
        break;

      case CaseAssignmentProcessType.Assignment:
        this.assignmentState = "CaseAssignmentType"
        break;

      case CaseAssignmentProcessType.Communication:
        this.assignmentState = "novalue"
        break;

      default:
        break;
    }
  }

  onSelecteChange($event) {

    if (!$event) return;

    this.treemodel = new OrganizationDataRangeSearchViewModel();
    switch ($event.toString()) {
      case OrganizationType.CallCenter.toString():
        this.treemodel.IsSelf = true;
        this.treemodel.Goal = OrganizationType.CallCenter;
        break;

      case OrganizationType.HeaderQuarter.toString():
        this.treemodel.NodeID = this.model.NodeID;
        this.treemodel.IsStretch = true;
        this.treemodel.Goal = OrganizationType.HeaderQuarter;
        break;

      case OrganizationType.Vendor.toString():
        this.treemodel.NodeID = this.model.NodeID;
        this.treemodel.Goal = OrganizationType.Vendor;
        break;

      default:
        break;
    }
  }

  removeValue(prefix: string) {
    for (let key in this.model) {
      if (key.startsWith(prefix) && !key.endsWith("UnitType")) {
        this.model[key] = null;
      }
    }
  }

  initFormGroup() {
    this.form = new FormGroup({
      NodeID: new FormControl(null, [
        Validators.required
      ]),

      InvoiceID: new FormControl(),
      InvoiceType: new FormControl(),
      CreateTimeRange: new FormControl(null, [
        Validators.required
      ]),
      CaseID: new FormControl(),
      CaseContent: new FormControl(),
      AssignmentProcessType: new FormControl(),

      CaseConcatUnitType: new FormControl(),
      ConcatName: new FormControl(),
      ConcatTelephone: new FormControl(),
      ConcatEmail: new FormControl(),
      ConcatStoreName: new FormControl(),
      ConcatStoreNo: new FormControl(),
      ConcatNodeName: new FormControl(),
      CaseComplainedUnitType: new FormControl(),
      CaseComplainedStoreName: new FormControl(),
      CaseComplainedStoreNo: new FormControl(),
      CaseComplainedNodeName: new FormControl(),
      ClassificationID: new FormControl(),
      AssignmentState: new FormControl(),
      NoticeDateTimeRange: new FormControl(),
      NoticeContent: new FormControl()
    })
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant("CASE_COMMON.NOTIFY_DATETIME"),
        name: 'NoticeDateTime'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ID"),
        name: 'CaseID',
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ASSIGNMENT_ID"),
        name: 'SN'
      },
      {
        text: this.translateService.instant("CASE_COMMON.ASSIGNMENT_MODE"),
        name: 'Mode'
      },
      {
        text: this.translateService.instant("CASE_COMMON.ASSIGNMENT_STATE"),
        name: 'Type'
      },
      {
        text: this.translateService.instant("HEADERQURTER_STORE_ASSIGNMENT_SEARCH.ASSIGNMENT_TARGET"),
        name: 'CaseAssignmentUser'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_RESPONDER"),
        name: 'ConcatUserName'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_USER"),
        name: 'ComplainedUserName'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_NODE"),
        name: 'ComplainedUserParentNamePath'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_CONTENT"),
        name: 'CaseContent',
        customer: true
      },
      {
        text: this.translateService.instant("CASE_COMMON.NOTIFY_CONTENT"),
        name: 'NoticeContent',
        customer: true
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator'
      },
    ];

  }

  showAdvanced() {

    this.showAdvancedFlag = !this.showAdvancedFlag;
    if (this.showAdvancedFlag == false)
      this.resetAdvancedWhere();
  }

  onBUSelectChange($event) {
    this.type = null;
  }

  private resetRelatedValue() {
    this.model.Type = null;
    this.model.InvoiceType = null;
    this.model.InvoiceID = null;
    this.model.AssignmentUsers = null;

    // 清空 轉派對象 UI 
    this.assignmentComplainedUsers = [];

  }

  private fillback() {

    // 回填企業別名稱
    let bu = this.getBu(this.buSelect, x => x.id == this.model.NodeID);
    this.model.NodeName = bu && bu.text;

    // 回填 轉派對象
    if (this.assignmentComplainedUsers && this.assignmentComplainedUsers.length > 0) {
      this.model.AssignmentUsers = [...this.assignmentComplainedUsers];
    } else {
      this.model.AssignmentUsers = null;
    }

    if (this.dynamicSeletor)
      this.model.ClassificationID = this.dynamicSeletor.lastHasValue;


    if (this.allNodeSelector) {
      let contants: OrganizationNodeViewModel[] = this.allNodeSelector.getAllValue();
      console.log("反應者 所有節點 ======>  ", contants);
      this.model.ConcatNode = contants;
    }

    if (this.allNodeSelectorComplained) {
      let complaineds: OrganizationNodeViewModel[] = this.allNodeSelectorComplained.getAllValue();
      console.log("被反應者 所有節點 ======>  ", complaineds);
      this.model.ComplainedNode = complaineds;
    }

  }

  private resetAdvancedWhere() {
    this.model.CaseComplainedNodeName = null;
    this.model.CaseComplainedStoreName = null;
    this.model.CaseComplainedStoreNo = null;
    this.model.CaseComplainedUnitType = null;
    this.model.ClassificationID = null;
  }

}
