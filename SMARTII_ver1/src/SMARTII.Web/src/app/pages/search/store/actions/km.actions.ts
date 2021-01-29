import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { KMClassificationNodeViewModel, KMDetailViewModel } from 'src/app/model/master.model';

export const ADD = '[KM] ADD';
export const ADD_SUCCESS = '[KM] ADD SUCCESS';
export const ADD_FAILED = '[KM] ADD FAILED';
export const EDIT = '[KM] EDIT';
export const EDIT_SUCCESS = '[KM] EDIT SUCCESS';
export const EDIT_FAILED = '[KM] EDIT FAILED';

export const DELETE = '[KM] DELETE';
export const DELETE_SUCCESS = '[KM] DELETE SUCCESS';
export const DELETE_FAILED = '[KM] DELETE FAILED';

export const LOAD_ENTRY = '[KM] LOAD ENTRY';
export const LOAD_DETAIL = '[KM] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[KM] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[KM] LOAD DETAIL FAILED';

export const LOAD_TREE = '[KM] LOAD TREE'
export const LOAD_TREE_SUCCESS = '[KM] LOAD TREE SUCCESS';
export const LOAD_TREE_FAILED = '[KM] LOAD TREE FAILED';

export const SELECT_ITEM = '[KM] SELECT ITEM';
export const RENAME_CLASSIFICATION = '[KM] RENAME CLASSIFICATION'
export const RENAME_CLASSIFICATION_SUCCESS = '[KM] RENAME CLASSIFICATION SUCCESS'
export const RENAME_CLASSIFICATION_FAILED = '[KM] RENAME CLASSIFICATION FAILED'
export const DELETE_CLASSIFICATION = '[KM] DELETE CLASSIFICATION'
export const DELETE_CLASSIFICATION_SUCCESS = '[KM] DELETE CLASSIFICATION SUCCESS'
export const DELETE_CLASSIFICATION_FAILED = '[KM] DELETE CLASSIFICATION FAILED'
export const ADD_CLASSIFICATION = '[KM] ADD CLASSIFICATION'
export const ADD_CLASSIFICATION_SUCCESS = '[KM] ADD CLASSIFICATION SUCCESS'
export const ADD_CLASSIFICATION_FAILED = '[KM] ADD CLASSIFICATION FAILED'
export const ADD_ROOT_CLASSIFICATION = '[KM] ADD ROOT CLASSIFICATION'
export const ADD_ROOT_CLASSIFICATION_SUCCESS = '[KM] ADD ROOT CLASSIFICATION SUCCESS'
export const ADD_ROOT_CLASSIFICATION_FAILED = '[KM] ADD ROOT CLASSIFICATION FAILED'
export const DRAG_CLASSIFICATION = '[KM] DRAG CLASSIFICATION'
export const DRAG_CLASSIFICATION_SUCCESS = '[KM] DRAG CLASSIFICATION SUCCESS'
export const DRAG_CLASSIFICATION_FAILED = '[KM] DRAG CLASSIFICATION FAILED'

export class selectItemAction implements Action {
    public type: string = SELECT_ITEM;
    constructor(public payload: KMClassificationNodeViewModel) { }
}

export class loadTreeAction implements Action {
    public type: string = LOAD_TREE;
    constructor(public payload = null) { }
}
export class loadTreeSuccessAction implements Action {
    public type: string = LOAD_TREE_SUCCESS;
    constructor(public payload: KMClassificationNodeViewModel[] = []) { }
}
export class loadTreeFailedAction implements Action {
    public type: string = LOAD_TREE_FAILED;
    constructor(public payload: string) { }
}
export class loadDetailAction implements Action {
    public type: string = LOAD_DETAIL;
    constructor(public payload: { ID?: number }) { }
}
export class loadDetailSuccessAction implements Action {
    public type: string = LOAD_DETAIL_SUCCESS;
    constructor(public payload: KMDetailViewModel) { }
}
export class loadDetailFailedAction implements Action {
    public type: string = LOAD_DETAIL_FAILED;
    constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
    public type: string = LOAD_ENTRY;
    constructor(public payload: KMDetailViewModel = new KMDetailViewModel()) { }
}


