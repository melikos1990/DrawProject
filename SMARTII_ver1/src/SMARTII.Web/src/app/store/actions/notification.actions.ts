

import { Action } from '@ngrx/store';
import { OrderPayload, NotficationCalcViewModel } from 'src/app/model/notification.model';
import { ResultPayload, EntrancePayload } from 'src/app/model/common.model';

export const NOTIFY_ORDER_OPEN = '[NOTIFY] NOTIFY_ORDER_OPEN';
export const GET_NOTIFICATION_COUNT = '[NOTIFY] GET_NOTIFICATION_COUNT';
export const GET_NOTIFICATION_COUNT_SUCCESS = '[NOTIFY] GET_NOTIFICATION_COUNT_SUCCESS';
export const GET_NOTIFICATION_COUNT_FAILED = '[NOTIFY] GET_NOTIFICATION_COUNT_FAILED';
export const REMOVE_PERSONAL_NOTIFICAIOTN = '[NOTIFY] REMOVE_PERSONAL_NOTIFICAIOTN';
export const REMOVE_PERSONAL_NOTIFICAIOTN_SUCCESS = '[NOTIFY] REMOVE_PERSONAL_NOTIFICAIOTN_SUCCESS';
export const REMOVE_PERSONAL_NOTIFICAIOTN_FAIL = '[NOTIFY] REMOVE_PERSONAL_NOTIFICAIOTN_FAIL';
export const REMOVE_PERSONAL_BILLBOARD = '[NOTIFY] REMOVE_PERSONAL_BILLBOARD';
export const REMOVEALL_PERSONAL_NOTIFICAIOTN = '[NOTIFY] REMOVEALL_PERSONAL_NOTIFICAIOTN';
export const REMOVEALL_PERSONAL_SUCCESS = '[NOTIFY] REMOVEALL_PERSONAL_SUCCESS';
export const REMOVEALL_PERSONAL_FAILED = '[NOTIFY] REMOVEALL_PERSONAL_FAILED';


export class orderOpen implements Action {
    type: string = NOTIFY_ORDER_OPEN;
    constructor(public payload: OrderPayload) { }
}

export class removePersonalNotification implements Action {
    type: string = REMOVE_PERSONAL_NOTIFICAIOTN;
    constructor(public payload: { id: number }) { }
}

export class clearNotice implements Action {
    type: string = REMOVEALL_PERSONAL_NOTIFICAIOTN;
    constructor(public payload: { userID: string }) { }
}

export class clearAllSuccessAction implements Action {
    type: string = REMOVEALL_PERSONAL_SUCCESS;
    constructor(public payload?: any) { }
}

export class clearAllFailedAction implements Action {
    type: string = REMOVEALL_PERSONAL_FAILED;
    constructor(public payload: any) { }
}

export class removePersonalNotificationSuccess implements Action {
    type: string = REMOVE_PERSONAL_NOTIFICAIOTN_SUCCESS;
    constructor(public payload?: any) { }
}

export class removePersonalNotificationFail implements Action {
    type: string = REMOVE_PERSONAL_NOTIFICAIOTN_FAIL;
    constructor(public payload: any) { }
}

export class getNotificationCount implements Action {
    type: string = GET_NOTIFICATION_COUNT;
    constructor(public payload: { userID: string }) { }
}

export class getNotificationCountSuccess implements Action {
    type: string = GET_NOTIFICATION_COUNT_SUCCESS;
    constructor(public payload: NotficationCalcViewModel) { }
}

export class getNotificationCountFailed implements Action {
    type: string = GET_NOTIFICATION_COUNT_FAILED;
    constructor(public payload: string) { }
}

export type Actions =
    orderOpen |
    removePersonalNotification |
    getNotificationCount |
    getNotificationCountSuccess |
    getNotificationCountFailed |
    removePersonalNotificationSuccess |
    removePersonalNotificationFail |
    clearNotice |
    clearAllSuccessAction |
    clearAllFailedAction;
