import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers/index';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { SummaryTargetViewModel } from 'src/app/model/organization.model';
import { VendorHomeSearchViewModel } from 'src/app/model/home.model';
import * as fromRootActions from 'src/app/store/actions';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { CaseAssignmentProcessType } from 'src/app/model/case.model';
import { environment } from 'src/environments/environment';

export const PREFIX = 'VendorHomeComponent';

@Component({
  selector: 'app-vendor-home',
  templateUrl: './vendor-home.component.html',
  styleUrls: ['./vendor-home.component.scss']
})
export class VendorHomeComponent extends FormBaseComponent implements OnInit {
  @ViewChild('table') table: ServerTableComponent;

  isEnable: boolean = false;

  columns: any[] = [];
  cuurentTarget: SummaryTargetViewModel;

  public model: VendorHomeSearchViewModel = new VendorHomeSearchViewModel();
  groupItem: SummaryTargetViewModel[] = [{ TargetID: this.hQHomeSearchType.UnFinishCase, TargetName: "案件數", TargetFrom: "未銷" }, { TargetID: this.hQHomeSearchType.UnFinishRejectCase, TargetName: "駁回數", TargetFrom: "未銷" }];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();
  
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
  onBtnEdit($event: any) {
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
  onBtnSearch($event: any) {
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
    this.cuurentTarget = $event;

    this.model.HQHomeSearchType = $event.TargetID;

    this.table.render();
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Summary/VendorSummary/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ID"),
        name: 'CaseID',
        disabled: false,
        order: "CASE_ID"
      },
      {
        text: this.translateService.instant("CASE_COMMON.NOTIFY_DATETIME"),
        name: 'NoticeDateTime',
        disabled: false,
        order: "NOTICE_DATETIME"
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ASSIGNMENT_ID"),
        name: 'SN',
        disabled: false,
        order: "ASSIGNMENT_ID"
      },
      {
        text: this.translateService.instant("CASE_COMMON.ASSIGNMENT_MODE"),
        name: 'Mode',
      },
      {
        text: this.translateService.instant("CASE_COMMON.ASSIGNMENT_STATE"),
        name: 'Type',
        disabled: false,
        order: "CASE_ASSIGNMENT_TYPE"
      },
      {
        text: this.translateService.instant("HEADERQURTER_STORE_ASSIGNMENT_SEARCH.ASSIGNMENT_TARGET"),
        name: 'CaseAssignmentUser'
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
        text: this.translateService.instant("CASE_COMMON.NOTIFY_CONTENT"),
        name: 'NoticeContent',
        disabled: false,
        customer: true,
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator'
      },
    ];
  }
}

