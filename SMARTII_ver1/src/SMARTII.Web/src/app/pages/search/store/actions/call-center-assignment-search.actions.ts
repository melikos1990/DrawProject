import { Action } from '@ngrx/store';
import { CaseAssignmentCallCenterSearchViewModel } from 'src/app/model/search.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const CASE_CC_REPORT = '[CASE_ASSIGNMENT_SEARCH] CASE_CC_REPORT';
export const CASE_CC_REPORT_SUCCESS = '[CASE_ASSIGNMENT_SEARCH] CASE_CC_REPORT_SUCCESS';
export const CASE_CC_REPORT_FAILED = '[CASE_ASSIGNMENT_SEARCH] CASE_CC_REPORT_FAILED';
export const CASE_CC_GETLIST = '[CASE_ASSIGNMENT_SEARCH] CASE_CC_GETLIST';
export const CASE_CC_GETLIST_SUCCESS = '[CASE_ASSIGNMENT_SEARCH] CASE_CC_GETLIST_SUCCESS';
export const CASE_CC_GETLIST_FAILED = '[CASE_ASSIGNMENT_SEARCH] CASE_CC_GETLIST_FAILED';


export class callCenterReport implements Action {
    public type: string = CASE_CC_REPORT;
    constructor(public payload: CaseAssignmentCallCenterSearchViewModel) { }
}


export class callCenterReportSuccess implements Action {
    public type: string = CASE_CC_REPORT_SUCCESS;
    constructor(public payload: Blob) { }
}


export class callCenterReportFailed implements Action {
    public type: string = CASE_CC_REPORT_FAILED;
    constructor(public payload: string) { }
}


export class callCenterGetList implements Action {
    public type: string = CASE_CC_GETLIST;
    constructor(public payload: EntrancePayload<CaseAssignmentCallCenterSearchViewModel>) { }
}


export class callCenterGetListSuccess implements Action {
    public type: string = CASE_CC_GETLIST_SUCCESS;
    constructor(public payload: ResultPayload<any[]>) { }
}


export class callCenterGetListFailed implements Action {
    public type: string = CASE_CC_GETLIST_FAILED;
    constructor(public payload: ResultPayload<any>) { }
}

export type Actions = callCenterGetList | callCenterGetListSuccess | callCenterGetListFailed |
callCenterReport | callCenterReportSuccess | callCenterReportFailed;
