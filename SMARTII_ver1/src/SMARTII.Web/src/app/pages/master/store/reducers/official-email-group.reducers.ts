import * as fromOfficialEmailGroupActions from '../actions/official-email-group.actions';
import { OfficialEmailGroupDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: OfficialEmailGroupDetailViewModel;
}

export const initialState: State = {
  detail: null,
};

export function reducer(state: State = initialState, action: fromOfficialEmailGroupActions.Actions) {
  switch (action.type) {
    case fromOfficialEmailGroupActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
