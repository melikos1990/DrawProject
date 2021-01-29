import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../../store/reducers"
import * as fromQuestionClassificationGuideActions from "../../store/actions/question-classification-guide.actions";
import * as fromRootActions from 'src/app/store/actions';
import { QuestionClassificationGuideSearchViewModel } from 'src/app/model/question-category.model';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';

import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
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
export const PREFIX = 'QuestionClassificationGuideComponent';

@Component({
  selector: 'question-classification-guide',
  templateUrl: './question-classification-guide.component.html',
  styleUrls: ['./question-classification-guide.component.scss']
})
export class QuestionClassificationGuideComponent extends FormBaseComponent implements OnInit {
  
  isEnable: boolean = false;

  placeholder: string = "請選擇";
  form: FormGroup;

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
  model = new QuestionClassificationGuideSearchViewModel();

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
  }

  @loggerMethod()
  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  initFormGroup() {
    this.form = new FormGroup({
      BuID: new FormControl(null, [
        Validators.required
      ]),
    })
  }


  initSmartTable() {
    this.ajax.url = 'Master/QuestionClassificationGuide/GetList'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      //企業別
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_GUIDE.BU_TITLE'),
        name: 'BuName',
        order: 'INDEX',
      },
       //問題分類
       {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_GUIDE.QUESTIONCLASSIFICATION'),
        name: 'Names',
        disabled: true,
        customer: true
      },
      //引導內容
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_GUIDE.GUIDECONTENT'),
        name: 'Content',
        order: 'INDEX',
      },
      //系統操作
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回serverClassificationID = null
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    this.model.ClassificationID = <any>this.dynamicSelect.lastHasValue;
    $event.criteria = this.model;
    console.log("$event.criteria => ", $event.criteria);
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {

    if (this.validSearchForm() == false)
      return;

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

        //先確認此BU 是否已經擁有問題分類
        const payload = new EntrancePayload<{ BuID: number }>();
        payload.data = { BuID: this.model.NodeID };
        //callback 設定
        payload.success = (hasAny: boolean) => {
    
          if (hasAny == false) {
            this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('此企業別必須先建立問題分類資料')));
            return;
          }
    
          this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
            url: './pages/master/question-classification-guide-detail',
            params: {
              actionType: ActionType.Add,
              nodeID: this.model.NodeID
            }
          }))    
        }
        
        this.store.dispatch(new fromQuestionClassificationGuideActions.CheckQuestionCategoryAction(payload));
  }

  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ id: number }>();
        payload.data = { id: $event.ID };
        payload.success = () => this.table.render();
        this.store.dispatch(new fromQuestionClassificationGuideActions.DeleteDetailAction(payload));
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
      url: './pages/master/question-classification-guide-detail',
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
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/question-classification-guide-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
        organizationType: $event.OrganizationType,
        node_ID: $event.Node_ID
      }
    }));
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

}


