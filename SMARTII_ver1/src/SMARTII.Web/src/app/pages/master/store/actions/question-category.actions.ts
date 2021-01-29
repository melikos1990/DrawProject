import { Action } from '@ngrx/store';
import { QuestionCategoryDetail, QuestionClassificationListViewModel, QuestionClassificationSearchViewModel } from 'src/app/model/question-category.model'
import { OrganizationType } from 'src/app/model/organization.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const CREATE_DETAIL = '[QUESTION-CATEGORY] CREATE_DETAIL';
export const CREATE_DETAIL_SUCCESS = '[QUESTION-CATEGORY] CREATE_DETAIL_SUCCESS';
export const GET_DETAIL = '[QUESTION-CATEGORY] GET_DETAIL';
export const GET_DETAIL_SUCCESS = '[QUESTION-CATEGORY] GET_DETAIL_SUCCESS';
export const EDIT_DETAIL = '[QUESTION-CATEGORY] EDIT_DETAIL';
export const EDIT_SUCCESS = '[QUESTION-CATEGORY] EDIT_SUCCESS';
export const EDIT_FAILED = '[QUESTION-CATEGORY] EDIT_FAILED';
export const DETELE_DETAIL = '[QUESTION-CATEGORY] DETELE_DETAIL';
export const DETELE_RANGE_DETAIL = '[QUESTION-CATEGORY] DETELE_RANGE_DETAIL';
export const CLEAR = '[QUESTION-CATEGORY] CLEAR';
export const EDIT_ORDER = '[QUESTION-CATEGORY] EDIT_ORDER';
export const EDIT_ORDER_SUCCESS = '[QUESTION-CATEGORY] EDIT_ORDER_SUCCESS';
export const EDIT_ORDER_FAILED = '[QUESTION-CATEGORY] EDIT_ORDER_FAILED';

export const DELETE_SUCCESS = '[QUESTION-CATEGORY] DELETE_SUCCESS';
export const SUCCESS = '[QUESTION-CATEGORY] SUCCESS';
export const FAILED = '[QUESTION-CATEGORY] FAILED';
export const GET_EXCEL = '[QUESTION-CATEGORY] GET_EXCEL';
export const GET_EXCEL_SUCCESS = '[QUESTION-CATEGORY] GET_EXCEL_SUCCESS';
export const GET_EXCEL_FAILED = '[QUESTION-CATEGORY] GET_EXCEL_FAILED';


export class CreateDetail implements Action {
    type: string = CREATE_DETAIL;
    constructor(public payload: QuestionCategoryDetail){}
}

// export class CreateDetailSuccess implements Action {
//     type: string = CREATE_DETAIL_SUCCESS;
//     constructor(public payload: {}){}
// } 

export class GetDetail implements Action {
    type: string = GET_DETAIL;
    constructor(public payload: {nodeID: number, id: number, organizationType: OrganizationType}){}
}

export class GetDetailSuccess implements Action {
    type: string = GET_DETAIL_SUCCESS;
    constructor(public payload: QuestionCategoryDetail){}
}

export class EditDetail implements Action {
    type: string = EDIT_DETAIL;
    constructor(public payload: QuestionCategoryDetail){}
}

export class EditSuccess implements Action {
    type: string = EDIT_SUCCESS;
    constructor(public payload: ResultPayload<any>){}
}

export class EditFailed implements Action {
    type: string = EDIT_FAILED;
    constructor(public payload: any){}
}

export class DeleteDetail implements Action {
    type: string = DETELE_DETAIL;
    constructor(public payload: EntrancePayload<{id: number}>){}
}

export class DeleteRangeDetail implements Action {
    type: string = DETELE_RANGE_DETAIL;
    constructor(public payload: EntrancePayload<QuestionCategoryDetail[]>){}
}

export class Clear implements Action {
    type: string = CLEAR;
    constructor(public payload?: any){}
}

export class DeleteSuccess implements Action {
    type: string = DELETE_SUCCESS;
    constructor(public payload: ResultPayload<any>){}
}

export class EditOrder implements Action {
    type: string = EDIT_ORDER;
    constructor(public payload: any[] ){}
}
export class EditOrderSuccess implements Action {
    type: string = EDIT_ORDER_SUCCESS;
    constructor(public payload: ResultPayload<any>){}
}

export class EditOrderFailed implements Action {
    type: string = EDIT_ORDER_FAILED;
    constructor(public payload: any){}
}

export class Success implements Action {
    type: string = SUCCESS;
    constructor(public payload: ResultPayload<any>){}
}

export class Failed implements Action {
    type: string = FAILED;
    constructor(public payload: any){}
}

export class GetRrport implements Action {
    type: string = GET_EXCEL;
    constructor(public payload: QuestionClassificationSearchViewModel){}
}

export class GetReportSuccess implements Action {
    public type: string = GET_EXCEL_SUCCESS;
    constructor(public payload: Blob) { }
  }
  
  
  export class GetReportFailed implements Action {
    public type: string = GET_EXCEL_FAILED;
    constructor(public payload: string) { }
  }


export type Actions = 
    GetDetail |
    GetDetailSuccess |
    CreateDetail |
    EditDetail |
    EditSuccess |
    EditFailed |
    DeleteRangeDetail |
    Clear |
    DeleteSuccess |
    EditOrder | 
    EditOrderSuccess |
    EditOrderFailed |
    Success |
    Failed |
    GetRrport |
    GetReportSuccess |
    GetReportFailed;


