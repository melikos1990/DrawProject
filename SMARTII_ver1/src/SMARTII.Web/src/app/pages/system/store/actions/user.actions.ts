import { Action } from '@ngrx/store';
import { UserDetailViewModel, IdentityWrapper, Identity, UserSearchViewModel } from 'src/app/model/authorize.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';


export const ADD = '[USER] ADD';
export const ADD_SUCCESS = '[USER] ADD SUCCESS';
export const ADD_FAILED = '[USER] ADD FAILED';
export const EDIT = '[USER] EDIT';
export const EDIT_SUCCESS = '[USER] EDIT SUCCESS';
export const EDIT_FAILED = '[USER] EDIT FAILED';
export const DISABLED = '[USER] DISABLED';
export const DISABLED_SUCCESS = '[USER] DISABLED SUCCESS';
export const DISABLED_FAILED = '[USER] DISABLED FAILED';
export const DISABLED_RANGE = '[USER] DISABLED RANGE';
export const DISABLED_RANGE_SUCCESS = '[USER] DISABLED RANGE SUCCESS';
export const DISABLED_RANGE_FAILED = '[USER] DISABLED RANGE FAILED';
export const LOAD_ENTRY = '[USER] LOAD ENTRY';
export const LOAD_DETAIL = '[USER] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[USER] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[USER] LOAD DETAIL FAILED';
export const REPORT = '[USER] REPORT';
export const REPORT_SUCCESS = '[USER] REPORT SUCCESS';
export const REPORT_FAILED = '[USER] REPORT FAILED';

export const VALID_AD_PASSWORD = '[USER] VALID AD PASSWORD';
export const VALID_AD_PASSWORD_SUCCESS = '[USER] VALID AD PASSWORD SUCCESS';
export const VALID_AD_PASSWORD_FAILED = '[USER] VALID AD PASSWORD FAILED';

export const RESET_PASSWORD = '[USER] RESET PASSWORD';
export const RESET_PASSWORD_SUCCESS = '[USER] RESET PASSWORD SUCCESS';
export const RESET_PASSWORD_FAILED = '[USER] RESET PASSWORD FAILED';

export const CHECK_NAME = '[USER] CHECK NAME';
export const CHECK_NAME_SUCCESS = '[USER] CHECK NAME SUCCESS';
export const CHECK_NAME_FAILED = '[USER] CHECK NAME FAILED';

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: {
    UserID: string
  }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: UserDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: UserDetailViewModel = new UserDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD;
  constructor(public payload: UserDetailViewModel) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS;
  constructor(public payload: string) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED;
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT;
  constructor(public payload: UserDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS;
  constructor(public payload: string) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED;
  constructor(public payload: string) { }
}

export class disableAction implements Action {
  public type: string = DISABLED;
  constructor(public payload: EntrancePayload<string>) { }
}
export class disableSuccessAction implements Action {
  public type: string = DISABLED_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableFailedAction implements Action {
  public type: string = DISABLED_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableRangeAction implements Action {
  public type: string = DISABLED_RANGE;
  constructor(public payload: EntrancePayload<string[]>) { }
}
export class disableRangeSuccessAction implements Action {
  public type: string = DISABLED_RANGE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableRangeFailedAction implements Action {
  public type: string = DISABLED_RANGE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class validADPasswordAction implements Action {
  public type: string = VALID_AD_PASSWORD;
  constructor(public payload: EntrancePayload<Identity>) { }
}
export class validADPasswordSuccessAction implements Action {
  public type: string = VALID_AD_PASSWORD_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}
export class validADPasswordFailedAction implements Action {
  public type: string = VALID_AD_PASSWORD_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class resetPasswordAction implements Action {
  public type: string = RESET_PASSWORD;
  constructor(public payload: EntrancePayload<string>) { }
}
export class resetPasswordSuccessAction implements Action {
  public type: string = RESET_PASSWORD_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}
export class resetPasswordFailedAction implements Action {
  public type: string = RESET_PASSWORD_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class checkNameAction implements Action {
  public type: string = CHECK_NAME
  constructor(public payload: EntrancePayload<{ ID: string, name: string }>) { }
}

export class checkNameSuccessAction implements Action {
  public type: string = CHECK_NAME_SUCCESS
  constructor(public payload: ResultPayload<{ isExist: boolean, message: string }>) { }
}

export class checkNameFailedAction implements Action {
  public type: string = CHECK_NAME_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class report implements Action {
  public type: string = REPORT;
  constructor(public payload: UserSearchViewModel) { }
}

export class reportSuccess implements Action {
  public type: string = REPORT_SUCCESS;
  constructor(public payload: Blob) { }
}

export class reportFailed implements Action {
  public type: string = REPORT_FAILED;
  constructor(public payload: string) { }
}



export type Actions =
  loadEntryAction |
  loadDetailAction |
  loadDetailSuccessAction |
  loadDetailFailedAction |
  addAction |
  addSuccessAction |
  addFailedAction |
  editAction |
  editSuccessAction |
  editFailedAction |
  disableAction |
  disableFailedAction |
  disableSuccessAction |
  disableRangeAction |
  disableRangeFailedAction |
  disableRangeSuccessAction |
  validADPasswordAction |
  validADPasswordSuccessAction |
  validADPasswordFailedAction |
  resetPasswordAction |
  resetPasswordSuccessAction |
  resetPasswordFailedAction |
  checkNameAction |
  checkNameSuccessAction |
  checkNameFailedAction |
  report |
  reportSuccess |
  reportFailed;
