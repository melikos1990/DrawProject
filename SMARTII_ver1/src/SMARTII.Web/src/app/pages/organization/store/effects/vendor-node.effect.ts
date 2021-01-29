import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromVendorNodeActions from '../actions/vendor-node.action';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { OrganizationNodeViewModel, VendorNodeViewModel, VendorNodeDetailViewModel, AddUserViewModel, AddJobViewModel } from 'src/app/model/organization.model';
import { ObjectService } from 'src/app/shared/service/object.service';




@Injectable()
export class VendorNodeEffects {
  constructor(
    private objectService: ObjectService,
    private http: HttpService,
    private actions$: Actions) { }


  @Effect()
  load$ = _entry$<EntrancePayload<any>>(this.actions$,
    fromVendorNodeActions.LOAD)
    .pipe(
      exhaustMap((payload: EntrancePayload<any>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<OrganizationNodeViewModel>>
          ('Organization/VendorNode/GetAllNested', null, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<VendorNodeViewModel>) =>
          of(new fromVendorNodeActions.loadSuccessAction(new ResultPayload<VendorNodeViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.loadFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.LOAD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );


  @Effect({
    dispatch: false
  })
  loadSuccess$ = _entry$<ResultPayload<OrganizationNodeViewModel>>(this.actions$,
    fromVendorNodeActions.LOAD_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb();
      })
    );


  @Effect()
  add$ = _entry$<EntrancePayload<VendorNodeViewModel>>(this.actions$,
    fromVendorNodeActions.ADD)
    .pipe(
      exhaustMap((payload: EntrancePayload<VendorNodeViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<VendorNodeViewModel>>
          ('Organization/VendorNode/Create', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.addSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.addFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增成功');
        const load$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  addFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );



  @Effect()
  loadDetail$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromVendorNodeActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<VendorNodeViewModel>>
          ('Organization/VendorNode/Get', {
            nodeID: payload.data
          });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<VendorNodeDetailViewModel>) =>
          of(new fromVendorNodeActions.loadDetailSuccessAction(new ResultPayload<VendorNodeDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.disabledFailedAction(new ResultPayload<string>(
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
  loadDetailSuccess$ = _entry$<ResultPayload<VendorNodeDetailViewModel>>(this.actions$,
    fromVendorNodeActions.DISABLE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb();


      })
    );

  @Effect()
  loadDetailFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );



  @Effect()
  edit$ = _entry$<EntrancePayload<VendorNodeDetailViewModel>>(this.actions$,
    fromVendorNodeActions.EDIT)
    .pipe(
      exhaustMap((payload: EntrancePayload<VendorNodeDetailViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<VendorNodeDetailViewModel>>
          ('Organization/VendorNode/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<VendorNodeDetailViewModel>) =>
          of(new fromVendorNodeActions.editSuccessAction(new ResultPayload<VendorNodeDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.editFailedAction(new ResultPayload<string>(
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
  editSuccess$ = _entry$<ResultPayload<VendorNodeDetailViewModel>>(this.actions$,
    fromVendorNodeActions.EDIT_SUCCESS).pipe(
      exhaustMap((payload) => {
        payload.cb && payload.cb();
        const popup$ = _success$('編輯成功');
        const loadTree$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        const loadDetail$ = of(
          new fromVendorNodeActions.loadDetailAction(new EntrancePayload<number>(payload.data.ID)));
        return concat(popup$, loadTree$, loadDetail$);

      })
    );


  @Effect()
  editFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );

  @Effect()
  addJob$ = _entry$<EntrancePayload<AddJobViewModel>>(this.actions$,
    fromVendorNodeActions.ADD_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<AddJobViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<AddJobViewModel>>
          ('Organization/VendorNode/AddJob', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.addJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.addJobFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.ADD_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增職稱成功');
        return popup$;

      })
    );


  @Effect()
  addJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.ADD_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );

  @Effect()
  addUser$ = _entry$<EntrancePayload<AddUserViewModel>>(this.actions$,
    fromVendorNodeActions.ADD_USER)
    .pipe(
      exhaustMap((payload: EntrancePayload<AddUserViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<AddUserViewModel>>
          ('Organization/VendorNode/AddUser', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.addUserSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.addUserFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.ADD_USER_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增成功');
        return popup$;

      })
    );


  @Effect()
  addUserFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.ADD_USER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );



  @Effect()
  deleteJob$ = _entry$<EntrancePayload<{ nodeJobID: number }>>(this.actions$,
    fromVendorNodeActions.DELETE_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ nodeJobID: number }>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<{ nodeJobID: number }>>
          ('Organization/VendorNode/DeleteJob', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.deleteJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.deleteJobFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.DELETE_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('刪除職稱成功');
        return popup$;

      })
    );


  @Effect()
  deleteJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.DELETE_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );




  @Effect()
  deleteUser$ = _entry$<EntrancePayload<{ nodeJobID: number, userID: string }>>(this.actions$,
    fromVendorNodeActions.DELETE_USER)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ nodeJobID: number, userID: string }>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<{ nodeJobID: number, userID: string }>>
          ('Organization/VendorNode/DeleteUser', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.deleteUserSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.deleteUserFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.DELETE_USER_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('刪除職稱成功');
        return popup$;

      })
    );


  @Effect()
  deleteUserFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.DELETE_USER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );


  @Effect()
  updateTree$ = _entry$<EntrancePayload<VendorNodeViewModel>>(this.actions$,
    fromVendorNodeActions.UPDATE_TREE)
    .pipe(
      exhaustMap((payload: EntrancePayload<VendorNodeViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<VendorNodeViewModel>>
          ('Organization/VendorNode/UpdateTree', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.updateTreeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.updateTreeFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.UPDATE_TREE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('編輯成功');
        const load$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  updateTreeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.UPDATE_TREE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );


  @Effect()
  disable$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromVendorNodeActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<VendorNodeViewModel>>
          ('Organization/VendorNode/Disable', {
            nodeID: payload.data
          });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.disabledSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromVendorNodeActions.disabledFailedAction(new ResultPayload<string>(
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
    fromVendorNodeActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('停用成功');
        const load$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromVendorNodeActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromVendorNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );

}
