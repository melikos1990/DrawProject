import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { HeaderQuarterNodeViewModel, HeaderQuarterNodeDetailViewModel, AddJobViewModel, AddUserViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';


export const LOAD = '[HEADERQUARTER NODE] LOAD';
export const LOAD_SUCCESS = '[HEADERQUARTER NODE] LOAD SUCCESS';
export const LOAD_FAILED = '[HEADERQUARTER NODE] LOAD FAILED';
export const LOAD_ENTRY = '[HEADERQUARTER NODE] LOAD ENTRY';
export const LOAD_DETAIL = '[HEADERQUARTER NODE] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[HEADERQUARTER NODE] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[HEADERQUARTER NODE] LOAD DETAIL FAILED';
export const ADD = '[HEADERQUARTER NODE] ADD';
export const ADD_SUCCESS = '[HEADERQUARTER NODE] ADD SUCCESS';
export const ADD_FAILED = '[HEADERQUARTER NODE] ADD FAILED';
export const EDIT = '[HEADERQUARTER NODE] EDIT';
export const EDIT_SUCCESS = '[HEADERQUARTER NODE] EDIT SUCCESS';
export const EDIT_FAILED = '[HEADERQUARTER NODE] EDIT FAILED';
export const DISABLE = '[HEADERQUARTER NODE] DISABLE ';
export const DISABLE_SUCCESS = '[HEADERQUARTER NODE] SUCCESS ';
export const DISABLE_FAILED = '[HEADERQUARTER NODE] FAILED ';
export const DELETE_USER = '[HEADERQUARTER NODE] DELETE USER';
export const DELETE_USER_SUCCESS = '[HEADERQUARTER NODE] DELETE USER SUCCESS';
export const DELETE_USER_FAILED = '[HEADERQUARTER NODE] DELETE USER FAILED';
export const ADD_USER = '[HEADERQUARTER NODE] ADD USER';
export const ADD_USER_SUCCESS = '[HEADERQUARTER NODE] ADD USER SUCCESS';
export const ADD_USER_FAILED = '[HEADERQUARTER NODE] ADD USER FAILED';
export const DELETE_JOB = '[HEADERQUARTER NODE] DELETE JOB';
export const DELETE_JOB_SUCCESS = '[HEADERQUARTER NODE] DELETE JOB SUCCESS';
export const DELETE_JOB_FAILED = '[HEADERQUARTER NODE] DELETE JOB FAILED';
export const ADD_JOB = '[HEADERQUARTER NODE] ADD JOB';
export const ADD_JOB_SUCCESS = '[HEADERQUARTER NODE] ADD JOB SUCCESS';
export const ADD_JOB_FAILED = '[HEADERQUARTER NODE] ADD JOB FAILED';
export const UPDATE_TREE = '[HEADERQUARTER NODE] UPDATE TREE';
export const UPDATE_TREE_SUCCESS = '[HEADERQUARTER NODE] UPDATE TREE SUCCESS';
export const UPDATE_TREE_FAILED = '[HEADERQUARTER NODE] UPDATE TREE FAILED';
export const SELECT_JOB = '[HEADERQUARTER NODE] SELECT JOB';

export class loadAction implements Action {
  public type: string = LOAD;
  constructor(public payload: EntrancePayload<any>) { }
}

export class loadSuccessAction implements Action {
  public type: string = LOAD_SUCCESS;
  constructor(public payload: ResultPayload<HeaderQuarterNodeViewModel>) { }
}

export class loadFailedAction implements Action {
  public type: string = LOAD_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor() { }
}

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: EntrancePayload<number>) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: ResultPayload<HeaderQuarterNodeDetailViewModel>) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class addAction implements Action {
  public type: string = ADD;
  constructor(public payload: EntrancePayload<HeaderQuarterNodeViewModel>) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class updateTreeAction implements Action {
  public type: string = UPDATE_TREE;
  constructor(public payload: EntrancePayload<HeaderQuarterNodeViewModel>) { }
}

export class updateTreeFailedAction implements Action {
  public type: string = UPDATE_TREE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class updateTreeSuccessAction implements Action {
  public type: string = UPDATE_TREE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}


export class editAction implements Action {
  public type: string = EDIT;
  constructor(public payload: EntrancePayload<HeaderQuarterNodeDetailViewModel>) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS;
  constructor(public payload: ResultPayload<HeaderQuarterNodeDetailViewModel>) { }
}

export class disabledAction implements Action {
  public type: string = DISABLE;
  constructor(public payload: EntrancePayload<number>) { }
}

export class disabledFailedAction implements Action {
  public type: string = DISABLE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class disabledSuccessAction implements Action {
  public type: string = DISABLE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}


export class addJobAction implements Action {
  public type: string = ADD_JOB;
  constructor(public payload: EntrancePayload<AddJobViewModel>) { }
}

export class addJobFailedAction implements Action {
  public type: string = ADD_JOB_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class addJobSuccessAction implements Action {
  public type: string = ADD_JOB_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteJobAction implements Action {
  public type: string = DELETE_JOB;
  constructor(public payload: EntrancePayload<{ nodeJobID: number }>) { }
}

export class deleteJobFailedAction implements Action {
  public type: string = DELETE_JOB_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteJobSuccessAction implements Action {
  public type: string = DELETE_JOB_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class addUserAction implements Action {
  public type: string = ADD_USER;
  constructor(public payload: EntrancePayload<AddUserViewModel>) { }
}


export class addUserSuccessAction implements Action {
  public type: string = ADD_USER_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class addUserFailedAction implements Action {
  public type: string = ADD_USER_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteUserAction implements Action {
  public type: string = DELETE_USER;
  constructor(public payload: EntrancePayload<{
    nodeJobID: number,
    userID: string,
  }>) { }
}

export class deleteUserSuccessAction implements Action {
  public type: string = DELETE_USER_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class deleteUserFailedAction implements Action {
  public type: string = DELETE_USER_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class selectJobAction implements Action {
  public type: string = SELECT_JOB;
  constructor(public payload: NodeJobListViewModel) { }
}


export type Actions =
  loadAction |
  loadSuccessAction |
  loadFailedAction |
  loadEntryAction |
  addAction |
  addFailedAction |
  addSuccessAction |
  updateTreeAction |
  updateTreeFailedAction |
  updateTreeSuccessAction |
  disabledAction |
  disabledSuccessAction |
  disabledFailedAction |
  loadDetailAction |
  loadDetailSuccessAction |
  loadDetailFailedAction |
  addJobAction |
  addJobSuccessAction |
  addJobFailedAction |
  deleteJobAction |
  deleteJobSuccessAction |
  deleteJobFailedAction |
  addUserAction |
  addUserSuccessAction |
  addUserFailedAction |
  deleteUserAction |
  deleteUserSuccessAction |
  deleteUserFailedAction |
  selectJobAction;
