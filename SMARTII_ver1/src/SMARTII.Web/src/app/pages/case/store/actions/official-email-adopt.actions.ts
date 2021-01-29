import { Action } from '@ngrx/store';
import { OfficialEmailAutoOrderViewModel, OfficialEmailAdminOrderViewModel, OfficialEmailReplyRengeViewModel, OfficialEmailListViewModel, OfficialEmailBatchAdoptViewModel, OfficialEmailAdoptViewModel } from 'src/app/model/case.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const ADOPT_EMAIL = '[CASE] ADOPT_EMAIL';
export const AUTO_ASSIGN_EMAIL = '[CASE] AUTO_ASSIGN_EMAIL';
export const ADMIN_ASSIGN_EMAIL = '[CASE] ADMIN_ASSIGN_EMAIL';
export const REPLY_EMAIL = '[CASE] REPLY_EMAIL';
export const DELETE_RANGE_EMAIL = '[CASE] DELETE_RANGE_EMAIL';

export const ADOPT_EMAIL_CASE_SUCCESS = '[CASE] ADOPT_EMAIL_CASE_SUCCESS';
export const ADOPT_EMAIL_SUCCESS = '[CASE] ADOPT_EMAIL_SUCCESS';
export const ADOPT_EMAIL_SUCCESS_SHOWINFO = '[CASE] ADOPT_EMAIL_SUCCESS_SHOWINFO';
export const ADOPT_EMAIL_FAILED = '[CASE] ADOPT_EMAIL_FAILED';
export const LOAD_CASE_SOURCE_NATIVE = '[CASE] LOAD_CASE_SOURCE_NATIVE';
export const LOAD_CASE_SOURCE_NATIVE_SUCCESS = '[CASE] LOAD_CASE_SOURCE_NATIVE_SUCCESS';
export const LOAD_CASE_SOURCE_NATIVE_FAILED = '[CASE] LOAD_CASE_SOURCE_NATIVE_FAILED';
export const BATCH_ADOPT_EMAIL = '[CASE] BATCH_ADOPT_EMAIL';

export class adoptEmail implements Action {
    type: string = ADOPT_EMAIL;
    constructor(public payload: EntrancePayload<OfficialEmailAdoptViewModel>) { }
}

export class autoAssignEmail implements Action {
    type: string = AUTO_ASSIGN_EMAIL;
    constructor(public payload: EntrancePayload<OfficialEmailAutoOrderViewModel>) { }
}


export class adminAssignEmail implements Action {
    type: string = ADMIN_ASSIGN_EMAIL;
    constructor(public payload: EntrancePayload<OfficialEmailAdminOrderViewModel>) { }
}


export class replyEmail implements Action {
    type: string = REPLY_EMAIL;
    constructor(public payload: EntrancePayload<OfficialEmailReplyRengeViewModel>) { }
}


export class deleteRangeEmail implements Action {
    type: string = DELETE_RANGE_EMAIL;
    constructor(public payload: EntrancePayload<OfficialEmailListViewModel[]>) { }
}

export class adoptEmailCaseSuccess implements Action {
    type: string = ADOPT_EMAIL_CASE_SUCCESS;
    constructor(public payload: ResultPayload<any>) { }
}

export class success implements Action {
    type: string = ADOPT_EMAIL_SUCCESS;
    constructor(public payload: ResultPayload<any>) { }
}


export class fail implements Action {
    type: string = ADOPT_EMAIL_FAILED;
    constructor(public payload: string) { }
}

export class successShowInfo implements Action {
    type: string = ADOPT_EMAIL_SUCCESS_SHOWINFO;
    constructor(public payload: ResultPayload<any>) { }
}

export class batchAdoptEmail implements Action {
    type: string = BATCH_ADOPT_EMAIL;
    constructor(public payload: EntrancePayload<OfficialEmailBatchAdoptViewModel>) { }
}

export type Actions =
    autoAssignEmail |
    adminAssignEmail |
    successShowInfo |
    adoptEmailCaseSuccess |
    success |
    fail |
    batchAdoptEmail;
