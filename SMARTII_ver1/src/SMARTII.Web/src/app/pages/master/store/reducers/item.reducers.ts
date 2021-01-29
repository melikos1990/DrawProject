import * as fromItemActions from '../actions/item.actions';
import { ItemDetailViewModel } from 'src/app/model/master.model';

export interface State {
  detail: ItemDetailViewModel;
  itemDetailLayout: string;
  itemListLayout: string;
}

export const initialState: State = {
  detail: null,
  itemDetailLayout: null,
  itemListLayout: null
};

export function reducer(state: State = initialState, action: fromItemActions.Actions) {
  switch (action.type) {
    case fromItemActions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        detail: action.payload
      };
    case fromItemActions.GET_ITEM_DETAIL_TEMPLATE_SUCCESS:
      return {
        ...state,
        itemDetailLayout: action.payload
      };
    case fromItemActions.LOAD_ENTRY:
      return {
        ...state,
        detail: action.payload,
        itemDetailLayout: null,
      };     
    case fromItemActions.GET_ITEM_LIST_TEMPLATE_SUCCESS:
      return {
        ...state,
        itemListLayout: action.payload
      };
    default:
      return state;
  }
}
