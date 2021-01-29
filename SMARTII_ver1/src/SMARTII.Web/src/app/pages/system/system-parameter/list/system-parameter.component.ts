import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { loggerClass, loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { SystemParameterSearchViewModel, SystemParameterListViewModel } from 'src/app/model/system.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from "src/app/store/reducers"
import * as fromRootActions from "src/app/store/actions"
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import * as fromSystemParameterActions from '../../store/actions/system-parameter.actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';

export const feature = 'SystemParameterComponent';

@Component({
  selector: 'app-system-parameter',
  templateUrl: './system-parameter.component.html',
  styleUrls: ['./system-parameter.component.scss']
})
@loggerClass()
export class SystemParameterComponent extends FormBaseComponent implements OnInit {

  JSON = JSON;

  /**
   * 這邊使用套件為 ptc-server-table
   * 請參照以下網址 ：
   * http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/ng-ptc-server-table?path=%2FREADME.md&version=GBmaster&_a=preview
   */
  @ViewChild('table')
  table: ServerTableComponent;

  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];
  isEnableResult: boolean = false; 
  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  /**
   * 查詢條件
   */
  public model: SystemParameterSearchViewModel = new SystemParameterSearchViewModel();


  constructor(
    private store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector , feature);

    this.featrueName = feature;
  }


  @loggerMethod()
  ngOnInit() {
    this.initializeTable();
  }

  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
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
  @AuthorizeMethod(feature, AuthenticationType.Read)
  @SearchCacheMethod(feature)
  btnRender($event: any) {
    //開啟查詢結果
    this.isEnableResult = null;
    
    this.table.render();
  }

  /**
   * 按鈕按下新增
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(feature)
  btnAdd($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/system/system-parameter-detail',
      params: {
        actionType: ActionType.Add,
      }
    }));
  }
  /**
   * 按鈕按下刪除
   * @param $event
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Read | AuthenticationType.Delete)
  @SearchCacheMethod(feature)
  btnBatchRemove($event: any) {

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

        const payload = new EntrancePayload<Array<SystemParameterListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromSystemParameterActions.deleteRangeAction(payload));
      }
    )));

  }

  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Delete | AuthenticationType.Add)
  @SearchCacheMethod(feature)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ ID: string, Key: string }>();
        payload.data = {
          ID: $event.ID,
          Key: $event.Key,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromSystemParameterActions.deleteAction(payload));
      }
    )));

  }


  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Read)
  @SearchCacheMethod(feature)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/system/system-parameter-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
        key: $event.Key,
      }
    }));
  }

  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Update | AuthenticationType.Add)
  @SearchCacheMethod(feature)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/system/system-parameter-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
        key: $event.Key,
      }
    }));
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'System/SystemParameter/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('SYSTEM_PARAMETER.ID'),
        name: 'ID',
        disabled: false,
        order: 'ID'
      },
      {
        text: this.translateService.instant('SYSTEM_PARAMETER.KEY'),
        name: 'Key',
        disabled: false,
        order: 'KEY'
      },
      {
        text: this.translateService.instant('SYSTEM_PARAMETER.VALUE'),
        name: 'Value',
        disabled: false,
        order: 'VALUE',
        customer : true,
      },
      {
        text: this.translateService.instant('SYSTEM_PARAMETER.TEXT'),
        name: 'Text',
        disabled: false,
        order: 'TEXT'
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
