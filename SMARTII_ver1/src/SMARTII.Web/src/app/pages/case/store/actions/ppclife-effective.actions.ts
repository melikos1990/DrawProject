import { Action } from '@ngrx/store';
import { PPCLifeEffectiveListViewModel, PPCLifeEffectiveSenderExecuteViewModel, PPCLifeEffectiveCaseListViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const SELECT_CHANGE = '[PPCLIFE_EFFECTIVE] SELECT CHANGE';
export const SAVE_SELECT = '[PPCLIFE_EFFECTIVE] SAVE_SELECT';
export const GET_CASE_LIST = '[PPCLIFE_EFFECTIVE] GET_USER_LIST';
export const NO_SEND = '[PPCLIFE_EFFECTIVE] NO SEND';
export const NO_SEND_SUCCESS = '[PPCLIFE_EFFECTIVE] NO SEND SUCCESS';
export const NO_SEND_FAIL = '[PPCLIFE_EFFECTIVE] NO SEND FAIL';
export const SEND = '[PPCLIFE_EFFECTIVE]  SEND';
export const SEND_SUCCESS = '[PPCLIFE_EFFECTIVE]  SEND SUCCESS';
export const SEND_FAIL = '[PPCLIFE_EFFECTIVE]  SEND FAIL';
export const TRIGGER_GET_ARRIVED_LIST = '[PPCLIFE_EFFECTIVE]  TRIGGER_GET_ARRIVED_LIST';
export const GET_CASE_LIST_SUCCESS = '[PPCLIFE_EFFECTIVE]  GET_USER_LIST_SUCCESS';
export const GET_CASE_LIST_FAIL = '[PPCLIFE_EFFECTIVE]  GET_USER_LIST_FAIL';
export const CLEAR_NOTIFICATION = '[PPCLIFE_EFFECTIVE]  CLEAR_NOTIFICATION';
export const IGNORE_CASE = '[PPCLIFE_EFFECTIVE]  IGNORE_CASE';
export const IGNORE_SUCCESS = '[PPCLIFE_EFFECTIVE]  IGNORE_SUCCESS';
export const IGNORE_FAIL = '[PPCLIFE_EFFECTIVE]  IGNORE_FAIL';
export const REFRESH_CASE_LIST = '[PPCLIFE_EFFECTIVE]  REFRESH_CASE_LIST';
export const REFRESH_EFFECTIVE_SUMMARY = '[PPCLIFE_EFFECTIVE]  REFRESH_EFFECTIVE_SUMMARY';


export class ClearNotification implements Action {
  type: string = CLEAR_NOTIFICATION;
  constructor(public payload = null) { }
}
export class TriggerGetArrivedList implements Action {
  type: string = TRIGGER_GET_ARRIVED_LIST;
  constructor(public payload = null) { }
}
export class selectChangeAction implements Action {
  type: string = SELECT_CHANGE;
  constructor(public payload: PPCLifeEffectiveListViewModel) { }
}
export class saveSelectChangeAction implements Action {
  type: string = SAVE_SELECT;
  constructor(public payload: PPCLifeEffectiveListViewModel) { }
}
export class refreshCaseList implements Action {
  type: string = REFRESH_CASE_LIST;
  constructor(public payload = null) { }
}
export class refreshEffectiveSummary implements Action {
  type: string = REFRESH_EFFECTIVE_SUMMARY;
  constructor(public payload = null) { }
}
export class getCaseListAction implements Action {
  type: string = GET_CASE_LIST;
  constructor(public payload: number) { }
}
export class getCaseListSuccessAction implements Action {
  type: string = GET_CASE_LIST_SUCCESS;
  constructor(public payload: PPCLifeEffectiveCaseListViewModel[]) { }
}
export class getCaseListFailedAction implements Action {
  type: string = GET_CASE_LIST_FAIL;
  constructor(public payload: string) { }
}

export class ignoreAction implements Action {
  type: string = IGNORE_CASE;
  constructor(public payload: EntrancePayload<PPCLifeEffectiveCaseListViewModel[]>) { }
}

export class ignoreSuccessAction implements Action {
  public type: string = IGNORE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class ignoreFailedAction implements Action {
  public type: string = IGNORE_FAIL
  constructor(public payload: ResultPayload<string>) { }
}

export class noSendAction implements Action {
  type: string = NO_SEND;
  constructor(public payload: EntrancePayload<number>) { }
}

export class noSendSuccessAction implements Action {
  public type: string = NO_SEND_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class noSendFailedAction implements Action {
  public type: string = NO_SEND_FAIL
  constructor(public payload: ResultPayload<string>) { }
}

export class sendAction implements Action {
  type: string = SEND;
  constructor(public payload: EntrancePayload<PPCLifeEffectiveSenderExecuteViewModel>) { }
}

export class sendSuccessAction implements Action {
  public type: string = SEND_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class sendFailedAction implements Action {
  public type: string = SEND_FAIL;
  constructor(public payload: ResultPayload<string>) { }
}


export type Actions =
  selectChangeAction |
  saveSelectChangeAction |
  getCaseListAction |
  noSendAction |
  noSendSuccessAction |
  noSendFailedAction |
  sendAction |
  sendSuccessAction |
  sendFailedAction | 
  TriggerGetArrivedList |
  getCaseListSuccessAction |
  getCaseListFailedAction | 
  ClearNotification | 
  ignoreAction |
  ignoreSuccessAction |
  ignoreFailedAction |
  refreshCaseList | 
  refreshEffectiveSummary;