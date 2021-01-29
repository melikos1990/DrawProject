import * as fromNodeDefinitionActions from '../actions/node-definition.actions';
import { NodeDefinitionDetailViewModel, JobDetailViewModel } from 'src/app/model/organization.model';


export interface State {
  detail: NodeDefinitionDetailViewModel;

}

export const initialState: State = {
  detail: null,
};

export function reducer(state: State = initialState, action: fromNodeDefinitionActions.Actions) {
  switch (action.type) {
    case fromNodeDefinitionActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromNodeDefinitionActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
