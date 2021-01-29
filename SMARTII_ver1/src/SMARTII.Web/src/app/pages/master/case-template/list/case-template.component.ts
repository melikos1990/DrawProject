import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { CaseTemplateSearchViewModel, CaseTemplateListViewModel } from 'src/app/model/master.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromCaseTemplateActions from '../../store/actions/case-template.actions';
import { FormGroup, FormControl, Validators } from '@angular/forms';
export const PREFIX = 'CaseTemplateComponent';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';

@Component({
  selector: 'app-case-template',
  templateUrl: './case-template.component.html',
  styleUrls: ['./case-template.component.scss']
})
export class CaseTemplateComponent extends FormBaseComponent implements OnInit {


  @ViewChild('table')
  table: ServerTableComponent;

  isEnable: boolean = false;
  form: FormGroup;

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: CaseTemplateSearchViewModel = new CaseTemplateSearchViewModel();

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
      BuID: new FormControl(null, [
        Validators.required
      ]),
      Content: new FormControl(),
      ClassificKey: new FormControl()
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
  @SearchCacheMethod(PREFIX)
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
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  btnAdd($event: any) {

    if (this.validSearchForm() == false) return;

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-template-detail',
      params: {
        actionType: ActionType.Add,
        BuID: this.model.BuID,
      }
    }));
  }
  /**
   * 按鈕按下刪除
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
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量刪除?',
      () => {

        const payload = new EntrancePayload<Array<CaseTemplateListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseTemplateActions.deleteRangeAction(payload));
      }
    )));

  }


  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseTemplateActions.deleteAction(payload));
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
      url: './pages/master/case-template-detail',
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
      url: './pages/master/case-template-detail',
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

    this.ajax.url = 'Master/CaseTemplate/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.CASE_TEMPLATE_BU'),
        name: 'BuName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.CASE_TEMPLATE_CLASSIFIC'),
        name: 'ClassificName',
        disabled: false,
        order: 'CLASSIFIC_KEY'
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.CASE_TEMPLATE_TITLE'),
        name: 'Title',
        disabled: false,
        order: 'TITLE'
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.CASE_TEMPLATE_CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT',
        customer: true
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.EMAIL_TITLE'),
        name: 'EmailTitle',
        disabled: false,
        order: 'EMAIL_TITLE'
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
