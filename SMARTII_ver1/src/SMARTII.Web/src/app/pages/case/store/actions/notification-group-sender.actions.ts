import { Action } from '@ngrx/store';
import { NotificationGroupSenderListViewModel, NotificationGroupUserListViewModel, NotificationGroupSenderExecuteViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const CLEAR_NOTIFICATION = '[NOTIFICATION_GROUP_SENDER] CLEAR NOTIFICATION';
export const TRIGGER_GET_ARRIVED_LIST = '[NOTIFICATION_GROUP_SENDER] TRIGGER GET ARRIVED LIST';
export const SELECT_CHANGE = '[NOTIFICATION_GROUP_SENDER] SELECT CHANGE';
export const GET_USER_LIST = '[NOTIFICATION_GROUP_SENDER] GET USER LIST';
export const GET_USER_LIST_SUCCESS = '[NOTIFICATION_GROUP_SENDER] GET USER LIST SUCCESS';
export const GET_USER_LIST_FAIL = '[NOTIFICATION_GROUP_SENDER] GET USER LIST FAIL';
export const NO_SEND = '[NOTIFICATION_GROUP_SENDER] NO SEND';
export const NO_SEND_SUCCESS = '[NOTIFICATION_GROUP_SENDER] NO SEND SUCCESS';
export const NO_SEND_FAIL = '[NOTIFICATION_GROUP_SENDER] NO SEND FAIL';
export const SEND = '[NOTIFICATION_GROUP_SENDER]  SEND';
export const SEND_SUCCESS = '[NOTIFICATION_GROUP_SENDER]  SEND SUCCESS';
export const SEND_FAIL = '[NOTIFICATION_GROUP_SENDER]  SEND FAIL';


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
  constructor(public payload: NotificationGroupSenderListViewModel) { }
}

export class getUserListAction implements Action {
  type: string = GET_USER_LIST;
  constructor(public payload: number) { }
}

export class getUserListSuccessAction implements Action {
  type: string = GET_USER_LIST_SUCCESS;
  constructor(public payload: NotificationGroupUserListViewModel[]) { }
}

export class getUserListFailedAction implements Action {
  type: string = GET_USER_LIST_FAIL;
  constructor(public payload: string) { }
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
  constructor(public payload: EntrancePayload<NotificationGroupSenderExecuteViewModel>) { }
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
  TriggerGetArrivedList |
  selectChangeAction |
  getUserListAction |
  getUserListSuccessAction |
  getUserListFailedAction |
  noSendAction |
  noSendSuccessAction |
  noSendFailedAction |
  sendAction |
  sendSuccessAction |
  sendFailedAction;
