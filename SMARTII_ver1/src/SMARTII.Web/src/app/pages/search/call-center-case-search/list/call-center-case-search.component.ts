import { Component, OnInit, Injector, Type, ViewChild, TemplateRef, ElementRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseCallCenterSearchViewModel } from 'src/app/model/search.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UnitType, OrganizationNodeViewModel } from 'src/app/model/organization.model';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { HashtagComponent } from 'src/app/shared/component/other/hashtag/hashtag.component';
import { CaseWarningSelectComponent } from 'src/app/shared/component/select/element/case-warning-select/case-warning-select.component';
import { UserSelectComponent } from 'src/app/shared/component/select/element/user-select/user-select.component';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { Store } from '@ngrx/store';
import { State as fromSearchReducer } from '../../store/reducers';
import * as CallCenterCaseSearchActions from '../../store/actions/call-center-case-search.actions';
import * as fromRootActions from 'src/app/store/actions';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { DynamicFinishReasonComponent } from 'src/app/shared/component/other/dynamic-finish-reason/dynamic-finish-reason.component';
import { filter, takeUntil } from 'rxjs/operators';
import { SearchBaseComponent } from '../../base/search-base.component';
import { ObjectService } from 'src/app/shared/service/object.service';
import { BuNodeDefinitionLevelSelectorComponent } from 'src/app/shared/component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { environment } from 'src/environments/environment';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';


const PREFIX = 'CallCenterCaseSearchComponent';

@Component({
  selector: 'app-call-center-case-search',
  templateUrl: './call-center-case-search.component.html',
  styleUrls: ['./call-center-case-search.component.scss']
})
export class CallCenterCaseSearchComponent extends SearchBaseComponent implements OnInit {

  @ViewChild('dynamicSeletor') dynamicSeletor: DynamicQuestionSelectComponent;
  @ViewChild('hashtag') tagRef: HashtagComponent;
  @ViewChild('warningSelect') warningSelect: CaseWarningSelectComponent;
  @ViewChild('applyUserSelect') applyUserSelect: UserSelectComponent;
  @ViewChild('buSelect') buSelect: BuSelectComponent;
  @ViewChild('finishReason') finishReason: DynamicFinishReasonComponent;

  @ViewChild('table') table: ServerTableComponent;
  @ViewChild('allNodeSelector') allNodeSelector: BuNodeDefinitionLevelSelectorComponent;
  @ViewChild('allNodeSelectorComplained') allNodeSelectorComplained: BuNodeDefinitionLevelSelectorComponent;

  unitType = UnitType;
  form: FormGroup;

  isEnable: boolean = true;
  showAdvancedFlag: boolean = false;

  model: CaseCallCenterSearchViewModel = {} as CaseCallCenterSearchViewModel;

  columns: any[] = [];

  tableLoading: boolean = false;
  caseList: any = [];


  constructor(
    public injector: Injector,
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
    this.store.select(x => x.mySearch.callCenterCaseSearch.caseList)
      .pipe(filter(x => !!(x)), takeUntil(this.destroy$))
      .subscribe(datas => {
        this.caseList = datas;
      })
  }

