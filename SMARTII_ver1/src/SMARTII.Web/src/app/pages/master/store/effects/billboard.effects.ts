import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromBillboardActions from '../actions/billboard.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { BillboardSearchViewModel, BillboardListViewModel, BillboardDetailViewModel } from 'src/app/model/master.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';

@Injectable()
export class BillboardEffects {
  constructor(
    private objectService: ObjectService,
    private http: HttpService,
    private actions$: Actions) { }



  @Effect()
  loadDetail$ = _entry$<{ ID?: number }>(this.actions$,
    fromBillboardActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload) => {

        const retrieve$ = this.http.get<AspnetJsonResult<BillboardDetailViewModel>>(
          'Master/Billboard/Get', payload);

        const handleSuccess = (result: AspnetJsonResult<BillboardDetailViewModel>) =>
          of(new fromBillboardActions.loadDetailSuccessAction(result.element));

        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromBillboardActions.loadDetailFailedAction(result.message));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromBillboardActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect({ dispatch: false })
  clearNotification$ = _entry$<any>(this.actions$,
    fromBillboardActions.CLEAR_NOTIFICATION)
    .pipe(
      exhaustMap(() => {
        const retrieve$ = this.http.post<AspnetJsonResult<BillboardListViewModel[]>>(
          'Master/Billboard/ClearNotification', null, {});
        return retrieve$;
      })
    );


  @Effect()
  getOwnList$ = _entry$<BillboardSearchViewModel>(this.actions$,
    fromBillboardActions.GET_OWN_LIST)
    .pipe(
      exhaustMap((payload) => {

        console.log(payload);

        const retrieve$ = this.http.post<AspnetJsonResult<BillboardListViewModel[]>>(
          'Master/Billboard/GetOwnList', null, payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<BillboardListViewModel[]>) =>
          of(new fromBillboardActions.getOwnListSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromBillboardActions.getOwnListFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  getOwnListFailed$ = _entry$<string>(this.actions$,
    fromBillboardActions.GET_OWN_LIST_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );




  @Effect()
  add$ = _entry$<BillboardDetailViewModel>(this.actions$,
    fromBillboardActions.ADD).pipe(
      exhaustMap((payload) => {

        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/Billboard/Create',
          null,
          formData
        );
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.addSuccessAction(result.message));

        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.addFailedAction(result.message));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromBillboardActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/billboard', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromBillboardActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<BillboardDetailViewModel>(this.actions$,
    fromBillboardActions.EDIT).pipe(
      exhaustMap((payload) => {

        const formData = this.objectService.convertToFormData(payload);

        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/Billboard/Update',
          null,
          formData
        );

        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.editSuccessAction(result.message));

        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.editFailedAction(result.message));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromBillboardActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/billboard', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromBillboardActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  delete$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
    fromBillboardActions.DELETE).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
          'Master/Billboard/Delete', payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.deleteSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.deleteFailedAction(new ResultPayload<string>(
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
    fromBillboardActions.DELETE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$('刪除成功');
        payload.cb && payload.cb();
        return concat(popup$);
      })
    );


  @Effect()
  deleteFailed$ = _entry$<any>(this.actions$,
    fromBillboardActions.DELETE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload);
      })
    );




  @Effect()
  deleteRange$ = _entry$<EntrancePayload<Array<BillboardListViewModel>>>(this.actions$,
    fromBillboardActions.DELETE_RANGE).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/Billboard/DeleteRange', null, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.deleteRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromBillboardActions.deleteRangeFailedAction(new ResultPayload<string>(
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
    fromBillboardActions.DELETE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        const popup$ = _success$('刪除成功');

        return concat(popup$);
      })
    );


  @Effect()
  deleteRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromBillboardActions.DELETE_RANGE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

}
