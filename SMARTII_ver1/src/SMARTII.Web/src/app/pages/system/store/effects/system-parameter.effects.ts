import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { Store } from '@ngrx/store';

import * as fromSystemParameterActions from '../actions/system-parameter.actions';
import { State as fromSystemParameterReducer } from '../reducers/system-parameter.reducers';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { SystemParameterDetailViewModel, SystemParameterListViewModel } from 'src/app/model/system.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';

@Injectable()
export class SystemParameterEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromSystemParameterActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<SystemParameterDetailViewModel>>('System/SystemParameter/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<SystemParameterDetailViewModel>) =>
          of(new fromSystemParameterActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<SystemParameterDetailViewModel>) =>
          of(new fromSystemParameterActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromSystemParameterActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  add$ = _entry$<SystemParameterDetailViewModel>(this.actions$,
    fromSystemParameterActions.ADD).pipe(
      exhaustMap((payload: SystemParameterDetailViewModel) => {

        let formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'System/SystemParameter/Create',
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromSystemParameterActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/system/system-parameter', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromSystemParameterActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<SystemParameterDetailViewModel>(this.actions$,
    fromSystemParameterActions.EDIT).pipe(
      exhaustMap((payload: SystemParameterDetailViewModel) => {

        let formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'System/SystemParameter/Update',
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromSystemParameterActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/system/system-parameter', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromSystemParameterActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  delete$ = _entry$<EntrancePayload<{ ID: string, Key: string }>>(this.actions$,
    fromSystemParameterActions.DELETE).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<SystemParameterDetailViewModel>>('System/SystemParameter/Delete', payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.deleteSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.deleteFailedAction(new ResultPayload<string>(
            result.message,
            payload.failed
          )));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  deleteSuccess$ = _entry$<any>(this.actions$,
    fromSystemParameterActions.DELETE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$('刪除成功');
        payload.cb && payload.cb();
        return concat(popup$);
      })
    );


  @Effect()
  deleteFailed$ = _entry$<any>(this.actions$,
    fromSystemParameterActions.DELETE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload);
      })
    );




  @Effect()
  deleteRange$ = _entry$<EntrancePayload<Array<SystemParameterListViewModel>>>(this.actions$,
    fromSystemParameterActions.DELETE_RANGE).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<SystemParameterListViewModel>>('System/SystemParameter/DeleteRange', null, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.deleteRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromSystemParameterActions.deleteRangeFailedAction(new ResultPayload<string>(
            result.message,
            payload.failed
          )));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  deleteRangeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromSystemParameterActions.DELETE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        const popup$ = _success$('刪除成功');

        return concat(popup$);
      })
    );


  @Effect()
  deleteRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromSystemParameterActions.DELETE_RANGE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );





}
