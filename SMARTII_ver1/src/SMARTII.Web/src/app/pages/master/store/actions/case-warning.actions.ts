import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseWarningDetailViewModel, CaseWarningListViewModel } from '../../../../model/master.model';

export const ADD = '[CASE_WARNING] ADD';
export const ADD_SUCCESS = '[CASE_WARNING] ADD SUCCESS';
export const ADD_FAILED = '[CASE_WARNING] ADD FAILED';
export const EDIT = '[CASE_WARNING] EDIT';
export const EDIT_SUCCESS = '[CASE_WARNING] EDIT SUCCESS';
export const EDIT_FAILED = '[CASE_WARNING] EDIT FAILED';
export const DISABLE = '[CASE_WARNING] DISABLE';
export const DISABLE_SUCCESS = '[CASE_WARNING] DISABLE SUCCESS';
export const DISABLE_FAILED = '[CASE_WARNING] DISABLE FAILED';
export const DISABLE_RANGE = '[CASE_WARNING] DISABLE RANGE';
export const DISABLE_RANGE_SUCCESS = '[CASE_WARNING] DISABLE RANGE SUCCESS';
export const DISABLE_RANGE_FAILED = '[CASE_WARNING] DISABLE RANGE FAILED';
export const LOAD_ENTRY = '[CASE_WARNING] LOAD ENTRY';
export const LOAD_DETAIL = '[CASE_WARNING] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[CASE_WARNING] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[CASE_WARNING] LOAD DETAIL FAILED';

export const ORDER = '[CASE_WARNING] ORDER';
export const ORDER_SUCCESS = '[CASE_WARNING] ORDER SUCCESS ';
export const ORDER_FAILED = '[CASE_WARNING] ORDER FAILED';


export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: CaseWarningDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: CaseWarningDetailViewModel = new CaseWarningDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: CaseWarningDetailViewModel) { }
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
  constructor(public payload: CaseWarningDetailViewModel) { }
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


export class disableRangeAction implements Action {
  public type: string = DISABLE_RANGE;
  constructor(public payload: EntrancePayload<Array<CaseWarningListViewModel>>) { }
}

export class disableRangeSuccessAction implements Action {
  public type: string = DISABLE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class disableRangeFailedAction implements Action {
  public type: string = DISABLE_RANGE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}

export class orderAction implements Action {
  public type: string = ORDER
  constructor(public payload: EntrancePayload<any[]>) { }
}

export class orderSuccessAction implements Action {
  public type: string = ORDER_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class orderFailedAction implements Action {
  public type: string = ORDER_FAILED;
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
  disableSuccessAction |
  disableRangeAction |
  disableRangeFailedAction |
  disableRangeSuccessAction |
  orderAction |
  orderSuccessAction |
  orderFailedAction;
