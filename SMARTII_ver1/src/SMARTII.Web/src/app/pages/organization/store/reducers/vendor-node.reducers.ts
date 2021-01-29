import * as fromVendorActions from '../actions/vendor-node.action';
import { VendorNodeViewModel, VendorNodeDetailViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';


export interface State {
  tree: VendorNodeViewModel;
  detail: VendorNodeDetailViewModel;
  job: NodeJobListViewModel;
}

export const initialState: State = {
  tree: null,
  detail: null,
  job: null
};

export function reducer(state: State = initialState, action: fromVendorActions.Actions) {
  switch (action.type) {
    case fromVendorActions.LOAD_ENTRY:
      return {
        ...state,
        detail: null
      };
    case fromVendorActions.LOAD_SUCCESS:
      return {
        ...state,
        tree: (<fromVendorActions.loadSuccessAction>action).payload.data
      };
    case fromVendorActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: (<fromVendorActions.loadDetailSuccessAction>action).payload.data
      };
    case fromVendorActions.SELECT_JOB:
      return {
        ...state,
        job: (<fromVendorActions.selectJobAction>action).payload
      };
    default:
      return state;
  }
}
