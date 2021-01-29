import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';

import * as fromCaseRemindActions from '../actions/case-remind.actions';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { of, concat } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { _success$, _failed$, _prompt$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { CaseRemindDetailViewModel, CaseRemindListViewModel } from 'src/app/model/master.model';


@Injectable()
export class CaseRemindEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions) { }


  @Effect()
  loadDetail$ = _entry$<{ ID?: number }>(this.actions$,
    fromCaseRemindActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CaseRemindDetailViewModel>>(
          'Master/CaseRemind/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseRemindDetailViewModel>) =>
          of(new fromCaseRemindActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromCaseRemindActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  add$ = _entry$<CaseRemindDetailViewModel>(this.actions$,
    fromCaseRemindActions.ADD).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseRemind/Create',
          null,
          payload
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/case-remind', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<CaseRemindDetailViewModel>(this.actions$,
    fromCaseRemindActions.EDIT).pipe(
      exhaustMap((payload) => {

        console.log(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseRemind/Update',
          null,
          payload
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/case-remind', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  delete$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
    fromCaseRemindActions.DELETE).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
          'Master/CaseRemind/Delete', payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.deleteSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.deleteFailedAction(new ResultPayload<string>(
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
    fromCaseRemindActions.DELETE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$('刪除成功');
        payload.cb && payload.cb();
        return concat(popup$);
      })
    );


  @Effect()
  deleteFailed$ = _entry$<any>(this.actions$,
    fromCaseRemindActions.DELETE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload);
      })
    );




  @Effect()
  deleteRange$ = _entry$<EntrancePayload<Array<CaseRemindListViewModel>>>(this.actions$,
    fromCaseRemindActions.DELETE_RANGE).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<string>>(
          'Master/CaseRemind/DeleteRange', null, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.deleteRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.deleteRangeFailedAction(new ResultPayload<string>(
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
    fromCaseRemindActions.DELETE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        const popup$ = _success$('刪除成功');

        return concat(popup$);
      })
    );


  @Effect()
  deleteRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseRemindActions.DELETE_RANGE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect()
  confirm$ = _entry$<{ ID: number }>(this.actions$,
    fromCaseRemindActions.CONFIRM).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
          'Master/CaseRemind/Confirm',
          payload
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.confirmSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.confirmFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  confirmSuccess$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.CONFIRM_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('追蹤結束');
        const direct$ = _route$('./pages/master/case-remind', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  confirmFailed$ = _entry$<string>(this.actions$,
    fromCaseRemindActions.CONFIRM_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );
  @Effect()
  checkCaseID$ = _entry$<EntrancePayload<{ caseID: string }>>(this.actions$,
    fromCaseRemindActions.CHECK_CASE_ID).pipe(
      exhaustMap((payload) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
          'Master/CaseRemind/CheckCaseID',
          payload.data
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.checkCaseIDSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseRemindActions.checkCaseIDFailedAction(new ResultPayload<string>(
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
  checkCaseIDSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseRemindActions.CHECK_CASE_ID_SUCCESS).pipe(
      exhaustMap((payload) => {

        payload.cb && payload.cb();

        return _success$("查詢成功，開啟新頁");
      })
    );


  @Effect()
  checkCaseIDFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseRemindActions.CHECK_CASE_ID_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );


}
