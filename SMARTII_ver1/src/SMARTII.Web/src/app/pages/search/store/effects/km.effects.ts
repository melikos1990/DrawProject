import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';


import * as fromKMActions from '../actions/km.actions';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { exhaustMap } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { of, concat } from 'rxjs';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { ObjectService } from 'src/app/shared/service/object.service';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { KMClassificationNodeViewModel, KMDetailViewModel } from 'src/app/model/master.model';


@Injectable()
export class KMEffects {
    constructor(
        private http: HttpService,
        private actions$: Actions,
        private objectService: ObjectService) { }


    @Effect()
    loadTree$ = _entry$<any>(this.actions$,
        fromKMActions.LOAD_TREE)
        .pipe(
            exhaustMap(() => {

                const retrieve$ = this.http.post<AspnetJsonResult<KMClassificationNodeViewModel[]>>('Master/KMClassification/GetKMTree', null, {});

                const handleSuccess = (result: AspnetJsonResult<KMClassificationNodeViewModel[]>) =>
                    of(new fromKMActions.loadTreeSuccessAction(result.element));

                const handleFailed = (result: AspnetJsonResult<string>) =>
                    of(new fromKMActions.loadTreeFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    loadTreeFailed$ = _entry$<string>(this.actions$,
        fromKMActions.LOAD_TREE_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );


    @Effect()
    loadDetail$ = _entry$<{ ID?: number }>(this.actions$,
        fromKMActions.LOAD_DETAIL)
        .pipe(
            exhaustMap((payload) => {

                const retrieve$ = this.http.get<AspnetJsonResult<KMDetailViewModel>>(
                    'Master/KMClassification/Get', payload);

                const handleSuccess = (result: AspnetJsonResult<KMDetailViewModel>) =>
                    of(new fromKMActions.loadDetailSuccessAction(result.element));

                const handleFailed = (result: AspnetJsonResultBase) =>
                    of(new fromKMActions.loadDetailFailedAction(result.message));

                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })

        );

    @Effect()
    loadDetailFailed$ = _entry$<string>(this.actions$,
        fromKMActions.LOAD_DETAIL_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );



    @Effect()
    add$ = _entry$<KMDetailViewModel>(this.actions$,
        fromKMActions.ADD).pipe(
            exhaustMap((payload) => {

                const formData = this.objectService.convertToFormData(payload);

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/Create',
                    null,
                    formData
                );
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.addSuccessAction(result.message));

                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.addFailedAction(result.message));

                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    addSuccess$ = _entry$<string>(this.actions$,
        fromKMActions.ADD_SUCCESS).pipe(
            exhaustMap(payload => {

                const popup$ = _success$('新增成功');
                const direct$ = _route$('./pages/search/km', {});

                return concat(popup$, direct$);
            })
        );


    @Effect()
    addFailed$ = _entry$<string>(this.actions$,
        fromKMActions.ADD_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );

    @Effect()
    edit$ = _entry$<KMDetailViewModel>(this.actions$,
        fromKMActions.EDIT).pipe(
            exhaustMap((payload) => {

                const formData = this.objectService.convertToFormData(payload);

                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/Update',
                    null,
                    formData
                );

                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.editSuccessAction(result.message));

                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.editFailedAction(result.message));

                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    editSuccess$ = _entry$<string>(this.actions$,
        fromKMActions.EDIT_SUCCESS).pipe(
            exhaustMap(payload => {

                const popup$ = _success$('編輯成功');
                const direct$ = _route$('./pages/search/km', {});

                return concat(popup$, direct$);
            })
        );


    @Effect()
    editFailed$ = _entry$<string>(this.actions$,
        fromKMActions.EDIT_FAILED).pipe(
            exhaustMap(payload => {
                return _failed$(payload);
            })
        );

    @Effect()
    delete$ = _entry$<EntrancePayload<{ ID: number }>>(this.actions$,
        fromKMActions.DELETE).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.get<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/Delete', payload.data);


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.deleteSuccessAction(new ResultPayload<string>(
                        result.message,
                        payload.success
                    )));

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.deleteFailedAction(new ResultPayload<string>(
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
    deleteSuccess$ = _entry$<any>(this.actions$,
        fromKMActions.DELETE_SUCCESS).pipe(
            exhaustMap(payload => {
                const popup$ = _success$('刪除成功');
                payload.cb && payload.cb();
                return concat(popup$);
            })
        );


    @Effect()
    deleteFailed$ = _entry$<any>(this.actions$,
        fromKMActions.DELETE_FAILED).pipe(
            exhaustMap(payload => {
                payload.cb && payload.cb();
                return _failed$(payload);
            })
        );




    @Effect()
    deleteClassification$ = _entry$<{ ID: number }>(this.actions$,
        fromKMActions.DELETE_CLASSIFICATION).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/DeleteClassification', payload, {});


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.deleteClassificationSuccessAction());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.deleteClassificationFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    deleteClassificationSuccess$ = _entry$<any>(this.actions$,
        fromKMActions.DELETE_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap(payload => {


                const popup$ = _success$('刪除成功');
                const success$ = of(new fromKMActions.loadTreeAction());
                return concat(popup$,success$);
            })
        );


    @Effect()
    deleteClassificationFailed$ = _entry$<any>(this.actions$,
        fromKMActions.DELETE_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {

                return _failed$(payload);
            })
        );



    @Effect()
    renameClassification$ = _entry$<{ ID: number, name: string }>(this.actions$,
        fromKMActions.RENAME_CLASSIFICATION).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/RenameClassification', payload, {});


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.RenameClassificationSuccessAction());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.RenameClassificationFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    renameClassificationSuccess$ = _entry$<any>(this.actions$,
        fromKMActions.RENAME_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap(payload => {

                const popup$ = _success$('編輯成功');
                const success$ = of(new fromKMActions.loadTreeAction());
                return concat(popup$,success$);
            })
        );


    @Effect()
    renameClassificationFailed$ = _entry$<any>(this.actions$,
        fromKMActions.RENAME_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {

                return _failed$(payload);
            })
        );


    @Effect()
    addClassification$ = _entry$<{ parentID: number, name: string }>(this.actions$,
        fromKMActions.ADD_CLASSIFICATION).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/CreateClassification', payload, {});


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.AddClassificationSuccessAction());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.AddClassificationFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    addClassificationSuccess$ = _entry$<any>(this.actions$,
        fromKMActions.ADD_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap(payload => {

                const popup$ = _success$('新增成功');
                const success$ = of(new fromKMActions.loadTreeAction());
                return concat(popup$,success$);
            })
        );


