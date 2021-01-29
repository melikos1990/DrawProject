import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromCaseTagActions from '../actions/case-tag.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { CaseTagDetailViewModel } from '../../../../model/master.model';

@Injectable()
export class CaseTagEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromCaseTagActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        const retrieve$ = this.http.get<AspnetJsonResult<CaseTagDetailViewModel>>('Master/CaseTag/Get', payload);

        const handleSuccess = (result: AspnetJsonResult<CaseTagDetailViewModel>) =>
          of(new fromCaseTagActions.loadDetailSuccessAction(result.element));


        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromCaseTagActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromCaseTagActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  add$ = _entry$<CaseTagDetailViewModel>(this.actions$,
    fromCaseTagActions.ADD).pipe(
      exhaustMap((payload) => {

        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseTag/Create',
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseTagActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseTagActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromCaseTagActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/case-tag', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromCaseTagActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<CaseTagDetailViewModel>(this.actions$,
    fromCaseTagActions.EDIT).pipe(
      exhaustMap((payload) => {

        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseTag/Update',
          null,
          payload
        );
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseTagActions.editSuccessAction(result.message));

        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseTagActions.editFailedAction(result.message));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromCaseTagActions.EDIT_SUCCESS).pipe(
      exhaustMap(() => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/case-tag', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromCaseTagActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  disable$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromCaseTagActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResultBase>('Master/CaseTag/Disabled', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCaseTagActions.disableSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseTagActions.disableFailedAction(new ResultPayload<string>(
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
  disableSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseTagActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/master/case-tag', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseTagActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

}
