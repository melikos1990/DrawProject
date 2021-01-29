

import * as fromRootReducer from 'src/app/store/reducers';
import * as fromCallCenterCaseSearchReducer from './call-center-case-search.reducers';
import * as fromHeaderQurterStoreCaseSearchReducer from './headerqurter-store-case-search.reducers';
import * as fromCallCenterCaseAssignmentSearchReducer from './call-center-case-assignment-search.reducers';
import * as fromHeaderQurterStoreCaseAssignmentSearchReducer from './headerqurter-store-case-assignment-search.reducers';
import * as fromVendorCaseAssignmentSearchReducer from './vendor-case-assignment-search.reducers';
import * as fromKMReducer from './km.reducers';
import { ActionType } from 'src/app/model/common.model';
import { createSelector } from '@ngrx/store';

export interface IndexState {
    callCenterCaseSearch: fromCallCenterCaseSearchReducer.State,
    headerQurterStoreCaseSearch: fromHeaderQurterStoreCaseSearchReducer.State,
    callCenterCaseAssignmentSearch: fromCallCenterCaseAssignmentSearchReducer.State,
    headerQurterStoreCaseAssignmentSearch: fromHeaderQurterStoreCaseAssignmentSearchReducer.State
    vendorCaseAssignmentSearch: fromVendorCaseAssignmentSearchReducer.State
    km: fromKMReducer.State;
}


export interface State extends fromRootReducer.State {
  mySearch: IndexState // 取名 mySearch 原因是 search 會導自Reducer無法觸發(可能是Nebular內部已在用search keyword)
}

export const reducer = {
    callCenterCaseSearch: fromCallCenterCaseSearchReducer.reducer,
    headerQurterStoreCaseSearch: fromHeaderQurterStoreCaseSearchReducer.reducer,
    callCenterCaseAssignmentSearch: fromCallCenterCaseAssignmentSearchReducer.reducer,
    headerQurterStoreCaseAssignmentSearch: fromHeaderQurterStoreCaseAssignmentSearchReducer.reducer,
    vendorCaseAssignmentSearch: fromVendorCaseAssignmentSearchReducer.reducer,
    km: fromKMReducer.reducer,
}

export const kmDetailSelector = (actionType: ActionType) => createSelector(
    (state: State) => state.mySearch.km.detail,
    (state: State) => state.mySearch.km.selectedItem,
    (detail, selectedItem) => {
  
  
      if (actionType == ActionType.Add && selectedItem) {
  
        detail.ClassificationID = selectedItem.ClassificationID;
        detail.ClassificationName = selectedItem.ClassificationName;
  
      }
  
      return detail;
  
  
    }
  );
