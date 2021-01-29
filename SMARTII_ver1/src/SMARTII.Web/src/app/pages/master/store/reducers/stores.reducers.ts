import * as fromStoresActions from '../actions/stores.actions';
import { StoresDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: StoresDetailViewModel;
  storesDetailLayout: string;
  storesListLayout: string;
}

export const initialState: State = {
  detail: null,
  storesDetailLayout: null,
  storesListLayout: null
};

export function reducer(state: State = initialState, action: fromStoresActions.Actions) {
  switch (action.type) {
    case fromStoresActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromStoresActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload,
        storesDetailLayout: null,
      };
    case fromStoresActions.GET_STORES_LIST_TEMPLATE_SUCCESS:
      return {
        ...state,
        storesListLayout: action.payload
      };
    default:
      return state;
  }
}
