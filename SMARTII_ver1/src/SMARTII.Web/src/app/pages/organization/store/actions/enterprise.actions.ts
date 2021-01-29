import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { EnterpriseDetailViewModel } from 'src/app/model/organization.model';


export const ADD = '[ENTERPRISE] ADD';
export const ADD_SUCCESS = '[ENTERPRISE] ADD SUCCESS';
export const ADD_FAILED = '[ENTERPRISE] ADD FAILED';
export const EDIT = '[ENTERPRISE] EDIT';
export const EDIT_SUCCESS = '[ENTERPRISE] EDIT SUCCESS';
export const EDIT_FAILED = '[ENTERPRISE] EDIT FAILED';
export const DELETE = '[ENTERPRISE] DELETE';
export const DELETE_SUCCESS = '[ENTERPRISE] DELETE SUCCESS';
export const DELETE_FAILED = '[ENTERPRISE] DELETE FAILED';
export const DELETE_RANGE = '[ENTERPRISE] DELETE RANGE';
export const DELETE_RANGE_SUCCESS = '[ENTERPRISE] DELETE RANGE SUCCESS';
export const DELETE_RANGE_FAILED = '[ENTERPRISE] DELETE RANGE FAILED';
export const LOAD_ENTRY = '[ENTERPRISE] LOAD ENTRY';
export const LOAD_DETAIL = '[ENTERPRISE] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[ENTERPRISE] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[ENTERPRISE] LOAD DETAIL FAILED';
export const DISABLED_DETAIL = '[ENTERPRISE] DISABLED_DETAIL';
export const DISABLED_DETAIL_SUCCESS = '[ENTERPRISE] DISABLED_DETAIL_SUCCESS';
export const DISABLED_DETAIL_FAILED = '[ENTERPRISE] DISABLED_DETAIL_FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: {
    EnterpriseID: number
  }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: EnterpriseDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: EnterpriseDetailViewModel = new EnterpriseDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD;
  constructor(public payload: EnterpriseDetailViewModel) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS;
  constructor(public payload: EnterpriseDetailViewModel) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED;
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT;
  constructor(public payload: EnterpriseDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS;
  constructor(public payload: EnterpriseDetailViewModel) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED;
  constructor(public payload: string) { }
}

export class deleteAction implements Action {
  public type: string = DELETE;
  constructor(public payload: EntrancePayload<string>) { }
}
export class deleteSuccessAction implements Action {
  public type: string = DELETE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteFailedAction implements Action {
  public type: string = DELETE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class deleteRangeAction implements Action {
  public type: string = DELETE_RANGE;
  constructor(public payload: EntrancePayload<number[]>) { }
}
export class deleteRangeSuccessAction implements Action {
  public type: string = DELETE_RANGE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteRangeFailedAction implements Action {
  public type: string = DELETE_RANGE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class disabledDetail implements Action {
  type: string = DISABLED_DETAIL;
  constructor(public payload: EntrancePayload<{ID?: number}>){}
}

export class disabledDetailSuccess implements Action {
  public type: string = DISABLED_DETAIL_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class disabledDetailFailed implements Action {
  public type: string = DISABLED_DETAIL_FAILED
  constructor(public payload: ResultPayload<string>) { }
}

export type Actions =
  loadEntryAction |
  loadDetailAction |
  loadDetailSuccessAction |
  loadDetailFailedAction |
  addAction |
  addSuccessAction |
  addFailedAction |
  editAction |
  editSuccessAction |
  editFailedAction |
  deleteAction |
  deleteFailedAction |
  deleteSuccessAction |
  deleteRangeAction |
  deleteRangeFailedAction |
  deleteRangeSuccessAction |
  disabledDetail |
  disabledDetailSuccess |
  disabledDetailFailed;
