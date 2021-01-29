import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';

import { State as fromQuestionCategoryReducer } from '../reducers/question-category.reducers'
import * as fromQuestionCategoryAction from '../actions/question-category.actions';

import { exhaustMap, tap, map } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { of, concat } from 'rxjs';


import { QuestionCategoryDetail } from 'src/app/model/question-category.model'

// customer service
import { HttpService } from 'src/app/shared/service/http.service';

//customer object

import { AspnetJsonResult, AspnetJsonResultBase, ResultPayload } from 'src/app/model/common.model';

// customer ngrx obsv
import { _httpflow$, _isHttpSuccess$ } from 'src/app/shared/ngrx/http.ngrx';
import { _unLoading$, _loadingWork$, _loading$ } from 'src/app/shared/ngrx/loading.ngrx'
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx'
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { QuestionClassificationSearchViewModel } from 'src/app/model/question-category.model';



@Injectable()
export class QuestionCategoryEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions
  ) { }


  @Effect()
  createDetail$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.CREATE_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<any>>('Master/QuestionClassification/Create', null, payload);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.Success(result.element));

        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.Failed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  editDetail$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.EDIT_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<any>>('Master/QuestionClassification/Update', null, payload);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.EditSuccess(result.element));

        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.EditFailed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  editSuccess$ = _entry$<ResultPayload<any>>(this.actions$,
    fromQuestionCategoryAction.EDIT_SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/question-category', {});

        return concat(popup$, direct$);
      })
    )


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.EDIT_FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload)
      })
    )



  @Effect()
  getDetail$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.GET_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<QuestionCategoryDetail>>('Master/QuestionClassification/Get', payload);

        const handleSuccess = (result: AspnetJsonResult<QuestionCategoryDetail>) =>
          of(new fromQuestionCategoryAction.GetDetailSuccess(result.element));

        const handleFailed = (result: AspnetJsonResult<QuestionCategoryDetail>) =>
          of(new fromQuestionCategoryAction.Failed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  editOrder$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.EDIT_ORDER)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<any>>('Master/QuestionClassification/EditOrder', null, payload);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.EditOrderSuccess(result.element));

        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.EditOrderFailed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    )




  @Effect()
  editOrderSuccess$ = _entry$<ResultPayload<any>>(this.actions$,
    fromQuestionCategoryAction.EDIT_ORDER_SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('排序成功');
        const direct$ = _route$('./pages/master/question-category', {});

        return concat(popup$, direct$);
      })
    )


  @Effect()
  editOrderFailed$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.EDIT_ORDER_FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload)
      })
    )


  @Effect()
  deteleRange$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.DETELE_RANGE_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {
        console.log("payload => ", payload);
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<any>>('Master/QuestionClassification/DisableRanage', null, payload.data);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.DeleteSuccess(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.Failed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  detele$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.DETELE_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<any>>('Master/QuestionClassification/Disable', payload.data);

        const handleSuccess = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.DeleteSuccess(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        const handleFailed = (result: AspnetJsonResult<any>) =>
          of(new fromQuestionCategoryAction.Failed(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider)

        return _loadingWork$(work$);
      })

    )

  @Effect()
  deteleSuccess$ = _entry$<ResultPayload<any>>(this.actions$,
    fromQuestionCategoryAction.DELETE_SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('停用成功');
        // const direct$ = _route$('./pages/master/question-category', {});


        payload.cb && payload.cb();

        return concat(popup$);
      })
    )



  @Effect()
  success$ = _entry$<ResultPayload<any>>(this.actions$,
    fromQuestionCategoryAction.SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/question-category', {});

        return concat(popup$, direct$);
      })
    )


  @Effect()
  failed$ = _entry$<string>(this.actions$,
    fromQuestionCategoryAction.FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload)
      })
    )


  @Effect()
  download$ = _entry$<QuestionClassificationSearchViewModel>(this.actions$,
    fromQuestionCategoryAction.GET_EXCEL)
    .pipe(
      exhaustMap((payload) => {

        const retrieve$ = this.http.download(
          'Master/QuestionClassification/GetExcelForQuestionClassification', 'post', payload);

        const handleSuccess = (result: Blob) => of(new fromQuestionCategoryAction.GetReportSuccess(result));


        const handleFailed = (result: Response) => of(new fromQuestionCategoryAction.GetReportFailed(result.statusText));


        const consider = (result: any) => result.status == "200";


        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

}
