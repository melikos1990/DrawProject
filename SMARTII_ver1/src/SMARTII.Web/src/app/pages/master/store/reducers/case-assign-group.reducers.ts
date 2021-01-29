import * as fromCaseAssignGroupActions from '../actions/case-assign-group.actions';
import { CaseAssignGroupDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: CaseAssignGroupDetailViewModel;
}

export const initialState: State = {
  detail: null
};

export function reducer(state: State = initialState, action: fromCaseAssignGroupActions.Actions) {
  switch (action.type) {
    case fromCaseAssignGroupActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromCaseAssignGroupActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
