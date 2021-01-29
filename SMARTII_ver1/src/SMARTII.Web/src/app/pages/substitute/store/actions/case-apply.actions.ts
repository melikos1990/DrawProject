import { Action } from '@ngrx/store';
import { CaseApplyCommitViewModel } from 'src/app/model/substitute.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const LOAD_ENTRY = '[CASE_APPLY] LOAD ENTRY';
export const APPLY = '[CASE_APPLY] APPLY';
export const APPLY_SUCCESS = '[CASE_APPLY] APPLY SUCCESS';
export const APPLY_FAILED = '[CASE_APPLY] APPLY FAILED';

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: CaseApplyCommitViewModel = new CaseApplyCommitViewModel()) { }
}

export class applyAction implements Action {
  public type: string = APPLY
  constructor(public payload: EntrancePayload<CaseApplyCommitViewModel>) { }
}

export class applySuccessAction implements Action {
  public type: string = APPLY_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class applyFailedAction implements Action {
  public type: string = APPLY_FAILED
  constructor(public payload: ResultPayload<string>) { }
}
export type Actions =
  loadEntryAction |
  applyAction |
  applySuccessAction |
  applyFailedAction;

