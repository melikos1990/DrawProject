import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { CaseRemindSearchViewModel, CaseRemindListViewModel } from 'src/app/model/master.model';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { AuthenticationType, User } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import * as fromCaseRemindActions from '../../store/actions/case-remind.actions';

import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ChangeInfo } from 'ptc-select2';
import { NumberValidator } from 'src/app/shared/data/validator';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';

export const PREFIX = 'CaseRemindComponent';

@Component({
  selector: 'app-case-remind',
  templateUrl: './case-remind.component.html',
  styleUrls: ['./case-remind.component.scss']
})
export class CaseRemindComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  selectUser: User;
  isEnable: boolean = false;
  form: FormGroup;

  items: any[] = [
    { id: true, text: "已完成" },
    { id: false, text: "未完成" },
  ]

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: CaseRemindSearchViewModel = new CaseRemindSearchViewModel();

  constructor(
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  @loggerMethod()
  ngOnInit() {
    this.initializeTable();
    this.initFormGroup();
  }

  @loggerMethod()
  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }


  initFormGroup() {
    this.form = new FormGroup({
      CaseID: new FormControl(),
      CaseAssignmentID: new FormControl(null, [
        NumberValidator
      ]),
      ActiveDateTimeRange: new FormControl(),
      CreateDateTimeRange: new FormControl(),
      UserIDs: new FormControl(),
      CreateUserID: new FormControl(),
      IsConfirm: new FormControl(),
      Level: new FormControl(),
      NodeID: new FormControl(null, [
        Validators.required
      ]),
      Content: new FormControl(),
    })
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  @loggerMethod()
  onItemChange($event: ChangeInfo) {
    if ($event.item !== undefined)
      this.selectUser = $event.item.extend;
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  /**
   * 按鈕按下新增
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  btnAdd($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-remind-detail',
      params: {
        actionType: ActionType.Add
      }
    }));
  }
  /**
   * 按鈕按下刪除 2/12確認不須批量刪除按鈕
   * @param $event
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Delete)
  btnBatchDelete($event: any) {

    const selectedItems = this.table.getSelectItem();

    if (selectedItems == null || selectedItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量刪除?',
      () => {

        const payload = new EntrancePayload<Array<CaseRemindListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseRemindActions.deleteRangeAction(payload));
      }
    )));

  }


  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {
    
    if ((Date.parse($event.ActiveStartDateTime)).valueOf() < Date.now() && (Date.parse($event.ActiveEndDateTime)).valueOf() > Date.now()){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('通知已發送，內容不可修改。')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseRemindActions.deleteAction(payload));
      }
    )));

  }
  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-remind-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
      }
    }));
  }


  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-remind-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
      }
    }));
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Master/CaseRemind/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'BuName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CASE_ID'),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CASE_ASSIGNMENT_ID'),
        name: 'AssignmentID',
        disabled: false,
        order: 'ASSIGNMENT_ID'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT'
      },
      {
        text: this.translateService.instant('CASE_REMIND.LEVEL'),
        name: 'LevelName',
        disabled: false,
        order: 'TYPE'
      },
      {
        text: this.translateService.instant('CASE_REMIND.ISCONFIRM'),
        name: 'IsConfirm',
        disabled: false,
        order: 'IS_CONFIRM'
      },
      {
        text: this.translateService.instant('CASE_REMIND.ACTIVE_DATETIME_RANGE'),
        name: 'ActiveStartDateTime',
        disabled: false,
        order: 'ACTIVE_START_DAETTIME'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CREATE_DATETIME_RANGE'),
        name: 'CreateDateTime',
        disabled: false,
        order: 'CREATE_DATETIME'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];


  }

}

