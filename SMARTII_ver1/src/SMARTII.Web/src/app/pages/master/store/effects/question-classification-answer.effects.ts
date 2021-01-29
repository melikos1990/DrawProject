import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';

import * as fromQuestionClassificationAnswerAction from '../actions/question-classification-answer.actions';

import { exhaustMap, tap, map, flatMap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { of, concat } from 'rxjs';


import { QuestionClassificationAnswerDetail, QuestionClassificationAnswerViewModel } from 'src/app/model/question-category.model'

// customer service
import { HttpService } from 'src/app/shared/service/http.service';

//customer object

import { AspnetJsonResult, AspnetJsonResultBase, ResultPayload, EntrancePayload } from 'src/app/model/common.model';

// customer ngrx obsv
import { _httpflow$, _isHttpSuccess$ } from 'src/app/shared/ngrx/http.ngrx';
import { _unLoading$, _loadingWork$, _loading$ } from 'src/app/shared/ngrx/loading.ngrx'
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx'
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';


@Injectable()
export class QuestionClassificationAnswerEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions
  ) { }


  @Effect()
  createDetail$ = _entry$<QuestionClassificationAnswerDetail[]>(this.actions$,
    fromQuestionClassificationAnswerAction.CREATE_DETAIL)
    .pipe(
      exhaustMap((payload) => {
        
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<QuestionClassificationAnswerDetail[]>>('Master/QuestionClassificationAnswer/Create', null, payload);

        const handleSuccess$ = (result: AspnetJsonResult<string>) =>
          of(new fromQuestionClassificationAnswerAction.SuccessAction(result.element));

        const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromQuestionClassificationAnswerAction.FailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  editDetail$ = _entry$<QuestionClassificationAnswerViewModel>(this.actions$,
    fromQuestionClassificationAnswerAction.EDIT_DETAIL)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<QuestionClassificationAnswerViewModel>>('Master/QuestionClassificationAnswer/Update', null, payload);

        const handleSuccess$ = (result: AspnetJsonResult<string>) =>
          of(new fromQuestionClassificationAnswerAction.EditSuccessAction(result.element));

        const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromQuestionClassificationAnswerAction.EditFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      })

    )



    @Effect()
    editSuccess$ = _entry$<string>(this.actions$,
      fromQuestionClassificationAnswerAction.EDIT_SUCCESS)
      .pipe(
        exhaustMap(payload => {
          const popup$ = _success$('編輯成功');
          const direct$ = _route$('./pages/master/question-classification-answer', {});
  
          return concat(popup$, direct$);
        })
      )
  
  
    @Effect()
    editFailed$ = _entry$<string>(this.actions$,
      fromQuestionClassificationAnswerAction.EDIT_FAILED)
      .pipe(
        exhaustMap(payload => {
          return _failed$(payload)
        })
      )


  @Effect()
  getDetail$ = _entry$<string>(this.actions$,
    fromQuestionClassificationAnswerAction.GET_DETAIL)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<QuestionClassificationAnswerDetail>>('Master/QuestionClassificationAnswer/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess$ = (result: AspnetJsonResult<QuestionClassificationAnswerDetail>) =>
          of(new fromQuestionClassificationAnswerAction.GetDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromQuestionClassificationAnswerAction.FailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      })

    )

  // @Effect()
  // editOrder$ = _entry$<string>(this.actions$,
  //   fromQuestionClassificationAnswerAction.EDIT_ORDER)
  //   .pipe(
  //     exhaustMap((payload: any) => {

  //       // 主要要做的事情 , 這邊是透過 http client 撈取資料
  //       const retrieve$ = this.http.post<AspnetJsonResult<any>>('Master/QuestionClassificationAnswer/EditOrder', null, payload);

  //       const handleSuccess$ = (result: AspnetJsonResult<any>) =>
  //         of(new fromQuestionClassificationAnswerAction.SuccessAction(result.element));

  //       const handleFailed$ = (result: AspnetJsonResult<any>) =>
  //         of(new fromQuestionClassificationAnswerAction.FailedAction(result.message));

  //       // 判斷是否成功或是失敗
  //       const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

  //       // 實際進行http 行為
  //       const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

  //       return _loadingWork$(work$);
  //     })

  //   )


  // @Effect()
  // deteleRange$ = _entry$<string>(this.actions$,
  //   fromQuestionClassificationAnswerAction.DETELE_RANGE_DETAIL)
  //   .pipe(
  //     exhaustMap((payload: any) => {
  //       console.log("payload => ", payload);
  //       // 主要要做的事情 , 這邊是透過 http client 撈取資料
  //       const retrieve$ = this.http.post<AspnetJsonResult<any>>('Master/QuestionClassificationAnswer/DisableRanage', null, payload.data);

  //       const handleSuccess$ = (result: AspnetJsonResult<any>) =>
  //         of(new fromQuestionClassificationAnswerAction.DeleteSuccessAction(new ResultPayload<string>(
  //           result.message,
  //           payload.success
  //         )));

  //       const handleFailed$ = (result: AspnetJsonResult<any>) =>
  //         of(new fromQuestionClassificationAnswerAction.FailedAction(result.message));

  //       // 判斷是否成功或是失敗
  //       const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

  //       // 實際進行http 行為
  //       const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

  //       return _loadingWork$(work$);
  //     })

  //   )

  @Effect()
  detele$ = _entry$<EntrancePayload<{id : number}>>(this.actions$,
    fromQuestionClassificationAnswerAction.DETELE_DETAIL)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<EntrancePayload<{id : number}>>>('Master/QuestionClassificationAnswer/Delete', payload.data);

        const handleSuccess$ = (result: AspnetJsonResult<string>) =>
          of(new fromQuestionClassificationAnswerAction.DeleteSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromQuestionClassificationAnswerAction.FailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  deteleSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromQuestionClassificationAnswerAction.DELETE_SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('刪除成功');

        payload.cb && payload.cb();

        return concat(popup$);
      })
    )



  @Effect()
  success$ = _entry$<string>(this.actions$,
    fromQuestionClassificationAnswerAction.SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/question-classification-answer', {});

        return concat(popup$, direct$);
      })
    )


  @Effect()
  failed$ = _entry$<string>(this.actions$,
    fromQuestionClassificationAnswerAction.FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload)
      })
    )

  @Effect()
  checkQuestionCategory$ = _entry$<EntrancePayload<{BuID? : number}>>(this.actions$,
    fromQuestionClassificationAnswerAction.CHECK_QUESTION_CATEGORY)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<EntrancePayload<{BuID? : number}>>>('Master/QuestionClassificationAnswer/CheckQuestionCategory', payload.data);        

        const handleSuccess$ = (result: AspnetJsonResult<boolean>) =>
          of(new fromQuestionClassificationAnswerAction.CheckSuccessAction(new ResultPayload<boolean>(
            result.element,
            payload.success
          )));

          const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromQuestionClassificationAnswerAction.FailedAction('取得bu問題分類資料發生錯誤'));


        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      }))


  @Effect({dispatch: false})
  checkSuccess$ = _entry$<ResultPayload<boolean>>(this.actions$,
    fromQuestionClassificationAnswerAction.CHECK_SUCCESS)
    .pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);
      })
    )

}
