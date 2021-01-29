import { Action } from '@ngrx/store';
import { CaseTagDetailViewModel } from '../../../../model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const ADD = '[CASE_TAG] ADD';
export const ADD_SUCCESS = '[CASE_TAG] ADD SUCCESS';
export const ADD_FAILED = '[CASE_TAG] ADD FAILED';
export const EDIT = '[CASE_TAG] EDIT';
export const EDIT_SUCCESS = '[CASE_TAG] EDIT SUCCESS';
export const EDIT_FAILED = '[CASE_TAG] EDIT FAILED';
export const LOAD_ENTRY = '[CASE_TAG] LOAD ENTRY';
export const LOAD_DETAIL = '[CASE_TAG] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[CASE_TAG] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[CASE_TAG] LOAD DETAIL FAILED';
export const DISABLE = '[CASE_TAG] DISABLE';
export const DISABLE_SUCCESS = '[CASE_TAG] DISABLE SUCCESS';
export const DISABLE_FAILED = '[CASE_TAG] DISABLE FAILED';


export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: CaseTagDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: CaseTagDetailViewModel = new CaseTagDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: CaseTagDetailViewModel) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS
  constructor(public payload: string) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT
  constructor(public payload: CaseTagDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS
  constructor(public payload: string) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED
  constructor(public payload: string) { }
}

export class disableAction implements Action {
  public type: string = DISABLE
  constructor(public payload: EntrancePayload<{ ID?: number }>) { }
}

export class disableSuccessAction implements Action {
  public type: string = DISABLE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class disableFailedAction implements Action {
  public type: string = DISABLE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}


export type Actions =
  loadDetailAction |
  loadDetailSuccessAction |
  loadDetailFailedAction |
  addAction |
  addSuccessAction |
  addFailedAction |
  loadEntryAction |
  disableAction |  
  disableFailedAction |
  disableSuccessAction;
