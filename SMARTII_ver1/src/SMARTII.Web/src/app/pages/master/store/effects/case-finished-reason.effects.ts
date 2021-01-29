import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromCaseFinishedActions from '../actions/case-finished-reason.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat, empty } from 'rxjs';
import { _failed$, _success$, _prompt$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { CaseFinishDataDetailViewModel, CaseFinishClassificationDetailViewModel } from 'src/app/model/master.model';

@Injectable()
export class CaseFinishedReasonEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions) { }


    @Effect()
    loadDetail$ = _entry$<any>(this.actions$,
        fromCaseFinishedActions.LOAD_DETAIL)
        .pipe(
            exhaustMap((payload: any) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<CaseFinishDataDetailViewModel>>('Master/CaseFinishReason/Get', payload);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<CaseFinishDataDetailViewModel>) =>
                    of(new fromCaseFinishedActions.loadDetailSuccessAction(result.element));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<CaseFinishDataDetailViewModel>) =>
                    of(new fromCaseFinishedActions.loadDetailFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    loadDetailFailed$ = _entry$<string>(this.actions$,
        fromCaseFinishedActions.LOAD_DETAIL_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );


    @Effect()
    loadClassificationDetail$ = _entry$<any>(this.actions$,
        fromCaseFinishedActions.LOAD_CLASSIFICATION_DETAIL)
        .pipe(
            exhaustMap((payload: any) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<CaseFinishClassificationDetailViewModel>>('Master/CaseFinishReason/GetClassification', payload);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<CaseFinishClassificationDetailViewModel>) =>
                    of(new fromCaseFinishedActions.loadClassificationDetailSuccessAction(result.element));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<string>) =>
                    of(new fromCaseFinishedActions.loadClassificationDetailFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    loadClassificationDetailFailed$ = _entry$<string>(this.actions$,
        fromCaseFinishedActions.LOAD_CLASSIFICATION_DETAIL_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );


    @Effect()
    add$ = _entry$<CaseFinishDataDetailViewModel>(this.actions$,
        fromCaseFinishedActions.ADD).pipe(
            exhaustMap((payload) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/Create',
                    null,
                    payload
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.addSuccessAction(result.message));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.addFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    addSuccess$ = _entry$<string>(this.actions$,
        fromCaseFinishedActions.ADD_SUCCESS).pipe(
            exhaustMap(payload => {

                const popup$ = _success$('新增成功');
                const direct$ = _route$('./pages/master/case-finished-reason', {});

                return concat(popup$, direct$);
            })
        );


    @Effect()
    addFailed$ = _entry$<string>(this.actions$,
        fromCaseFinishedActions.ADD_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );

    @Effect()
    edit$ = _entry$<CaseFinishDataDetailViewModel>(this.actions$,
        fromCaseFinishedActions.EDIT).pipe(
            exhaustMap((payload: CaseFinishDataDetailViewModel) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/Update',
                    null,
                    payload
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.editSuccessAction(result.message));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.editFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    editSuccess$ = _entry$<string>(this.actions$,
        fromCaseFinishedActions.EDIT_SUCCESS).pipe(
            exhaustMap(() => {

                const popup$ = _success$('編輯成功');
                const direct$ = _route$('./pages/master/case-finished-reason', {});

                return concat(popup$, direct$);
            })
        );


    @Effect()
    editFailed$ = _entry$<string>(this.actions$,
        fromCaseFinishedActions.EDIT_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );

    @Effect()
    disabled$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
        fromCaseFinishedActions.DISABLED).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/Disabled', payload.data);


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.disabledSuccessAction(new ResultPayload<string>(
                        result.message,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.disabledFailedAction(new ResultPayload<string>(
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
    disabledSuccess$ = _entry$<any>(this.actions$,
        fromCaseFinishedActions.DISABLED_SUCCESS).pipe(
            exhaustMap(payload => {
                const popup$ = _success$('停用成功');
                payload.cb && payload.cb();
                return concat(popup$);
            })
        );


    @Effect()
    disabledFailed$ = _entry$<any>(this.actions$,
        fromCaseFinishedActions.DISABLED_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload);
            })
        );


    @Effect()
    addClassification$ = _entry$<EntrancePayload<CaseFinishClassificationDetailViewModel>>(this.actions$,
        fromCaseFinishedActions.ADD_CLASSIFICATION).pipe(
            exhaustMap((payload) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/CreateClassification',
                    null,
                    payload.data
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.addClassificationSuccessAction(new ResultPayload<string>(
                        result.message,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.addClassificationFailedAction(new ResultPayload<string>(
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
    addClassificationSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.ADD_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                const popup$ = _success$('新增成功');
                return concat(popup$);
            })
        );


    @Effect()
    addClassificationFailed$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.ADD_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload.data);
            })
        );

    @Effect()
    editClassification$ = _entry$<EntrancePayload<CaseFinishClassificationDetailViewModel>>(this.actions$,
        fromCaseFinishedActions.EDIT_CLASSIFICATION).pipe(
            exhaustMap((payload) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/UpdateClassification',
                    null,
                    payload.data
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.editClassificationSuccessAction(new ResultPayload<string>(
                        result.message,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.editClassificationFailedAction(new ResultPayload<string>(
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
    editClassificationSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.EDIT_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap((payload) => {
                payload.cb && payload.cb();
                const popup$ = _success$('編輯成功');
                return concat(popup$);
            })
        );


    @Effect()
    editClassificationFailed$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.EDIT_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload.data);
            })
        );




    @Effect()
    checkSingle$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
        fromCaseFinishedActions.CHECK_SINGLE).pipe(
            exhaustMap((payload) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/CheckSingle',
                    payload.data
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.checkSingleSuccessAction(new ResultPayload<{ isExist: boolean, message: string }>(
                        {
                            isExist: result.element,
                            message: result.message
                        },
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.checkSingleFailedAction(new ResultPayload<string>(
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
    checkSingleSuccess$ = _entry$<ResultPayload<{ isExist: boolean, message: string }>>(this.actions$,
        fromCaseFinishedActions.CHECK_SINGLE_SUCCESS).pipe(
            exhaustMap((payload) => {


                if (payload.data.isExist === true) {
                    const popup$ = _prompt$(`此類型已有預設選項 : 【${payload.data.message}】 是否覆蓋 ? `, () => {
                        payload.cb && payload.cb();
                    });
                    return concat(popup$);
                } else {

                    payload.cb && payload.cb();
                    return empty();
                }

            })
        );


    @Effect()
    checkSingleFailed$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.CHECK_SINGLE_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload.data);
            })
        );


    @Effect()
    order$ = _entry$<EntrancePayload<any[]>>(this.actions$,
        fromCaseFinishedActions.ORDER_DATA).pipe(
            exhaustMap((payload) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/OrderBy',
                    {},
                    payload.data
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.orderSuccessAction(new ResultPayload<string>(
                        result.message,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.orderFailedAction(new ResultPayload<string>(
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
        fromCaseFinishedActions.ORDER_DATA_SUCCESS).pipe(
            exhaustMap((payload) => {

                payload.cb && payload.cb();
                const popup$ = _success$('排序成功');
                return concat(popup$);

            })
        );


    @Effect()
    orderFailed$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.ORDER_DATA_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload.data);
            })
        );


    @Effect()
    orderClassification$ = _entry$<EntrancePayload<any[]>>(this.actions$,
        fromCaseFinishedActions.ORDER_CLASSIFICATION).pipe(
            exhaustMap((payload) => {


                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/CaseFinishReason/OrderByClassification',
                    {},
                    payload.data
                );

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.orderSuccessAction(new ResultPayload<string>(
                        result.message,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromCaseFinishedActions.orderFailedAction(new ResultPayload<string>(
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
    orderClassificationSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.ORDER_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap((payload) => {

                payload.cb && payload.cb();
                const popup$ = _success$('排序成功');
                return concat(popup$);

            })
        );


    @Effect()
    orderClassificationFailed$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseFinishedActions.ORDER_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload.data);
            })
        );




}
