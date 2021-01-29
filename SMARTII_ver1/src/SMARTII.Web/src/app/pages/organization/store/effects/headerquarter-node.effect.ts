import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromHeaderQuarterNodeActions from '../actions/headerquarter-node.action';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap, tap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { OrganizationNodeViewModel, HeaderQuarterNodeViewModel, HeaderQuarterNodeDetailViewModel, AddUserViewModel, AddJobViewModel } from 'src/app/model/organization.model';
import { ObjectService } from 'src/app/shared/service/object.service';
import { HttpParams } from '@angular/common/http';




@Injectable()
export class HeaderQuarterNodeEffects {
  constructor(
    private objectService: ObjectService,
    private http: HttpService,
    private actions$: Actions) { }


  @Effect()
  load$ = _entry$<EntrancePayload<any>>(this.actions$,
    fromHeaderQuarterNodeActions.LOAD)
    .pipe(
      exhaustMap((payload: EntrancePayload<any>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<OrganizationNodeViewModel>>
          ('Organization/HeaderQuarterNode/GetAllNested', null, {});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<HeaderQuarterNodeViewModel>) =>
          of(new fromHeaderQuarterNodeActions.loadSuccessAction(new ResultPayload<HeaderQuarterNodeViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.loadFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.LOAD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  loadSuccess$ = _entry$<ResultPayload<OrganizationNodeViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.LOAD_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb();
      })
    );


  @Effect()
  add$ = _entry$<EntrancePayload<HeaderQuarterNodeViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.ADD)
    .pipe(
      exhaustMap((payload: EntrancePayload<HeaderQuarterNodeViewModel>) => {


        let body = new HttpParams();
        body = body.set('', JSON.stringify(payload.data));

        // const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<HeaderQuarterNodeViewModel>>
          ('Organization/HeaderQuarterNode/Create', null, body);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.addSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.addFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增成功');
        const load$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  addFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );



  @Effect()
  updateTree$ = _entry$<EntrancePayload<HeaderQuarterNodeViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.UPDATE_TREE)
    .pipe(
      exhaustMap((payload: EntrancePayload<HeaderQuarterNodeViewModel>) => {

        let body = new HttpParams();
        body = body.set('', JSON.stringify(payload.data));

        // const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<HeaderQuarterNodeViewModel>>
          ('Organization/HeaderQuarterNode/UpdateTree', null, body);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.updateTreeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.updateTreeFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.UPDATE_TREE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('編輯成功');
        const load$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  updateTreeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.UPDATE_TREE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );

  @Effect()
  disable$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromHeaderQuarterNodeActions.DISABLE)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<HeaderQuarterNodeViewModel>>
          ('Organization/HeaderQuarterNode/Disable', {
            nodeID: payload.data
          });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.disabledSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.disabledFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('停用成功');
        const load$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        return concat(popup$, load$);

      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const load$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        const popup$ = _failed$(payload.data);
        return concat(popup$, load$);
      })
    );

  @Effect()
  loadDetail$ = _entry$<EntrancePayload<number>>(this.actions$,
    fromHeaderQuarterNodeActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: EntrancePayload<number>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<HeaderQuarterNodeViewModel>>
          ('Organization/HeaderQuarterNode/Get', {
            nodeID: payload.data
          });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<HeaderQuarterNodeDetailViewModel>) =>
          of(new fromHeaderQuarterNodeActions.loadDetailSuccessAction(new ResultPayload<HeaderQuarterNodeDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.disabledFailedAction(new ResultPayload<string>(
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
  loadDetailSuccess$ = _entry$<ResultPayload<HeaderQuarterNodeDetailViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.DISABLE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb();


      })
    );

  @Effect()
  loadDetailFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );

  @Effect()
  edit$ = _entry$<EntrancePayload<HeaderQuarterNodeDetailViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.EDIT)
    .pipe(
      exhaustMap((payload: EntrancePayload<HeaderQuarterNodeDetailViewModel>) => {


        const formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<HeaderQuarterNodeDetailViewModel>>
          ('Organization/HeaderQuarterNode/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<HeaderQuarterNodeDetailViewModel>) =>
          of(new fromHeaderQuarterNodeActions.editSuccessAction(new ResultPayload<HeaderQuarterNodeDetailViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.editFailedAction(new ResultPayload<string>(
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
  editSuccess$ = _entry$<ResultPayload<HeaderQuarterNodeDetailViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.EDIT_SUCCESS).pipe(
      exhaustMap((payload) => {
        payload.cb && payload.cb();
        const popup$ = _success$('編輯成功');
        const loadTree$ = of(new fromHeaderQuarterNodeActions.loadAction(new EntrancePayload()));
        const loadDetail$ = of(
          new fromHeaderQuarterNodeActions.loadDetailAction(new EntrancePayload<number>(payload.data.ID)));
        return concat(popup$, loadTree$, loadDetail$);

      })
    );


  @Effect()
  editFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );





  @Effect()
  addUser$ = _entry$<EntrancePayload<AddUserViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.ADD_USER)
    .pipe(
      exhaustMap((payload: EntrancePayload<AddUserViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<AddUserViewModel>>
          ('Organization/HeaderQuarterNode/AddUser', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.addUserSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.addUserFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.ADD_USER_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增成功');
        return popup$;

      })
    );


  @Effect()
  addUserFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.ADD_USER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );



  @Effect()
  addJob$ = _entry$<EntrancePayload<AddJobViewModel>>(this.actions$,
    fromHeaderQuarterNodeActions.ADD_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<AddJobViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<AddJobViewModel>>
          ('Organization/HeaderQuarterNode/AddJob', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.addJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.addJobFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.ADD_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('新增職稱成功');
        return popup$;

      })
    );


  @Effect()
  addJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.ADD_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );



  @Effect()
  deleteJob$ = _entry$<EntrancePayload<{ nodeJobID: number }>>(this.actions$,
    fromHeaderQuarterNodeActions.DELETE_JOB)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ nodeJobID: number }>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<{ nodeJobID: number }>>
          ('Organization/HeaderQuarterNode/DeleteJob', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.deleteJobSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.deleteJobFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.DELETE_JOB_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('刪除職稱成功');
        return popup$;

      })
    );


  @Effect()
  deleteJobFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.DELETE_JOB_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );




  @Effect()
  deleteUser$ = _entry$<EntrancePayload<{ nodeJobID: number, userID: string }>>(this.actions$,
    fromHeaderQuarterNodeActions.DELETE_USER)
    .pipe(
      exhaustMap((payload: EntrancePayload<{ nodeJobID: number, userID: string }>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<{ nodeJobID: number, userID: string }>>
          ('Organization/HeaderQuarterNode/DeleteUser', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.deleteUserSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromHeaderQuarterNodeActions.deleteUserFailedAction(new ResultPayload<string>(
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
    fromHeaderQuarterNodeActions.DELETE_USER_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('刪除職稱成功');
        return popup$;

      })
    );


  @Effect()
  deleteUserFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromHeaderQuarterNodeActions.DELETE_USER_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _failed$(payload.data);
        return popup$;
      })
    );


}
