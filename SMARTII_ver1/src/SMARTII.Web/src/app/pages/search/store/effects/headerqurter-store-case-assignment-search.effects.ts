import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { of } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';

import * as fromHeaderQurterStoreCaseAssignmentSearchActions from '../actions/headerqurter-store-assignment-search.actions';
import { CaseAssignmentHeaderqurterStoreSearchViewModel } from 'src/app/model/search.model';
import { _failed$ } from 'src/app/shared/ngrx/alert.ngrx';
import * as moment from 'moment'

@Injectable()
export class HeaderqurterStoreCaseAssignmentSearchEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions) { }


    @Effect()
    getList$ = _entry$<EntrancePayload<CaseAssignmentHeaderqurterStoreSearchViewModel>>(this.actions$,
        fromHeaderQurterStoreCaseAssignmentSearchActions.CASE_HS_GETLIST)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.post('Case/Case/GetCaseAssignmentForHSList', null, payload.data);

                const handleSuccess = (result: AspnetJsonResult<any[]>) =>
                    of(new fromHeaderQurterStoreCaseAssignmentSearchActions.headerqurterStoreGetListSuccess(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromHeaderQurterStoreCaseAssignmentSearchActions.headerqurterStoreGetListFailed(new ResultPayload(
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
        fromHeaderQurterStoreCaseAssignmentSearchActions.CASE_HS_GETLIST_SUCCESS)
        .pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );

    @Effect()
    getListFailed$ = _entry$<ResultPayload<any>>(this.actions$,
        fromHeaderQurterStoreCaseAssignmentSearchActions.CASE_HS_GETLIST_FAILED)
        .pipe(
            exhaustMap((payload) => {
                payload.cb && payload.cb();
                return _failed$(payload.msg);
            })
        );


    @Effect()
    download$ = _entry$<CaseAssignmentHeaderqurterStoreSearchViewModel>(this.actions$,
        fromHeaderQurterStoreCaseAssignmentSearchActions.CASE_HS_REPORT)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.download(
                    'Case/Case/GetExcelCaseAssignmentForHS', 'post', payload);

                const handleSuccess = (result: Blob) =>
                    of(new fromHeaderQurterStoreCaseAssignmentSearchActions.headerqurterStoreReportSuccess(result));

                const handleFailed = (result: Response) =>
                    of(new fromHeaderQurterStoreCaseAssignmentSearchActions.headerqurterStoreReportFailed(result.statusText));

                const consider = (result: any) => result.status == "200";

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );


    @Effect()
    downloadFailed$ = _entry$<any>(this.actions$,
        fromHeaderQurterStoreCaseAssignmentSearchActions.CASE_HS_REPORT_FAILED)
        .pipe(
            exhaustMap((payload) => {
                return _failed$(payload);
            })
        );
}
