import { Injectable } from "@angular/core";
import { ObjectService } from 'src/app/shared/service/object.service';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { Effect, Actions } from '@ngrx/effects';

import * as fromEmailAdoptActions from '../actions/official-email-adopt.actions';
import { OfficialEmailAdoptResult, OfficialEmailAutoOrderViewModel, OfficialEmailAdminOrderViewModel, OfficialEmailReplyRengeViewModel, OfficialEmailListViewModel, OfficialEmailBatchAdoptViewModel, OfficialEmailAdoptViewModel } from 'src/app/model/case.model';
import { flatMap, exhaustMap, tap } from 'rxjs/operators';
import { HttpService } from 'src/app/shared/service/http.service';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload, ActionType } from 'src/app/model/common.model';
import { of, concat } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';

@Injectable()
export class OfficialEmailAdoptEffects {

    constructor(
        private actions$: Actions,
        private http: HttpService,
    ) { }

    @Effect()
    autoAssignEmail$ = _entry$<EntrancePayload<OfficialEmailAutoOrderViewModel>>(this.actions$,
        fromEmailAdoptActions.AUTO_ASSIGN_EMAIL)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<OfficialEmailAutoOrderViewModel>>('Case/OfficialEmail/AutoOrder', null, payload.data);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<OfficialEmailAdoptResult<any>>) =>
                    of(new fromEmailAdoptActions.successShowInfo(new ResultPayload<OfficialEmailAdoptResult<any>>(
                        result.element,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromEmailAdoptActions.fail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );

    @Effect()
    adminAssignEmail$ = _entry$<EntrancePayload<OfficialEmailAdminOrderViewModel>>(this.actions$,
        fromEmailAdoptActions.ADMIN_ASSIGN_EMAIL)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<OfficialEmailAdminOrderViewModel>>('Case/OfficialEmail/AdminOrder', null, payload.data);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<OfficialEmailAdoptResult<any>>) =>
                    of(new fromEmailAdoptActions.successShowInfo(new ResultPayload<OfficialEmailAdoptResult<any>>(
                        result.element,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromEmailAdoptActions.fail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );

    @Effect()
    replyEmail$ = _entry$<EntrancePayload<OfficialEmailReplyRengeViewModel>>(this.actions$,
        fromEmailAdoptActions.REPLY_EMAIL)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<OfficialEmailReplyRengeViewModel>>('Case/OfficialEmail/ReplyRenge', null, payload.data);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<OfficialEmailAdoptResult<any>>) =>
                    of(new fromEmailAdoptActions.successShowInfo(new ResultPayload<OfficialEmailAdoptResult<any>>(
                        result.element,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromEmailAdoptActions.fail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );

    @Effect()
    adoptEmail$ = _entry$<EntrancePayload<OfficialEmailAdoptViewModel>>(this.actions$,
        fromEmailAdoptActions.ADOPT_EMAIL)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<string>>('Case/OfficialEmail/Adopt', null, payload.data);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<any>) =>
                    of(new fromEmailAdoptActions.adoptEmailCaseSuccess(new ResultPayload<any>(
                        result.element,
                        payload.success,
                        null,
                        result.message
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromEmailAdoptActions.fail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );

    @Effect()
    deleteRangeEmail$ = _entry$<EntrancePayload<OfficialEmailListViewModel[]>>(this.actions$,
        fromEmailAdoptActions.DELETE_RANGE_EMAIL)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<OfficialEmailListViewModel[]>>('Case/OfficialEmail/DeleteRange', null, payload.data);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<OfficialEmailAdoptResult<any>>) =>
                    of(new fromEmailAdoptActions.success(new ResultPayload<OfficialEmailAdoptResult<any>>(
                        result.element,
                        payload.success,
                        null,
                        result.message
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromEmailAdoptActions.fail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );

    @Effect()
    fail$ = _entry$<string>(this.actions$,
        fromEmailAdoptActions.ADOPT_EMAIL_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );

    @Effect()
    success$ = _entry$<ResultPayload<any>>(this.actions$,
        fromEmailAdoptActions.ADOPT_EMAIL_CASE_SUCCESS).pipe(
            exhaustMap(payload => {
                const popup$ = _success$('認養成功');
                const direct$ = _route$('./pages/case/case-create', {
                    actionType: ActionType.Update,
                    caseID: payload.data
                });
                return concat(popup$, direct$);
            })
        );


    @Effect()
    adoptEmailCaseSuccess$ = _entry$<ResultPayload<any>>(this.actions$,
        fromEmailAdoptActions.ADOPT_EMAIL_SUCCESS).pipe(
            exhaustMap(payload => {

                payload.cb && payload.cb();

                return _success$(payload.msg);
            })
        );

    @Effect()
    successShowInfo$ = _entry$<ResultPayload<any>>(this.actions$,
        fromEmailAdoptActions.ADOPT_EMAIL_SUCCESS_SHOWINFO).pipe(
            exhaustMap(payload => {
                let element = payload.data;
                let mesg = `成功: ${element.SuccessCount} 失敗: ${element.FailCount}`;

                payload.cb && payload.cb();

                return _success$(mesg);
            })
        );

    @Effect()
    batchAdoptEmail$ = _entry$<EntrancePayload<OfficialEmailBatchAdoptViewModel>>(this.actions$,
        fromEmailAdoptActions.BATCH_ADOPT_EMAIL)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                //const retrieve$ = this.http.post<AspnetJsonResult<OfficialEmailListViewModel[]>>('Case/OfficialEmail/DeleteRange', null, payload.data);

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<OfficialEmailBatchAdoptViewModel>>('Case/OfficialEmail/BatchAdopt', null, payload.data);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<OfficialEmailAdoptResult<any>>) =>
                    of(new fromEmailAdoptActions.success(new ResultPayload<OfficialEmailAdoptResult<any>>(
                        result.element,
                        payload.success,
                        null,
                        result.message
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromEmailAdoptActions.fail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


}
