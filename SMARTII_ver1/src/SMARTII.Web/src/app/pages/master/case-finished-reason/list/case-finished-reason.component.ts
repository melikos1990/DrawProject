import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { PtcServerTableComponent, PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { State as fromMasterReducer } from '../../store/reducers';
import { Store } from '@ngrx/store';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { CaseFinishDataSearchViewModel, CaseFinishClassificationDetailViewModel, CaseFinishClassificationListViewModel, CaseFinishDataListViewModel } from 'src/app/model/master.model';

import * as fromRootActions from 'src/app/store/actions';
import * as fromCaseFinishedReasonActions from '../../store/actions/case-finished-reason.actions';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CaseFinishReasonClassificationModalComponent } from '../modal/case-finished-reason-classification-model/case-finish-reason-classification-modal.component';
import { CaseFinishedReasonSelectComponent } from 'src/app/shared/component/select/element/case-finished-reason-select/case-finished-reason-select.component';
import { CaseFinishedReasonOrderModelComponent } from '../modal/case-finished-reason-order-model/case-finished-reason-order-model.component';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';


const PREFIX = 'CaseFinishedReasonComponent';

@Component({
  selector: 'app-case-finished-reason',
  templateUrl: './case-finished-reason.component.html',
  styleUrls: ['./case-finished-reason.component.scss']
})
export class CaseFinishedReasonComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table') table: PtcServerTableComponent
  @ViewChild('select') select: CaseFinishedReasonSelectComponent;
  public form: FormGroup;
  option = {};

  columns: any[] = [];

  isEnable: boolean = false;
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  model: CaseFinishDataSearchViewModel = new CaseFinishDataSearchViewModel();

  constructor(
    private modalService: NgbModal,
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }


  ngOnInit() {
    this.initializeForm();
    this.initializeTable();
  }

  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnOrderClassification() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const ref = this.modalService.open(CaseFinishedReasonOrderModelComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseFinishedReasonOrderModelComponent>ref.componentInstance);
    instance.title = this.translateService.instant('CASE_FINISHED_REASON.ORDER_CLASSIFICATION')
    instance.ajaxOpt = {
      url: "Master/CaseFinishReason/GetClassificationList",
      method: "get",
      body: {
        nodeID: this.model.NodeID,
      }
    }
    instance.onBtnOrder = this.orderClassification.bind(this);
  }


  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnOrderData() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.model.ClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_FINISHED_REASON.CHOSE_CATEGORY'))));
      return;
    }

    const ref = this.modalService.open(CaseFinishedReasonOrderModelComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseFinishedReasonOrderModelComponent>ref.componentInstance);
    instance.title = this.translateService.instant('CASE_FINISHED_REASON.ORDER_DATA')
    instance.classificationName = this.select.items.find(x=> x.id == this.model.ClassificationID).text;
    instance.ajaxOpt = {
      url: "Master/CaseFinishReason/GetDataList",
      method: "get",
      body: {
        classificationID: this.model.ClassificationID,
      }
    }

    instance.onBtnOrder = this.orderData.bind(this);
  }

  orderData(data: CaseFinishDataListViewModel[]) {

    if (!data || data.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_FINISHED_REASON.NO_ORDER'))));
      return;
    }

    const payload = new EntrancePayload<any[]>();
    payload.data = data
    payload.success = () => {
      this.modalService.dismissAll();
    }

    this.store.dispatch(new fromCaseFinishedReasonActions.orderAction(payload));
  }
  orderClassification(data: CaseFinishClassificationListViewModel[]) {


    if (!data || data.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_FINISHED_REASON.NO_ORDER'))));
      return;
    }

    const payload = new EntrancePayload<any[]>();
    payload.data = data
    payload.success = () => {
      this.modalService.dismissAll();
    }

    this.store.dispatch(new fromCaseFinishedReasonActions.orderClassificationAction(payload));
  }


  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAddClassification($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const ref = this.modalService.open(CaseFinishReasonClassificationModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseFinishReasonClassificationModalComponent>ref.componentInstance);
    const model = new CaseFinishClassificationDetailViewModel();
    model.NodeID = this.model.NodeID;
    model.OrganizationType = this.organizationType.HeaderQuarter;
    model.IsEnabled = true;
    instance.uiActionType = this.actionType.Add;
    this.store.dispatch(new fromCaseFinishedReasonActions.loadClassificationEntryAction(model));
    instance.btnAddClassification = this.addClassification.bind(this);
  }

  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnUpdateClassification($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.model.ClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_FINISHED_REASON.CHOSE_CATEGORY'))));
      return;
    }

    const ref = this.modalService.open(CaseFinishReasonClassificationModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseFinishReasonClassificationModalComponent>ref.componentInstance);
    instance.uiActionType = this.actionType.Update;
    this.store.dispatch(new fromCaseFinishedReasonActions.loadClassificationDetailAction({ ID: this.model.ClassificationID }));
    instance.btnUpdateClassification = this.updateClassification.bind(this);
  }

  updateClassification(model: CaseFinishClassificationDetailViewModel) {

    const payload = new EntrancePayload<CaseFinishClassificationDetailViewModel>();
    payload.data = model
    payload.success = () => {
      this.modalService.dismissAll();
      this.select.getList();
    }

    this.store.dispatch(new fromCaseFinishedReasonActions.editClassificationAction(payload));

  }

  addClassification(model: CaseFinishClassificationDetailViewModel) {

    const payload = new EntrancePayload<CaseFinishClassificationDetailViewModel>();
    payload.data = model
    payload.success = () => {
      this.modalService.dismissAll();
      this.select.getList();
    }
    this.store.dispatch(new fromCaseFinishedReasonActions.addClassificationAction(payload));
  }
  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    this.model.OrganizationType = this.organizationType.HeaderQuarter;

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

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage(this.translateService.instant('COMMON.DISABLED_CHECK'),
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromCaseFinishedReasonActions.disabledAction(payload));
      }
    )));

  }

  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-finished-reason-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
      }
    }));
  }


  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/case-finished-reason-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
      }
    }));
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
      url: './pages/master/case-finished-reason-detail',
      params: {
        actionType: ActionType.Add,
        nodeID: this.model.NodeID,
      }
    }));
  }

  onBuChange(){
    this.model.ClassificationID = null;
  }

  /**
   * 初始化form 資訊
   */
  initializeForm() {

    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),

    });

  }

  /**
  * 初始化Table資訊
  */
  initializeTable() {

    this.ajax.url = 'Master/CaseFinishReason/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'NodeName',
        disabled: false,
        order: 'CASE_FINISH_REASON_CLASSIFICATION.NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_FINISHED_REASON.CLASSIFICATION_NAME'),
        name: 'ClassificationName',
        disabled: false,
        order: 'CASE_FINISH_REASON_CLASSIFICATION.TITLE'
      },
      {
        text: this.translateService.instant('CASE_FINISHED_REASON.TEXT'),
        name: 'Text',
        disabled: false,
        order: 'TEXT'
      },
      {
        text: this.translateService.instant('CASE_FINISHED_REASON.IS_MULTIPLE'),
        name: 'IsMutiple',
        disabled: false,
        order: 'CASE_FINISH_REASON_CLASSIFICATION.IS_MULTIPLE'
      },
      {
        text: this.translateService.instant('CASE_FINISHED_REASON.IS_ENABLED'),
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
