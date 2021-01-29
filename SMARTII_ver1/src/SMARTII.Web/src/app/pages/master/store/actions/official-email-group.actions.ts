import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { OfficialEmailGroupDetailViewModel } from 'src/app/model/master.model';


export const ADD = '[OFFICIAL EMAIL GROUP] ADD';
export const ADD_SUCCESS = '[OFFICIAL EMAIL GROUP] ADD SUCCESS';
export const ADD_FAILED = '[OFFICIAL EMAIL GROUP] ADD FAILED';
export const EDIT = '[OFFICIAL EMAIL GROUP] EDIT';
export const EDIT_SUCCESS = '[OFFICIAL EMAIL GROUP] EDIT SUCCESS';
export const EDIT_FAILED = '[OFFICIAL EMAIL GROUP] EDIT FAILED';
export const DISABLE = '[OFFICIAL EMAIL GROUP] DISABLE';
export const DISABLE_SUCCESS = '[OFFICIAL EMAIL GROUP] DISABLE SUCCESS';
export const DISABLE_FAILED = '[OFFICIAL EMAIL GROUP] DISABLE FAILED';
export const LOAD_ENTRY = '[OFFICIAL EMAIL GROUP] LOAD ENTRY';
export const LOAD_DETAIL = '[OFFICIAL EMAIL GROUP] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[OFFICIAL EMAIL GROUP] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[OFFICIAL EMAIL GROUP] LOAD DETAIL FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: OfficialEmailGroupDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: OfficialEmailGroupDetailViewModel = new OfficialEmailGroupDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: OfficialEmailGroupDetailViewModel) { }
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
  constructor(public payload: OfficialEmailGroupDetailViewModel) { }
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
