import { Action } from '@ngrx/store';
import { BillboardSearchViewModel, BillboardListViewModel, BillboardDetailViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const CLEAR_NOTIFICATION = '[BILLBOARD] CLEAR NOTIFICATION'

export const GET_OWN_LIST = '[BILLBOARD] GET OWN LIST';
export const GET_OWN_LIST_SUCCESS = '[BILLBOARD] GET OWN LIST SUCCESS';
export const GET_OWN_LIST_FAIL = '[BILLBOARD] GET OWN LIST FAILED';
export const ADD = '[BILLBOARD] ADD';
export const ADD_SUCCESS = '[BILLBOARD] ADD SUCCESS';
export const ADD_FAILED = '[BILLBOARD] ADD FAILED';
export const EDIT = '[BILLBOARD] EDIT';
export const EDIT_SUCCESS = '[BILLBOARD] EDIT SUCCESS';
export const EDIT_FAILED = '[BILLBOARD] EDIT FAILED';
export const DELETE = '[BILLBOARD] DELETE';
export const DELETE_SUCCESS = '[BILLBOARD] DELETE SUCCESS';
export const DELETE_FAILED = '[BILLBOARD] DELETE FAILED';
export const DELETE_RANGE = '[BILLBOARD] DELETE RANGE';
export const DELETE_RANGE_SUCCESS = '[BILLBOARD] DELETE RANGE SUCCESS';
export const DELETE_RANGE_FAILED = '[BILLBOARD] DELETE RANGE FAILED';
export const LOAD_ENTRY = '[BILLBOARD] LOAD ENTRY';
export const LOAD_DETAIL = '[BILLBOARD] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[BILLBOARD] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[BILLBOARD] LOAD DETAIL FAILED';

export class ClearNotificationAction implements Action {
  public type: string = CLEAR_NOTIFICATION;
  constructor(public payload = null) { }
}

export class getOwnListAction implements Action {
  public type: string = GET_OWN_LIST;
  constructor(public payload: BillboardSearchViewModel) { }
}

export class getOwnListSuccessAction implements Action {
  public type: string = GET_OWN_LIST_SUCCESS;
  constructor(public payload: BillboardListViewModel[]) { }
}

export class getOwnListFailedAction implements Action {
  public type: string = GET_OWN_LIST_FAIL;
  constructor(public payload: string) { }
}


export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: BillboardDetailViewModel) { }
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
  constructor(public payload: BillboardDetailViewModel) { }
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
  constructor(public payload: EntrancePayload<Array<BillboardListViewModel>>) { }
}

export class deleteRangeSuccessAction implements Action {
  public type: string = DELETE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteRangeFailedAction implements Action {
  public type: string = DELETE_RANGE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: BillboardDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: BillboardDetailViewModel = new BillboardDetailViewModel()) { }
}

export type Actions =
  getOwnListAction |
  getOwnListSuccessAction |
  getOwnListFailedAction |
  addAction |
  addSuccessAction |
  addFailedAction |
  deleteAction |
  deleteFailedAction |
  deleteSuccessAction |
  deleteRangeAction |
  deleteRangeFailedAction |
  deleteRangeSuccessAction |
  loadEntryAction |
  loadDetailAction |
  loadDetailFailedAction |
  loadDetailSuccessAction;
