import * as fromQuestionCategoryAction from '../actions/question-classification-answer.actions';
import { QuestionClassificationAnswerDetail } from 'src/app/model/question-category.model';

export interface State {
	detail: QuestionClassificationAnswerDetail
}

export const initialState: State = {
	detail: null
}

export function reducer(state: State = initialState, action: fromQuestionCategoryAction.Actions) {
	switch (action.type) {

		case fromQuestionCategoryAction.GET_DETAIL_SUCCESS:
			return {
				...state,
				detail: action.payload
			}

		case fromQuestionCategoryAction.CLEAR:
			return {
				...state,
				detail: null
			};

		default:
			return state;
	}
}
