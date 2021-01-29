import * as fromNotificationGroupActions from '../actions/notification-group.actions';
import { NotificationGroupDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: NotificationGroupDetailViewModel;
}

export const initialState: State = {
  detail: null,
};

export function reducer(state: State = initialState, action: fromNotificationGroupActions.Actions) {
  switch (action.type) {
    case fromNotificationGroupActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
