import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const NOTICE = '[CASE_NOTICE] NOTICE';
export const NOTICE_SUCCESS = '[CASE_NOTICE] NOTICE SUCCESS';
export const NOTICE_FAILED = '[CASE_NOTICE] NOTICE FAILED';

export const NOTICE_RANGE = '[CASE_NOTICE] NOTICE RANGE';
export const NOTICE_RANGE_SUCCESS = '[CASE_NOTICE] NOTICE RANGE SUCCESS';
export const NOTICE_RANGE_FAILED = '[CASE_NOTICE] NOTICE RANGE FAILED';

export class noticeAction implements Action {
  public type: string = NOTICE
  constructor(public payload: EntrancePayload<{ caseID: string, ID: number }>) { }
}

export class noticeSuccessAction implements Action {
  public type: string = NOTICE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class noticeFailedAction implements Action {
  public type: string = NOTICE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}

export class noticeRangeAction implements Action {
  public type: string = NOTICE_RANGE
  constructor(public payload: EntrancePayload<number[]>) { }
}

export class noticeRangeSuccessAction implements Action {
  public type: string = NOTICE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class noticeRangeFailedAction implements Action {
  public type: string = NOTICE_RANGE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}
export type Actions =
  noticeAction |
  noticeSuccessAction |
  noticeFailedAction |
  noticeRangeAction |
  noticeRangeSuccessAction |
  noticeRangeFailedAction;

