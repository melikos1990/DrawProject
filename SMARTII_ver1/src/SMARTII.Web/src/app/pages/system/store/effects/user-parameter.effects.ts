import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';

import * as fromUserParameterAction from '../actions/user-parameter.actions';

import { exhaustMap, tap, map, flatMap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { of, concat } from 'rxjs';


import { UserParameterlViewModel } from 'src/app/model/master.model';

// customer service
import { HttpService } from 'src/app/shared/service/http.service';

//customer object

import { AspnetJsonResult, AspnetJsonResultBase, ResultPayload, EntrancePayload } from 'src/app/model/common.model';

// customer ngrx obsv
import { _httpflow$, _isHttpSuccess$ } from 'src/app/shared/ngrx/http.ngrx';
import { _unLoading$, _loadingWork$, _loading$ } from 'src/app/shared/ngrx/loading.ngrx'
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx'
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';


@Injectable()
export class UserParameterEffects { 
  constructor(
    private objectService: ObjectService,
    private http: HttpService,
    private actions$: Actions
  ) { }


  @Effect()
  editDetail$ = _entry$<EntrancePayload<UserParameterlViewModel>>(this.actions$,
    fromUserParameterAction.UPDATE_USER_PARAMETER)
    .pipe(
      exhaustMap((payload) => {

        const formData = this.objectService.convertToFormData(payload.data);
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          'Master/UserParameter/Update',
          null,
          formData
        );

        const handleSuccess$ = (result: AspnetJsonResultBase) =>
          of(new fromUserParameterAction.SuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromUserParameterAction.FailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      })

    )



  @Effect()
  getDetail$ = _entry$<EntrancePayload<{USER_ID : string}>>(this.actions$,
    fromUserParameterAction.GET_USER_PARAMETER_DETAIL)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<EntrancePayload<{USER_ID : string}>>>('Master/UserParameter/Get', payload.data);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess$ = (result: AspnetJsonResult<UserParameterlViewModel>) =>
          of(new fromUserParameterAction.GetDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed$ = (result: AspnetJsonResultBase) =>
          of(new fromUserParameterAction.FailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider$ = (result: AspnetJsonResultBase) => result.isSuccess;
 
        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess$, handleFailed$, retrieve$, consider$)

        return _loadingWork$(work$);
      })

    )




  @Effect()
  success$ = _entry$<ResultPayload<string>>(this.actions$,
    fromUserParameterAction.SUCCESS)
    .pipe(
      exhaustMap(payload => {
        const popup$ = _success$('編輯成功');

        payload.cb && payload.cb();

        return concat(popup$);

      })
    )


  @Effect()
  failed$ = _entry$<string>(this.actions$,
    fromUserParameterAction.FAILED)
    .pipe(
      exhaustMap(payload => {
        return _failed$(payload)
      })
    )


}
