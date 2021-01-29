import { Action } from '@ngrx/store';
import { CaseHeaderqurterStoreSearchViewModel } from 'src/app/model/search.model';
import { ResultPayload, EntrancePayload } from 'src/app/model/common.model';

export const CASE_HS_REPORT = '[CASE_SEARCH] CASE_HS_REPORT';
export const CASE_HS_REPORT_SUCCESS = '[CASE_SEARCH] CASE_HS_REPORT_SUCCESS';
export const CASE_HS_REPORT_FAILED = '[CASE_SEARCH] CASE_HS_REPORT_FAILED';
export const CASE_HS_GETLIST = '[CASE_SEARCH] CASE_HS_GETLIST';
export const CASE_HS_GETLIST_SUCCESS = '[CASE_SEARCH] CASE_HS_GETLIST_SUCCESS';
export const CASE_HS_GETLIST_FAILED = '[CASE_SEARCH] CASE_HS_GETLIST_FAILED';

export class headerqurterStoreReport implements Action {
  public type: string = CASE_HS_REPORT;
  constructor(public payload: CaseHeaderqurterStoreSearchViewModel) { }
}


export class headerqurterStoreReportSuccess implements Action {
  public type: string = CASE_HS_REPORT_SUCCESS;
  constructor(public payload: Blob) { }
}


export class headerqurterStoreReportFailed implements Action {
  public type: string = CASE_HS_REPORT_FAILED;
  constructor(public payload: string) { }
}


export class headerqurterStoreGetList implements Action {
  public type: string = CASE_HS_GETLIST;
  constructor(public payload: EntrancePayload<CaseHeaderqurterStoreSearchViewModel>) { }
}


export class headerqurterStoreGetListSuccess implements Action {
  public type: string = CASE_HS_GETLIST_SUCCESS;
  constructor(public payload: ResultPayload<any[]>) { }
}


export class headerqurterStoreGetListFailed implements Action {
  public type: string = CASE_HS_GETLIST_FAILED;
  constructor(public payload: ResultPayload<any>) { }
}


export type Actions = headerqurterStoreReport | headerqurterStoreReportSuccess | headerqurterStoreReportFailed |
headerqurterStoreGetList | headerqurterStoreGetListSuccess | headerqurterStoreGetListFailed;