    @Effect()
    addClassificationFailed$ = _entry$<any>(this.actions$,
        fromKMActions.ADD_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {

                return _failed$(payload);
            })
        );


    @Effect()
    addRootClassification$ = _entry$<{ nodeID: number, name: string }>(this.actions$,
        fromKMActions.ADD_ROOT_CLASSIFICATION).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/CreateRootClassification', payload, {});


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.AddClassificationSuccessAction());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.AddClassificationFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    addRootClassificationSuccess$ = _entry$<any>(this.actions$,
        fromKMActions.ADD_ROOT_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap(() => {

                const popup$ = _success$('新增成功');
                const success$ = of(new fromKMActions.loadTreeAction());
                return concat(popup$,success$);
            })
        );


    @Effect()
    addRootClassificationFailed$ = _entry$<any>(this.actions$,
        fromKMActions.ADD_ROOT_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {

                return _failed$(payload);
            })
        );


    @Effect()
    dragClassification$ = _entry$<{ ID?: number, parentID?: number }>(this.actions$,
        fromKMActions.DRAG_CLASSIFICATION).pipe(
            exhaustMap(payload => {

                // 主要要做的事情 , 這邊是透過 http client 撈取資料
                const retrieve$ = this.http.post<AspnetJsonResult<boolean>>(
                    'Master/KMClassification/DragClassification', payload, {});


                // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
                const handleSuccess = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.DragClassificationSuccessAction());

                // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
                const handleFailed = (result: AspnetJsonResult<boolean>) =>
                    of(new fromKMActions.DragClassificationFailedAction(result.message));

                // 判斷是否成功或是失敗
                const consider = (result: AspnetJsonResultBase) => result.isSuccess;

                // 實際進行http 行為
                const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

                return _loadingWork$(work$);
            })
        );


    @Effect()
    dragClassificationSuccess$ = _entry$<any>(this.actions$,
        fromKMActions.DRAG_CLASSIFICATION_SUCCESS).pipe(
            exhaustMap(() => {

                return of(new fromKMActions.loadTreeAction());
            })
        );


    @Effect()
    dragClassificationFailed$ = _entry$<any>(this.actions$,
        fromKMActions.DRAG_CLASSIFICATION_FAILED).pipe(
            exhaustMap(payload => {

                const popup$ = _failed$(payload);
                const reload$ = of(new fromKMActions.loadTreeAction());
                return concat(popup$, reload$)
            })
        );



}
