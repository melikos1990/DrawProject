import * as CaseApplyActions from '../actions/case-apply.actions';
import { CaseApplyCommitViewModel } from 'src/app/model/substitute.model';



export interface State {
  commit: CaseApplyCommitViewModel;
}

export const initialState: State = {
  commit: null
};

export function reducer(state: State = initialState, action: CaseApplyActions.Actions) {
  switch (action.type) {
    default:
      return state;
  }
}