  btnRender($event) {

    if (!this.validForm(this.form)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()))
      return;
    }

    

    this.isEnable = null;
    this.tableLoading = true;

    let hendle = function () {
      this.tableLoading = false;
    }

    this.fillback();
    let payload = new EntrancePayload(this.model);

    payload.success = hendle.bind(this);
    payload.failed = hendle.bind(this);

    console.log("payload =>", JSON.stringify(payload));
    this.store.dispatch(new CallCenterCaseSearchActions.callCenterGetList(payload));

  }

  btnReport() {
    this.fillback();
    this.store.dispatch(new CallCenterCaseSearchActions.callCenterReport(this.model));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event) {
    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Read,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  onBtnEdit($event) {
    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Update,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');
  }


  initFormGroup() {
    this.form = new FormGroup({
      NodeID: new FormControl(null, [
        Validators.required
      ]),
      ApplyUserID: new FormControl(),
      CaseConcatUnitType: new FormControl(),
      CaseComplainedUnitType: new FormControl(),
      IsAttension: new FormControl(),
      ExpectDateTimeRange: new FormControl(),
      CaseContent: new FormControl(),
      CaseTagList: new FormControl(),
      CaseSourceType: new FormControl(),
      CaseType: new FormControl(),
      CaseWarningID: new FormControl(),
      CreateTimeRange: new FormControl(null, [
        Validators.required
      ]),
      CaseID: new FormControl(),
      ConcatName: new FormControl(),
      ConcatTelephone: new FormControl(),
      ConcatEmail: new FormControl(),
      ConcatStoreName: new FormControl(),
      ConcatStoreNo: new FormControl(),
      ConcatNodeName: new FormControl(),
      CaseComplainedStoreName: new FormControl(),
      CaseComplainedStoreNo: new FormControl(),
      CaseComplainedNodeName: new FormControl(),
      ClassificationID: new FormControl(),
      IsPrevention: new FormControl(),
      CaseSourceID: new FormControl(),
      FinishContent: new FormControl(),
      InvoiceID: new FormControl()
    })
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant("CASE_COMMON.CASE_SOURCE_TYPE"),
        name: 'CaseSourceType',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CREATE_DATETIME"),
        name: 'CreateTime',
        disabled: false,
        order: ''
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ID"),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_WARNING_ID"),
        name: 'CaseWarning',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_TYPE"),
        name: 'CaseType',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_RESPONDER"),
        name: 'ConcatUserName',
        disabled: false,
        order: ''
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_USER"),
        name: 'CaseComplainedUserName',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_CONTENT"),
        name: 'CaseContent',
        disabled: false,
        order: '',
        customer: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.APPLY_USER"),
        name: 'ApplyUserName',
        disabled: false,
        order: ''
      },
      {
        text: this.translateService.instant("CALL_CENTER_CASE_SEARCH.FIRST_CLASSIFICATION"),
        name: 'FirstClassification',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.FINISH_CONTENT"),
        name: 'FinishContent',
        disabled: false,
        order: 'CASE_ID',
        customer: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_NODE"),
        name: 'ComplainedUserParentNamePath',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
      },
    ];

  }


  showAdvanced() {

    this.showAdvancedFlag = !this.showAdvancedFlag;
    if (this.showAdvancedFlag == false)
      this.resetAdvancedWhere();
  }

  private fillback() {

    this.model.ClassificationID = this.dynamicSeletor ? this.dynamicSeletor.lastHasValue : null;

    // 回填 案件標籤
    let tags = this.getTags(this.tagRef);
    if (tags && tags.length > 0) {
      this.model.CaseTagIDList = tags.map(x => x.caseTagID);
      this.model.CaseTagList = tags.map(x => x.caseTagText);
    }
    else{
      this.model.CaseTagIDList = null;
      this.model.CaseTagList = null;
    }

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


    // 回填案件等級名稱
    let warning = this.getWarning(this.warningSelect, x => x.id == this.model.CaseWarningID);
    this.model.CaseWarningName = warning && warning.text;

    // 回填負責人名稱
    this.model.ApplyUserName = this.applyUserSelect.currentItem ? this.applyUserSelect.currentItem.text : null;

    // 回填企業別名稱
    let bu = this.getBu(this.buSelect, x => x.id == this.model.NodeID);
    this.model.NodeName = bu && bu.text;

    // 回填 結案原因
    let reasons = this.getReason(this.finishReason);
    if (reasons) {
      this.model.ReasonIDs = reasons.map(x => x.ID);
      this.model.ReasonNames = reasons.map(x => x.Text);
    }
    else{
      this.model.ReasonIDs = null;
      this.model.ReasonNames = null;
    }


  }

  private resetAdvancedWhere() {
    // this.model.IsAttension = null;
    this.model.CaseType = null;
    this.model.CaseWarningID = null;
    this.model.CaseWarningName = null;
    this.model.CaseComplainedNodeName = null;
    this.model.CaseComplainedStoreName = null;
    this.model.CaseComplainedStoreNo = null;
    this.model.CaseComplainedUnitType = null;
    this.model.CaseTagList = null;
    this.model.CaseTagIDList = null;
    this.model.ClassificationID = null;
    this.model.ReasonIDs = null;
    this.model.ReasonNames = null;
    this.model.ExpectDateTimeRange = null;
  }
}

