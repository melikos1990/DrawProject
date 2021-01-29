import * as fromCaseWarningActions from '../actions/case-warning.actions';
import { CaseWarningDetailViewModel } from 'src/app/model/master.model';
export interface State {
  detail: CaseWarningDetailViewModel;
}

export const initialState: State = {
  detail: null
};

export function reducer(state: State = initialState, action: fromCaseWarningActions.Actions) {
  switch (action.type) {
    case fromCaseWarningActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromCaseWarningActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
