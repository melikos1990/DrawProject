import * as fromQuestionUserParameterAction from '../actions/user-parameter.actions';
import { UserParameterlViewModel } from 'src/app/model/master.model';

export interface State {
  detail: UserParameterlViewModel;
}

export const initialState: State = {
  detail: null,
};

export function reducer(state: State = initialState, action: fromQuestionUserParameterAction.Actions) {
  switch (action.type) {
    case fromQuestionUserParameterAction.GET_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };   
    default:
      return state;
  }
}
