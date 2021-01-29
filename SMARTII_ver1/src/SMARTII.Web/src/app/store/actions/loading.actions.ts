import { Action } from '@ngrx/store'

export const VISIBLE_LOADING = "[LOADING] OPEN";
export const INVISIBLE_LOADING = "[LOADING] CLOSE";



export class visibleLoadingAction implements Action{
    type: string = VISIBLE_LOADING
    constructor(){ }
}

export class invisibleLoadingAction implements Action{
    type: string = INVISIBLE_LOADING
    constructor(){ }
}

export type Actions = visibleLoadingAction | invisibleLoadingAction;