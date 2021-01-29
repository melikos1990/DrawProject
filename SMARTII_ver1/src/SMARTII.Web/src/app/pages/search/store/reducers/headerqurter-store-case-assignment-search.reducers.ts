import * as fromCaseHeaderqurterStoreAssignmentSearchAction from '../actions/headerqurter-store-assignment-search.actions';
import { ResultPayload } from 'src/app/model/common.model';

export interface State {
    caseAssignmentList: any[]
}

export const initialState: State = {
    caseAssignmentList: []
};

export function reducer(state: State = initialState, action: fromCaseHeaderqurterStoreAssignmentSearchAction.Actions) {
  switch (action.type) {

    case fromCaseHeaderqurterStoreAssignmentSearchAction.CASE_HS_GETLIST_SUCCESS:
      return {
        ...state,
        caseAssignmentList: (<ResultPayload<any>>action.payload).data
      }

    default:
      return state;
  }
}
