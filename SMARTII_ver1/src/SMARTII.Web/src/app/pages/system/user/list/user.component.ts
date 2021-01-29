import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from "src/app/store/reducers"
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType, UserSearchViewModel } from 'src/app/model/authorize.model';
import * as fromRootActions from "src/app/store/actions";
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromUserActions from '../../store/actions/user.actions';
import { RoleSelectComponent } from 'src/app/shared/component/select/element/role-select/role-select.component';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';
export const PREFIX = 'UserComponent';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent extends FormBaseComponent implements OnInit {

  @ViewChild("roleSelect") roleSelect: RoleSelectComponent;

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

  public enabledItems = [
    { id: true, text: '啟用' },
    { id: false, text: '停用' }
  ];

  public adItems = [
    { id: true, text: '是' },
    { id: false, text: '否' }
  ];



  isEnable: boolean = false;

  public model = new UserSearchViewModel();


  constructor(
    private store: Store<fromRootReducers>,
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
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {
    
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
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/system/user-detail',
      params: {
        actionType: ActionType.Add,
      }
    }));
  }

  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<string>();
        payload.data = $event.UserID;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromUserActions.disableAction(payload));
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
      url: './pages/system/user-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.UserID,
      }
    }));
  }

  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/system/user-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.UserID,
      }
    }));
  }

  /**
   * 匯出Excel
   */
  btnReport(){
    console.log("btnReport");
    this.fillback();
    this.store.dispatch(new fromUserActions.report(this.model));
  }

  fillback(){
    
    if(this.model.RoleIDs && this.roleSelect && this.roleSelect.items.length > 0){
      
      let roleIDs = [...this.model.RoleIDs];
      let items = [...this.roleSelect.items];
      
      // 取得選擇的順序
      let selectRoles = roleIDs.map(id => {
        let item = items.filter(x => id == x.id)[0];
        return item; 
      })
      
      
      this.model.RoleNames = (selectRoles && selectRoles.length > 0) ?
                                    selectRoles.map(x => x.text) :
                                    null;

    }

    console.log("this.model => ", this.model);
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Organization/User/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('USER.ACCOUNT'),
        name: 'Account',
        disabled: false,
        order: 'ACCOUNT',
      },
      {
        text: this.translateService.instant('USER.IS_SYSTEM_USER'),
        name: 'IsSystemUser',
        disabled: false,
        order: 'IS_SYSTEM_USER',
      },
      {
        text: this.translateService.instant('USER.IS_AD'),
        name: 'IsAD',
        disabled: false,
        order: 'IS_AD'
      },
      {
        text: this.translateService.instant('USER.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('USER.ROLE_NAME'),
        name: 'RoleNames',
        disabled: true,
        order: '',
        customer : true
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
