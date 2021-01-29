import * as fromLoadingActions from '../actions/loading.actions';

export interface State {
    visible: boolean
}

export const initialState: State = {
    visible: false
}

export function reducer(state = initialState, action: fromLoadingActions.Actions): State {
    console.log(action.type);
    switch (action.type) {
        case fromLoadingActions.INVISIBLE_LOADING:
            return {
                ...state,
                visible: false
            }
        case fromLoadingActions.VISIBLE_LOADING:
            return {
                ...state,
                visible: true
            }
        default : {
            return state;
        }

    }

}