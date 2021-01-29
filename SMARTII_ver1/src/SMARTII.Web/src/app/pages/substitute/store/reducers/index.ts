import * as fromCaseApplyReducer from "./case-apply.reducers";
import * as fromCaseNoticeReducer from "./case-notice.reducers";
import * as fromRootReducer from "../../../../store/reducers"


export interface IndexState {
  caseApply: fromCaseApplyReducer.State;
  caseNotice: fromCaseNoticeReducer.State;
}

export interface State extends fromRootReducer.State {
  substitute: IndexState;
}

export const reducer = {
  caseApply: fromCaseApplyReducer.reducer,
  caseNotice: fromCaseNoticeReducer.reducer,
}
