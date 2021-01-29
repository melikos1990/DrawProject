import * as fromHeaderQuarterActions from '../actions/headerquarter-node.action';
import { HeaderQuarterNodeViewModel, HeaderQuarterNodeDetailViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';


export interface State {
  tree: HeaderQuarterNodeViewModel;
  detail: HeaderQuarterNodeDetailViewModel;
  job: NodeJobListViewModel;
}

export const initialState: State = {
  tree: null,
  detail: null,
  job: null
};

export function reducer(state: State = initialState, action: fromHeaderQuarterActions.Actions) {
  switch (action.type) {
    case fromHeaderQuarterActions.LOAD_ENTRY:
      return {
        ...state,
        detail: null
      };
    case fromHeaderQuarterActions.LOAD_SUCCESS:
      return {
        ...state,
        tree: (<fromHeaderQuarterActions.loadSuccessAction>action).payload.data
      };
    case fromHeaderQuarterActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: (<fromHeaderQuarterActions.loadDetailSuccessAction>action).payload.data
      };
    case fromHeaderQuarterActions.SELECT_JOB:
      return {
        ...state,
        job: (<fromHeaderQuarterActions.selectJobAction>action).payload
      };
    default:
      return state;
  }
}
