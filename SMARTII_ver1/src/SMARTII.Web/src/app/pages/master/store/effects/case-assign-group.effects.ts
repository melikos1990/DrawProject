import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromCaseAssignGroupActions from '../actions/case-assign-group.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseAssignGroupDetailViewModel, CaseAssignGroupListViewModel } from 'src/app/model/master.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';

@Injectable()
export class CaseAssignGroupEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromCaseAssignGroupActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CaseAssignGroupDetailViewModel>>('Master/CaseAssignGroup/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignGroupDetailViewModel>) =>
          of(new fromCaseAssignGroupActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<CaseAssignGroupDetailViewModel>) =>
          of(new fromCaseAssignGroupActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromCaseAssignGroupActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  add$ = _entry$<CaseAssignGroupDetailViewModel>(this.actions$,
    fromCaseAssignGroupActions.ADD).pipe(
      exhaustMap((payload: CaseAssignGroupDetailViewModel) => {

        //const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseAssignGroup/Create',
          null,
          payload
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromCaseAssignGroupActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/case-assign-group', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromCaseAssignGroupActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<CaseAssignGroupDetailViewModel>(this.actions$,
    fromCaseAssignGroupActions.EDIT).pipe(
      exhaustMap((payload: CaseAssignGroupDetailViewModel) => {

       // const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/CaseAssignGroup/Update',
          null,
          payload
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromCaseAssignGroupActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/case-assign-group', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromCaseAssignGroupActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  delete$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
    fromCaseAssignGroupActions.DELETE).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
          'Master/CaseAssignGroup/Delete', payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.deleteSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.deleteFailedAction(new ResultPayload<string>(
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
  deleteSuccess$ = _entry$<any>(this.actions$,
    fromCaseAssignGroupActions.DELETE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$('刪除成功');
        payload.cb && payload.cb();
        return concat(popup$);
      })
    );


  @Effect()
  deleteFailed$ = _entry$<any>(this.actions$,
    fromCaseAssignGroupActions.DELETE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload);
      })
    );




  @Effect()
  deleteRange$ = _entry$<EntrancePayload<Array<CaseAssignGroupListViewModel>>>(this.actions$,
    fromCaseAssignGroupActions.DELETE_RANGE).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<string>>
        ('Master/CaseAssignGroup/DeleteRange', null, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.deleteRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseAssignGroupActions.deleteRangeFailedAction(new ResultPayload<string>(
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
  deleteRangeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseAssignGroupActions.DELETE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        const popup$ = _success$('刪除成功');

        return concat(popup$);
      })
    );


  @Effect()
  deleteRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseAssignGroupActions.DELETE_RANGE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

}
