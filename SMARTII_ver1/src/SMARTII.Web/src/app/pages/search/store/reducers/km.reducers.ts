import * as fromKMActions from '../actions/km.actions';
import { KMClassificationNodeViewModel, KMDetailViewModel } from 'src/app/model/master.model';
import { createSelector } from '@ngrx/store';

export interface State {
    tree: KMClassificationNodeViewModel[];
    detail: KMDetailViewModel;
    selectedItem: KMClassificationNodeViewModel;
}

export const initialState: State = {
    tree: null,
    detail: null,
    selectedItem: null
};


  

export function reducer(state: State = initialState, action: fromKMActions.Actions) {
    switch (action.type) {
        case fromKMActions.LOAD_TREE_SUCCESS:
            return {
                ...state,
                tree: action.payload
            };
        case fromKMActions.LOAD_DETAIL_SUCCESS:
            return {
                ...state,
                detail: action.payload
            };
        case fromKMActions.LOAD_ENTRY:
            return {
                ...state,
                detail: action.payload
            };
        case fromKMActions.SELECT_ITEM:
            return {
                ...state,
                selectedItem: action.payload
            };
        default:
            return state;
    }
}
