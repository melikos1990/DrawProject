import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { RoleSearchViewModel, AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromRoleReducer } from "src/app/store/reducers"
import * as fromRootActions from 'src/app/store/actions';
import * as fromRoleActions from '../../store/actions/role.actions';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';

export const PREFIX = 'RoleComponent';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent extends FormBaseComponent implements OnInit {

  public enabledItems = [
    { id: true, text: '啟用' },
    { id: false, text: '停用' }
  ];

  isEnable: boolean = false;

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

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();


  public model = new RoleSearchViewModel();

  constructor(
    public store: Store<fromRoleReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

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
   * 按鈕按下新增
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  btnAdd($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/system/role-detail',
      params: {
        actionType: ActionType.Add,
      }
    }));
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete)
  btnDisableRnage($event: any) {

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

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量停用?',
      () => {
        const payload = new EntrancePayload<number[]>();
        payload.data = selectedItems.map(x => x.RoleID);
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromRoleActions.disableRangeAction(payload));
      }
    )));

  }

  /**
   * 當ptc server table 按下停用
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<string>();
        payload.data = $event.RoleID;
        payload.success = () => this.btnRender(null);

        this.store.dispatch(new fromRoleActions.disableAction(payload));
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
      url: './pages/system/role-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.RoleID,
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
      url: './pages/system/role-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.RoleID,
      }
    }));
  }


  initializeTable() {

    this.ajax.url = 'Organization/Role/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('ROLE.ROLE_NAME'),
        name: 'RoleName',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('ROLE.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('COMMON.ACTION'),
        name: 'p-operator',
        disabled: true,
        order: ''
      },
    ];

  }

}
