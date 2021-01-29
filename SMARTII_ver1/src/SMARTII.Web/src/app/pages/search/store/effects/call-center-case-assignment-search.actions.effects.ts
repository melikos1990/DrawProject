


import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap, map } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { of } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';

import * as fromCallCenterCaseAssignmentSearchActions from '../actions/call-center-assignment-search.actions';
import { CaseCallCenterSearchViewModel } from 'src/app/model/search.model';
import { _failed$ } from 'src/app/shared/ngrx/alert.ngrx';
import * as moment from 'moment'

@Injectable()
export class CallCenterCaseAssignmentSearchEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions) { }

    @Effect()
    download$ = _entry$<CaseCallCenterSearchViewModel>(this.actions$,
        fromCallCenterCaseAssignmentSearchActions.CASE_CC_REPORT)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.download(
                    'Case/Case/GetExcelCaseAssignmentForCustomer', 'post', payload);

                const handleSuccess = (result: Blob) =>
                    of(new fromCallCenterCaseAssignmentSearchActions.callCenterReportSuccess(result));

                const handleFailed = (result: Response) =>
                    of(new fromCallCenterCaseAssignmentSearchActions.callCenterReportFailed(result.statusText));

                const consider = (result: any) => result.status == "200";

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    downloadFailed$ = _entry$<any>(this.actions$,
        fromCallCenterCaseAssignmentSearchActions.CASE_CC_REPORT_FAILED)
        .pipe(
            exhaustMap((payload) => {
                return _failed$(payload);
            })
        );


    @Effect()
    getList$ = _entry$<EntrancePayload<CaseCallCenterSearchViewModel>>(this.actions$,
        fromCallCenterCaseAssignmentSearchActions.CASE_CC_GETLIST)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.post('Case/Case/GetCaseAssignmentForCustomerList', null, payload.data);

                const handleSuccess = (result: AspnetJsonResult<any[]>) =>
                    of(new fromCallCenterCaseAssignmentSearchActions.callCenterGetListSuccess(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromCallCenterCaseAssignmentSearchActions.callCenterGetListFailed(new ResultPayload(
                        null,
                        payload.failed,
                        null,
                        result.message
                    )));


                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return work$;
            })

        );


    @Effect({
        dispatch: false
    })
    getListSuc$ = _entry$<ResultPayload<any[]>>(this.actions$,
        fromCallCenterCaseAssignmentSearchActions.CASE_CC_GETLIST_SUCCESS)
        .pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );

    @Effect()
    getListFailed$ = _entry$<ResultPayload<any>>(this.actions$,
        fromCallCenterCaseAssignmentSearchActions.CASE_CC_GETLIST_FAILED)
        .pipe(
            exhaustMap((payload) => {
                payload.cb && payload.cb();
                return _failed$(payload.msg);
            })
        );


    private saveFile(blob: Blob) {
        var a: any = document.createElement("a");
        document.body.appendChild(a);
        a.style = "display: none";

        var url = window.URL.createObjectURL(blob);
        a.href = url;
        a.download = `${moment().format("YYYYMMDDHHmmss")}_案件查詢(客服用).xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
    }

}
