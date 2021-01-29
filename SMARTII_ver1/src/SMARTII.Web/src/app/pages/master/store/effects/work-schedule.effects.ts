import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromWorkScheduleActions from '../actions/work-schedule.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, ResultPayload, EntrancePayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$, _failedExport$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { WorkScheduleDetailViewModel } from '../../../../model/master.model';

@Injectable()
export class WorkScheduleEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions) { }


  @Effect()
  createDetail$ = _entry$<EntrancePayload<WorkScheduleDetailViewModel[]>>(this.actions$,
    fromWorkScheduleActions.ADD_WORK_SCHEDULE)
    .pipe(
      exhaustMap(payload => {

        const retrieve$ = this.http.post<AspnetJsonResult<WorkScheduleDetailViewModel[]>>('Master/WorkSchedule/Create', null, payload.data);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.addWorkScheduleSuccess(result.message));


        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.addWorkScheduleFailed(new ResultPayload(
            result.extend,
            payload.failed,
            payload.dataExport,
            result.message
          )));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromWorkScheduleActions.ADD_WORK_SCHEDULE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$(payload);
        const router$ = _route$("./pages/master/work-schedule", {});
        return concat(popup$, router$);
      })
    );


  @Effect()
  addFailed$ = _entry$<ResultPayload<any>>(this.actions$,
    fromWorkScheduleActions.ADD_WORK_SCHEDULE_FAILED).pipe(
      exhaustMap(payload => {
        return _failedExport$(payload.msg, payload.dataExport, payload.data);
      })
    );


  @Effect()
  loadDetail$ = _entry$<{ id: any }>(this.actions$,
    fromWorkScheduleActions.LOAD_DETAIL)
    .pipe(
      exhaustMap(payload => {

        const retrieve$ = this.http.get<AspnetJsonResult<WorkScheduleDetailViewModel[]>>('Master/WorkSchedule/Get', payload);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.loadDetailSuccess(result.element));


        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.loadDetailFailed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFail$ = _entry$<string>(this.actions$,
    fromWorkScheduleActions.LOAD_DETAIL_FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })

    );

  @Effect()
  delete$ = _entry$<EntrancePayload<WorkScheduleDetailViewModel>>(this.actions$,
    fromWorkScheduleActions.DELETE_DETAIL)
    .pipe(
      exhaustMap(payload => {

        const retrieve$ = this.http.post<AspnetJsonResult<WorkScheduleDetailViewModel[]>>('Master/WorkSchedule/Delete', null, payload.data);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.success(new ResultPayload<any>(
            result.element,
            payload.success,
            null,
            result.message
          )));


        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.loadDetailFailed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  deleteRange$ = _entry$<EntrancePayload<WorkScheduleDetailViewModel[]>>(this.actions$,
    fromWorkScheduleActions.DELETE_DETAIL_RANGE)
    .pipe(
      exhaustMap(payload => {
        debugger;
        const retrieve$ = this.http.post<AspnetJsonResult<WorkScheduleDetailViewModel[]>>('Master/WorkSchedule/DeleteRange', null, payload.data);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.success(new ResultPayload<any>(
            result.element,
            payload.success,
            null,
            result.message
          )));


        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.failedShowDetail(new ResultPayload(
            result.extend,
            payload.failed,
            payload.dataExport,
            result.message
          )));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  success$ = _entry$<ResultPayload<any>>(this.actions$,
    fromWorkScheduleActions.SUCCESS)
    .pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();

        return _success$(payload.msg);
      })

    );

  @Effect()
  failed$ = _entry$<string>(this.actions$,
    fromWorkScheduleActions.FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })

    );

  @Effect()
  failedShowDetail$ = _entry$<ResultPayload<any>>(this.actions$,
    fromWorkScheduleActions.FAILED_SHOWDETAIL).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failedExport$(payload.msg, payload.dataExport, payload.data);
      })
    );

  @Effect()
  editDetail$ = _entry$<WorkScheduleDetailViewModel>(this.actions$,
    fromWorkScheduleActions.EDIT_WORK_SCHEDULE)
    .pipe(
      exhaustMap(payload => {

        const retrieve$ = this.http.post<AspnetJsonResult<WorkScheduleDetailViewModel>>('Master/WorkSchedule/Update', null, payload);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.editWorkScheduleSuccess(result.element));


        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromWorkScheduleActions.failed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );

  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromWorkScheduleActions.EDIT_WORK_SCHEDULE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$(payload);
        const router$ = _route$("./pages/master/work-schedule", {});
        return concat(popup$, router$);
      })
    );
}
