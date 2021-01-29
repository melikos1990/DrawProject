import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CaseFinishDataDetailViewModel, CaseFinishClassificationDetailViewModel } from 'src/app/model/master.model';


export const ADD = '[CASE_FINISHED_REASON] ADD';
export const ADD_SUCCESS = '[CASE_FINISHED_REASON] ADD SUCCESS';
export const ADD_FAILED = '[CASE_ASSOGN_GROUPCASE_FINISHED_REASON] ADD FAILED';
export const EDIT = '[CASE_FINISHED_REASON] EDIT';
export const EDIT_SUCCESS = '[CASE_FINISHED_REASON] EDIT SUCCESS';
export const EDIT_FAILED = '[CASE_FINISHED_REASON] EDIT FAILED';
export const DISABLED = '[CASE_FINISHED_REASON] DISABLED';
export const DISABLED_SUCCESS = '[CASE_FINISHED_REASON] DISABLED SUCCESS';
export const DISABLED_FAILED = '[CASE_FINISHED_REASON] DISABLED FAILED';
export const LOAD_ENTRY = '[CASE_FINISHED_REASON] LOAD ENTRY';
export const LOAD_DETAIL = '[CASE_FINISHED_REASON] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[CASE_FINISHED_REASON] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[CASE_FINISHED_REASON] LOAD DETAIL FAILED';
export const LOAD_CLASSIFICATION_ENTRY = '[CASE_FINISHED_REASON] LOAD CLASSIFICATION ENTRY';
export const LOAD_CLASSIFICATION_DETAIL = '[CASE_FINISHED_REASON] LOAD CLASSIFICATION DETAIL';
export const LOAD_CLASSIFICATION_DETAIL_SUCCESS = '[CASE_FINISHED_REASON] LOAD CLASSIFICATION DETAIL SUCCESS';
export const LOAD_CLASSIFICATION_DETAIL_FAILED = '[CASE_FINISHED_REASON] LOAD CLASSIFICATION DETAIL FAILED';


export const ADD_CLASSIFICATION = '[CASE_FINISHED_REASON] ADD CLASSIFICATION';
export const ADD_CLASSIFICATION_SUCCESS = '[CASE_FINISHED_REASON] ADD CLASSIFICATION SUCCESS ';
export const ADD_CLASSIFICATION_FAILED = '[CASE_FINISHED_REASON] ADD CLASSIFICATION FAILED';
export const EDIT_CLASSIFICATION = '[CASE_FINISHED_REASON] EDIT CLASSIFICATION';
export const EDIT_CLASSIFICATION_SUCCESS = '[CASE_FINISHED_REASON] EDIT CLASSIFICATION SUCCESS';
export const EDIT_CLASSIFICATION_FAILED = '[CASE_FINISHED_REASON] EDIT CLASSIFICATION FAILED';

export const CHECK_SINGLE = '[CASE_FINISHED_REASON] CHECK SINGLE';
export const CHECK_SINGLE_SUCCESS = '[CASE_FINISHED_REASON] CHECK SINGLE SUCCESS';
export const CHECK_SINGLE_FAILED = '[CASE_FINISHED_REASON] CHECK SINGLE FAILED';


export const ORDER_DATA = '[CASE_FINISHED_REASON] ORDER DATA';
export const ORDER_DATA_SUCCESS = '[CASE_FINISHED_REASON] ORDER DATA SUCCESS ';
export const ORDER_DATA_FAILED = '[CASE_FINISHED_REASON] ORDER DATA FAILED';

export const ORDER_CLASSIFICATION = '[CASE_FINISHED_REASON] ORDER CLASSIFICATION';
export const ORDER_CLASSIFICATION_SUCCESS = '[CASE_FINISHED_REASON] ORDER CLASSIFICATION SUCCESS ';
export const ORDER_CLASSIFICATION_FAILED = '[CASE_FINISHED_REASON] ORDER CLASSIFICATION FAILED';

