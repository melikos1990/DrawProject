import * as fromWorkScheduleAction from '../actions/work-schedule.actions';
import { QuestionClassificationGuideDetail } from 'src/app/model/question-category.model';
import { WorkScheduleDetailViewModel } from 'src/app/model/master.model';

export interface State {
	detail: WorkScheduleDetailViewModel
}

export const initialState: State = {
	detail: null
}

export function reducer(state: State = initialState, action: fromWorkScheduleAction.Actions) {
	let { type, payload } = action;

	switch (type) {
		
		case fromWorkScheduleAction.LOAD_DETAIL_ENTRY:
		case fromWorkScheduleAction.LOAD_DETAIL_SUCCESS:
			return {
				...state,
				detail: payload
			}
		
		default:
			return state;
	}
}
