import * as fromCaseTemplateActions from '../actions/case-template.actions';
import { CaseTemplateDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: CaseTemplateDetailViewModel;
  canFastFinish: boolean;
}

export const initialState: State = {
  detail: null,
  canFastFinish: false
};

export function reducer(state: State = initialState, action: fromCaseTemplateActions.Actions) {
  switch (action.type) {
    case fromCaseTemplateActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      }; 
    case fromCaseTemplateActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    case fromCaseTemplateActions.CHACK_FASTFINISH_FAILED:
    case fromCaseTemplateActions.CHACK_FASTFINISH_SUCCESS:
      return {
        ...state,
        canFastFinish: action.payload
      };
    default:
      return state;
  }
}
