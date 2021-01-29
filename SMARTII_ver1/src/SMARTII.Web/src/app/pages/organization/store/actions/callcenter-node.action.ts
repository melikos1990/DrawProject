import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { CallCenterNodeViewModel, CallCenterNodeDetailViewModel, AddJobViewModel, AddUserViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';


export const LOAD = '[CALLCENTER NODE] LOAD';
export const LOAD_SUCCESS = '[CALLCENTER NODE] LOAD SUCCESS';
export const LOAD_FAILED = '[CALLCENTER NODE] LOAD FAILED';
export const LOAD_ENTRY = '[CALLCENTER NODE] LOAD ENTRY';
export const LOAD_DETAIL = '[CALLCENTER NODE] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[CALLCENTER NODE] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[CALLCENTER NODE] LOAD DETAIL FAILED';
export const ADD = '[CALLCENTER NODE] ADD';
export const ADD_SUCCESS = '[CALLCENTER NODE] ADD SUCCESS';
export const ADD_FAILED = '[CALLCENTER NODE] ADD FAILED';
export const EDIT = '[CALLCENTER NODE] EDIT';
export const EDIT_SUCCESS = '[CALLCENTER NODE] EDIT SUCCESS';
export const EDIT_FAILED = '[CALLCENTER NODE] EDIT FAILED';
export const DISABLE = '[CALLCENTER NODE] DISABLE ';
export const DISABLE_SUCCESS = '[CALLCENTER NODE] SUCCESS ';
export const DISABLE_FAILED = '[CALLCENTER NODE] FAILED ';
export const DELETE_USER = '[CALLCENTER NODE] DELETE USER';
export const DELETE_USER_SUCCESS = '[CALLCENTER NODE] DELETE USER SUCCESS';
export const DELETE_USER_FAILED = '[CALLCENTER NODE] DELETE USER FAILED';
export const ADD_USER = '[CALLCENTER NODE] ADD USER';
export const ADD_USER_SUCCESS = '[CALLCENTER NODE] ADD USER SUCCESS';
export const ADD_USER_FAILED = '[CALLCENTER NODE] ADD USER FAILED';
export const DELETE_JOB = '[CALLCENTER NODE] DELETE JOB';
export const DELETE_JOB_SUCCESS = '[CALLCENTER NODE] DELETE JOB SUCCESS';
export const DELETE_JOB_FAILED = '[CALLCENTER NODE] DELETE JOB FAILED';
export const ADD_JOB = '[CALLCENTER NODE] ADD JOB';
export const ADD_JOB_SUCCESS = '[CALLCENTER NODE] ADD JOB SUCCESS';
export const ADD_JOB_FAILED = '[CALLCENTER NODE] ADD JOB FAILED';
export const UPDATE_TREE = '[CALLCENTER NODE] UPDATE TREE';
export const UPDATE_TREE_SUCCESS = '[CALLCENTER NODE] UPDATE TREE SUCCESS';
export const UPDATE_TREE_FAILED = '[CALLCENTER NODE] UPDATE TREE FAILED';
export const SELECT_JOB = '[CALLCENTER NODE] SELECT JOB';

export class loadAction implements Action {
  public type: string = LOAD;
  constructor(public payload: EntrancePayload<any>) { }
}

export class loadSuccessAction implements Action {
  public type: string = LOAD_SUCCESS;
  constructor(public payload: ResultPayload<CallCenterNodeViewModel>) { }
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
  constructor(public payload: ResultPayload<CallCenterNodeDetailViewModel>) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class addAction implements Action {
  public type: string = ADD;
  constructor(public payload: EntrancePayload<CallCenterNodeViewModel>) { }
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
  constructor(public payload: EntrancePayload<CallCenterNodeViewModel>) { }
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
  constructor(public payload: EntrancePayload<CallCenterNodeDetailViewModel>) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS;
  constructor(public payload: ResultPayload<CallCenterNodeDetailViewModel>) { }
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

