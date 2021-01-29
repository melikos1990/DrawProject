import * as fromUserActions from '../actions/user.actions';

import { UserDetailViewModel } from 'src/app/model/authorize.model';

export interface State {
  detail: UserDetailViewModel;

}

export const initialState: State = {
  detail: null
};

export function reducer(state: State = initialState, action: fromUserActions.Actions) {
  switch (action.type) {
    case fromUserActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromUserActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
