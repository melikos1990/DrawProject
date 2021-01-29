import { Action } from '@ngrx/store';
import { QuestionClassificationGuideDetail, QuestionClassificationGuideViewModel } from 'src/app/model/question-category.model'
import { OrganizationType } from 'src/app/model/organization.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const CREATE_DETAIL = '[QUESTION-GUIDE-CATEGORY] CREATE_DETAIL';
export const CREATE_DETAIL_SUCCESS = '[QUESTION-GUIDE-CATEGORY] CREATE_DETAIL_SUCCESS';
export const GET_DETAIL = '[QUESTION-GUIDE-CATEGORY] GET_DETAIL';
export const GET_DETAIL_SUCCESS = '[QUESTION-GUIDE-CATEGORY] GET_DETAIL_SUCCESS';
export const EDIT_DETAIL = '[QUESTION-GUIDE-CATEGORY] EDIT_DETAIL';
export const EDIT_SUCCESS = '[QUESTION-GUIDE-CATEGORY] EDIT_SUCCESS';
export const EDIT_FAILED = '[QUESTION-GUIDE-CATEGORY] EDIT_FAILED';
export const DETELE_DETAIL = '[QUESTION-GUIDE-CATEGORY] DETELE_DETAIL';
export const DETELE_RANGE_DETAIL = '[QUESTION-GUIDE-CATEGORY] DETELE_RANGE_DETAIL';
export const CLEAR = '[QUESTION-GUIDE-CATEGORY] CLEAR';
export const EDIT_ORDER = '[QUESTION-GUIDE-CATEGORY] EDIT_ORDER';

export const DELETE_SUCCESS = '[QUESTION-GUIDE-CATEGORY] DELETE_SUCCESS';
export const SUCCESS = '[QUESTION-GUIDE-CATEGORY] SUCCESS';
export const FAILED = '[QUESTION-GUIDE-CATEGORY] FAILED';
export const CHECK_QUESTION_CATEGORY = '[QUESTION-GUIDE-CATEGORY] CHECK_QUESTION_CATEGORY';
export const CHECK_SUCCESS = '[QUESTION-GUIDE-CATEGORY] CHECK_SUCCESS';

export class CreateDetailAction implements Action {
    type: string = CREATE_DETAIL;
    constructor(public payload: QuestionClassificationGuideViewModel){}
}

export class GetDetailAction implements Action {
    type: string = GET_DETAIL;
    constructor(public payload: {id: number}){}
}

export class GetDetailSuccessAction implements Action {
    type: string = GET_DETAIL_SUCCESS;
    constructor(public payload: QuestionClassificationGuideDetail){}
}

export class EditDetailAction implements Action {
    type: string = EDIT_DETAIL;
    constructor(public payload: QuestionClassificationGuideViewModel){}
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
    constructor(public payload: EntrancePayload<QuestionClassificationGuideDetail[]>){}
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


