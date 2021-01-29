import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { CaseAssignmentHeaderqurterStoreSearchViewModel, CaseAssignmentModeType } from 'src/app/model/search.model';
import { SearchBaseComponent } from '../../base/search-base.component';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { Store } from '@ngrx/store';
import { State as fromSearchReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as HeaderqurterStoreCaseAssignmentSearchActions from '../../store/actions/headerqurter-store-assignment-search.actions';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { EntrancePayload } from 'src/app/model/common.model';
import { filter, takeUntil } from 'rxjs/operators';
import { CaseAssignmentProcessType } from 'src/app/model/case.model';
import { ObjectService } from 'src/app/shared/service/object.service';
import { BuNodeDefinitionLevelSelectorComponent } from 'src/app/shared/component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { OrganizationNodeViewModel } from 'src/app/model/organization.model';
import { environment } from 'src/environments/environment';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';


const PREFIX = "HeaderqurterBUAssignmentSearchComponent";

@Component({
  selector: 'app-headerqurter-bu-assignment-search',
  templateUrl: './headerqurter-bu-assignment-search.component.html',
  styleUrls: ['./headerqurter-bu-assignment-search.component.scss']
})
export class HeaderqurterBUAssignmentSearchComponent extends SearchBaseComponent implements OnInit {

  @ViewChild('dynamicSeletor') dynamicSeletor: DynamicQuestionSelectComponent;
  @ViewChild('allNodeTreeSelector') selectorRef: TemplateRef<any>;
  @ViewChild('buSelect') buSelect: BuSelectComponent;
  @ViewChild('allNodeSelector') allNodeSelector: BuNodeDefinitionLevelSelectorComponent;
  @ViewChild('allNodeSelectorComplained') allNodeSelectorComplained: BuNodeDefinitionLevelSelectorComponent;


  organizationRef: NgbModalRef;

  form: FormGroup;

  isEnable: boolean = true;
  showAdvancedFlag: boolean = false;

  model: CaseAssignmentHeaderqurterStoreSearchViewModel = {} as CaseAssignmentHeaderqurterStoreSearchViewModel;

  columns: any[] = [];

  tableLoading: boolean = false;
  assignmentList: any = [];


  assignmentState: string;
  assignmentComplainedUsers: any[] = [];
  assignmentProcessType: any = CaseAssignmentProcessType;

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
    this.model.IsBusinessAll = true; //BU功能中為True (與總部/門市 共用funtion) 
  }

  setTimeRange() {

    this.model.CreateTimeRange = this.objectService.setDateTimeRange(new Date(), -7);
  }

  subscription() {
    this.store.select(x => x.mySearch.headerQurterStoreCaseAssignmentSearch.caseAssignmentList)
      .pipe(filter(x => !!(x)), takeUntil(this.destroy$))
      .subscribe(datas => {
        this.assignmentList = datas;
      })
  }

  btnReport($event: any) {
    this.fillback();
    this.store.dispatch(new HeaderqurterStoreCaseAssignmentSearchActions.headerqurterStoreReport(this.model));
  }
  btnRender($event: any) {

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

    this.store.dispatch(new HeaderqurterStoreCaseAssignmentSearchActions.headerqurterStoreGetList(payload));
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

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  onBtnEdit($event) {
    const url = `${environment.webHostPrefix}/pages/case/case-assignment-detail`.toCustomerUrl({
      actionType: this.actionType.Update,
      caseID: $event.CaseID,
      ID: $event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment ? $event.SN : $event.IdentityID,
      type: $event.CaseAssignmentProcessType
    })
    window.open(url, '_blank');
  }



  showAdvanced() {
    this.showAdvancedFlag = !this.showAdvancedFlag;
    if (this.showAdvancedFlag == false)
      this.resetAdvancedWhere();
  }

  btnAddUser(user) {

    this.assignmentComplainedUsers.push(user);
    this.dismissOrganizationModel();

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

  btnAddUnitModal() {

    if (!this.validForm(this.form)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()))
      return;
    }

    this.organizationRef = this.modalService.open(this.selectorRef, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
  }

  btnDeleteUser(user) {

    let idx = this.assignmentComplainedUsers.findIndex(x => user.ID == x.ID);
    this.assignmentComplainedUsers.splice(idx, 1);

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
      NoticeContent: new FormControl(),
      CaseContent: new FormControl()
    })
  }


  private resetRelatedValue() {
    this.model.Type = null;
    this.model.InvoiceType = null;
    this.model.InvoiceID = null;
    this.model.AssignmentUsers = null;

    // 清空 轉派對象 UI 
    this.assignmentComplainedUsers = [];

  }

  private resetAdvancedWhere() {
    // this.model.IsAttension = null;
    this.model.CaseComplainedNodeName = null;
    this.model.CaseComplainedStoreName = null;
    this.model.CaseComplainedStoreNo = null;
    this.model.CaseComplainedUnitType = null;
    this.model.ClassificationID = null;
  }

  private fillback() {

    if (this.dynamicSeletor)
      this.model.ClassificationID = this.dynamicSeletor.lastHasValue;


    // 回填 轉派對象
    if (this.assignmentComplainedUsers && this.assignmentComplainedUsers.length > 0) {
      this.model.AssignmentUsers = [...this.assignmentComplainedUsers];
    } else {
      this.model.AssignmentUsers = null;
    }

    // 回填企業別名稱
    let bu = this.getBu(this.buSelect, x => x.id == this.model.NodeID);
    this.model.NodeName = bu && bu.text;


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

}
