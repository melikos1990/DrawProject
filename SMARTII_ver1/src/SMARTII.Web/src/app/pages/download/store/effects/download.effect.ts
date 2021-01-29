import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';

import * as downloadActions from '../actions/download.actions';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { of } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';


@Injectable()
export class DownloadEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions) { }


    @Effect()
    asodownload$ = _entry$<any>(this.actions$,
        downloadActions.DOWNLOAD_ASO_REPORT)
        .pipe(
            exhaustMap((payload) => {

                const second = 180;
                let header = new HttpHeaders({ timeout: `${second * 1000}` });

                const retrieve$ = this.http.download(`${payload.providerKey}/Report/GetAsoReport`, "get", payload.params, header );

                const handleSuccess = (result: Response) => of(new downloadActions.downloadReportSuccess(result.statusText));

                const handleFailed = (result: Response) => of(new downloadActions.downloadReportFailed(result.statusText));


                const consider = (result: any) => result.status == "200";


                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    download$ = _entry$<any>(this.actions$,
        downloadActions.DOWNLOAD_REPORT)
        .pipe(
            exhaustMap((payload) => {

                const second = 180;
                let header = new HttpHeaders({ timeout: `${second * 1000}` });

                const retrieve$ = this.http.download(`${payload.providerKey}/Report/GetReport`, "get", payload.params, header);
                
                const handleSuccess = (result: Response) => of(new downloadActions.downloadReportSuccess(result.statusText));

                const handleFailed = (result: Response) => of(new downloadActions.downloadReportFailed(result.statusText));


                const consider = (result: any) => result.status == "200";


                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );


    @Effect()
    downloadSuccess$ = _entry$<any>(this.actions$,
        downloadActions.DOWNLOAD_REPORT_SUCCESS)
        .pipe(
            exhaustMap((payload) => {
                return _success$(payload);
            })
        );

    @Effect()
    downloadFailed$ = _entry$<any>(this.actions$,
        downloadActions.DOWNLOAD_REPORT_FAILED)
        .pipe(
            exhaustMap((payload) => {
                return _failed$(payload);
            })
        );


}