import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { of } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';

import * as fromCallCenterCaseSearchActions from '../actions/call-center-case-search.actions';
import { CaseCallCenterSearchViewModel } from 'src/app/model/search.model';
import { _failed$ } from 'src/app/shared/ngrx/alert.ngrx';
import * as moment from 'moment'

@Injectable()
export class CallCenterCaseSearchEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions) { }



    @Effect()
    download$ = _entry$<CaseCallCenterSearchViewModel>(this.actions$,
        fromCallCenterCaseSearchActions.CASE_CC_REPORT)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.download(
                    'Case/Case/GetExcelCaseForCustomer', 'post', payload);

                const handleSuccess = (result: Blob) => of(new fromCallCenterCaseSearchActions.callCenterReportSuccess(result));
                

                const handleFailed = (result: Response) => of(new fromCallCenterCaseSearchActions.callCenterReportFailed(result.statusText));
                

                const consider = (result: any) => result.status == "200";
                

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    downloadFailed$ = _entry$<any>(this.actions$,
        fromCallCenterCaseSearchActions.CASE_CC_REPORT_FAILED)
        .pipe(
            exhaustMap((payload) => {
                return _failed$(payload);
            })
        );


    @Effect()
    getList$ = _entry$<EntrancePayload<CaseCallCenterSearchViewModel>>(this.actions$,
        fromCallCenterCaseSearchActions.CASE_CC_GETLIST)
        .pipe(
            exhaustMap((payload) => {
                
                console.log("GetCaseForCustomerList =>", payload.data);

                const retrieve$ = this.http.post('Case/Case/GetCaseForCustomerList', null, payload.data);

                const handleSuccess = (result: AspnetJsonResult<any[]>) =>
                    of(new fromCallCenterCaseSearchActions.callCenterGetListSuccess(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromCallCenterCaseSearchActions.callCenterGetListFailed(new ResultPayload(
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
        fromCallCenterCaseSearchActions.CASE_CC_GETLIST_SUCCESS)
        .pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );

    @Effect()
    getListFailed$ = _entry$<ResultPayload<any>>(this.actions$,
        fromCallCenterCaseSearchActions.CASE_CC_GETLIST_FAILED)
        .pipe(
            exhaustMap((payload) => {
                payload.cb && payload.cb();
                return _failed$(payload.msg);
            })
        );

}
