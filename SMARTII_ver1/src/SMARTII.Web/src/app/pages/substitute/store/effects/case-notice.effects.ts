import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import * as fromCaseNoticeActions from '../actions/case-notice.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload, ActionType } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';

@Injectable()
export class CaseNoticeEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  notice$ = _entry$<EntrancePayload<{ caseID: string, ID: number }>>(this.actions$,
    fromCaseNoticeActions.NOTICE).pipe(
      exhaustMap((payload: EntrancePayload<{ caseID: string, ID: number }>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<number>>(
          'Substitute/CaseNotice/Notice', {
          ID: payload.data.ID
        }
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseNoticeActions.noticeSuccessAction(new ResultPayload<string>(
            payload.data.caseID,
            payload.success
          )));


        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseNoticeActions.noticeFailedAction(new ResultPayload<string>(
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
  noticeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseNoticeActions.NOTICE_SUCCESS).pipe(
      exhaustMap(payload => {

        const direct$ = _route$('./pages/case/case-create', {
          actionType: ActionType.Update,
          caseID: payload.data
        });

        return concat(direct$);
      })
    );


  @Effect()
  noticeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseNoticeActions.NOTICE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect()
  noticeRange$ = _entry$<EntrancePayload<number[]>>(this.actions$,
    fromCaseNoticeActions.NOTICE_RANGE).pipe(
      exhaustMap((payload: EntrancePayload<number[]>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<number[]>>(
          'Substitute/CaseNotice/NoticeRange', null, payload.data
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseNoticeActions.noticeRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));


        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseNoticeActions.noticeRangeFailedAction(new ResultPayload<string>(
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
  noticeRangeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseNoticeActions.NOTICE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        const popup$ = _success$('批次確認成功');

        return concat(popup$);
      })
    );


  @Effect()
  noticeRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseNoticeActions.NOTICE_RANGE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );
}
