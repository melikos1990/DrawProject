import * as fromRouteActions from '../actions/route.actions';


export interface State {
  navigate: {
    url: string,
    params: any
  };
  breadcrumbs: any[];
}

export const initialState: State = {
  navigate: null,
  breadcrumbs: []
}

export function reducer(state = initialState, action: fromRouteActions.Actions): State {
  
  switch (action.type) {
    case fromRouteActions.CHANGE_ROUTE:

      return {
        ...state,
        navigate: {
          url: action.payload.url,
          params: action.payload.params,
        },
      }
    case fromRouteActions.CHANGE_BREADCRUMB:
      return {
        ...state,
        breadcrumbs: action.payload,
      }
    default: {
      return state;
    }

  }

}
