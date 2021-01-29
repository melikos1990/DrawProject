import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromCallCenterNodeActions from '../actions/callcenter-node.action';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { OrganizationNodeViewModel, CallCenterNodeViewModel, CallCenterNodeDetailViewModel, AddUserViewModel, AddJobViewModel } from 'src/app/model/organization.model';
import { ObjectService } from 'src/app/shared/service/object.service';




@Injectable()
export class CallCenterNodeEffects {
  constructor(
    private objectService: ObjectService,
    private http: HttpService,
    private actions$: Actions) { }


  @Effect()
  load$ = _entry$<EntrancePayload<any>>(this.actions$,
    fromCallCenterNodeActions.LOAD)
    .pipe(
      exhaustMap((payload: EntrancePayload<any>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<OrganizationNodeViewModel>>
          ('Organization/CallCenterNode/GetAllNested', null, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CallCenterNodeViewModel>) =>
          of(new fromCallCenterNodeActions.loadSuccessAction(new ResultPayload<CallCenterNodeViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.loadFailedAction(new ResultPayload<string>(
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
  loadFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.LOAD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  loadSuccess$ = _entry$<ResultPayload<OrganizationNodeViewModel>>(this.actions$,
    fromCallCenterNodeActions.LOAD_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb();
      })
    );


  @Effect()
  add$ = _entry$<EntrancePayload<CallCenterNodeViewModel>>(this.actions$,
    fromCallCenterNodeActions.ADD)
    .pipe(
      exhaustMap((payload: EntrancePayload<CallCenterNodeViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CallCenterNodeViewModel>>
          ('Organization/CallCenterNode/Create', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.addSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.addFailedAction(new ResultPayload<string>(
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
  addSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增成功');
        const load$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  addFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );
  @Effect()
  updateTree$ = _entry$<EntrancePayload<CallCenterNodeViewModel>>(this.actions$,
    fromCallCenterNodeActions.UPDATE_TREE)
    .pipe(
      exhaustMap((payload: EntrancePayload<CallCenterNodeViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CallCenterNodeViewModel>>
          ('Organization/CallCenterNode/UpdateTree', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.updateTreeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.updateTreeFailedAction(new ResultPayload<string>(
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
  updateTreeSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.UPDATE_TREE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('編輯成功');
        const load$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  updateTreeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.UPDATE_TREE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );

  @Effect()
  disable$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromCallCenterNodeActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CallCenterNodeViewModel>>
          ('Organization/CallCenterNode/Disable', {
            nodeID: payload.data
          });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.disabledSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.disabledFailedAction(new ResultPayload<string>(
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
  disableSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('停用成功');
        const load$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );

  @Effect()
  loadDetail$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromCallCenterNodeActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CallCenterNodeViewModel>>
          ('Organization/CallCenterNode/Get', {
            nodeID: payload.data
          });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CallCenterNodeDetailViewModel>) =>
          of(new fromCallCenterNodeActions.loadDetailSuccessAction(new ResultPayload<CallCenterNodeDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.disabledFailedAction(new ResultPayload<string>(
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
  @Effect({
    dispatch: false
  })
  loadDetailSuccess$ = _entry$<ResultPayload<CallCenterNodeDetailViewModel>>(this.actions$,
    fromCallCenterNodeActions.DISABLE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb();


      })
    );

  @Effect()
  loadDetailFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );

  @Effect()
  edit$ = _entry$<EntrancePayload<CallCenterNodeDetailViewModel>>(this.actions$,
    fromCallCenterNodeActions.EDIT)
    .pipe(
      exhaustMap((payload: EntrancePayload<CallCenterNodeDetailViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CallCenterNodeDetailViewModel>>
          ('Organization/CallCenterNode/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CallCenterNodeDetailViewModel>) =>
          of(new fromCallCenterNodeActions.editSuccessAction(new ResultPayload<CallCenterNodeDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.editFailedAction(new ResultPayload<string>(
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
  editSuccess$ = _entry$<ResultPayload<CallCenterNodeDetailViewModel>>(this.actions$,
    fromCallCenterNodeActions.EDIT_SUCCESS).pipe(
      exhaustMap((payload) => {
        payload.cb && payload.cb();
        const popup$ = _success$('編輯成功');
        const loadTree$ = of(new fromCallCenterNodeActions.loadAction(new EntrancePayload()));
        const loadDetail$ = of(
          new fromCallCenterNodeActions.loadDetailAction(new EntrancePayload<number>(payload.data.ID)));
        return concat(popup$, loadTree$, loadDetail$);

      })
    );


  @Effect()
  editFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );

  @Effect()
  addUser$ = _entry$<EntrancePayload<AddUserViewModel>>(this.actions$,
    fromCallCenterNodeActions.ADD_USER)
    .pipe(
      exhaustMap((payload: EntrancePayload<AddUserViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<AddUserViewModel>>
          ('Organization/CallCenterNode/AddUser', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.addUserSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.addUserFailedAction(new ResultPayload<string>(
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
  addUserSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.ADD_USER_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增成功');
        return popup$;

      })
    );


  @Effect()
  addUserFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.ADD_USER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );



  @Effect()
  addJob$ = _entry$<EntrancePayload<AddJobViewModel>>(this.actions$,
    fromCallCenterNodeActions.ADD_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<AddJobViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<AddJobViewModel>>
          ('Organization/CallCenterNode/AddJob', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.addJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.addJobFailedAction(new ResultPayload<string>(
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
  addJobSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.ADD_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增職稱成功');
        return popup$;

      })
    );


  @Effect()
  addJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.ADD_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );



  @Effect()
  deleteJob$ = _entry$<EntrancePayload<{ nodeJobID: number }>>(this.actions$,
    fromCallCenterNodeActions.DELETE_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ nodeJobID: number }>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<{ nodeJobID: number }>>
          ('Organization/CallCenterNode/DeleteJob', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.deleteJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.deleteJobFailedAction(new ResultPayload<string>(
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
  deleteJobSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.DELETE_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('刪除職稱成功');
        return popup$;

      })
    );


  @Effect()
  deleteJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.DELETE_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );




  @Effect()
  deleteUser$ = _entry$<EntrancePayload<{ nodeJobID: number, userID: string }>>(this.actions$,
    fromCallCenterNodeActions.DELETE_USER)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ nodeJobID: number, userID: string }>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<{ nodeJobID: number, userID: string }>>
          ('Organization/CallCenterNode/DeleteUser', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.deleteUserSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCallCenterNodeActions.deleteUserFailedAction(new ResultPayload<string>(
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
  deleteUserSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.DELETE_USER_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('刪除職稱成功');
        return popup$;

      })
    );


  @Effect()
  deleteUserFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCallCenterNodeActions.DELETE_USER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );


}
