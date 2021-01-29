import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { Store } from '@ngrx/store';
import { State as fromMasterReducers } from "../../store/reducers"
import * as fromQuestionCategoryActions from "../../store/actions/question-classification-answer.actions";
import * as fromRootActions from 'src/app/store/actions';
import { QuestionClassificationAnswerSearchViewModel } from 'src/app/model/question-category.model';
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
export const PREFIX = 'QuestionClassificationAnswerComponent';

@Component({
  selector: 'question-classification-answer',
  templateUrl: './question-classification-answer.component.html',
  styleUrls: ['./question-classification-answer.component.scss']
})
export class QuestionClassificationAnswerComponent extends FormBaseComponent implements OnInit {

  isEnable: boolean = false;
  maxLevel: any[] = [];

  placeholder: string = "請選擇";
  form: FormGroup;

  items: any[] = [
    { id: true, text: "啟用" },
    { id: false, text: "停用" },
  ]

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
  model = new QuestionClassificationAnswerSearchViewModel();


  constructor(
    public injector: Injector,
    private store: Store<fromMasterReducers>,
    public http: HttpService) {
    super(injector, PREFIX);
  }

  @loggerMethod()
  ngOnInit(): void {
    this.initSmartTable();
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
      Keyword: new FormControl(),
    })
  }



  initSmartTable() {
    this.ajax.url = 'Master/QuestionClassificationAnswer/GetList'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      //企業別
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_ANSWER.BU_TITLE'),
        name: 'BuName',
        order: 'INDEX',
      },
      //問題分類
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_ANSWER.QUESTIONCLASSIFICATION'),
        name: 'Names',
        disabled: true,
        customer: true
      },
      //主旨
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_ANSWER.SUBJECT'),
        name: 'Title',
        disabled: false,
        order: 'INDEX'
      },
      //內容
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_ANSWER.CONTENT'),
        name: 'Content',
        disabled: true,
        order: 'INDEX',
        customer: true
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
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  criteria($event: PtcServerTableRequest<any>) {

    $event.criteria = this.model;

  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
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
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  btnAdd($event: any) {

    if (this.validSearchForm() == false)
      return;


    //先確認此BU 是否已經擁有問題分類
    const payload = new EntrancePayload<{ BuID: number }>();
    payload.data = { BuID: this.model.NodeID };
    payload.success = (hasAny: boolean) => {

      if (hasAny == false) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('此企業別必須先建立問題分類資料')));
        return;
      }

      this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
        url: './pages/master/question-classification-answer-detail',
        params: {
          actionType: ActionType.Add,
          nodeID: this.model.NodeID
        }
      }))

    }
    
    this.store.dispatch(new fromQuestionCategoryActions.CheckQuestionCategoryAction(payload));


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
        this.store.dispatch(new fromQuestionCategoryActions.DeleteDetailAction(payload));
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
      url: './pages/master/question-classification-answer-detail',
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
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/question-classification-answer-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
      }
    }));
  }

  /**
   * 表單驗證
   */
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


