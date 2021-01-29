import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { Store } from '@ngrx/store';

import * as fromUserActions from '../actions/user.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat, empty } from 'rxjs';
import { _failed$, _success$, _prompt$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { User, UserDetailViewModel, Identity, UserSearchViewModel } from 'src/app/model/authorize.model';

@Injectable()
export class UserEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  loadDetail$ = _entry$<any>(this.actions$,
    fromUserActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: any) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<UserDetailViewModel>>('Organization/User/Get', payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<UserDetailViewModel>) =>
          of(new fromUserActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromUserActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  add$ = _entry$<UserDetailViewModel>(this.actions$,
    fromUserActions.ADD)
    .pipe(
      exhaustMap((payload: UserDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<UserDetailViewModel>>('Organization/User/Create', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromUserActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/system/user', {});
        return concat(popup$, direct$);

      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromUserActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<UserDetailViewModel>(this.actions$,
    fromUserActions.EDIT)
    .pipe(
      exhaustMap((payload: UserDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<UserDetailViewModel>>('Organization/User/Update', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromUserActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/system/user', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromUserActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );



  @Effect()
  disableRange$ = _entry$<EntrancePayload<string[]>>(this.actions$,
    fromUserActions.DISABLED_RANGE)
    .pipe(
      exhaustMap((payload: EntrancePayload<string[]>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<string>>('Organization/User/DisableRange', null, payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.disableRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.disableRangeFailedAction(new ResultPayload<string>(
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
    fromUserActions.DISABLED_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/system/user', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.DISABLED_RANGE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );


  @Effect()
  disable$ = _entry$<EntrancePayload<string>>(this.actions$,
    fromUserActions.DISABLED)
    .pipe(
      exhaustMap((payload: EntrancePayload<string>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>('Organization/User/Disable', {
          UserID: payload.data
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.disableSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.disableFailedAction(new ResultPayload<string>(
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
    fromUserActions.DISABLED_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('停用成功');
        const direct$ = _route$('./pages/system/user', {});


        payload.cb && payload.cb();

        return concat(popup$, direct$);
      })
    );


  @Effect()
  disableFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.DISABLED_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );



  @Effect()
  validADPassword$ = _entry$<EntrancePayload<Identity>>(this.actions$,
    fromUserActions.VALID_AD_PASSWORD)
    .pipe(
      exhaustMap((payload: EntrancePayload<Identity>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<string>>('Organization/User/ValidADPassword', null, payload.data);


        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.validADPasswordSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));


        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.validADPasswordFailedAction(new ResultPayload<string>(
            result.message,
            payload.failed
          )));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  validADSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.VALID_AD_PASSWORD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$(payload.data);

        payload.cb && payload.cb();

        return popup$;
      })
    );


  @Effect()
  validADFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.VALID_AD_PASSWORD_FAILED).pipe(
      exhaustMap(payload => {

        const popup$ = _failed$(payload.data);

        payload.cb && payload.cb();

        return popup$;
      })
    );

  @Effect()
  resetPassword$ = _entry$<EntrancePayload<string>>(this.actions$,
    fromUserActions.RESET_PASSWORD)
    .pipe(
      exhaustMap((payload: EntrancePayload<string>) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>('Organization/User/ResetPassword', {
          account: payload.data
        });


        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.resetPasswordSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));


        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromUserActions.resetPasswordFailedAction(new ResultPayload<string>(
            result.message,
            payload.failed
          )));

        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  resetPasswordSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.RESET_PASSWORD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$(payload.data);

        payload.cb && payload.cb();

        return popup$;
      })
    );


  @Effect()
  resetPasswordFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.RESET_PASSWORD_FAILED).pipe(
      exhaustMap(payload => {

        const popup$ = _failed$(payload.data);

        payload.cb && payload.cb();

        return popup$;
      })
    );


  @Effect()
  checkName$ = _entry$<EntrancePayload<{ ID: string, name: string }>>(this.actions$,
    fromUserActions.CHECK_NAME).pipe(
      exhaustMap((payload) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
          'Organization/User/CheckName',
          payload.data
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromUserActions.checkNameSuccessAction(new ResultPayload<{ isExist: boolean, message: string }>(
            {
              isExist: result.element,
              message: result.message
            },
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromUserActions.checkNameFailedAction(new ResultPayload<string>(
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
  checkNameSuccess$ = _entry$<ResultPayload<{ isExist: boolean, message: string }>>(this.actions$,
    fromUserActions.CHECK_NAME_SUCCESS).pipe(
      exhaustMap((payload) => {


        if (payload.data.isExist === true) {
          const popup$ = _prompt$(`姓名 : 【${payload.data.message}】 重複 是否存檔 ? `, () => {
            payload.cb && payload.cb();
          });
          return concat(popup$);
        } else {

          payload.cb && payload.cb();
          return empty();
        }

      })
    );


  @Effect()
  checkNameFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserActions.CHECK_NAME_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );


  @Effect()
  download$ = _entry$<UserSearchViewModel>(this.actions$,
    fromUserActions.REPORT)
    .pipe(
      exhaustMap((payload) => {

        const retrieve$ = this.http.download(
          'Organization/User/GetReport', 'post', payload);

        const handleSuccess = (result: Blob) => of(new fromUserActions.reportSuccess(result));


        const handleFailed = (result: Response) => of(new fromUserActions.reportFailed(result.statusText));


        const consider = (result: any) => result.status == "200";


        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  downloadFailed$ = _entry$<any>(this.actions$,
    fromUserActions.REPORT_FAILED)
    .pipe(
      exhaustMap((payload) => {
        return _failed$(payload);
      })
    );

}
