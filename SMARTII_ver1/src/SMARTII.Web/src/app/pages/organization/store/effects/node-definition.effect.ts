import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromNodeDefinitionActions from '../actions/node-definition.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload, ActionType } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { NodeDefinitionDetailViewModel, OrganizationType, JobDetailViewModel } from 'src/app/model/organization.model';


@Injectable()
export class NodeDefinitionEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromNodeDefinitionActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<NodeDefinitionDetailViewModel>>
          ('Organization/NodeDefinition/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<NodeDefinitionDetailViewModel>) =>
          of(new fromNodeDefinitionActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromNodeDefinitionActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  add$ = _entry$<NodeDefinitionDetailViewModel>(this.actions$,
    fromNodeDefinitionActions.ADD)
    .pipe(
      exhaustMap((payload: NodeDefinitionDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<NodeDefinitionDetailViewModel>>
          ('Organization/NodeDefinition/Create', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<NodeDefinitionDetailViewModel>) =>
          of(new fromNodeDefinitionActions.addSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  addSuccess$ = _entry$<NodeDefinitionDetailViewModel>(this.actions$,
    fromNodeDefinitionActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/organization/node-definition-detail', {
          actionType: ActionType.Update,
          id: payload.ID,
          organizationType: payload.OrganizationType
        });
        return concat(popup$, direct$);

      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromNodeDefinitionActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<NodeDefinitionDetailViewModel>(this.actions$,
    fromNodeDefinitionActions.EDIT)
    .pipe(
      exhaustMap((payload: NodeDefinitionDetailViewModel) => {

        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<NodeDefinitionDetailViewModel>>
          ('Organization/NodeDefinition/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromNodeDefinitionActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/organization/node-definition', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromNodeDefinitionActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  disable$ = _entry$<EntrancePayload<{ organizationType: OrganizationType, ID: number }>>(this.actions$,
    fromNodeDefinitionActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ organizationType: OrganizationType, ID: number }>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>('Organization/NodeDefinition/Disable', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.disableSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.disableFailedAction(new ResultPayload<string>(
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
    fromNodeDefinitionActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/organization/node-definition', {});

        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNodeDefinitionActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );


  @Effect()
  addJob$ = _entry$<EntrancePayload<JobDetailViewModel>>(this.actions$,
    fromNodeDefinitionActions.ADD_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<JobDetailViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<JobDetailViewModel>>
          ('Organization/NodeDefinition/CreateJob', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<JobDetailViewModel>) =>
          of(new fromNodeDefinitionActions.addJobSuccessAction(new ResultPayload<JobDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.addJobFailedAction(new ResultPayload<string>(
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
  addJobSuccess$ = _entry$<ResultPayload<JobDetailViewModel>>(this.actions$,
    fromNodeDefinitionActions.ADD_JOB_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb(payload.data);
        return _success$('新增成功');

      })
    );


  @Effect()
  addJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNodeDefinitionActions.ADD_JOB_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect()
  editJob$ = _entry$<EntrancePayload<JobDetailViewModel>>(this.actions$,
    fromNodeDefinitionActions.EDIT_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<JobDetailViewModel>) => {

        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<NodeDefinitionDetailViewModel>>
          ('Organization/NodeDefinition/UpdateJob', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<JobDetailViewModel>) =>
          of(new fromNodeDefinitionActions.editJobSuccessAction(new ResultPayload<JobDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.editJobFailedAction(new ResultPayload<string>(
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
  editJobSuccess$ = _entry$<ResultPayload<JobDetailViewModel>>(this.actions$,
    fromNodeDefinitionActions.EDIT_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb(payload.data);
        return _success$('編輯成功');
      })
    );


  @Effect()
  editJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNodeDefinitionActions.EDIT_JOB_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );


  @Effect()
  disableJob$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromNodeDefinitionActions.DISABLE_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>('Organization/NodeDefinition/DisableJob', {
          ID: payload.data
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.disableJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromNodeDefinitionActions.disableJobFailedAction(new ResultPayload<string>(
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
  disableJobSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNodeDefinitionActions.DISABLE_JOB_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');

        payload.cb && payload.cb();

        return concat(popup$);
      })
    );


  @Effect()
  disableJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromNodeDefinitionActions.DISABLE_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );


}
