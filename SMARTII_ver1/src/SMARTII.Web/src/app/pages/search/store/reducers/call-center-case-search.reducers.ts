import * as fromCaseCenterCaseSearchAction from '../actions/call-center-case-search.actions';
import { ResultPayload } from 'src/app/model/common.model';

export interface State {
  caseList: any[];
}

export const initialState: State = {
  caseList: null
};

export function reducer(state: State = initialState, action: fromCaseCenterCaseSearchAction.Actions) {
  
  switch (action.type) {
    case fromCaseCenterCaseSearchAction.CASE_CC_GETLIST_SUCCESS:
      return {
        ...state,
        caseList: (<ResultPayload<any>>action.payload).data
      }

    default:
      return {...state};
  }
}
