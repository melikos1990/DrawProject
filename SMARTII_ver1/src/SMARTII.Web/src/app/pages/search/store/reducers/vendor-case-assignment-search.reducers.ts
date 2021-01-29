import * as fromCaseVendorAssignmentSearchAction from '../actions/vendor-assignment-search.actions';
import { ResultPayload } from 'src/app/model/common.model';

export interface State {
  caseAssignmentList: any[]
}

export const initialState: State = {
  caseAssignmentList: []
};

export function reducer(state: State = initialState, action: fromCaseVendorAssignmentSearchAction.Actions) {
  switch (action.type) {

    case fromCaseVendorAssignmentSearchAction.CASE_VENDOR_GETLIST_SUCCESS:
      return {
        ...state,
        caseAssignmentList: (<ResultPayload<any>>action.payload).data
      }

    default:
      return state;
  }
}
