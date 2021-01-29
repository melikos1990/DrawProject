import { Effect, Actions } from '@ngrx/effects';
import { HttpService } from 'src/app/shared/service/http.service';
import { ObjectService } from 'src/app/shared/service/object.service';
import { Injectable } from '@angular/core';
import { AspnetJsonResult, EntrancePayload, AspnetJsonResultBase, ResultPayload } from 'src/app/model/common.model';
import * as fromPPCLifeEffectActions from '../actions/ppclife-effective.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { of, concat } from 'rxjs';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { NotificationGroupSenderExecuteViewModel, PPCLifeEffectiveCaseListViewModel, PPCLifeEffectiveListViewModel } from 'src/app/model/master.model';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';

@Injectable()
export class PPCLifrEffectiveEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }



  @Effect({ dispatch: false })
  clearNotification$ = _entry$<any>(this.actions$,
    fromPPCLifeEffectActions.CLEAR_NOTIFICATION).pipe(
      exhaustMap(() => {
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(`PPCLIFE/NotificationSender/ClearNotification`, {}, {});
        return retrieve$;
      })
    );



  @Effect()
  getCaseList$ = _entry$<number>(this.actions$,
    fromPPCLifeEffectActions.GET_CASE_LIST).pipe(
      exhaustMap(payload => {
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<PPCLifeEffectiveListViewModel[]>>(`PPCLIFE/NotificationSender/GetCaseList`, {
          effectiveID: payload
        }, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<PPCLifeEffectiveCaseListViewModel[]>) =>
          of(new fromPPCLifeEffectActions.getCaseListSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromPPCLifeEffectActions.getCaseListFailedAction(result.message));


        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  getCaseListFailed$ = _entry$<string>(this.actions$,
    fromPPCLifeEffectActions.GET_CASE_LIST_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  ignore$ = _entry$<EntrancePayload<PPCLifeEffectiveCaseListViewModel[]>>(this.actions$,
    fromPPCLifeEffectActions.IGNORE_CASE).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<PPCLifeEffectiveCaseListViewModel[]>(`PPCLIFE/NotificationSender/Disregard`, null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromPPCLifeEffectActions.ignoreSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromPPCLifeEffectActions.ignoreFailedAction(new ResultPayload<string>(
            result.element,
            payload.failed
          )));


        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResult<string>) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );

  @Effect()
  ignoreSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromPPCLifeEffectActions.IGNORE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('無視完成', () => {
          payload.cb();
        });
        return concat(popup$);
      })
    );


  @Effect()
  ignoreFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromPPCLifeEffectActions.IGNORE_FAIL).pipe(
      exhaustMap(payload => {

        return _failed$(payload.data, () => {
          payload.cb();
        });
      })
    );


  @Effect()
  noSend$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromPPCLifeEffectActions.NO_SEND).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(`PPCLIFE/NotificationSender/NoSend`, {
          EffectiveID: payload.data
        }, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromPPCLifeEffectActions.noSendSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromPPCLifeEffectActions.noSendFailedAction(new ResultPayload<string>(
            result.element,
            payload.failed
          )));


        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResult<string>) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  noSendSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromPPCLifeEffectActions.NO_SEND_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('不通知完成', () => {
          payload.cb();
        });
        return concat(popup$);
      })
    );


  @Effect()
  noSendFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromPPCLifeEffectActions.NO_SEND_FAIL).pipe(
      exhaustMap(payload => {

        return _failed$(payload.data, () => {
          payload.cb();
        });
      })
    );




  @Effect()
  send$ = _entry$<EntrancePayload<NotificationGroupSenderExecuteViewModel>>(this.actions$,
    fromPPCLifeEffectActions.SEND).pipe(
      exhaustMap(payload => {

        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(`PPCLIFE/NotificationSender/Send`, null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromPPCLifeEffectActions.sendSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromPPCLifeEffectActions.sendFailedAction(new ResultPayload<string>(
            result.message,
            payload.failed
          )));


        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResult<string>) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  sendSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromPPCLifeEffectActions.SEND_SUCCESS).pipe(
      exhaustMap(payload => {


        const popup$ = _success$('通知完成', () => {
          payload.cb();
        });
        return concat(popup$);
      })
    );


  @Effect()
  sendFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromPPCLifeEffectActions.SEND_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload.data, () => {
          payload.cb();
        });
      })
    );
}