import { Action } from '@ngrx/store'
import { User, Identity, IdentityWrapper, ChangePasswordViewModel, JobPosition, resultBox } from 'src/app/model/authorize.model';
import { Menu } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const LOGIN = '[AUTH] LOGIN';
export const LOGIN_SUCCESS = '[AUTH] LOGIN SUCCESS';
export const LOGIN_FAILED = '[AUTH] LOGIN FAILED';
export const LOGOFF = '[AUTH] LOGOFF';
export const LOGOFF_SUCCESS = '[AUTH] LOGOFF SUCCESS';
export const LOGOFF_FAILED = '[AUTH] LOGOFF FAILED';
export const AUTH_DENY = '[AUTH] AUTH DENY';
export const CLEAR_AUTH = '[AUTH] AUTH CLEAR';
export const TOKEN_MEMBER = '[AUTH] TOKEN MEMBER';
export const CACHE_MENU = '[AUTH] CACHE MENU';
export const CACHE_JOB_POSITION = '[AUTH] CACHE JOB POSITION';
export const PARSE_AUTH = '[AUTH] PARSE AUTH';
export const CHANGE_PASSWORD_DISPLAY = '[AUTH] CHANGE_PASSWORD_DISPLAY';
export const ACTIVATE_EXIST_IDENTITY = '[AUTH] ACTIVATE_EXIST_IDENTITY';
export const RESET_PASSWORD = '[AUTH] RESET_PASSWORD';
export const RESET_PASSWORD_SUCCESS = '[AUTH] RESET_PASSWORD_SUCCESS';
export const RESET_PASSWORD_FAILED = '[AUTH] RESET_PASSWORD_FAILED';


export class loginAction implements Action {
  type: string = LOGIN
  constructor(public payload: Identity) { }
}

export class loginFailedAction implements Action {
  type: string = LOGIN_FAILED
  constructor(public payload: {
    mode?: string,
    message: string
  }) { }
}

export class loginSuccessAction implements Action {
  type: string = LOGIN_SUCCESS
  constructor(public payload: IdentityWrapper) { }

}

export class logOffAction implements Action {
  type: string = LOGOFF
  constructor(public payload: any = null) { }
}

export class logoffSuccessAction implements Action {
  type: string = LOGOFF_SUCCESS
  constructor(public payload: any = null) { }
}

export class logoffFailedAction implements Action {
  type: string = LOGOFF_FAILED
  constructor(public payload: any = null) { }
}

export class authDenyAction implements Action {
  type: string = AUTH_DENY
  constructor(public payload: string = null) { }
}


export class clearAuthAction implements Action {
  type: string = CLEAR_AUTH
  constructor(public payload: any = null) { }
}

export class parseAuthAction implements Action {
  type: string = PARSE_AUTH
  constructor(public payload: any = null) { }
}

export class tokenMemberAction implements Action {
  type: string = TOKEN_MEMBER;
  constructor(public payload: User = null) { }
}

export class CacheMenuAction implements Action {
  type: string = CACHE_MENU;
  constructor(public payload: Menu[] = []) { }
}

export class CacheJobPositionAction implements Action {
  type: string = CACHE_JOB_POSITION;
  constructor(public payload: JobPosition[] = []) { }
}


export class activateExistIdentity implements Action {
  type: string = ACTIVATE_EXIST_IDENTITY
  constructor(public payload: any = null) { }
}

export class changePasswordDisplay implements Action {
  type: string = CHANGE_PASSWORD_DISPLAY;
  constructor(public payload: resultBox = null) { }
}

export class resetPasswordAction implements Action {
  type: string = RESET_PASSWORD;
  constructor(public payload: EntrancePayload<ChangePasswordViewModel>) { }
}

export class resetPasswordSuccessAction implements Action {
  type: string = RESET_PASSWORD_SUCCESS;
  constructor(public payload: ResultPayload<{ message: string }>) { }
}

export class resetPasswordFailedAction implements Action {
  type: string = RESET_PASSWORD_FAILED;
  constructor(public payload: ResultPayload<{ message: string }>) { }
}

export type Actions =
  authDenyAction |
  loginAction |
  loginSuccessAction |
  loginFailedAction |
  logOffAction |
  logoffFailedAction |
  logoffSuccessAction |
  tokenMemberAction |
  CacheMenuAction |
  CacheJobPositionAction |
  clearAuthAction |
  parseAuthAction |
  activateExistIdentity |
  changePasswordDisplay |
  resetPasswordAction |
  resetPasswordSuccessAction |
  resetPasswordFailedAction
  ;
