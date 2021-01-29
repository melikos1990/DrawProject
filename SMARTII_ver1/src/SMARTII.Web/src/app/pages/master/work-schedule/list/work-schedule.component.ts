import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { WorkScheduleSearchViewModel } from 'src/app/model/master.model';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromWorkScheduleActions from 'src/app/pages/master/store/actions/work-schedule.actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';


export const PREFIX = 'WorkScheduleComponent';

@Component({
  selector: 'app-work-schedule',
  templateUrl: './work-schedule.component.html',
  styleUrls: ['./work-schedule.component.scss']
})
export class WorkScheduleComponent extends FormBaseComponent implements OnInit {


  @ViewChild('table')
  table: ServerTableComponent;
  @ViewChild("localTableExportTemp") localTableExportTemp: TemplateRef<any>;

  isEnable: boolean = false;
  form: FormGroup;

  model: WorkScheduleSearchViewModel = {} as WorkScheduleSearchViewModel;


  columns: any[] = [];
  columnsExport: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  dateTimeOpt = {
    showDropdowns: true,
    timePicker24Hour: false,
    timePicker: false,
  }

  constructor(
    public injector: Injector,
    public store: Store<fromMasterReducer>,
  ) {
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
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event: any) {

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/work-schedule-detail',
      params: {
        actionType: this.actionType.Add
      }
    }));
  }

  /**
   * 批次刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Admin)
  btnDeleteRange($event: any) {

    let datas = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (datas == null || datas.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批次刪除?',
      () => {
        let payload = new EntrancePayload(datas);
        payload.success = this.btnRender.bind(this);
        payload.dataExport = this.localTableExportTemp;

        this.store.dispatch(new fromWorkScheduleActions.deleteRangeDetail(payload));
      }
    )));
  }

  /**
   * 當ptc server table 按下刪除
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {

    if(this.canEditAndDelete($event)){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("資料早於系統日期，不可編輯")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        let payload = new EntrancePayload($event);
        payload.success = this.btnRender.bind(this);

        this.store.dispatch(new fromWorkScheduleActions.deleteDetail(payload));
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
      url: './pages/master/work-schedule-detail',
      params: {
        actionType: this.actionType.Read,
        id: $event.ID
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

    if(this.canEditAndDelete($event)){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("資料早於系統日期，不可編輯")));
      return;
    }

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/work-schedule-detail',
      params: {
        actionType: this.actionType.Update,
        id: $event.ID
      }
    }));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {

    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }


  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Master/WorkSchedule/GetList/'.toHostApiUrl();
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
        text: this.translateService.instant('WORK_SCHEDULE.TABLE_DATATIME'),
        name: 'Date',
        disabled: true,
        order: '',
      },
      {
        text: this.translateService.instant('WORK_SCHEDULE.TABLE_TITLE'),
        name: 'Title',
        disabled: true,
        order: '',
      },
      {
        text: this.translateService.instant('WORK_SCHEDULE.TABLE_CLASSIFICATION'),
        name: 'WorkType',
        disabled: true,
        order: '',
      },
      {
        text: this.translateService.instant('WORK_SCHEDULE.UPDATE_USERNAME_LABEL'),
        name: 'UpdateUserName',
        disabled: true,
        order: '',
      },
      {
        text: this.translateService.instant('WORK_SCHEDULE.UPDATE_DATETIME_LABEL'),
        name: 'UpdateDateTime',
        disabled: true,
        order: '',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];

    this.columnsExport = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'BuName'
      },
      {
        text: this.translateService.instant('WORK_SCHEDULE.TABLE_DATATIME'),
        name: 'Date'
      }
    ]

  }

  initFormGroup() {
    this.model.YearTime = this.defaultDateTime("YYYY-MM-DD");

    this.form = new FormGroup({
      BuID: new FormControl(null, [
        Validators.required
      ]),
      YearTime: new FormControl(this.model.YearTime, [
        Validators.maxLength(4),
        Validators.minLength(4),
      ])
    })
  }

  canEditAndDelete(data) {
    return new Date(data.Date) < new Date();
  }

}
