import * as fromNotificationActions from "../actions/notification.actions";
import { OrderPayload, NotficationCalcViewModel } from 'src/app/model/notification.model';



export interface State {
  orderPayload: OrderPayload,
  notificationCalc: NotficationCalcViewModel
}

export const initialState: State = {
  orderPayload: null,
  notificationCalc: null
};

export function reducer(state = initialState, action: fromNotificationActions.Actions): State {

  switch (action.type) {
    case fromNotificationActions.NOTIFY_ORDER_OPEN:
      return {
        ...state,
        orderPayload: <OrderPayload>action.payload
      };
    case fromNotificationActions.GET_NOTIFICATION_COUNT_SUCCESS:
      return {
        ...state,
        notificationCalc: <NotficationCalcViewModel>action.payload
      };
    default: {
      return state;
    }

  }

}