export class addAction implements Action {
    public type: string = ADD
    constructor(public payload: KMDetailViewModel) { }
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
    constructor(public payload: KMDetailViewModel) { }
}

export class editSuccessAction implements Action {
    public type: string = EDIT_SUCCESS
    constructor(public payload: string) { }
}

export class editFailedAction implements Action {
    public type: string = EDIT_FAILED
    constructor(public payload: string) { }
}


export class deleteAction implements Action {
    public type: string = DELETE
    constructor(public payload: EntrancePayload<{ ID?: number }>) { }
}

export class deleteSuccessAction implements Action {
    public type: string = DELETE_SUCCESS
    constructor(public payload: ResultPayload<string>) { }
}

export class deleteFailedAction implements Action {
    public type: string = DELETE_FAILED
    constructor(public payload: ResultPayload<string>) { }
}


export class deleteClassificationAction implements Action {
    public type: string = DELETE_CLASSIFICATION
    constructor(public payload: { ID?: number }) { }
}

export class deleteClassificationSuccessAction implements Action {
    public type: string = DELETE_CLASSIFICATION_SUCCESS
    constructor(public payload = null) { }
}

export class deleteClassificationFailedAction implements Action {
    public type: string = DELETE_CLASSIFICATION_FAILED
    constructor(public payload: string) { }
}


export class RenameClassificationAction implements Action {
    public type: string = RENAME_CLASSIFICATION
    constructor(public payload: { ID?: number, name: string }) { }
}

export class RenameClassificationSuccessAction implements Action {
    public type: string = RENAME_CLASSIFICATION_SUCCESS
    constructor(public payload = null) { }
}

export class RenameClassificationFailedAction implements Action {
    public type: string = RENAME_CLASSIFICATION_FAILED
    constructor(public payload: string) { }
}


export class AddClassificationAction implements Action {
    public type: string = ADD_CLASSIFICATION
    constructor(public payload: { parentID?: number, name: string }) { }
}

export class AddClassificationSuccessAction implements Action {
    public type: string = ADD_CLASSIFICATION_SUCCESS
    constructor(public payload = null) { }
}

export class AddClassificationFailedAction implements Action {
    public type: string = ADD_CLASSIFICATION_FAILED
    constructor(public payload: string) { }
}

export class AddRootClassificationAction implements Action {
    public type: string = ADD_ROOT_CLASSIFICATION
    constructor(public payload: { nodeID?: number, name: string }) { }
}

export class AddRootClassificationSuccessAction implements Action {
    public type: string = ADD_ROOT_CLASSIFICATION_SUCCESS
    constructor(public payload = null) { }
}

export class AddRootClassificationFailedAction implements Action {
    public type: string = ADD_ROOT_CLASSIFICATION_FAILED
    constructor(public payload: string) { }
}

export class DragClassificationAction implements Action {
    public type: string = DRAG_CLASSIFICATION
    constructor(public payload: { ID?: number, parentID: number }) { }
}

export class DragClassificationSuccessAction implements Action {
    public type: string = DRAG_CLASSIFICATION_SUCCESS
    constructor(public payload = null) { }
}

export class DragClassificationFailedAction implements Action {
    public type: string = DRAG_CLASSIFICATION_FAILED
    constructor(public payload: string) { }
}




export type Actions =
    loadTreeAction |
    loadTreeSuccessAction |
    loadTreeFailedAction |
    loadDetailAction |
    loadDetailSuccessAction |
    loadDetailFailedAction |
    loadEntryAction |
    deleteAction |
    deleteSuccessAction |
    deleteFailedAction |
    addAction |
    addSuccessAction |
    addFailedAction |
    editAction |
    editSuccessAction |
    editFailedAction |
    deleteClassificationAction |
    deleteClassificationSuccessAction |
    deleteClassificationFailedAction |
    RenameClassificationAction |
    RenameClassificationSuccessAction |
    RenameClassificationFailedAction |
    AddClassificationAction |
    AddClassificationSuccessAction |
    AddClassificationFailedAction |
    AddRootClassificationAction |
    AddRootClassificationSuccessAction |
    AddRootClassificationFailedAction;