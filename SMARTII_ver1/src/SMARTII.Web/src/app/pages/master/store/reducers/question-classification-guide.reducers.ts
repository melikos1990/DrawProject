import * as fromQuestionGuideAction from '../actions/question-classification-guide.actions';
import { QuestionClassificationGuideDetail } from 'src/app/model/question-category.model';

export interface State {
	detail: QuestionClassificationGuideDetail
}

export const initialState: State = {
	detail: null
}

export function reducer(state: State = initialState, action: fromQuestionGuideAction.Actions) {
	switch (action.type) {

		case fromQuestionGuideAction.GET_DETAIL_SUCCESS:
			return {
				...state,
				detail: action.payload
			}

		case fromQuestionGuideAction.CLEAR:
			return {
				...state,
				detail: null
			};

		default:
			return state;
	}
}
