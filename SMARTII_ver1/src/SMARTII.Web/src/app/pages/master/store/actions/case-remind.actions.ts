import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseRemindDetailViewModel, CaseRemindListViewModel } from 'src/app/model/master.model';


export const ADD = '[CASE_REMIND] ADD';
export const ADD_SUCCESS = '[CASE_REMIND] ADD SUCCESS';
export const ADD_FAILED = '[CASE_REMIND] ADD FAILED';
export const EDIT = '[CASE_REMIND] EDIT';
export const EDIT_SUCCESS = '[CASE_REMIND] EDIT SUCCESS';
export const EDIT_FAILED = '[CASE_REMIND] EDIT FAILED';
export const DELETE = '[CASE_REMIND] DELETE';
export const DELETE_SUCCESS = '[CASE_REMIND] DELETE SUCCESS';
export const DELETE_FAILED = '[CASE_REMIND] DELETE FAILED';
export const DELETE_RANGE = '[CASE_REMIND] DELETE RANGE';
export const DELETE_RANGE_SUCCESS = '[CASE_REMIND] DELETE RANGE SUCCESS';
export const DELETE_RANGE_FAILED = '[CASE_REMIND] DELETE RANGE FAILED';
export const CONFIRM = '[CASE_REMIND] CONFIRM';
export const CONFIRM_SUCCESS = '[CASE_REMIND] CONFIRM SUCCESS';
export const CONFIRM_FAILED = '[CASE_REMIND] CONFIRM FAILED';
export const LOAD_ENTRY = '[CASE_REMIND] LOAD ENTRY';
export const LOAD_DETAIL = '[CASE_REMIND] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[CASE_REMIND] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[CASE_REMIND] LOAD DETAIL FAILED';
export const CHECK_CASE_ID = '[CASE_REMIND] CHECK_CASE_ID';
export const CHECK_CASE_ID_SUCCESS = '[CASE_REMIND] CHECK_CASE_ID_SUCCESS';
export const CHECK_CASE_ID_FAILED = '[CASE_REMIND] CHECK_CASE_ID_FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: CaseRemindDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: CaseRemindDetailViewModel = new CaseRemindDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: CaseRemindDetailViewModel) { }
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
  constructor(public payload: CaseRemindDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS
  constructor(public payload: string) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED
  constructor(public payload: string) { }
}


export class deleteAction implements Action {
  public type: string = DELETE
  constructor(public payload: EntrancePayload<{ ID?: number }>) { }
}

export class deleteSuccessAction implements Action {
  public type: string = DELETE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteFailedAction implements Action {
  public type: string = DELETE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}


export class deleteRangeAction implements Action {
  public type: string = DELETE_RANGE;
  constructor(public payload: EntrancePayload<Array<CaseRemindListViewModel>>) { }
}

export class deleteRangeSuccessAction implements Action {
  public type: string = DELETE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteRangeFailedAction implements Action {
  public type: string = DELETE_RANGE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}

export class confirmAction implements Action {
  public type: string = CONFIRM
  constructor(public payload: { ID: number }) { }
}

export class confirmSuccessAction implements Action {
  public type: string = CONFIRM_SUCCESS
  constructor(public payload: string) { }
}

export class confirmFailedAction implements Action {
  public type: string = CONFIRM_FAILED
  constructor(public payload: string) { }
}

export class checkCaseIDAction implements Action {
  public type: string = CHECK_CASE_ID
  constructor(public payload: EntrancePayload<{ caseID: string }>) { }
}

export class checkCaseIDSuccessAction implements Action {
  public type: string = CHECK_CASE_ID_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class checkCaseIDFailedAction implements Action {
  public type: string = CHECK_CASE_ID_FAILED;
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
  deleteAction |
  deleteFailedAction |
  deleteSuccessAction |
  deleteRangeAction |
  deleteRangeFailedAction |
  deleteRangeSuccessAction |
  confirmAction |
  confirmSuccessAction |
  confirmFailedAction |
  checkCaseIDAction |
  checkCaseIDSuccessAction |
  checkCaseIDFailedAction;
