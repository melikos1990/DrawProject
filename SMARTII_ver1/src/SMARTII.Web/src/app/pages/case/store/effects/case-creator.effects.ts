
import { Actions, Effect } from '@ngrx/effects';
import { ObjectService } from 'src/app/shared/service/object.service';
import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';

import * as fromCaseCreatorActions from '../actions/case-creator.actions';
import { exhaustMap, tap, flatMap, mergeMap, withLatestFrom, catchError, switchMap, concatMap, map } from 'rxjs/operators';
import { AspnetJsonResult, AspnetJsonResultBase, EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseSourceViewModel, CaseViewModel, CaseAssignmentViewModel, CaseAssignmentComplaintInvoiceViewModel, CaseAssignmentComplaintNoticeViewModel, CaseAssignmentBaseViewModel, CaseAssignmentCommunicateViewModel, CaseFocusType } from 'src/app/model/case.model';
import { of, empty, concat } from 'rxjs';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { _failed$ } from 'src/app/shared/ngrx/alert.ngrx';
import { EmailSenderViewModel } from 'src/app/model/shared.model';

import { State as fromCaseReducers } from '../../store/reducers'
import { Store } from '@ngrx/store';

@Injectable()
export class CaseCreatorEffects {
  constructor(
    private store$: Store<fromCaseReducers>,
    private http: HttpService,
    private actions$: Actions,
    private objectService: ObjectService) { }


  @Effect({ dispatch: false })
  clearAllAction$ = _entry$<any>(this.actions$,
    fromCaseCreatorActions.CLEAR_ALL)
    .pipe(
      mergeMap(() => this.http.get('Case/Case/LeaveAll', {})),
      catchError(() => empty())

    );

  @Effect({ dispatch: false })
  removeCaseTabAction$ = _entry$<{
    sourceID: string,
    caseID: string
  }>(this.actions$,
    fromCaseCreatorActions.REMOVE_CASE_TAB)
    .pipe(
      mergeMap((payload) =>
        this.http.get('Case/Case/LeaveRoom', { caseID: payload.caseID })
      ),
      catchError(() => empty())

    );

