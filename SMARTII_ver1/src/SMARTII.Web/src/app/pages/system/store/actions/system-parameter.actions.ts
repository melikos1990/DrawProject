import { Action } from '@ngrx/store';
import { SystemParameterDetailViewModel, SystemParameterListViewModel } from 'src/app/model/system.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const ADD = '[SYSTEM_PARAMETER] ADD';
export const ADD_SUCCESS = '[SYSTEM_PARAMETER] ADD SUCCESS';
export const ADD_FAILED = '[SYSTEM_PARAMETER] ADD FAILED';
export const EDIT = '[SYSTEM_PARAMETER] EDIT';
export const EDIT_SUCCESS = '[SYSTEM_PARAMETER] EDIT SUCCESS';
export const EDIT_FAILED = '[SYSTEM_PARAMETER] EDIT FAILED';
export const DELETE = '[SYSTEM_PARAMETER] DELETE';
export const DELETE_SUCCESS = '[SYSTEM_PARAMETER] DELETE SUCCESS';
export const DELETE_FAILED = '[SYSTEM_PARAMETER] DELETE FAILED';
export const DELETE_RANGE = '[SYSTEM_PARAMETER] DELETE RANGE';
export const DELETE_RANGE_SUCCESS = '[SYSTEM_PARAMETER] DELETE RANGE SUCCESS';
export const DELETE_RANGE_FAILED = '[SYSTEM_PARAMETER] DELETE RANGE FAILED';
export const LOAD_ENTRY = '[SYSTEM_PARAMETER] LOAD ENTRY';
export const LOAD_DETAIL = '[SYSTEM_PARAMETER] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[SYSTEM_PARAMETER] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[SYSTEM_PARAMETER] LOAD DETAIL FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: {
    ID: string,
    Key: string
  }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: SystemParameterDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: SystemParameterDetailViewModel = new SystemParameterDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: SystemParameterDetailViewModel) { }
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
  constructor(public payload: SystemParameterDetailViewModel) { }
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
  constructor(public payload: EntrancePayload<{ ID: string, Key: string }>) { }
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
  constructor(public payload: EntrancePayload<Array<SystemParameterListViewModel>>) { }
}

export class deleteRangeSuccessAction implements Action {
  public type: string = DELETE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteRangeFailedAction implements Action {
  public type: string = DELETE_RANGE_FAILED
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
  deleteRangeSuccessAction;
