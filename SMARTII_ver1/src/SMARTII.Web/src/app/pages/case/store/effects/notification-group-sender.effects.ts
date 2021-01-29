import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';

import * as fromNotificationGroupSenderActions from '../actions/notification-group-sender.actions';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { of, concat } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { _success$, _failed$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { NotificationGroupUserListViewModel, NotificationGroupSenderExecuteViewModel } from 'src/app/model/master.model';
import { ObjectService } from 'src/app/shared/service/object.service';


@Injectable()
export class NotificationGroupSenderEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }



  @Effect({ dispatch: false })
  clearNotification$ = _entry$<any>(this.actions$,
    fromNotificationGroupSenderActions.CLEAR_NOTIFICATION).pipe(
      exhaustMap(() => {
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(`Master/NotificationGroupSender/ClearNotification`, {}, {});
        return retrieve$;
      })
    );



  @Effect()
  getUserList$ = _entry$<number>(this.actions$,
    fromNotificationGroupSenderActions.GET_USER_LIST).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<NotificationGroupUserListViewModel[]>>(`Master/NotificationGroupSender/GetUserList`, {
          groupID: payload
        }, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<NotificationGroupUserListViewModel[]>) =>
          of(new fromNotificationGroupSenderActions.getUserListSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNotificationGroupSenderActions.getUserListFailedAction(result.message));


        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  getUserListFailed$ = _entry$<string>(this.actions$,
    fromNotificationGroupSenderActions.GET_USER_LIST_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  noSend$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromNotificationGroupSenderActions.NO_SEND).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(`Master/NotificationGroupSender/NoSend`, {
          groupID: payload.data
        }, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromNotificationGroupSenderActions.noSendSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNotificationGroupSenderActions.noSendFailedAction(new ResultPayload<string>(
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
  noSendSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNotificationGroupSenderActions.NO_SEND_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('不通知完成', () => {
          payload.cb();
        });
        return concat(popup$);
      })
    );


  @Effect()
  noSendFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNotificationGroupSenderActions.NO_SEND_FAIL).pipe(
      exhaustMap(payload => {

        return _failed$(payload.data, () => {
          payload.cb();
        });
      })
    );




  @Effect()
  send$ = _entry$<EntrancePayload<NotificationGroupSenderExecuteViewModel>>(this.actions$,
    fromNotificationGroupSenderActions.SEND).pipe(
      exhaustMap(payload => {

        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(`Master/NotificationGroupSender/Send`, null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromNotificationGroupSenderActions.sendSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNotificationGroupSenderActions.sendFailedAction(new ResultPayload<string>(
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
  sendSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNotificationGroupSenderActions.SEND_SUCCESS).pipe(
      exhaustMap(payload => {


        const popup$ = _success$('通知完成', () => {
          payload.cb();
        });
        return concat(popup$);
      })
    );


  @Effect()
  sendFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNotificationGroupSenderActions.SEND_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload.data, () => {
          payload.cb();
        });
      })
    );
}
