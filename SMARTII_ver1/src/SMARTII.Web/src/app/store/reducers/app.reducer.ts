import * as fromAppActions from "../actions/app.actions";



export interface State {
  cluture: string;
  contextMenu: {
    position: {
      x: number,
      y: number
    },
    display: boolean,
    cbDist: { key: string , value: (obj?) => void }[]

  }
}

export const initialState: State = {
  cluture: null,
  contextMenu: {
    position: {
      x: 0,
      y: 0
    },
    display: false,
    cbDist: null
  }
};

export function reducer(state = initialState, action: fromAppActions.Actions): State {

  switch (action.type) {
    case fromAppActions.CHANGE_CULTURE:
      return {
        ...state,
        cluture: (<fromAppActions.changeCultureAction>action).payload,
      };
    case fromAppActions.CONTEXT_MENU:
      return {
        ...state,
        contextMenu: (<fromAppActions.contextMenuAction>action).payload,
      };
    default: {
      return state;
    }

  }

}
