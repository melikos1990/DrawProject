import { Action } from '@ngrx/store'
import { PtcSwalOption } from 'ptc-swal';

export const ALERT_OPEN = "[ALERT] OPEN";
export const ALERT_CLEAR = "[ALERT] CLEAR";
export const CUSTOMER_ALERT_OPEN = "[CUSTOMER ALERT] OPEN";
export const CUSTOMER_ALERT_CLEAR = "[CUSTOMER ALERT] CLEAR";

export class alertOpenAction implements Action {
  type: string = ALERT_OPEN
  constructor(public payload: {
    detail: PtcSwalOption,
    isLoop: boolean,
    confirm?: (res: any) => void,
    cancel?: () => void
  }) {

  }
}

export class clearAction implements Action {
  type: string = ALERT_CLEAR
  constructor(public payload: any = null) { }
}

export class CustomerAlertOpenAction implements Action {
  type: string = CUSTOMER_ALERT_OPEN
  constructor(public payload: any) { }
}

export class CustomerClearAction implements Action {
  type: string = CUSTOMER_ALERT_CLEAR
  constructor(public payload: any = null) { }
}

export type Actions =
  alertOpenAction |
  clearAction |
  CustomerAlertOpenAction |
  CustomerClearAction;