  @Effect({ dispatch: false })
  removeSorceTabAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.REMOVE_SOURCE_TAB)
    .pipe(
      mergeMap((payload) =>
        this.http.get('Case/Case/LeaveAllRoom', { sourceID: payload })
      ),
      catchError(() => empty())
    );

  @Effect()
  loadCaseAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE)
    .pipe(
      flatMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CaseViewModel>>('Case/Case/GetCase', {
          caseID: payload
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseViewModel>) =>
          of(new fromCaseCreatorActions.loadCaseSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.loadCaseFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return work$;
      })

    );

  @Effect()
  loadCaseFailedAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  loadCaseSuccessAction$ = _entry$<CaseViewModel>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SUCCESS).pipe(
      map(payload => {
        return new fromCaseCreatorActions.joinRoomAction(payload.CaseID);
      }),
      // concatMap(payload => {

      //   const main$ = this.http.get('Case/Case/JoinRoom', { caseID: payload.CaseID }).pipe(
      //     switchMap(() => {
      //       return concat(
      //         of(new fromCaseCreatorActions.unActiveCaseTabAction()),
      //         of(new fromCaseCreatorActions.activeCaseTabAction(payload.CaseID))
      //       );
      //     })
      //   );

      //   return main$
      // }),

    );

  @Effect()
  loadSourceCheckAction$ = _entry$<EntrancePayload<{ caseID: string, sourceTab: string }>>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE_CHECK)
    .pipe(
      exhaustMap((payload) => {
        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/GetCaseSourceCheck', {
          caseID: payload.data.caseID,
          sourceTab: payload.data.sourceTab
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) =>
          of(new fromCaseCreatorActions.loadCaseSourceSuccessAction({ ...result.element, FocusCaseID: payload.data.caseID }));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.loadCaseSourceFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return work$;
      })

    );

  @Effect()
  loadSourceAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/GetCaseSource', {
          caseID: payload
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) =>
          of(new fromCaseCreatorActions.loadCaseSourceSuccessAction({ ...result.element, FocusCaseID: payload }));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.loadCaseSourceFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return work$;
      })

    );

  @Effect()
  loadSourceFailedAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  loadSourceSuccessAction$ = _entry$<CaseSourceViewModel>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE_SUCCESS).pipe(
      tap((payload) => {
        //payload.cb && payload.cb();
      }),
      exhaustMap((payload) => {
        return of(new fromCaseCreatorActions.activeSourceTabAction(payload.SourceID))
      })
    );

  @Effect()
  loadSourceNativeAction$ = _entry$<{ sourceID: string, navigate: CaseFocusType }>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE_NATIVE)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/GetNativeCaseSource', {
          caseSourceID: payload.sourceID
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) => {
          result.element.navigate = payload.navigate;
          return of(new fromCaseCreatorActions.loadCaseSourceNativeSuccessAction(result.element));
        }

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.loadCaseSourceNativeFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);
      })

    );

  @Effect()
  loadSourceNativeFailedAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE_NATIVE_FAILED).pipe(
      exhaustMap(payload => {
        return _failed$(payload);
      })
    );

  @Effect()
  loadSourceNativeSuccessAction$ = _entry$<CaseSourceViewModel>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_SOURCE_NATIVE_SUCCESS).pipe(
      tap(() => {

      }),
      exhaustMap((payload) => {
        return of(new fromCaseCreatorActions.activeSourceTabAction(payload.SourceID))
      })
    );


  @Effect()
  addCaseSourceAction$ = _entry$<EntrancePayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/SaveCaseSource', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) =>
          of(new fromCaseCreatorActions.addCaseSourceSuccessAction(new ResultPayload<CaseSourceViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseSourceFailedAction(new ResultPayload<string>(
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
  addCaseSourceFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  addCaseSourceSuccessAction$ = _entry$<ResultPayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );



  @Effect()
  editCaseAction$ = _entry$<EntrancePayload<{ CaseViewModel: CaseViewModel, roleID: number }>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE)
    .pipe(
      exhaustMap((payload) => {
        var formData = this.objectService.convertToFormData(payload.data.CaseViewModel);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseViewModel>>('Case/Case/UpdateCase', { roleID: payload.data.roleID }, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseViewModel>) =>
          of(new fromCaseCreatorActions.editCaseSuccessAction(new ResultPayload<CaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.editCaseFailedAction(new ResultPayload<string>(
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
  editCaseFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  editCaseSuccessAction$ = _entry$<ResultPayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );

  @Effect()
  editCaseSourceAction$ = _entry$<EntrancePayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_SOURCE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/UpdateCaseSource', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) =>
          of(new fromCaseCreatorActions.editCaseSourceSuccessAction(new ResultPayload<CaseSourceViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.editCaseSourceFailedAction(new ResultPayload<string>(
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
  editCaseSourceFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_SOURCE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  editCaseSourceSuccessAction$ = _entry$<ResultPayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_SOURCE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );


  @Effect()
  addCaseSourceCompleteAction$ = _entry$<EntrancePayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE_COMPLETE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/SaveCaseSourceComplete', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) =>
          of(new fromCaseCreatorActions.addCaseSourceCompleteSuccessAction(new ResultPayload<CaseSourceViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseSourceCompleteFailedAction(new ResultPayload<string>(
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
  addCaseSourceCompleteFastFinishedAction$ = _entry$<EntrancePayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE_COMPLETE_AND_FAST_FINISH)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/SaveCaseSourceComplete', {
          isFastFinished: true
        }, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseSourceViewModel>) =>
          of(new fromCaseCreatorActions.addCaseSourceCompleteSuccessAction(new ResultPayload<CaseSourceViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseSourceCompleteFailedAction(new ResultPayload<string>(
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
  addCaseSourceCompleteFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE_COMPLETE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  addCaseSourceCompleteSuccessAction$ = _entry$<ResultPayload<CaseSourceViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SOURCE_COMPLETE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );



  @Effect()
  loadCaseIDsAction$ = _entry$<EntrancePayload<string>>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_IDS)
    .pipe(
      exhaustMap((payload) => {

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string[]>>('Case/Case/GetCaseIDs', {
          sourceID: payload.data
        });

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string[]>) => of(new fromCaseCreatorActions.loadCaseIDsSuccessAction(new ResultPayload(
          {
            sorceID: payload.data,
            caseIDs: result.element
          },
          payload.success
        )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.loadCaseIDsFailedAction(result.message));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return work$;
      })

    );

  @Effect()
  loadCaseIDsFailedAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_IDS_FAILED).pipe(
      exhaustMap(payload => {

        return _failed$(payload);
      })
    );

  @Effect({
    dispatch: false
  })
  loadCaseIDsSuccessAction$ = _entry$<ResultPayload<any>>(this.actions$,
    fromCaseCreatorActions.LOAD_CASE_IDS_SUCCESS).pipe(
      tap((payload) => {
        payload.cb && payload.cb();
      })
    );

  @Effect()
  addCaseAction$ = _entry$<EntrancePayload<CaseViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseViewModel>>('Case/Case/SaveCase', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseViewModel>) =>
          of(new fromCaseCreatorActions.addCaseSuccessAction(new ResultPayload<CaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseFailedAction(new ResultPayload<string>(
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
  addCaseFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  addCaseSuccessAction$ = _entry$<ResultPayload<CaseViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );


  @Effect()
  finishCaseAction$ = _entry$<EntrancePayload<CaseViewModel>>(this.actions$,
    fromCaseCreatorActions.FINISH_CASE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseViewModel>>('Case/Case/FinishCase', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseViewModel>) =>
          of(new fromCaseCreatorActions.finishCaseSuccessAction(new ResultPayload<CaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.finishCaseFailedAction(new ResultPayload<string>(
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
  finishCaseFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.FINISH_CASE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  finishCaseSuccessAction$ = _entry$<ResultPayload<CaseViewModel>>(this.actions$,
    fromCaseCreatorActions.FINISH_CASE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );



  @Effect()
  addCaseAssignmentInvoiceAction$ = _entry$<EntrancePayload<CaseAssignmentComplaintInvoiceViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_ASSIGNMENT_INVOCIE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentComplaintInvoiceViewModel>>('Case/Case/SaveCaseAssignmentInvoice', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentBaseViewModel>) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateSuccessAction(new ResultPayload<CaseAssignmentBaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateFailedAction(new ResultPayload<string>(
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
  addCaseAssignmentNoticeAction$ = _entry$<EntrancePayload<CaseAssignmentComplaintNoticeViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_ASSIGNMENT_NOTICE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentComplaintNoticeViewModel>>('Case/Case/SaveCaseAssignmentNotice', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentBaseViewModel>) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateSuccessAction(new ResultPayload<CaseAssignmentBaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateFailedAction(new ResultPayload<string>(
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
  addCaseAssignmentAction$ = _entry$<EntrancePayload<CaseAssignmentViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_ASSIGNMENT)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/Case/SaveCaseAssignment', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentBaseViewModel>) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateSuccessAction(new ResultPayload<CaseAssignmentBaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateFailedAction(new ResultPayload<string>(
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
  addCaseAssignmentCommunicateAction$ = _entry$<EntrancePayload<CaseAssignmentCommunicateViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_ASSIGNMENT_COMMUNICATE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentCommunicateViewModel>>('Case/Case/SaveCaseAssignmentCommunicate', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentBaseViewModel>) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateSuccessAction(new ResultPayload<CaseAssignmentBaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.addCaseAssignmentAggregateFailedAction(new ResultPayload<string>(
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
  addCaseAssignmentAggregateFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_ASSIGNMENT_AGGREGATE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  addCaseAssignmentAggregateSuccessAction$ = _entry$<ResultPayload<CaseAssignmentBaseViewModel>>(this.actions$,
    fromCaseCreatorActions.ADD_CASE_ASSIGNMENT_AGGREGATE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );

  @Effect()
  sendCaseAssignmentInvoiceAction$ = _entry$<EntrancePayload<{
    identityID: number,
    model: EmailSenderViewModel
  }>>(this.actions$,
    fromCaseCreatorActions.SEND_CASE_ASSIGNMENT_INVOICE)
    .pipe(
      exhaustMap((payload) => {

        console.log(payload)

        var formData = this.objectService.convertToFormData(payload.data.model);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/Case/SendCaseAssignmentInvoice', {
          identityID: payload.data.identityID
        }, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<Boolean>) =>
          of(new fromCaseCreatorActions.sendCaseAssignmentInvoiceSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.sendCaseAssignmentInvoiceFailedAction(new ResultPayload<string>(
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
  sendCaseAssignmentInvoiceFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.SEND_CASE_ASSIGNMENT_INVOICE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  sendCaseAssignmentInvoiceSuccessAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.SEND_CASE_ASSIGNMENT_INVOICE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );


  @Effect()
  resendCaseAssignmentInvoiceAction$ = _entry$<EntrancePayload<{
    identityID: string,
    model: CaseAssignmentComplaintInvoiceViewModel,
  }>>(this.actions$,
    fromCaseCreatorActions.RESEND_CASE_ASSIGNMENT_INVOICE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data.model);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentComplaintInvoiceViewModel>>('Case/Case/ResendCaseAssignmentInvoice', {
          identityID: payload.data.identityID
        }, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromCaseCreatorActions.resendCaseAssignmentInvoiceSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.resendCaseAssignmentInvoiceFailedAction(new ResultPayload<string>(
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
  resendCaseAssignmentInvoiceFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.RESEND_CASE_ASSIGNMENT_INVOICE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  resendCaseAssignmentInvoiceSuccessAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.RESEND_CASE_ASSIGNMENT_INVOICE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );


  @Effect()
  caseFinishedReplyMailAction$ = _entry$<EntrancePayload<{
    caseID: string,
    model: EmailSenderViewModel
  }>>(this.actions$,
    fromCaseCreatorActions.CASE_FINISHED_REPLY_MAIL)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data.model);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<string>>('Case/Case/CaseFinishedMailReply', {
          caseID: payload.data.caseID
        }, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromCaseCreatorActions.caseFinishedReplyMailSuccessAction(new ResultPayload<string>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.caseFinishedReplyMailFailedAction(new ResultPayload<string>(
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
  caseFinishedReplyMailFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.CASE_FINISHED_REPLY_MAIL_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  caseFinishedReplyMailSuccessAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.CASE_FINISHED_REPLY_MAIL_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );


  @Effect()
  editCaseAssignmentInvoiceAction$ = _entry$<EntrancePayload<CaseAssignmentComplaintInvoiceViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_ASSIGNMENT_INVOICE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/UpdateCaseAssignmentInvoice', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentComplaintInvoiceViewModel>) =>
          of(new fromCaseCreatorActions.editCaseAssignmentInvoiceSuccessAction(new ResultPayload<CaseAssignmentComplaintInvoiceViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.editCaseAssignmentInvoiceFailedAction(new ResultPayload<string>(
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
  editCaseAssignmentInvoiceFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_ASSIGNMENT_INVOICE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  editCaseAssignmentInvoiceSuccessAction$ = _entry$<ResultPayload<CaseAssignmentComplaintInvoiceViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_ASSIGNMENT_INVOICE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );

  @Effect()
  editCaseAssignmentNoticeAction$ = _entry$<EntrancePayload<CaseAssignmentComplaintNoticeViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_ASSIGNMENT_NOTICE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseSourceViewModel>>('Case/Case/UpdateCaseAssignmentNotice', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentComplaintNoticeViewModel>) =>
          of(new fromCaseCreatorActions.editCaseAssignmentNoticeSuccessAction(new ResultPayload<CaseAssignmentComplaintNoticeViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.editCaseAssignmentNoticeFailedAction(new ResultPayload<string>(
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
  editCaseAssignmentNoticeFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_ASSIGNMENT_NOTICE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  editCaseAssignmentNoticeSuccessAction$ = _entry$<ResultPayload<CaseAssignmentComplaintNoticeViewModel>>(this.actions$,
    fromCaseCreatorActions.EDIT_CASE_ASSIGNMENT_NOTICE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );

  @Effect()
  finishCaseAssignmentAction$ = _entry$<EntrancePayload<CaseAssignmentViewModel>>(this.actions$,
    fromCaseCreatorActions.FINISH_CASEASSIGNMENT)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/Case/FinishedCaseAssignment', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentViewModel>) =>
          of(new fromCaseCreatorActions.finishCaseAssignmentSuccessAction(new ResultPayload<CaseAssignmentViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.finishCaseAssignmentFailedAction(new ResultPayload<string>(
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
  finishCaseAssignmentFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.FINISH_CASEASSIGNMENT_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  finishCaseAssignmentSuccessAction$ = _entry$<ResultPayload<CaseAssignmentViewModel>>(this.actions$,
    fromCaseCreatorActions.FINISH_CASEASSIGNMENT_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );


  @Effect()
  rejectCaseAssignmentAction$ = _entry$<EntrancePayload<CaseAssignmentViewModel>>(this.actions$,
    fromCaseCreatorActions.REJECT_CASEASSIGNMENT)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/Case/RejectCaseAssigment', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseAssignmentViewModel>) =>
          of(new fromCaseCreatorActions.rejectCaseAssignmentSuccessAction(new ResultPayload<CaseAssignmentViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.rejectCaseAssignmentFailedAction(new ResultPayload<string>(
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
  rejectCaseAssignmentFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.REJECT_CASEASSIGNMENT_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  rejectCaseAssignmentSuccessAction$ = _entry$<ResultPayload<CaseAssignmentViewModel>>(this.actions$,
    fromCaseCreatorActions.REJECT_CASEASSIGNMENT_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );




  @Effect()
  unlockAction$ = _entry$<EntrancePayload<CaseViewModel>>(this.actions$,
    fromCaseCreatorActions.UNLOCK_CASE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseViewModel>>('Case/Case/UnlockCase', null, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<CaseViewModel>) =>
          of(new fromCaseCreatorActions.unLockSuccessAction(new ResultPayload<CaseViewModel>(
            result.element,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.unLockFailedAction(new ResultPayload<string>(
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
  unlockFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.UNLOCK_CASE_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  unlockSuccessAction$ = _entry$<ResultPayload<CaseViewModel>>(this.actions$,
    fromCaseCreatorActions.UNLOCK_CASE_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );

  @Effect({
    dispatch: false
  })
  joinRoomAction$ = _entry$<string>(this.actions$,
    fromCaseCreatorActions.JOIN_ROOM).pipe(
      tap(payload => {
        this.http.get('Case/Case/JoinRoom', { caseID: payload }).subscribe()
      }),
    );



  @Effect()
  sendCaseAssignmentAction$ = _entry$<EntrancePayload<{
    assignmentID: number,
    caseID: string,
    model: EmailSenderViewModel
  }>>(this.actions$,
    fromCaseCreatorActions.SEND_CASE_ASSIGNMENT)
    .pipe(
      exhaustMap((payload) => {


        var formData = this.objectService.convertToFormData(payload.data.model);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.post<AspnetJsonResult<CaseAssignmentViewModel>>('Case/Case/SendCaseAssignment', {
          assignmentID: payload.data.assignmentID,
          caseID: payload.data.caseID,
        }, formData);

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<Boolean>) =>
          of(new fromCaseCreatorActions.sendCaseAssignmentInvoiceSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.sendCaseAssignmentInvoiceFailedAction(new ResultPayload<string>(
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
  sendCaseAssignmentFailedAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.SEND_CASE_ASSIGNMENT_FAILED).pipe(
      exhaustMap(payload => {

        payload.cb && payload.cb();

        return _failed$(payload.data);
      })
    );

  @Effect({
    dispatch: false
  })
  sendCaseAssignmentSuccessAction$ = _entry$<ResultPayload<string>>(this.actions$,
    fromCaseCreatorActions.SEND_CASE_ASSIGNMENT_SUCCESS).pipe(
      tap(payload => {
        payload.cb && payload.cb(payload.data);

      })
    );

  @Effect()
  cancelInoviceAction$ = _entry$<EntrancePayload<{InvoiceIdentityID : number}>>(this.actions$,
    fromCaseCreatorActions.CALCEL_INVOICE)
    .pipe(
      exhaustMap((payload) => {

        var formData = this.objectService.convertToFormData(payload.data);

        // 主要要做的事情 , 這邊是透過 http client 撈取資料
        const retrieve$ = this.http.get<AspnetJsonResult<string>>(
          'Case/Case/CancelInovice', {InvoiceIdentityID : payload.data.InvoiceIdentityID});

        // 成功時將呼叫 loadDetailSuccess$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromCaseCreatorActions.sendCaseAssignmentSuccessAction(new ResultPayload<string>(
            result.message,
            payload.success
          )));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResultBase) =>
          of(new fromCaseCreatorActions.sendCaseAssignmentFailedAction(new ResultPayload<string>(
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

}


