import * as fromCaseAssignmentActions from '../actions/case-assignment.action';

export interface State {

}

export const initialState: State = {

};



export function reducer(state: State = initialState, action: fromCaseAssignmentActions.Actions) {
    switch (action.type) {

        default:
            return state;
    }
}

