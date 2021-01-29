import * as SystemParameterActions from '../actions/system-parameter.actions';
import { SystemParameterDetailViewModel } from 'src/app/model/system.model';



export interface State {
  detail: SystemParameterDetailViewModel;
}

export const initialState: State = {
  detail: null
};

export function reducer(state: State = initialState, action: SystemParameterActions.Actions) {
  switch (action.type) {
    case SystemParameterActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case SystemParameterActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
