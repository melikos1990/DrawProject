import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { of } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';

import * as fromVendorCaseAssignmentSearchActions from '../actions/vendor-assignment-search.actions';
import { CaseAssignmentVendorSearchViewModel } from 'src/app/model/search.model';
import { _failed$ } from 'src/app/shared/ngrx/alert.ngrx';
import { HttpResponse } from '@angular/common/http';

@Injectable()
export class VendorCaseAssignmentSearchEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions) { }


    @Effect()
    getList$ = _entry$<EntrancePayload<CaseAssignmentVendorSearchViewModel>>(this.actions$,
        fromVendorCaseAssignmentSearchActions.CASE_VENDOR_GETLIST)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.post('Case/Case/GetCaseAssignmentForVendorList', null, payload.data);

                const handleSuccess = (result: AspnetJsonResult<any[]>) =>
                    of(new fromVendorCaseAssignmentSearchActions.vendorGetListSuccess(new ResultPayload(
                        result.element,
                        payload.success
                    )));

                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromVendorCaseAssignmentSearchActions.vendorGetListFailed(new ResultPayload(
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
        fromVendorCaseAssignmentSearchActions.CASE_VENDOR_GETLIST_SUCCESS)
        .pipe(
            tap((payload) => {
                payload.cb && payload.cb();
            })
        );

    @Effect()
    getListFailed$ = _entry$<ResultPayload<any>>(this.actions$,
        fromVendorCaseAssignmentSearchActions.CASE_VENDOR_GETLIST_FAILED)
        .pipe(
            exhaustMap((payload) => {
                payload.cb && payload.cb();
                return _failed$(payload.msg);
            })
        );


    @Effect()
    download$ = _entry$<CaseAssignmentVendorSearchViewModel>(this.actions$,
        fromVendorCaseAssignmentSearchActions.CASE_VENDOR_REPORT)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.download(
                    'Case/Case/GetExcelCaseAssignmentForVendor', 'post', payload);

                const handleSuccess = (result: Blob) =>
                    of(new fromVendorCaseAssignmentSearchActions.vendorReportSuccess(result));

                const handleFailed = (result: Response) =>
                    of(new fromVendorCaseAssignmentSearchActions.vendorReportFailed(result.statusText));

                const consider = (result: HttpResponse<Blob>) => {
                    return result.status === 200;
                }

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );


    @Effect()
    downloadFailed$ = _entry$<any>(this.actions$,
        fromVendorCaseAssignmentSearchActions.CASE_VENDOR_REPORT_FAILED)
        .pipe(
            exhaustMap((payload) => {
                return _failed$(payload);
            })
        );
}
