import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromStoresActions from '../actions/stores.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { StoresDetailViewModel, StoresListViewModel } from 'src/app/model/master.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { tryGetStoreKey, commonBu } from 'src/global';
import { OrganizationType } from 'src/app/model/organization.model';

@Injectable()
export class StoresEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }




  @Effect()
  loadDetail$ = _entry$<{ ID?: number, OrganizationType?: OrganizationType }>(this.actions$,
    fromStoresActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: { ID?: number, OrganizationType?: OrganizationType }) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<StoresDetailViewModel>>(`${commonBu}/Store/Get`, payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<StoresDetailViewModel>) =>
          of(new fromStoresActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromStoresActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromStoresActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  getStoresListTemplate$ = _entry$<{ nodeKey: string }>(
    this.actions$,
    fromStoresActions.GET_STORES_LIST_TEMPLATE)
    .pipe(
      exhaustMap((payload: { nodeKey: string }) => {
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<string>('Common/System/GetStoreListTemplate', payload)
          .pipe(exhaustMap(x => {
            return of(new fromStoresActions.getStoresListTemplateSuccessAction(x));
          }));

        return _loadingWork$(retrieve$);
      })

    );

  @Effect()
  getStoresListTemplateFailed$ = _entry$<string>(this.actions$,
    fromStoresActions.GET_STORES_LIST_TEMPLATE_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<StoresDetailViewModel>(this.actions$,
    fromStoresActions.EDIT).pipe(
      exhaustMap((payload: StoresDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        // 現行業務行為門市皆不需要各自實作，若後續有需要再行調整
        const providerKey = tryGetStoreKey(payload.NodeKey);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料commonBu
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          `${providerKey}/Store/Update`,
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromStoresActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromStoresActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromStoresActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/stores', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromStoresActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

}
