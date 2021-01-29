import * as fromEnterpriseActions from '../actions/enterprise.actions';
import { EnterpriseDetailViewModel } from 'src/app/model/organization.model';

export interface State {
  detail: EnterpriseDetailViewModel;
}

export const initialState: State = {
  detail: null,
};

export function reducer(state: State = initialState, action: fromEnterpriseActions.Actions) {
  switch (action.type) {
    case fromEnterpriseActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromEnterpriseActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
