import * as fromAlertActions from '../actions/alert.actions';
import { PtcSwalOption, PtcSwalType } from 'ptc-swal';
import { TemplateRef } from '@angular/core';

export interface State {
  opts: {
    isLoop: boolean,
    confirm: (res: any) => void,
    cancel: () => void,
    detail: PtcSwalOption,
  }
  customerOpts: {
    templateRef: TemplateRef<any>,
    detail: {
      title: string;
      frameClass: string,
      contentClass: string,
      type: PtcSwalType,
      confirm?: () => void,
      cancel?: () => void
    },
    data: any
  }
}

export const initialState: State = {
  opts: null,
  customerOpts: null
}

export function reducer(state = initialState, action: fromAlertActions.Actions): State {

  switch (action.type) {
    case fromAlertActions.ALERT_OPEN:
      return {
        ...state,
        opts: action.payload
      }
    case fromAlertActions.ALERT_CLEAR:
      return initialState;
    case fromAlertActions.CUSTOMER_ALERT_OPEN:
      return {
        ...state,
        customerOpts: action.payload
      }
    case fromAlertActions.CUSTOMER_ALERT_CLEAR:
      return {
        ...state,
        customerOpts: null
      }
    default: {
      return state;
    }

  }

}
