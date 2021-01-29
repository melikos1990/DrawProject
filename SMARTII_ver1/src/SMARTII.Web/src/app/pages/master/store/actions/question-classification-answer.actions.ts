import { Action } from '@ngrx/store';
import { QuestionClassificationAnswerDetail, QuestionClassificationAnswerViewModel } from 'src/app/model/question-category.model'
import { OrganizationType } from 'src/app/model/organization.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const CREATE_DETAIL = '[QUESTION-ANSWER-CATEGORY] CREATE_DETAIL';
export const CREATE_DETAIL_SUCCESS = '[QUESTION-ANSWER-CATEGORY] CREATE_DETAIL_SUCCESS';
export const GET_DETAIL = '[QUESTION-ANSWER-CATEGORY] GET_DETAIL';
export const GET_DETAIL_SUCCESS = '[QUESTION-ANSWER-CATEGORY] GET_DETAIL_SUCCESS';
export const EDIT_DETAIL = '[QUESTION-ANSWER-CATEGORY] EDIT_DETAIL';
export const EDIT_SUCCESS = '[QUESTION-ANSWER-CATEGORY] EDIT_SUCCESS';
export const EDIT_FAILED = '[QUESTION-ANSWER-CATEGORY] EDIT_FAILED';
export const DETELE_DETAIL = '[QUESTION-ANSWER-CATEGORY] DETELE_DETAIL';
export const DETELE_RANGE_DETAIL = '[QUESTION-ANSWER-CATEGORY] DETELE_RANGE_DETAIL';
export const CLEAR = '[QUESTION-ANSWER-CATEGORY] CLEAR';
export const EDIT_ORDER = '[QUESTION-ANSWER-CATEGORY] EDIT_ORDER';

export const DELETE_SUCCESS = '[QUESTION-ANSWER-CATEGORY] DELETE_SUCCESS';
export const SUCCESS = '[QUESTION-ANSWER-CATEGORY] SUCCESS';
export const FAILED = '[QUESTION-ANSWER-CATEGORY] FAILED';
export const CHECK_QUESTION_CATEGORY = '[QUESTION-ANSWER-CATEGORY] CHECK_QUESTION_CATEGORY';
export const CHECK_SUCCESS = '[QUESTION-ANSWER-CATEGORY] CHECK_SUCCESS';

export class CreateDetailAction implements Action {
    type: string = CREATE_DETAIL;
    constructor(public payload: QuestionClassificationAnswerViewModel[]){}
}

// export class CreateDetailSuccess implements Action {
//     type: string = CREATE_DETAIL_SUCCESS;
//     constructor(public payload: {}){}
// } 

export class GetDetailAction implements Action {
    type: string = GET_DETAIL;
    constructor(public payload: {id: number}){}
}

export class GetDetailSuccessAction implements Action {
    type: string = GET_DETAIL_SUCCESS;
    constructor(public payload: QuestionClassificationAnswerDetail){}
}

export class EditDetailAction implements Action {
    type: string = EDIT_DETAIL;
    constructor(public payload: QuestionClassificationAnswerViewModel){}
}
export class EditSuccessAction implements Action {
    type: string = EDIT_SUCCESS;
    constructor(public payload: string){}
}

export class EditFailedAction implements Action {
    type: string = EDIT_FAILED;
    constructor(public payload: string){}
}

export class DeleteDetailAction implements Action {
    type: string = DETELE_DETAIL;
    constructor(public payload: EntrancePayload<{id: number}>){}
}

export class DeleteRangeDetailAction implements Action {
    type: string = DETELE_RANGE_DETAIL;
    constructor(public payload: EntrancePayload<QuestionClassificationAnswerDetail[]>){}
}

export class ClearAction implements Action {
    type: string = CLEAR;
    constructor(public payload?: any){}
}

export class DeleteSuccessAction implements Action {
    type: string = DELETE_SUCCESS;
    constructor(public payload: ResultPayload<any>){}
}

export class CheckSuccessAction implements Action {
    type: string = CHECK_SUCCESS;
    constructor(public payload: ResultPayload<any>){}
}

export class EditOrderAction implements Action {
    type: string = EDIT_ORDER;
    constructor(public payload: any[] ){}
}

export class SuccessAction implements Action {
    type: string = SUCCESS;
    constructor(public payload: string){}
}

export class FailedAction implements Action {
    type: string = FAILED;
    constructor(public payload: string){}
}

export class CheckQuestionCategoryAction implements Action {
    type: string = CHECK_QUESTION_CATEGORY;
    constructor(public payload: EntrancePayload<{BuID: number}>){}
}

export type Actions = 
    GetDetailAction |
    GetDetailSuccessAction |
    CreateDetailAction |
    EditDetailAction | 
    EditSuccessAction |
    EditFailedAction |
    DeleteRangeDetailAction |
    ClearAction |
    DeleteSuccessAction |
    CheckSuccessAction |
    EditOrderAction | 
    SuccessAction |
    FailedAction |
    CheckQuestionCategoryAction;


