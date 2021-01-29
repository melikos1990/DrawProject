import * as fromCaseTemplateReducer from "./case-template.reducers";
import * as fromItemReducer from "./item.reducers";
import * as fromQuestionCategoryReducer from "./question-category.reducers";
import * as fromQuestionAnswerCategoryReducer from "./question-classification-answer.reducers";
import * as fromQuestionGuideCategoryReducer from "./question-classification-guide.reducers";
import * as fromBillboardReducer from "./billboard.reduders";
import * as fromCaseAssignGroupReducer from "./case-assign-group.reducers";
import * as fromNotificationGroupReducer from './notification-group.reducers';
import * as fromCaseTagReducer from './case-tag.reducers';
import * as fromStoresReducer from "./stores.reducers";
import * as fromCaseFinishedReasonReducer from './case-finished-reason.reducers';
import * as fromCaseRemindReducer from './case-remind.reducers';
import * as fromRootReducer from 'src/app/store/reducers';
import * as fromOfficialEmailGroupReducer from "./official-email-group.reducers";
import * as fromCaseWarningReducer from "./case-warning.reducers";
import * as fromWorkScheduleReducer from "./work-schedule.reducers";

import { createSelector } from '@ngrx/store';
import { ActionType } from 'src/app/model/common.model';



export interface IndexState {
  caseTemplate: fromCaseTemplateReducer.State;
  item: fromItemReducer.State;
  questionCategory: fromQuestionCategoryReducer.State;
  questionClassificationAnswer: fromQuestionAnswerCategoryReducer.State;
  questionClassificationGuide: fromQuestionGuideCategoryReducer.State;
  notificationGroup: fromNotificationGroupReducer.State;
  billboard: fromBillboardReducer.State;
  caseAssignGroup: fromCaseAssignGroupReducer.State;
  caseTag: fromCaseTagReducer.State;
  caseFinishedReason: fromCaseFinishedReasonReducer.State;
  stores: fromStoresReducer.State;
  caseRemind: fromCaseRemindReducer.State;
  officialEmailGroup: fromOfficialEmailGroupReducer.State;
  caseWarning: fromCaseWarningReducer.State;
  workSchedule: fromWorkScheduleReducer.State;
}

export interface State extends fromRootReducer.State {
  master: IndexState;
}

export const reducer = {
  caseTemplate: fromCaseTemplateReducer.reducer,
  item: fromItemReducer.reducer,
  questionCategory: fromQuestionCategoryReducer.reducer,
  questionClassificationAnswer: fromQuestionAnswerCategoryReducer.reducer,
  questionClassificationGuide: fromQuestionGuideCategoryReducer.reducer,
  billboard: fromBillboardReducer.reducer,
  caseAssignGroup: fromCaseAssignGroupReducer.reducer,
  notificationGroup: fromNotificationGroupReducer.reducer,
  caseTag: fromCaseTagReducer.reducer,
  caseFinishedReason: fromCaseFinishedReasonReducer.reducer,
  stores: fromStoresReducer.reducer,
  caseRemind: fromCaseRemindReducer.reducer,
  officialEmailGroup: fromOfficialEmailGroupReducer.reducer,
  caseWarning: fromCaseWarningReducer.reducer,
  workSchedule: fromWorkScheduleReducer.reducer,
}




