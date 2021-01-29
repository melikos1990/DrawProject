import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromRoleActions from '../actions/role.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat, empty } from 'rxjs';
import { _failed$, _success$, _prompt$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { RoleDetailViewModel } from 'src/app/model/authorize.model';


@Injectable()
export class RoleEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromRoleActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<RoleDetailViewModel>>('Organization/Role/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<RoleDetailViewModel>) =>
          of(new fromRoleActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromRoleActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  add$ = _entry$<RoleDetailViewModel>(this.actions$,
    fromRoleActions.ADD)
    .pipe(
      exhaustMap((payload: RoleDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<RoleDetailViewModel>>('Organization/Role/Create', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromRoleActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/system/role', {});
        return concat(popup$, direct$);

      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromRoleActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<RoleDetailViewModel>(this.actions$,
    fromRoleActions.EDIT)
    .pipe(
      exhaustMap((payload: RoleDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<RoleDetailViewModel>>('Organization/Role/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromRoleActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/system/role', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromRoleActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  disable$ = _entry$<EntrancePayload<string>>(this.actions$,
    fromRoleActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<string>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>('Organization/Role/Disable', {
          RoleID: payload.data
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.disableSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.disableFailedAction(new ResultPayload<string>(
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
    fromRoleActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/system/role', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromRoleActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

  @Effect()
  disableRange$ = _entry$<EntrancePayload<number[]>>(this.actions$,
    fromRoleActions.DISABLE_RANGE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number[]>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<number[]>>('Organization/Role/DisableRange', {}, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.disableRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromRoleActions.disableRangeFailedAction(new ResultPayload<string>(
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
  disableRangeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromRoleActions.DISABLE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('批次停用成功');
        const direct$ = _route$('./pages/system/role', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromRoleActions.DISABLE_RANGE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

}
