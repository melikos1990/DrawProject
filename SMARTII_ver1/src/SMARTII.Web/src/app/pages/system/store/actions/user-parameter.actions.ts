import { Action } from '@ngrx/store';
import { UserParameterlViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const GET_USER_PARAMETER_DETAIL = '[USER_PARAMETER] GET_USER_PARAMETER_DETAIL';
export const GET_USER_PARAMETER_DETAIL_SUCCESS = '[USER_PARAMETER] GET_USER_PARAMETER_DETAIL_SUCCESS';
export const UPDATE_USER_PARAMETER = '[USER_PARAMETER] UPDATE_USER_PARAMETER';
export const SUCCESS = '[USER_PARAMETER] SUCCESS';
export const FAILED = '[USER_PARAMETER] FAILED';
export const GET_DETAIL_SUCCESS = '[USER_PARAMETER] GET_DETAIL_SUCCESS';



export class getUserParameterDetailAction implements Action {
  public type: string = GET_USER_PARAMETER_DETAIL;
  constructor(public payload: EntrancePayload<{userID: string}>){}
}

export class getUserParameterDetailSuccessAction implements Action {
  public type: string = GET_USER_PARAMETER_DETAIL_SUCCESS;
  constructor(public payload: string) { }
}

export class updateUserParameterAction implements Action {
  public type: string = UPDATE_USER_PARAMETER;
  constructor(public payload: EntrancePayload<UserParameterlViewModel>) { }
}

export class SuccessAction implements Action {
  type: string = SUCCESS;
  constructor(public payload: ResultPayload<string>){}
}

export class FailedAction implements Action {
  type: string = FAILED;
  constructor(public payload: string){}
}

export class GetDetailSuccessAction implements Action {
  type: string = GET_DETAIL_SUCCESS;
  constructor(public payload: UserParameterlViewModel){}
}

export type Actions =
  getUserParameterDetailAction |
  getUserParameterDetailSuccessAction |
  updateUserParameterAction |
  SuccessAction | 
  FailedAction |
  GetDetailSuccessAction;
