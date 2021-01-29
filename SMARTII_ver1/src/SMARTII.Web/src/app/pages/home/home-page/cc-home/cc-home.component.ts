import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers/index';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { SummaryTargetViewModel } from 'src/app/model/organization.model';
import { CcHomeSearchViewModel } from 'src/app/model/home.model';
import * as fromRootActions from 'src/app/store/actions';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { environment } from 'src/environments/environment';

export const PREFIX = 'CcHomeComponent';

@Component({
  selector: 'app-cc-home',
  templateUrl: './cc-home.component.html',
  styleUrls: ['./cc-home.component.scss']
})
export class CcHomeComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table') table: ServerTableComponent;

  public isAttentsionItems = [
    { id: true, text: '是' },
    { id: false, text: '否' }
  ];

  isEnable: boolean = false;

  cuurentTarget: SummaryTargetViewModel;

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: CcHomeSearchViewModel = new CcHomeSearchViewModel();

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
    ($event.direction == undefined || $event.direction == null) &&  ($event.direction = "asc")
  }

  /**
    * 按下編輯
    */
  @loggerMethod()
  onBtnEdit($event: any) {

    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Update,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');

  }
  /**
   * 按下查詢
   */
  @loggerMethod()
  onBtnSearch($event: any) {

    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Read,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');

  }

  /**
   * 個人案件BU切換
   */
  @loggerMethod()
  onPersonalClick($event: SummaryTargetViewModel) {

    //開啟查詢結果
    this.isEnable = null;

    this.model.BuID = $event.TargetID;
    this.model.IsSelf = true;

    $event.TargetFrom = 'Personal';

    this.cuurentTarget = $event;

    this.table.render();
  }

  /**
   * 全部案件BU切換
   */
  @loggerMethod()
  onTotalClick($event: SummaryTargetViewModel) {

    //開啟查詢結果
    this.isEnable = null;

    this.model.BuID = $event.TargetID;
    this.model.IsSelf = false;

    $event.TargetFrom = 'Total';

    this.cuurentTarget = $event;

    this.table.render();
  }

  /**
   * 關注案件切換
   */
  @loggerMethod()
  onisAttentsionChange($event) {

    this.table.render();
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {
    //this.model.IsAttention = true;

    this.ajax.url = 'Summary/CallCenterSummary/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant("CASE_COMMON.CASE_ID"),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_SOURCE_TYPE"),
        name: 'CaseSourceType',
        disabled: false,
        order: 'CASE_SOURCE.SOURCE_TYPE'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CREATE_DATETIME"),
        name: 'CreateTime',
        disabled: false,
        order: 'CREATE_DATETIME'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_WARNING_ID"),
        name: 'CaseWarning',
        disabled: false,
        order: 'CASE_WARNING_ID'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_TYPE"),
        name: 'CaseType',
        disabled: false,
        order: 'CASE_TYPE'
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_RESPONDER"),
        name: 'ConcatUserName',
        disabled: true
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_USER"),
        name: 'CaseComplainedUserName',
        disabled: true
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_COMPAINED_NODE"),
        name: 'ComplainedUserParentNamePath',
        disabled: true
      },
      {
        text: this.translateService.instant("CASE_COMMON.CASE_CONTENT"),
        name: 'CaseContent',
        disabled: false,
        order: 'CONTENT',
        customer: true,
      },      
      {
        text: this.translateService.instant("CASE_COMMON.APPLY_USER"),
        name: 'ApplyUserName',
        disabled: false,
        order: 'APPLY_USERNAME'
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
