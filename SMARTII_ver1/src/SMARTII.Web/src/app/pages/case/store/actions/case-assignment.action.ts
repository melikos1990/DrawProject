import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseAssignmentViewModel } from 'src/app/model/case.model';

export const EDIT_CASEASSIGNMENT = '[CASE ASSIGNMENT]  EDIT_CASEASSIGNMENT';
export const EDIT_CASEASSIGNMENT_SUCCESS = '[CASE ASSIGNMENT]  EDIT_CASEASSIGNMENT_SUCCESS';
export const EDIT_CASEASSIGNMENT_FAILED = '[CASE ASSIGNMENT]  EDIT_CASEASSIGNMENT_FAILED';
export const REFILL_CASEASSIGNMENT = '[CASE ASSIGNMENT]  REFILL_CASEASSIGNMENT';
export const REFILL_CASEASSIGNMENT_SUCCESS = '[CASE ASSIGNMENT]  REFILL_CASEASSIGNMENT_SUCCESS';
export const REFILL_CASEASSIGNMENT_FAILED = '[CASE ASSIGNMENT]  REFILL_CASEASSIGNMENT_FAILED';
export const PROCESSED_CASEASSIGNMENT = '[CASE ASSIGNMENT] PROCESSED_CASEASSIGNMENT';
export const PROCESSED_CASEASSIGNMENT_SUCCESS = '[CASE ASSIGNMENT]  PROCESSED_CASEASSIGNMENT_SUCCESS';
export const PROCESSED_CASEASSIGNMENT_FAILED = '[[CASE ASSIGNMENT]  PROCESSED_CASEASSIGNMENT_FAILED';


export class editCaseAssignmentAction implements Action {
    type: string = EDIT_CASEASSIGNMENT;
    constructor(public payload: EntrancePayload<CaseAssignmentViewModel>) { }
}


export class editCaseAssignmentSuccessAction implements Action {
    type: string = EDIT_CASEASSIGNMENT_SUCCESS;
    constructor(public payload: ResultPayload<CaseAssignmentViewModel>) { }
}

export class editCaseAssignmentFailedAction implements Action {
    type: string = EDIT_CASEASSIGNMENT_FAILED;
    constructor(public payload: ResultPayload<string>) { }
}

export class refillCaseAssignmentAction implements Action {
    type: string = REFILL_CASEASSIGNMENT;
    constructor(public payload: EntrancePayload<CaseAssignmentViewModel>) { }
}


export class refillCaseAssignmentSuccessAction implements Action {
    type: string = REFILL_CASEASSIGNMENT_SUCCESS;
    constructor(public payload: ResultPayload<CaseAssignmentViewModel>) { }
}

export class refillCaseAssignmentFailedAction implements Action {
    type: string = REFILL_CASEASSIGNMENT_FAILED;
    constructor(public payload: ResultPayload<string>) { }
}



export class processedCaseAssignmentAction implements Action {
    type: string = PROCESSED_CASEASSIGNMENT;
    constructor(public payload: EntrancePayload<CaseAssignmentViewModel>) { }
}


export class processedCaseAssignmentSuccessAction implements Action {
    type: string = PROCESSED_CASEASSIGNMENT_SUCCESS;
    constructor(public payload: ResultPayload<CaseAssignmentViewModel>) { }
}

export class processedCaseAssignmentFailedAction implements Action {
    type: string = PROCESSED_CASEASSIGNMENT_FAILED;
    constructor(public payload: ResultPayload<string>) { }
}



export type Actions =
    editCaseAssignmentAction |
    editCaseAssignmentSuccessAction |
    editCaseAssignmentFailedAction |
    refillCaseAssignmentAction |
    refillCaseAssignmentSuccessAction |
    refillCaseAssignmentFailedAction |
    processedCaseAssignmentAction |
    processedCaseAssignmentSuccessAction |
    processedCaseAssignmentFailedAction;