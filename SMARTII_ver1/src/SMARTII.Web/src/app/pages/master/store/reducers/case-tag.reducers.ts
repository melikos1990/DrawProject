import * as fromCaseTagActions from '../actions/case-tag.actions';
import { CaseTagDetailViewModel } from 'src/app/model/master.model';
export interface State {
  detail: CaseTagDetailViewModel;
}

export const initialState: State = {
  detail: null
};

export function reducer(state: State = initialState, action: fromCaseTagActions.Actions) {
  switch (action.type) {
    case fromCaseTagActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromCaseTagActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
