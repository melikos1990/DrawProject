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
import { CaseWarningSearchViewModel, CaseWarningListViewModel } from '../../../../model/master.model';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as fromCaseWarningActions from '../../store/actions/case-warning.actions';
import { CaseWarningOrderModelComponent } from '../modal/case-warning-order-model.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';


const PREFIX = 'CaseWarningComponent';

@Component({
  selector: 'app-case-warning',
  templateUrl: './case-warning.component.html',
  styleUrls: ['./case-warning.component.scss']
})
export class CaseWarningComponent extends FormBaseComponent implements OnInit {

  @ViewChild(BuSelectComponent) select: BuSelectComponent;

  @ViewChild('table')
  table: ServerTableComponent;

  columns: any[] = [];

  isEnable: boolean = false;
  form: FormGroup;
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: CaseWarningSearchViewModel = new CaseWarningSearchViewModel();

  constructor(
    private modalService: NgbModal,
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

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
      ])
    })
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnOrder() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const ref = this.modalService.open(CaseWarningOrderModelComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseWarningOrderModelComponent>ref.componentInstance);
    instance.title = this.translateService.instant('CASE_WARNING.ORDER')
    instance.ajaxOpt = {
      url: "Master/CaseWarning/GetDataList",
      method: "get",
      body: {
        nodeID: this.model.NodeID,
      }
    }
    instance.onBtnOrder = this.order.bind(this);
  }

  order(data: CaseWarningListViewModel[]) {

    if (!data || data.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("無資料可排序")));
      return;
    }

    const payload = new EntrancePayload<any[]>();
    payload.data = data
    payload.success = () => {
      this.modalService.dismissAll();
    }

    this.store.dispatch(new fromCaseWarningActions.orderAction(payload));
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
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

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
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-warning-detail',
      params: {
        actionType: ActionType.Add,
        NodeID: this.select.value,
      }
    }));
  }
  /**
    * 按鈕按下刪除
    * @param $event
    */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Delete)
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

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量停用?',
      () => {

        const payload = new EntrancePayload<Array<CaseWarningListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseWarningActions.disableRangeAction(payload));
      }
    )));

  }


  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseWarningActions.disableAction(payload));
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
      url: './pages/master/case-warning-detail',
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
      url: './pages/master/case-warning-detail',
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

    this.ajax.url = 'Master/CaseWarning/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'NodeName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_WARNING.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('CASE_WARNING.WORK_HOUR'),
        name: 'WorkHour',
        disabled: false,
        order: 'WORK_HOUR'
      },
      {
        text: this.translateService.instant('CASE_WARNING.IS_ENABLED'),
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

}

