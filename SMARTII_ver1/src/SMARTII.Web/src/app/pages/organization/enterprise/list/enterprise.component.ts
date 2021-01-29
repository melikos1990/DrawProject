import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { EnterpriseSearchViewModel } from 'src/app/model/organization.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromOrganizationReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';
import * as fromEnterpriseActions from "../../store/actions/enterprise.actions";

export const PREFIX = 'EnterpriseComponent';

@Component({
  selector: 'app-enterprise',
  templateUrl: './enterprise.component.html',
  styleUrls: ['./enterprise.component.scss']
})
export class EnterpriseComponent extends FormBaseComponent implements OnInit {

  /**
   * 這邊使用套件為 ptc-server-table
   * 請參照以下網址 ：
   * http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/ng-ptc-server-table?path=%2FREADME.md&version=GBmaster&_a=preview
   */
  @ViewChild('table')
  table: ServerTableComponent;

  isEnable: boolean = false;

  /**
 * 定義顯示之欄位 , 用途請參照以上網址
 */
  columns: any[] = [];

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();


  public model: EnterpriseSearchViewModel = new EnterpriseSearchViewModel();

  constructor(
    public store: Store<fromOrganizationReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
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
   * 按鈕按下新增
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  btnAdd($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/organization/enterprise-detail',
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
  @SearchCacheMethod(PREFIX)
  btnRender($event: any) {

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  /**
 * 當ptc server table 按下查詢
 */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/organization/enterprise-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.EnterpriseID,
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
      url: './pages/organization/enterprise-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.EnterpriseID,
      }
    }));
  }

    /**
   * 當ptc server table 按下停用
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  onBtnDisable($event: any) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = { ID: $event.EnterpriseID };
        payload.success = () => this.table.render();
        this.store.dispatch(new fromEnterpriseActions.disabledDetail(payload));
      }
    )));

  }

  /**
    * 初始化Table資訊
    */
  initializeTable() {

    this.ajax.url = 'Organization/Enterprise/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('ENTERPRISE.ENTERPRISE_NAME'),
        name: 'EnterpriseName',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('ENTERPRISE.IS_ENABLED'),
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
