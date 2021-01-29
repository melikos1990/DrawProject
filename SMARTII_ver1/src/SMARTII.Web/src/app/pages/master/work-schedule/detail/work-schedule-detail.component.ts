import { Component, OnInit, Injector, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { WorkScheduleDetailViewModel, WorkType } from 'src/app/model/master.model';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../../store/reducers";
import * as fromRootActions from "src/app/store/actions";
import * as fromWorkScheduleActions from "../../store/actions/work-schedule.actions";
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ActivatedRoute } from '@angular/router';
import { State as fromMasterReducer } from '../../store/reducers';
import { filter, takeUntil } from 'rxjs/operators';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';


export const PREFIX = 'WorkScheduleComponent';

@Component({
  selector: 'app-work-schedule-detail',
  templateUrl: './work-schedule-detail.component.html',
  styleUrls: ['./work-schedule-detail.component.scss']
})
export class WorkScheduleDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild("datePicker") datePicker: ElementRef<any>;
  @ViewChild("localTableExportTemp") localTableExportTemp: TemplateRef<any>;

  columns: any[] = [];
  columnsExport: any[] = [];
  createModel: WorkScheduleDetailViewModel[] = [];
  titleTypeString: string = "";
  public uiActionType: ActionType;
  public form: FormGroup;
  workType: any = WorkType;

  model: WorkScheduleDetailViewModel = {} as WorkScheduleDetailViewModel;


  constructor(
    public injector: Injector,
    public store: Store<fromReducers>,
    private active: ActivatedRoute,
  ) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeForm();
    this.initializeTable();
    this.subscription();
  }


  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.store
      .select((state: fromMasterReducer) => state.master.workSchedule.detail)
      .pipe(
        filter(x => !!(x)),
        takeUntil(this.destroy$))
      .subscribe(detail => {
        console.log("detail => ", detail);
        this.model = detail;
      });

  }


  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      id: params['id']
    };

    switch (this.uiActionType) {

      case this.actionType.Add:
        this.store.dispatch(new fromWorkScheduleActions.loadEntry(new WorkScheduleDetailViewModel()));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case this.actionType.Update:
        this.store.dispatch(new fromWorkScheduleActions.loadDetail(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case this.actionType.Read:
        this.store.dispatch(new fromWorkScheduleActions.loadDetail(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
      default:
        break;
    }
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd() {

    if (!this.model.BuID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    let datas = [];

    if (this.model.BuID instanceof Array) {
      this.model.BuID.forEach(buid => {
        datas = [...datas, ...this.createModel.map(model => ({ ...model, BuID: buid }))];
      })
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        let payload = new EntrancePayload(datas);
        payload.dataExport = this.localTableExportTemp;

        this.store.dispatch(new fromWorkScheduleActions.addWorkSchedule(payload));
      }
    )));


  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnEdit() {

    if (this.validSearchForm() == false) return;

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromWorkScheduleActions.editWorkSchedule(this.model));
      }
    )));

  }


  @loggerMethod()
  btnBack() {
    history.back();
  }

  btnDelete($event) {
    console.log($event);

    let idx = this.createModel.findIndex(x => x.DateStr == $event.DateStr);

    if (idx < 0) return;

    let _items = [...this.createModel];
    _items.splice(idx, 1);

    this.createModel = _items;
  }

  btnAddItem() {

    if (this.validSearchForm() == false) return;

    if (!this.model.DateStr) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if ((this.model.WorkType == null || this.model.WorkType == undefined)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("至少需勾選一項類型")));
      return;
    }

    let dates = this.splitDateRange(this.model.DateStr)

    let exist = dates.some(date => this.hasDateTime(date))

    if (exist) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("資料已存在")));
      return;
    }

    let _models = dates.map(date => ({ ...this.model, DateStr: date }));

    this.createModel = [...this.createModel, ..._models];
    this.resetModel();
  }

  checkboxChange(workType: WorkType) {
    this.model.WorkType = workType;
  }

  initializeForm() {
    this.form = new FormGroup({
      BuID: new FormControl(this.model.BuID, [
        Validators.required,
      ]),
      DateStr: new FormControl(this.model.DateStr, [
        Validators.required,
      ]),
      Title: new FormControl(this.model.Title, [
        Validators.required,
        Validators.maxLength(10)
      ]),
      WorkType: new FormControl(this.model.WorkType),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  initializeTable() {

    this.columns = [
      {
        text: '日期',
        name: 'DateStr'
      },
      {
        text: '名稱',
        name: 'Title',
      },
      {
        text: '類別',
        name: 'WorkType',
        customer: true
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true
      },
    ];

    this.columnsExport = [
      {
        text: '企業別',
        name: 'BuName'
      },
      {
        text: '日期',
        name: 'Date'
      }
    ]

  }


  private resetModel() {
    this.model.WorkType = null;
    this.model.DateStr = "";
    this.model.Title = "";

    // 清空 DatePicker UI
    this.datePicker.nativeElement.value = "";
  }

  private hasDateTime(date: any) {
    return this.createModel.some(x => x.DateStr == date);
  }

  private splitDateRange(dateRange: string) {

    if (!dateRange) return;

    let start = dateRange.split('-')[0].trim();
    let end = dateRange.split('-')[1].trim();

    // Usage
    var dates = this.getDates(new Date(start), new Date(end));

    return dates;
  }

  // 返回 範圍區間的所有時間
  private getDates(startDate, endDate) {
    var dates = [],
      currentDate = startDate,
      addDays = function (days) {
        var date = new Date(this.valueOf());
        date.setDate(date.getDate() + days);
        return date;
      };
    while (currentDate <= endDate) {
      dates.push(this.dateTimeToString(currentDate, "YYYY-MM-DD"));
      currentDate = addDays.call(currentDate, 1);
    }
    return dates;
  };

  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
  }
}
