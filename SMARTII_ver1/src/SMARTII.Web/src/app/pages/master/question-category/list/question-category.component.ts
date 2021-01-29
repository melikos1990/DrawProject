import { Component, OnInit, ViewChild, Injector, OnDestroy, ViewChildren, QueryList } from '@angular/core';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../../store/reducers"
import * as fromQuestionCategoryActions from "../../store/actions/question-category.actions";
import * as fromRootActions from 'src/app/store/actions';
import { QuestionClassificationSearchViewModel, QuestionCategoryDetail, QuestionClassificationListViewModel } from 'src/app/model/question-category.model';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';

import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { HeaderQuarterNodeDetailViewModel } from 'src/app/model/organization.model';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';

/**
 * 功能名稱
 * - 於模組中的驗證 , 應用範圍如下 :
 * - src/shared/component/authorize.directive.ts  (UI 權限判定)
 * - src/shared/decorator/authorize.decorator.ts  (component/function 權限判定)
 * - src/shared/service/authorize.decorator.ts    (權限驗證實作)
 */
export const PREFIX = 'QuestionCategoryComponent';

@Component({
  selector: 'app-question-category',
  templateUrl: './question-category.component.html',
  styleUrls: ['./question-category.component.scss']
})

export class QuestionCategoryComponent extends FormBaseComponent implements OnInit {

  isEnable: boolean = false;
  maxLevel: any[] = [];

  placeholder: string = "請選擇";
  form: FormGroup;

  items: any[] = [
    { id: true, text: "啟用" },
    { id: false, text: "停用" },
  ]

  isAdmin: boolean = false;

  @ViewChild(BuSelectComponent) select: BuSelectComponent;
  @ViewChild(DynamicQuestionSelectComponent) dynamicSelect: DynamicQuestionSelectComponent;

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

  /**
   * 這邊為畫面上的查詢條件 , 預設為建立之物件
   */
  public model = new QuestionClassificationSearchViewModel();


  node: HeaderQuarterNodeDetailViewModel;

  get getSelectedBuName() {
    return this.node || this.node.Name;
  }

  constructor(
    public injector: Injector,
    private store: Store<fromReducers>,
    public http: HttpService) {
    super(injector, PREFIX);
  }


  @loggerMethod()
  ngOnInit(): void {
    this.initSmartTable();
    this.initFormGroup();
    this.hasAdmin();
  }

  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  initFormGroup() {
    this.form = new FormGroup({
      BuID: new FormControl(null, [
        Validators.required
      ]),
      IsEnable: new FormControl()
    })
  }

  onSelectedChange($event: HeaderQuarterNodeDetailViewModel) {
    this.node = $event;
    this.model.IsEnable = true;
  }

  initSmartTable() {
    this.ajax.url = 'Master/QuestionClassification/GetList'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('QUESTIONCATEGORY.BU_TITLE'),
        name: 'BuName',
        // customer : true,
      },
      {
        text: this.translateService.instant('QUESTIONCATEGORY.QUESTIONCLASSIFICATION'),
        name: 'Names',
        disabled: false,
        order: 'ID',
        customer: true
      },
      {
        text: this.translateService.instant('QUESTIONCATEGORY.ENABLE_TITLE'),
        name: 'IsEnable',
        disabled: false,
        order: 'ID'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    let item = this.dynamicSelect.getLastHasValueBySelectedItem();
    this.model.ParnetIDPath = item != null ? item.extend.ParentPath : null;

    $event.criteria = this.model;
    console.log("$event.criteria => ", $event.criteria);
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
   * 按鈕按下查詢
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event: any) {

    if (this.validSearchForm() == false) return;

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/question-category-detail',
      params: {
        actionType: ActionType.Add,
        node_ID: this.model.BuID
      }
    }))
  }

  /**
   * 按鈕按下停用
   * @param $event
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Delete)
  btnBatchDisable($event: any) {

    const selectedItems = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (selectedItems == null || selectedItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('未選擇項目')));
      return;
    }

    var datas = selectedItems.map(x => {
      let data = new QuestionClassificationListViewModel();
      data.ID = x.ID;
      return data;
    })

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量停用?',
      () => {
        const payload = new EntrancePayload<QuestionCategoryDetail[]>();
        payload.data = datas;
        payload.success = () => this.table.render();
        this.store.dispatch(new fromQuestionCategoryActions.DeleteRangeDetail(payload));
      }
    )));

  }

  /**
   * 當按鈕 按下匯出
   */
  btnReport(){
    this.store.dispatch(new fromQuestionCategoryActions.GetRrport(this.model));
  }


  /**
   * 當ptc server table 按下停用
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<{ id: number }>();
        payload.data = { id: $event.ID };
        payload.success = () => this.table.render();
        this.store.dispatch(new fromQuestionCategoryActions.DeleteDetail(payload));
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
      url: './pages/master/question-category-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
        organizationType: $event.OrganizationType,
        node_ID: $event.Node_ID
      }
    }));

  }

  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/question-category-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
        organizationType: $event.OrganizationType,
        node_ID: $event.Node_ID
      }
    }));
  }


  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  btnOrderby() {

    if (this.validSearchForm() == false) return;

    let payload = {
      url: './pages/master/question-category-order',
      params: {
        buID: this.model.BuID,
        buName: this.select.getItem(this.model.BuID).text
      }
    }

    if (<any>this.dynamicSelect.lastHasValue)
      payload.params["parentID"] = <any>this.dynamicSelect.lastHasValue;

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction(payload));
  }

  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else {
      return true;
    }
  }

  hasAdmin(){
    this.ishasAuth$(this.authType.Admin).subscribe(result => {

      this.isAdmin = result;

    })
  }

}


