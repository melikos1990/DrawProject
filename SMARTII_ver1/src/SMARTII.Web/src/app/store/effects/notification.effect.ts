import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { Actions, Effect } from '@ngrx/effects';
import * as fromRootAction from '../actions';
import * as fromNotificationAction from '../actions/notification.actions';
import { exhaustMap, map, concatMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, ResultPayload, AspnetJsonResultBase, EntrancePayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { NotficationCalcViewModel } from 'src/app/model/notification.model';
import { of, concat } from 'rxjs';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx'
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';



@Injectable()
export class NotificationEffects {

    constructor(
        public http: HttpService,
        public actions$: Actions,
        public authenticationService: AuthenticationService
    ) { }


    @Effect()
    removePersonalNotification$ = _entry$<{ id: number }>(this.actions$,
        fromNotificationAction.REMOVE_PERSONAL_NOTIFICAIOTN)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<{ id: number }>>('Common/Notification/DeletePersonal', payload);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResultBase) => of(new fromNotificationAction.removePersonalNotificationSuccess());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromNotificationAction.removePersonalNotificationFail(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return work$;
            })
        );


    @Effect()
    clearNotice$ = _entry$<{ userID: string }>(this.actions$,
        fromNotificationAction.REMOVEALL_PERSONAL_NOTIFICAIOTN)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<{ userID: string }>>('Common/Notification/DeleteAllPersonal', payload);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess$ = (result: AspnetJsonResult<NotficationCalcViewModel>) => 
                of(new fromNotificationAction.clearAllSuccessAction());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed$ = (result: AspnetJsonResult<NotficationCalcViewModel>) =>
                    of(new fromNotificationAction.clearAllFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider);

                return work$;
            })
        );

    @Effect()
    clearAllSuccess$ = _entry$<string>(this.actions$,
        fromNotificationAction.REMOVEALL_PERSONAL_SUCCESS).pipe(
            exhaustMap(payload => {
                 const popup$ = _success$('清除成功');
                 //const direct$ = _route$('./pages/home/home-page', {});
                 let result$ = this.authenticationService.getCompleteUser()
                    .pipe(
                        concatMap(user => of(new fromNotificationAction.getNotificationCount({ userID: user.UserID })))
                    );
               
                 return concat(popup$, result$);
            })
        )


    @Effect()
    clearAllFailed$ = _entry$<string>(this.actions$,
        fromNotificationAction.REMOVEALL_PERSONAL_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        )

    @Effect()
    removeNotificationSuccess$ = _entry$<string>(this.actions$,
        fromNotificationAction.REMOVE_PERSONAL_NOTIFICAIOTN_SUCCESS).pipe(
            exhaustMap(payload => {
                let result$ = this.authenticationService.getCompleteUser()
                    .pipe(
                        concatMap(user => of(new fromNotificationAction.getNotificationCount({ userID: user.UserID })))
                    );

                return result$;
            })
        );

    @Effect()
    removeNotificationFail$ = _entry$<string>(this.actions$,
        fromNotificationAction.REMOVE_PERSONAL_NOTIFICAIOTN_FAIL).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );

    @Effect()
    getNotificationCount$ = _entry$<{ userID: string }>(this.actions$,
        fromNotificationAction.GET_NOTIFICATION_COUNT)
        .pipe(
            exhaustMap((payload) => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<{ userID: string }>>('Common/Notification/GetNotificationCount', payload);

                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<NotficationCalcViewModel>) =>
                    of(new fromNotificationAction.getNotificationCountSuccess(result.element));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<NotficationCalcViewModel>) =>
                    of(new fromNotificationAction.getNotificationCountFailed(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return work$;
            })
        );

    @Effect({
        dispatch: false
    })
    getNotificationCountFailed$ = _entry$<string>(this.actions$,
        fromNotificationAction.GET_NOTIFICATION_COUNT_FAILED).pipe(
            tap(payload => {
                // return _failed$(payload);
            })
        );

}
