import * as fromNotificationGroupSenderActions from '../actions/notification-group-sender.actions';
import { NotificationGroupSenderListViewModel, NotificationGroupUserListViewModel } from 'src/app/model/master.model';

export interface State {
  selected: NotificationGroupSenderListViewModel;
  users: NotificationGroupUserListViewModel[];
  triggerFetch: any;
}

export const initialState: State = {
  selected: null,
  users: [],
  triggerFetch: null
};

export function reducer(state: State = initialState, action: fromNotificationGroupSenderActions.Actions) {
  switch (action.type) {
    case fromNotificationGroupSenderActions.SELECT_CHANGE:
      return {
        ...state,
        selected: { ...(<fromNotificationGroupSenderActions.selectChangeAction>action).payload }
      };
    case fromNotificationGroupSenderActions.GET_USER_LIST_SUCCESS:
      return {
        ...state,
        users: [...(<fromNotificationGroupSenderActions.getUserListSuccessAction>action).payload]
      };
    case fromNotificationGroupSenderActions.TRIGGER_GET_ARRIVED_LIST:
      return {
        ...state,
        triggerFetch: Symbol()
      };
    default:
      return state;
  }
}
