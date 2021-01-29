import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { CaseTagSearchViewModel } from '../../../../model/master.model';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as fromCaseTagActions from '../../store/actions/case-tag.actions';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';


const PREFIX = 'CaseTagComponent';

@Component({
  selector: 'app-case-tag',
  templateUrl: './case-tag.component.html',
  styleUrls: ['./case-tag.component.scss']
})
export class CaseTagComponent extends FormBaseComponent implements OnInit {

  @ViewChild(BuSelectComponent) select: BuSelectComponent;

  @ViewChild('table')
  table: ServerTableComponent;

  columns: any[] = [];

  isEnable: boolean = false;
  form: FormGroup;
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: CaseTagSearchViewModel = new CaseTagSearchViewModel();

  constructor(
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.initFormGroup();
  }

  @loggerMethod()
  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  initFormGroup(){
    this.form = new FormGroup({
      BuID: new FormControl(null, [
        Validators.required
      ])
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
    
    if(this.validSearchForm() == false) return;

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
    
    if(this.validSearchForm() == false) return;

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-tag-detail',
      params: {
        actionType: ActionType.Add,
        BuID: this.select.value,
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
      url: './pages/master/case-tag-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
      }
    }));
  }

  /**
   * 當ptc server table 按下停用
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = { ID: $event.ID };
        payload.success = () => this.table.render();
        this.store.dispatch(new fromCaseTagActions.disableAction(payload));
      }
    )));
  }

    /**
   * 當ptc server table 按下檢視
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-tag-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
      }
    }));
  }


  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Master/CaseTag/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'BuName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_TAG.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('CASE_TAG.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
      },
    ];


  }

  //驗證查詢表單
  private validSearchForm(){
    if(this.validForm(this.form) == false){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else {
      return true;
    }
  }

}
