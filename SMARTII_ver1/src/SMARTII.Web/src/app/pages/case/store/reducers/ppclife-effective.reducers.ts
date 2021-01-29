import * as fromPPCLifeEffectiveActions from '../actions/ppclife-effective.actions';
import { PPCLifeEffectiveListViewModel, PPCLifeEffectiveCaseListViewModel } from 'src/app/model/master.model';



export interface State {
  selected: PPCLifeEffectiveListViewModel;
  saveSelected: PPCLifeEffectiveListViewModel;
  refreshList: symbol;
  refreshMain: symbol;
  caselist: PPCLifeEffectiveCaseListViewModel[];
  triggerFetch: any;
}

export const initialState: State = {
  selected: null,
  saveSelected: null,
  caselist: [],
  triggerFetch: null,
  refreshList: null,
  refreshMain: null,
};


export function reducer(state: State = initialState, action: fromPPCLifeEffectiveActions.Actions) {
  switch (action.type) {
    case fromPPCLifeEffectiveActions.SELECT_CHANGE:
      return {
        ...state,
        selected: { ...(<fromPPCLifeEffectiveActions.selectChangeAction>action).payload }
      };
      case fromPPCLifeEffectiveActions.SAVE_SELECT:
        return {
          ...state,
          saveSelected: { ...(<fromPPCLifeEffectiveActions.selectChangeAction>action).payload }
        };
    case fromPPCLifeEffectiveActions.REFRESH_CASE_LIST:
      return {
        ...state,
        refreshList: Symbol()
      };
    case fromPPCLifeEffectiveActions.REFRESH_EFFECTIVE_SUMMARY:
      return {
        ...state,
        refreshMain: Symbol()
      };
    //取得案件成功 存入Store
    case fromPPCLifeEffectiveActions.GET_CASE_LIST_SUCCESS:
      return {
        ...state,
        caselist: [...(<fromPPCLifeEffectiveActions.getCaseListSuccessAction>action).payload]
      };
    case fromPPCLifeEffectiveActions.TRIGGER_GET_ARRIVED_LIST:
      return {
        ...state,
        triggerFetch: Symbol()
      };
    default:
      return state;
  }
}
