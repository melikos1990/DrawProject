import * as fromCaseHeaderqurterStoreSearchAction from '../actions/headerqurter-store-case-search.actions';
import { ResultPayload } from 'src/app/model/common.model';

export interface State {
  caseList: any[]
}

export const initialState: State = {
  caseList: []
};

export function reducer(state: State = initialState, action: fromCaseHeaderqurterStoreSearchAction.Actions) {
  switch (action.type) {

    case fromCaseHeaderqurterStoreSearchAction.CASE_HS_GETLIST_SUCCESS:
      return {
        ...state,
        caseList: (<ResultPayload<any>>action.payload).data
      }

    default:
      return state;
  }
}
