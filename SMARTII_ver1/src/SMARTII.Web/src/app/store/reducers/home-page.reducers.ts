import * as fromHomePageActions from '../actions/home-page.actions';
import { OrganizationType } from 'src/app/model/organization.model';

export interface State {
    homeID: string;
}

export const initialState: State = {
    homeID: null,
};

export function reducer(state: State = initialState, action: fromHomePageActions.Actions) {
    switch (action.type) {
        case fromHomePageActions.CHANGE_HOME_TYPE:
            return {
                ...state,
                homeID: action.payload
            };
        default:
            return state;
    }
}
