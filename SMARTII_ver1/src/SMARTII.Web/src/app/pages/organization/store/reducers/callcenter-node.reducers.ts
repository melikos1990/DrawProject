import * as fromCallCenterActions from '../actions/callcenter-node.action';
import { CallCenterNodeViewModel, CallCenterNodeDetailViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';


export interface State {
  tree: CallCenterNodeViewModel;
  detail: CallCenterNodeDetailViewModel;
  job: NodeJobListViewModel;
}

export const initialState: State = {
  tree: null,
  detail: null,
  job: null
};

export function reducer(state: State = initialState, action: fromCallCenterActions.Actions) {
  switch (action.type) {
    case fromCallCenterActions.LOAD_ENTRY:
      return {
        ...state,
        detail: null
      };
    case fromCallCenterActions.LOAD_SUCCESS:
      return {
        ...state,
        tree: (<fromCallCenterActions.loadSuccessAction>action).payload.data
      };
    case fromCallCenterActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: (<fromCallCenterActions.loadDetailSuccessAction>action).payload.data
      };
    case fromCallCenterActions.SELECT_JOB:
      return {
        ...state,
        job: (<fromCallCenterActions.selectJobAction>action).payload
      };
    default:
      return state;
  }
}
