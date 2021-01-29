import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import { Store } from '@ngrx/store';

import * as fromCaseApplyActions from '../actions/case-apply.actions';
import { State as fromCaseApplyReducer } from '../reducers/case-apply.reducers';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseApplyCommitViewModel } from 'src/app/model/substitute.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';

@Injectable()
export class CaseApplyEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect()
  apply$ = _entry$<EntrancePayload<CaseApplyCommitViewModel>>(this.actions$,
    fromCaseApplyActions.APPLY).pipe(
      exhaustMap((payload: EntrancePayload<CaseApplyCommitViewModel>) => {


        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseApplyCommitViewModel>>(
          'Substitute/CaseApply/Apply',
          null,
          payload.data
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseApplyActions.applySuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));


        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromCaseApplyActions.applyFailedAction(new ResultPayload<string>(
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
  applySuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseApplyActions.APPLY_SUCCESS).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        const popup$ = _success$('成功');

        return concat(popup$);
      })
    );


  @Effect()
  applyFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseApplyActions.APPLY_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );
}
