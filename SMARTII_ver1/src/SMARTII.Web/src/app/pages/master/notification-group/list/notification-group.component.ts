import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { NotificationGroupSearchViewModel, NotificationGroupListViewModel } from 'src/app/model/master.model';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import * as fromNotificationGroupActions from '../../store/actions/notification-group.actions';

import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';
export const PREFIX = 'NotificationGroupComponent';

@Component({
  selector: 'app-notification-group',
  templateUrl: './notification-group.component.html',
  styleUrls: ['./notification-group.component.scss']
})
export class NotificationGroupComponent extends FormBaseComponent implements OnInit {


  @ViewChild('table')
  table: ServerTableComponent;

  isEnable: boolean = false;
  form: FormGroup;

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: NotificationGroupSearchViewModel = new NotificationGroupSearchViewModel();

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

  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  initFormGroup() {
    this.form = new FormGroup({
      NodeID: new FormControl(null, [
        Validators.required
      ]),
      CalcMode: new FormControl()
    })
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {

    if (this.validSearchForm() == false) return;

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  /**
   * 按鈕按下新增
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event: any) {

    if (this.validSearchForm() == false) return;

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/notification-group-detail',
      params: {
        actionType: ActionType.Add,
        BuID: this.model.NodeID,
      }
    }));
  }
  /**
   * 按鈕按下刪除
   * @param $event
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Admin)
  btnBatchDelete($event: any) {

    const selectedItems = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (selectedItems == null || selectedItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量刪除?',
      () => {

        const payload = new EntrancePayload<Array<NotificationGroupListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromNotificationGroupActions.deleteRangeAction(payload));
      }
    )));

  }


  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromNotificationGroupActions.deleteAction(payload));
      }
    )));

  }
  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/notification-group-detail',
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
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/notification-group-detail',
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

    this.ajax.url = 'Master/NotificationGroup/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.BU_NAME'),
        name: 'NodeName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.NAME'),
        name: 'Name',
        disabled: true,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.CALC_MODE'),
        name: 'CalcMode',
        disabled: false,
        order: 'CALC_MODE'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.TARGETS'),
        name: 'Targets',
        disabled: true,
        customer: true,
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.ALERT_CYCLE_DAY'),
        name: 'AlertCycleDay',
        disabled: false,
        order: 'ALERT_CYCLE_DAY'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.ALERT_COUNT'),
        name: 'AlertCount',
        disabled: false,
        order: 'ALERT_COUNT'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];


  }

  //驗證查詢表單
  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else {
      return true;
    }
  }

}
