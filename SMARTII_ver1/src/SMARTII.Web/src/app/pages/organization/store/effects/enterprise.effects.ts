import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromEnterpriseActions from '../actions/enterprise.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { EnterpriseDetailViewModel } from 'src/app/model/organization.model';

@Injectable()
export class EnterpriseEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromEnterpriseActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<EnterpriseDetailViewModel>>('Organization/Enterprise/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<EnterpriseDetailViewModel>) =>
          of(new fromEnterpriseActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromEnterpriseActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  add$ = _entry$<EnterpriseDetailViewModel>(this.actions$,
    fromEnterpriseActions.ADD)
    .pipe(
      exhaustMap((payload: EnterpriseDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<EnterpriseDetailViewModel>>('Organization/Enterprise/Create', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<EnterpriseDetailViewModel>) =>
          of(new fromEnterpriseActions.addSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromEnterpriseActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/organization/enterprise', {});
        return concat(popup$, direct$);

      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromEnterpriseActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<EnterpriseDetailViewModel>(this.actions$,
    fromEnterpriseActions.EDIT)
    .pipe(
      exhaustMap((payload: EnterpriseDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<EnterpriseDetailViewModel>>('Organization/Enterprise/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<EnterpriseDetailViewModel>) =>
          of(new fromEnterpriseActions.editSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromEnterpriseActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/organization/enterprise', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromEnterpriseActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );



  @Effect()
  deleteRange$ = _entry$<EntrancePayload<number[]>>(this.actions$,
    fromEnterpriseActions.DELETE_RANGE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number[]>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<string>>('Organization/Enterprise/DeleteRange', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.deleteRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.deleteRangeFailedAction(new ResultPayload<string>(
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
    fromEnterpriseActions.DELETE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('刪除成功');
        const direct$ = _route$('./pages/organization/enterprise', {});

        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  deleteRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromEnterpriseActions.DELETE_RANGE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );




  @Effect()
  delete$ = _entry$<EntrancePayload<string>>(this.actions$,
    fromEnterpriseActions.DELETE)
    .pipe(
      exhaustMap((payload: EntrancePayload<string>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>('Organization/Enterprise/Delete', {
          EnterpriseID: payload.data
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.deleteSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.deleteFailedAction(new ResultPayload<string>(
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
  deleteSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromEnterpriseActions.DELETE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('刪除成功');
        const direct$ = _route$('./pages/organization/enterprise', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  deleteFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromEnterpriseActions.DELETE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

  @Effect()
  disable$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromEnterpriseActions.DISABLED_DETAIL)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResultBase>('Organization/Enterprise/Disabled', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.disabledDetailSuccess(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromEnterpriseActions.disabledDetailFailed(new ResultPayload<string>(
            result.message,
            payload.failed
          )));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    )

    @Effect()
    disableSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
      fromEnterpriseActions.DISABLED_DETAIL_SUCCESS).pipe(
        exhaustMap(payload => {
  
          const popup$ = _success$('停用成功');
          const direct$ = _route$('./pages/organization/enterprise', {});
  
  
          payload.cb && payload.cb();
  
          return concat(popup$, direct$);
        })
      );
  
  
    @Effect()
    disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
      fromEnterpriseActions.DISABLED_DETAIL_FAILED).pipe(
        exhaustMap(payload => {
          payload.cb && payload.cb();
          return _failed$(payload.data);
        })
      );
  




}
