import * as fromCaseRemindActions from '../actions/case-remind.actions';
import { CaseRemindDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: CaseRemindDetailViewModel;
}

export const initialState: State = {
  detail: null,
};

export function reducer(state: State = initialState, action: fromCaseRemindActions.Actions) {
  switch (action.type) {
    case fromCaseRemindActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
