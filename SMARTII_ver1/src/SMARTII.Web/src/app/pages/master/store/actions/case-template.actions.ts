import { Action } from '@ngrx/store';
import { CaseTemplateDetailViewModel, CaseTemplateListViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const ADD = '[CASE_TEMPLATE] ADD';
export const ADD_SUCCESS = '[CASE_TEMPLATE] ADD SUCCESS';
export const ADD_FAILED = '[CASE_TEMPLATE] ADD FAILED';
export const EDIT = '[CASE_TEMPLATE] EDIT';
export const EDIT_SUCCESS = '[CASE_TEMPLATE] EDIT SUCCESS';
export const EDIT_FAILED = '[CASE_TEMPLATE] EDIT FAILED';
export const DELETE = '[CASE_TEMPLATE] DELETE';
export const DELETE_SUCCESS = '[CASE_TEMPLATE] DELETE SUCCESS';
export const DELETE_FAILED = '[CASE_TEMPLATE] DELETE FAILED';
export const DELETE_RANGE = '[CASE_TEMPLATE] DELETE RANGE';
export const DELETE_RANGE_SUCCESS = '[CASE_TEMPLATE] DELETE RANGE SUCCESS';
export const DELETE_RANGE_FAILED = '[CASE_TEMPLATE] DELETE RANGE FAILED';
export const LOAD_ENTRY = '[CASE_TEMPLATE] LOAD ENTRY';
export const LOAD_DETAIL = '[CASE_TEMPLATE] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[CASE_TEMPLATE] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[CASE_TEMPLATE] LOAD DETAIL FAILED';
export const CHACK_FASTFINISH = '[CASE_TEMPLATE] CHACK FASTFINISH';
export const CHACK_FASTFINISH_SUCCESS = '[CASE_TEMPLATE] CHACK FASTFINISH SUCCESS';
export const CHACK_FASTFINISH_FAILED = '[CASE_TEMPLATE] CHACK FASTFINISH FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: CaseTemplateDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: CaseTemplateDetailViewModel = new CaseTemplateDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: CaseTemplateDetailViewModel) { }
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
  constructor(public payload: CaseTemplateDetailViewModel) { }
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
  constructor(public payload: EntrancePayload<Array<CaseTemplateListViewModel>>) { }
}

export class deleteRangeSuccessAction implements Action {
  public type: string = DELETE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteRangeFailedAction implements Action {
  public type: string = DELETE_RANGE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}

export class chackFastFinish implements Action {
  public type: string = CHACK_FASTFINISH;
  constructor(public payload: { buID: number }){}
}


export class chackFastFinishSuccess implements Action {
  public type: string = CHACK_FASTFINISH_SUCCESS;
  constructor(public payload: boolean){}
}


export class chackFastFinishFailed implements Action {
  public type: string = CHACK_FASTFINISH_FAILED;
  constructor(public payload: boolean){}
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
  chackFastFinish |
  chackFastFinishSuccess |
  chackFastFinishFailed;
