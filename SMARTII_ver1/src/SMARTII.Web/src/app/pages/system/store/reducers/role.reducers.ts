import * as fromRoleActions from '../actions/role.actions';
import { RoleDetailViewModel } from 'src/app/model/authorize.model';


export interface State {
  detail: RoleDetailViewModel;
}

export const initialState: State = {
  detail: null
};

export function reducer(state: State = initialState, action: fromRoleActions.Actions) {
  switch (action.type) {
    case fromRoleActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromRoleActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
