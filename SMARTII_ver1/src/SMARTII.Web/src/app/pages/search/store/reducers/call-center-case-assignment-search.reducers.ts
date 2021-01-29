import * as fromCaseCenterCaseAssignmentSearchAction from '../actions/call-center-assignment-search.actions';
import { ResultPayload } from 'src/app/model/common.model';

export interface State {
  caseAssignmentList: any[];
}

export const initialState: State = {
    caseAssignmentList: []
};

export function reducer(state: State = initialState, action: fromCaseCenterCaseAssignmentSearchAction.Actions) {
  switch (action.type) {

    case fromCaseCenterCaseAssignmentSearchAction.CASE_CC_GETLIST_SUCCESS:
      return {
        ...state,
        caseAssignmentList: (<ResultPayload<any>>action.payload).data
      }

    default:
      return state;
  }
}
