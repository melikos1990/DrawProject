import * as fromBillboardActions from '../actions/billboard.actions';
import { BillboardListViewModel, BillboardDetailViewModel } from 'src/app/model/master.model';


export interface State {
  ownList: BillboardListViewModel[];
  detail: BillboardDetailViewModel;
}

export const initialState: State = {
  ownList: null,
  detail: null
};

export function reducer(state: State = initialState, action: fromBillboardActions.Actions) {
  switch (action.type) {
    case fromBillboardActions.GET_OWN_LIST_SUCCESS:
      return {
        ...state,
        ownList: action.payload
      };
    case fromBillboardActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromBillboardActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload
      };
    default:
      return state;
  }
}
