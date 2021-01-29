
import { Actions, Effect } from '@ngrx/effects';
import { ObjectService } from 'src/app/shared/service/object.service';
import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';

import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { _failed$ } from 'src/app/shared/ngrx/alert.ngrx';

import * as fromCaseAssignmentActions from "../actions/case-assignment.action";
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, EntrancePayload, ResultPayload, AspnetJsonResultBase } from 'src/app/model/common.model';
import { CaseAssignmentViewModel } from 'src/app/model/case.model';
import { of } from 'rxjs';

@Injectable()
export class CaseAssignmentEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions,
        private objectService: ObjectService) { }


    @Effect()
    editCaseAssignmnetAction$ = _entry$<EntrancePayload<CaseAssignmentViewModel>>(this.actions$,
        fromCaseAssignmentActions.EDIT_CASEASSIGNMENT)
        .pipe(
            exhaustMap((payload) => {

                const formData = this.objectService.convertToFormData(payload.data);

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/CaseAssignment/SaveCaseAssignment', null, formData);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<CaseAssignmentViewModel>) =>
                    of(new fromCaseAssignmentActions.editCaseAssignmentSuccessAction(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromCaseAssignmentActions.editCaseAssignmentFailedAction(new ResultPayload(
                        result.message,
                        payload.success
                    )));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    editCaseAssignmnetFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseAssignmentActions.EDIT_CASEASSIGNMENT_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload.data);
            })
        );

    @Effect({
        dispatch: false
    })
    editCaseAssignmnetSuccessAction$ = _entry$<ResultPayload<CaseAssignmentViewModel>>(this.actions$,
        fromCaseAssignmentActions.EDIT_CASEASSIGNMENT_SUCCESS).pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );

    @Effect()
    refillCaseAssignmnetAction$ = _entry$<EntrancePayload<CaseAssignmentViewModel>>(this.actions$,
        fromCaseAssignmentActions.REFILL_CASEASSIGNMENT)
        .pipe(
            exhaustMap((payload) => {

                const formData = this.objectService.convertToFormData(payload.data);

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/CaseAssignment/SaveRefill', null, formData);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<CaseAssignmentViewModel>) =>
                    of(new fromCaseAssignmentActions.refillCaseAssignmentSuccessAction(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromCaseAssignmentActions.refillCaseAssignmentFailedAction(new ResultPayload(
                        result.message,
                        payload.success
                    )));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    refillCaseAssignmnetFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseAssignmentActions.REFILL_CASEASSIGNMENT_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload.data);
            })
        );

    @Effect({
        dispatch: false
    })
    refillCaseAssignmnetSuccessAction$ = _entry$<ResultPayload<CaseAssignmentViewModel>>(this.actions$,
        fromCaseAssignmentActions.REFILL_CASEASSIGNMENT_SUCCESS).pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );
    @Effect()
    processedCaseAssignmnetAction$ = _entry$<EntrancePayload<CaseAssignmentViewModel>>(this.actions$,
        fromCaseAssignmentActions.PROCESSED_CASEASSIGNMENT)
        .pipe(
            exhaustMap((payload) => {

                const formData = this.objectService.convertToFormData(payload.data);

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/CaseAssignment/ProcessedCaseAssignment', null, formData);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<CaseAssignmentViewModel>) =>
                    of(new fromCaseAssignmentActions.processedCaseAssignmentSuccessAction(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromCaseAssignmentActions.processedCaseAssignmentFailedAction(new ResultPayload(
                        result.message,
                        payload.success
                    )));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    processedCaseAssignmnetFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
        fromCaseAssignmentActions.PROCESSED_CASEASSIGNMENT_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload.data);
            })
        );

    @Effect({
        dispatch: false
    })
    processedCaseAssignmnetSuccessAction$ = _entry$<ResultPayload<CaseAssignmentViewModel>>(this.actions$,
        fromCaseAssignmentActions.PROCESSED_CASEASSIGNMENT_SUCCESS).pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );
}


