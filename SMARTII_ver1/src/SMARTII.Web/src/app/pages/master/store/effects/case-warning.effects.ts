import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromCaseWarningActions from '../actions/case-warning.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { CaseWarningDetailViewModel, CaseWarningListViewModel } from '../../../../model/master.model';

@Injectable()
export class CaseWarningEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromCaseWarningActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        const retrieve$ = this.http.get<AspnetJsonResult<CaseWarningDetailViewModel>>('Master/CaseWarning/Get', payload);

        const handleSuccess = (result: AspnetJsonResult<CaseWarningDetailViewModel>) =>
          of(new fromCaseWarningActions.loadDetailSuccessAction(result.element));


        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromCaseWarningActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromCaseWarningActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  add$ = _entry$<CaseWarningDetailViewModel>(this.actions$,
    fromCaseWarningActions.ADD).pipe(
      exhaustMap((payload) => {

        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseWarning/Create',
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseWarningActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseWarningActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromCaseWarningActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/case-warning', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromCaseWarningActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<CaseWarningDetailViewModel>(this.actions$,
    fromCaseWarningActions.EDIT).pipe(
      exhaustMap((payload) => {

        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseWarning/Update',
          null,
          payload
        );
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseWarningActions.editSuccessAction(result.message));

        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseWarningActions.editFailedAction(result.message));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromCaseWarningActions.EDIT_SUCCESS).pipe(
      exhaustMap(() => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/case-warning', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromCaseWarningActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  disableRange$ = _entry$<EntrancePayload<Array<CaseWarningListViewModel>>>(this.actions$,
    fromCaseWarningActions.DISABLE_RANGE).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResultBase>('Master/CaseWarning/DisableRange', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCaseWarningActions.disableRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseWarningActions.disableRangeFailedAction(new ResultPayload<string>(
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
  disableRangeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseWarningActions.DISABLE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/master/case-warning', {});

        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseWarningActions.DISABLE_RANGE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );




  @Effect()
  disable$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromCaseWarningActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResultBase>('Master/CaseWarning/Disabled', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCaseWarningActions.disableSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseWarningActions.disableFailedAction(new ResultPayload<string>(
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
  disableSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseWarningActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/master/case-warning', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseWarningActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

  @Effect()
  order$ = _entry$<EntrancePayload<any[]>>(this.actions$,
    fromCaseWarningActions.ORDER).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseWarning/OrderBy',
          {},
          payload.data
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseWarningActions.orderSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseWarningActions.orderFailedAction(new ResultPayload<string>(
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
  orderSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseWarningActions.ORDER_SUCCESS).pipe(
      exhaustMap((payload) => {

        payload.cb && payload.cb();
        const popup$ = _success$('排序成功');
        return concat(popup$);

      })
    );


  @Effect()
  orderFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseWarningActions.ORDER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );



}
