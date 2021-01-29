import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { NotificationGroupDetailViewModel, NotificationGroupListViewModel } from 'src/app/model/master.model';


export const ADD = '[NOTIFICATION GROUP] ADD';
export const ADD_SUCCESS = '[NOTIFICATION GROUP] ADD SUCCESS';
export const ADD_FAILED = '[NOTIFICATION GROUP] ADD FAILED';
export const EDIT = '[NOTIFICATION GROUP] EDIT';
export const EDIT_SUCCESS = '[NOTIFICATION GROUP] EDIT SUCCESS';
export const EDIT_FAILED = '[NOTIFICATION GROUP] EDIT FAILED';
export const DELETE = '[NOTIFICATION GROUP] DELETE';
export const DELETE_SUCCESS = '[NOTIFICATION GROUP] DELETE SUCCESS';
export const DELETE_FAILED = '[NOTIFICATION GROUP] DELETE FAILED';
export const DELETE_RANGE = '[NOTIFICATION GROUP] DELETE RANGE';
export const DELETE_RANGE_SUCCESS = '[NOTIFICATION GROUP] DELETE RANGE SUCCESS';
export const DELETE_RANGE_FAILED = '[NOTIFICATION GROUP] DELETE RANGE FAILED';
export const LOAD_ENTRY = '[NOTIFICATION GROUP] LOAD ENTRY';
export const LOAD_DETAIL = '[NOTIFICATION GROUP] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[NOTIFICATION GROUP] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[NOTIFICATION GROUP] LOAD DETAIL FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: NotificationGroupDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: NotificationGroupDetailViewModel = new NotificationGroupDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: NotificationGroupDetailViewModel) { }
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
  constructor(public payload: NotificationGroupDetailViewModel) { }
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
  constructor(public payload: EntrancePayload<Array<NotificationGroupListViewModel>>) { }
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