export class loadDetailAction implements Action {
    public type: string = LOAD_DETAIL;
    constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
    public type: string = LOAD_DETAIL_SUCCESS;
    constructor(public payload: CaseFinishDataDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
    public type: string = LOAD_DETAIL_FAILED;
    constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
    public type: string = LOAD_ENTRY;
    constructor(public payload: CaseFinishDataDetailViewModel = new CaseFinishDataDetailViewModel()) { }
}

export class addAction implements Action {
    public type: string = ADD
    constructor(public payload: CaseFinishDataDetailViewModel) { }
}

export class addSuccessAction implements Action {
    public type: string = ADD_SUCCESS
    constructor(public payload: string) { }
}

export class addFailedAction implements Action {
    public type: string = ADD_FAILED
    constructor(public payload: string) { }
}

export class editAction implements Action {
    public type: string = EDIT
    constructor(public payload: CaseFinishDataDetailViewModel) { }
}

export class editSuccessAction implements Action {
    public type: string = EDIT_SUCCESS
    constructor(public payload: string) { }
}

export class editFailedAction implements Action {
    public type: string = EDIT_FAILED
    constructor(public payload: string) { }
}


export class disabledAction implements Action {
    public type: string = DISABLED
    constructor(public payload: EntrancePayload<{ ID?: number }>) { }
}

export class disabledSuccessAction implements Action {
    public type: string = DISABLED_SUCCESS
    constructor(public payload: ResultPayload<string>) { }
}

export class disabledFailedAction implements Action {
    public type: string = DISABLED_FAILED
    constructor(public payload: ResultPayload<string>) { }
}


export class loadClassificationDetailAction implements Action {
    public type: string = LOAD_CLASSIFICATION_DETAIL;
    constructor(public payload: { ID?: number }) { }
}

export class loadClassificationDetailSuccessAction implements Action {
    public type: string = LOAD_CLASSIFICATION_DETAIL_SUCCESS;
    constructor(public payload: CaseFinishClassificationDetailViewModel) { }
}

export class loadClassificationDetailFailedAction implements Action {
    public type: string = LOAD_CLASSIFICATION_DETAIL_FAILED;
    constructor(public payload: string) { }
}

export class loadClassificationEntryAction implements Action {
    public type: string = LOAD_CLASSIFICATION_ENTRY;
    constructor(public payload: CaseFinishClassificationDetailViewModel = new CaseFinishClassificationDetailViewModel()) { }
}


export class addClassificationAction implements Action {
    public type: string = ADD_CLASSIFICATION
    constructor(public payload: EntrancePayload<CaseFinishClassificationDetailViewModel>) { }
}

export class addClassificationSuccessAction implements Action {
    public type: string = ADD_CLASSIFICATION_SUCCESS
    constructor(public payload: ResultPayload<string>) { }
}

export class addClassificationFailedAction implements Action {
    public type: string = ADD_CLASSIFICATION_FAILED
    constructor(public payload: ResultPayload<string>) { }
}

export class editClassificationAction implements Action {
    public type: string = EDIT_CLASSIFICATION
    constructor(public payload: EntrancePayload<CaseFinishClassificationDetailViewModel>) { }
}

export class editClassificationSuccessAction implements Action {
    public type: string = EDIT_CLASSIFICATION_SUCCESS
    constructor(public payload: ResultPayload<string>) { }
}

export class editClassificationFailedAction implements Action {
    public type: string = EDIT_CLASSIFICATION_FAILED
    constructor(public payload: ResultPayload<string>) { }
}

export class checkSingleAction implements Action {
    public type: string = CHECK_SINGLE
    constructor(public payload: EntrancePayload<{ ID: number }>) { }
}

export class checkSingleSuccessAction implements Action {
    public type: string = CHECK_SINGLE_SUCCESS
    constructor(public payload: ResultPayload<{ isExist: boolean, message: string }>) { }
}

export class checkSingleFailedAction implements Action {
    public type: string = CHECK_SINGLE_FAILED;
    constructor(public payload: ResultPayload<string>) { }
}


export class orderAction implements Action {
    public type: string = ORDER_DATA
    constructor(public payload: EntrancePayload<any[]>) { }
}

export class orderSuccessAction implements Action {
    public type: string = ORDER_DATA_SUCCESS
    constructor(public payload: ResultPayload<string>) { }
}

export class orderFailedAction implements Action {
    public type: string = ORDER_DATA_FAILED;
    constructor(public payload: ResultPayload<string>) { }
}

export class orderClassificationAction implements Action {
    public type: string = ORDER_CLASSIFICATION
    constructor(public payload: EntrancePayload<any[]>) { }
}

export class orderClassificationSuccessAction implements Action {
    public type: string = ORDER_CLASSIFICATION_SUCCESS
    constructor(public payload: ResultPayload<string>) { }
}

export class orderClassificationFailedAction implements Action {
    public type: string = ORDER_CLASSIFICATION_FAILED;
    constructor(public payload: ResultPayload<string>) { }
}

export type Actions =
    loadDetailAction |
    loadDetailSuccessAction |
    loadDetailFailedAction |
    loadEntryAction |
    loadClassificationDetailAction |
    loadClassificationDetailSuccessAction |
    loadClassificationDetailFailedAction |
    loadClassificationEntryAction |
    disabledAction |
    disabledFailedAction |
    disabledSuccessAction |
    addAction |
    addSuccessAction |
    addFailedAction |
    editAction |
    editSuccessAction |
    editFailedAction |
    addClassificationAction |
    addClassificationSuccessAction |
    addClassificationFailedAction |
    editClassificationAction |
    editClassificationSuccessAction |
    editClassificationFailedAction |
    checkSingleAction |
    checkSingleSuccessAction |
    checkSingleFailedAction |
    orderSuccessAction |
    orderAction |
    orderFailedAction |
    orderClassificationAction |
    orderClassificationSuccessAction |
    orderClassificationFailedAction;
