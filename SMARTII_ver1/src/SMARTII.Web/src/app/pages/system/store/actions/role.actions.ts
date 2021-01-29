import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { RoleDetailViewModel } from 'src/app/model/authorize.model';


export const ADD = '[ROLE] ADD';
export const ADD_SUCCESS = '[ROLE] ADD SUCCESS';
export const ADD_FAILED = '[ROLE] ADD FAILED';
export const EDIT = '[ROLE] EDIT';
export const EDIT_SUCCESS = '[ROLE] EDIT SUCCESS';
export const EDIT_FAILED = '[ROLE] EDIT FAILED';

export const LOAD_ENTRY = '[ROLE] LOAD ENTRY';
export const LOAD_DETAIL = '[ROLE] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[ROLE] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[ROLE] LOAD DETAIL FAILED';

export const DISABLE = '[ROLE] DISABLE';
export const DISABLE_SUCCESS = '[ROLE] DISABLE SUCCESS';
export const DISABLE_FAILED = '[ROLE] DISABLE FAILED';

export const DISABLE_RANGE = '[ROLE] DISABLE RANGE';
export const DISABLE_RANGE_SUCCESS = '[ROLE] DISABLE RANGE SUCCESS';
export const DISABLE_RANGE_FAILED = '[ROLE] DISABLE RANGE FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: {
    RoleID: number
  }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: RoleDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: RoleDetailViewModel = new RoleDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD;
  constructor(public payload: RoleDetailViewModel) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS;
  constructor(public payload: string) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED;
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT;
  constructor(public payload: RoleDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS;
  constructor(public payload: string) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED;
  constructor(public payload: string) { }
}

export class disableAction implements Action {
  public type: string = DISABLE;
  constructor(public payload: EntrancePayload<string>) { }
}
export class disableSuccessAction implements Action {
  public type: string = DISABLE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableFailedAction implements Action {
  public type: string = DISABLE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableRangeAction implements Action {
  public type: string = DISABLE_RANGE;
  constructor(public payload: EntrancePayload<number[]>) { }
}
export class disableRangeSuccessAction implements Action {
  public type: string = DISABLE_RANGE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableRangeFailedAction implements Action {
  public type: string = DISABLE_RANGE_FAILED;
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
  disableAction |
  disableFailedAction |
  disableSuccessAction |
  disableRangeAction |
  disableRangeFailedAction |
  disableRangeSuccessAction;
