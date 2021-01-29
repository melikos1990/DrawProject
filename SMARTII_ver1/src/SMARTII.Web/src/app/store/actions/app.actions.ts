import { Action } from '@ngrx/store';

export const CHANGE_CULTURE = '[APP] CHANGE_CULTURE';
export const CONTEXT_MENU = '[APP] CONTXET_MENU';

export class changeCultureAction implements Action {
  type: string = CHANGE_CULTURE;
  constructor(public payload: string) { }
}

export class contextMenuAction implements Action {
  type: string = CONTEXT_MENU;
  constructor(public payload: {
    position: {
      x: number,
      y: number
    },
    title?: string,
    display: boolean,
    cbDist: { key: string, value: (obj?) => void }[]
  }) { }

}

export type Actions = changeCultureAction | contextMenuAction;
