import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromItemActions from '../actions/item.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { ItemDetailViewModel, ItemListViewModel, ItemExportViewModel } from 'src/app/model/master.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$, _failedExport$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { tryGetProviderKey, commonBu } from 'src/global';

@Injectable()
export class ItemEffects {
  constructor(
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }




  @Effect()
  loadDetail$ = _entry$<{ ID?: number }>(this.actions$,
    fromItemActions.LOAD_DETAIL)
    .pipe(
      exhaustMap((payload: { ID?: number }) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<ItemDetailViewModel>>(`${commonBu}/Item/Get`, payload);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<ItemDetailViewModel>) =>
          of(new fromItemActions.loadDetailSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromItemActions.loadDetailFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadDetailFailed$ = _entry$<string>(this.actions$,
    fromItemActions.LOAD_DETAIL_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  loadDetailSuccess$ = _entry$<ItemDetailViewModel>(this.actions$,
    fromItemActions.LOAD_DETAIL_SUCCESS).pipe(
      exhaustMap(payload => {
        return of(new fromItemActions.getItemDetailTemplateAction({ nodeKey: payload.NodeKey }));
      })
    );


  @Effect()
  getItemDetailTemplate$ = _entry$<{ nodeKey: string }>(
    this.actions$,
    fromItemActions.GET_ITEM_DETAIL_TEMPLATE)
    .pipe(
      exhaustMap((payload: { nodeKey: string }) => {
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<string>('Common/System/GetItemDetailTemplate', payload)
          .pipe(exhaustMap(x => {
            return of(new fromItemActions.getItemDetailTemplateSuccessAction(x));
          }));

        return _loadingWork$(retrieve$);
      })

    );

  @Effect()
  getItemDetailTemplateFailed$ = _entry$<string>(this.actions$,
    fromItemActions.GET_ITEM_DETAIL_TEMPLATE_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  getItemListTemplate$ = _entry$<{ nodeKey: string }>(
    this.actions$,
    fromItemActions.GET_ITEM_LIST_TEMPLATE)
    .pipe(
      exhaustMap((payload: { nodeKey: string }) => {
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<string>('Common/System/GetItemListTemplate', payload)
          .pipe(exhaustMap(x => {
            return of(new fromItemActions.getItemListTemplateSuccessAction(x));
          }));

        return _loadingWork$(retrieve$);
      })

    );

  @Effect()
  getItemListTemplateFailed$ = _entry$<string>(this.actions$,
    fromItemActions.GET_ITEM_LIST_TEMPLATE_FAIL).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );


  @Effect()
  add$ = _entry$<ItemDetailViewModel>(this.actions$,
    fromItemActions.ADD).pipe(
      exhaustMap((payload: ItemDetailViewModel) => {

        const formData = this.objectService.convertToFormData(payload);

        const providerKey = tryGetProviderKey(payload.NodeKey);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          `${providerKey}/Item/Create`,
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromItemActions.addSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromItemActions.addFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  addSuccess$ = _entry$<string>(this.actions$,
    fromItemActions.ADD_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('新增成功');
        const direct$ = _route$('./pages/master/item', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  addFailed$ = _entry$<string>(this.actions$,
    fromItemActions.ADD_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  edit$ = _entry$<ItemDetailViewModel>(this.actions$,
    fromItemActions.EDIT).pipe(
      exhaustMap((payload: ItemDetailViewModel) => {


        const formData = this.objectService.convertToFormData(payload);

        const providerKey = tryGetProviderKey(payload.NodeKey);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
          `${providerKey}/Item/Update`,
          null,
          formData
        );

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResultBase) =>
          of(new fromItemActions.editSuccessAction(result.message));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromItemActions.editFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  editSuccess$ = _entry$<string>(this.actions$,
    fromItemActions.EDIT_SUCCESS).pipe(
      exhaustMap(payload => {

        const popup$ = _success$('編輯成功');
        const direct$ = _route$('./pages/master/item', {});

        return concat(popup$, direct$);
      })
    );


  @Effect()
  editFailed$ = _entry$<string>(this.actions$,
    fromItemActions.EDIT_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );





  @Effect()
  disable$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
    fromItemActions.DISABLE).pipe(
      exhaustMap(payload => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResultBase>(`${commonBu}/Item/Disable`, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromItemActions.disableSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromItemActions.disableFailedAction(new ResultPayload<string>(
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
  disableSuccess$ = _entry$<any>(this.actions$,
    fromItemActions.DISABLE_SUCCESS).pipe(
      exhaustMap(payload => {
        const popup$ = _success$('停用成功');
        payload.cb && payload.cb();
        return concat(popup$);
      })
    );


  @Effect()
  disableFailed$ = _entry$<any>(this.actions$,
    fromItemActions.DISABLE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload);
      })
    );




  @Effect()
  disableRange$ = _entry$<EntrancePayload<Array<ItemListViewModel>>>(this.actions$,
    fromItemActions.DISABLE_RANGE).pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResultBase>(`${commonBu}/Item/DisableRange`, null, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromItemActions.disableRangeSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<boolean>) =>
          of(new fromItemActions.disableRangeFailedAction(new ResultPayload<string>(
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
    fromItemActions.DISABLE_RANGE_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('停用成功');
        return concat(popup$);
      })
    );


  @Effect()
  disableRangeFailed$ = _entry$<ResultPayload<string>>(this.actions$,
    fromItemActions.DISABLE_RANGE_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failed$(payload.data);
      })
    );


  @Effect()
  upload$ = _entry$<EntrancePayload<FormData>>(this.actions$,
    fromItemActions.UPLOAD).pipe(
      exhaustMap((payload) => {
        
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResultBase>(`${commonBu}/Item/Upload`, null, payload.data);


        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<boolean>) =>
          of(new fromItemActions.uploadSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<ItemExportViewModel[]>) =>
          of(new fromItemActions.uploadFailedAction(new ResultPayload<any>(
            result.extend,
            payload.failed,
            payload.dataExport,
            result.message
          )));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })
    );


  @Effect()
  uploadSuccess$ = _entry$<ResultPayload<string>>(this.actions$,
    fromItemActions.UPLOAD_SUCCESS).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        const popup$ = _success$('上傳成功');
        return concat(popup$);
      })
    );


  @Effect()
  uploadFailed$ = _entry$<ResultPayload<ItemExportViewModel[]>>(this.actions$,
    fromItemActions.UPLOAD_FAILED).pipe(
      exhaustMap(payload => {
        payload.cb && payload.cb();
        return _failedExport$(payload.msg, payload.dataExport, payload.data);
      })
    );


}