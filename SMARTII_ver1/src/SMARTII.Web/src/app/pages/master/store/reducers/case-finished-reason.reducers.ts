import * as fromCaseFinishedActions from '../actions/case-finished-reason.actions';
import { CaseFinishDataDetailViewModel, CaseFinishClassificationDetailViewModel } from 'src/app/model/master.model';


export interface State {
    detail: CaseFinishDataDetailViewModel;
    classificationDetail: CaseFinishClassificationDetailViewModel;
}

export const initialState: State = {
    detail: null,
    classificationDetail: null
};

export function reducer(state: State = initialState, action: fromCaseFinishedActions.Actions) {
    switch (action.type) {
        case fromCaseFinishedActions.LOAD_DETAIL_SUCCESS:
            return {
                ...state,
                detail: action.payload
            };
        case fromCaseFinishedActions.LOAD_ENTRY:
            return {
                ...state,
                detail: action.payload
            };
        case fromCaseFinishedActions.LOAD_CLASSIFICATION_DETAIL_SUCCESS:
            return {
                ...state,
                classificationDetail: action.payload
            };
        case fromCaseFinishedActions.LOAD_CLASSIFICATION_ENTRY:
            return {
                ...state,
                classificationDetail: action.payload
            };

        default:
            return state;
    }
}
