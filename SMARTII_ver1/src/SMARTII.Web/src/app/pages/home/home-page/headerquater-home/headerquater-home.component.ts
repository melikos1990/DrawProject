import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers/index';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { SummaryTargetViewModel } from 'src/app/model/organization.model';
import { HQHomeSearchViewModel } from 'src/app/model/home.model';
import * as fromRootActions from 'src/app/store/actions';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { CaseAssignmentProcessType } from 'src/app/model/case.model';
import { environment } from 'src/environments/environment';

export const PREFIX = 'HeaderquaterHomeComponent';

@Component({
  selector: 'app-headerquater-home',
  templateUrl: './headerquater-home.component.html',
  styleUrls: ['./headerquater-home.component.scss']
})
export class HeaderquaterHomeComponent extends FormBaseComponent implements OnInit {
  @ViewChild('assignmentTable') assignmentTable: ServerTableComponent;

  isVisibleBtnEdit: boolean = true;

  isEnable: boolean = false;

  unFinishColumns: any[] = [];

  cuurentTarget: SummaryTargetViewModel;

  public model: HQHomeSearchViewModel = new HQHomeSearchViewModel();

  groupUnFinishItem: SummaryTargetViewModel[] = [
    { TargetID: this.hQHomeSearchType.UnFinishCase, TargetName: "案件數", TargetFrom: "未銷" },
    { TargetID: this.hQHomeSearchType.UnFinishRejectCase, TargetName: "駁回數", TargetFrom: "未銷" },
    { TargetID: this.hQHomeSearchType.UnFinishUnderCase, TargetName: "轄下案件數", TargetFrom: "未銷" }
  ];

  isUnFinishTable: boolean = false;

  assignmentAjax: PtcAjaxOptions = new PtcAjaxOptions();

  constructor(
    public store: Store<fromRootReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
    ($event.direction == undefined || $event.direction == null) && ($event.direction = "asc")
  }

  /**
    * LocalTable按下編輯
    */
  @loggerMethod()
  onBtnEditAssignment($event: any) {
    const url = `${environment.webHostPrefix}/pages/case/case-assignment-detail`.toCustomerUrl({
      actionType: this.actionType.Update,
      caseID: $event.CaseID,
      ID: $event.SN,
      type: CaseAssignmentProcessType.Assignment
    })
    window.open(url, '_blank');
  }
  /**
   * LocalTable按下查詢
   */
  @loggerMethod()
  onBtnSearchAssignment($event: any) {
    const url = `${environment.webHostPrefix}/pages/case/case-assignment-detail`.toCustomerUrl({
      actionType: this.actionType.Read,
      caseID: $event.CaseID,
      ID: $event.SN,
      type: CaseAssignmentProcessType.Assignment
    })
    window.open(url, '_blank');
  }

  /**
   * 按下未銷案件數查詢
   */
  @loggerMethod()
  onUnFinishClick($event: SummaryTargetViewModel) {
    //開啟查詢結果
    this.isEnable = null;
    this.isUnFinishTable = null;

    this.cuurentTarget = $event;

    //僅有在查看底人員案件時，不需顯示編輯按鈕
    $event.TargetID === this.hQHomeSearchType.UnFinishUnderCase ? this.isVisibleBtnEdit = false : this.isVisibleBtnEdit = true;

    this.model.HQHomeSearchType = $event.TargetID;
    this.assignmentTable.render();
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.assignmentAjax.url = 'Summary/HeaderQuarterSummary/GetUnFinishList/'.toHostApiUrl();
    this.assignmentAjax.method = 'POST';
    this.unFinishColumns = [
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ID"),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("COMMON.ID"),
        name: 'SN',
        disabled: false,
        order: 'ASSIGNMENT_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_SOURCE_TYPE"),
        name: 'CaseSourceType',
        disabled: false,
        order: 'CASE.CASE_SOURCE.SOURCE_TYPE'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CREATE_DATETIME"),
        name: 'CreateTime',
        disabled: false,
        order: 'CASE.CREATE_DATETIME'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_WARNING_ID"),
        name: 'CaseWarning',
        disabled: false,
        order: 'CASE.CASE_WARNING_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_TYPE"),
        name: 'CaseType',
        disabled: false,
        order: 'CASE.CASE_TYPE'
      },
      {
        text: this.translateService.instant("CASE_COMMON.ASSIGNMENT_USER"),
        name: 'CaseAssignmentUser',
        disabled: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_RESPONDER"),
        name: 'ConcatUserName',
        disabled: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_USER"),
        name: 'ComplainedUserName',
        disabled: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_NODE"),
        name: 'ComplainedUserParentNamePath',
        disabled: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_CONTENT"),
        name: 'CaseContent',
        disabled: false,
        order: 'CASE.CONTENT',
        customer: true,
      },
      {
        text: this.translateService.instant("CASE_COMMON.APPLY_USER"),
        name: 'ApplyUserName',
        disabled: false,
        order: 'CASE.APPLY_USERNAME'
      },
      {
        text: this.translateService.instant("CALL_CENTER_CASE_SEARCH.FIRST_CLASSIFICATION"),
        name: 'FirstClassification',
        disabled: true
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
      },
    ];
  }
}

