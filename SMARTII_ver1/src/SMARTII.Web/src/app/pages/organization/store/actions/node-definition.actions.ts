import { Action } from '@ngrx/store';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { OrganizationType, NodeDefinitionDetailViewModel, JobDetailViewModel } from '../../../../model/organization.model';


export const ADD = '[NODE DEFINITION] ADD';
export const ADD_SUCCESS = '[NODE DEFINITION] ADD SUCCESS';
export const ADD_FAILED = '[NODE DEFINITION] ADD FAILED';
export const EDIT = '[NODE DEFINITION] EDIT';
export const EDIT_SUCCESS = '[NODE DEFINITION] EDIT SUCCESS';
export const EDIT_FAILED = '[NODE DEFINITION] EDIT FAILED';
export const DISABLE = '[NODE DEFINITION] DISABLE';
export const DISABLE_SUCCESS = '[NODE DEFINITION] DISABLE SUCCESS';
export const DISABLE_FAILED = '[NODE DEFINITION] DISABLE FAILED';
export const LOAD_ENTRY = '[NODE DEFINITION] LOAD ENTRY';
export const LOAD_DETAIL = '[NODE DEFINITION] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[NODE DEFINITION] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[NODE DEFINITION] LOAD DETAIL FAILED';

export const ADD_JOB = '[NODE DEFINITION] ADD JOB';
export const ADD_JOB_SUCCESS = '[NODE DEFINITION] ADD JOB SUCCESS';
export const ADD_JOB_FAILED = '[NODE DEFINITION] ADD JOB FAILED';
export const EDIT_JOB = '[NODE DEFINITION] EDIT JOB';
export const EDIT_JOB_SUCCESS = '[NODE DEFINITION] EDIT JOB SUCCESS';
export const EDIT_JOB_FAILED = '[NODE DEFINITION] EDIT JOB FAILED';
export const DISABLE_JOB = '[NODE DEFINITION] DISABLE JOB';
export const DISABLE_JOB_SUCCESS = '[NODE DEFINITION] DISABLE JOB SUCCESS';
export const DISABLE_JOB_FAILED = '[NODE DEFINITION] DISABLE JOB FAILED';


export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: {
    OrganizationType: OrganizationType,
    ID: number
  }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: NodeDefinitionDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: NodeDefinitionDetailViewModel = new NodeDefinitionDetailViewModel()) { }
}

export class addAction implements Action {
  public type: string = ADD;
  constructor(public payload: NodeDefinitionDetailViewModel) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS;
  constructor(public payload: NodeDefinitionDetailViewModel) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED;
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT;
  constructor(public payload: NodeDefinitionDetailViewModel) { }
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
  public type: string = DISABLE;
  constructor(public payload: EntrancePayload<{
    organizationType: OrganizationType,
    ID: number
  }>) { }
}
export class disableSuccessAction implements Action {
  public type: string = DISABLE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableFailedAction implements Action {
  public type: string = DISABLE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class addJobAction implements Action {
  public type: string = ADD_JOB;
  constructor(public payload: EntrancePayload<JobDetailViewModel>) { }
}

export class addJobSuccessAction implements Action {
  public type: string = ADD_JOB_SUCCESS;
  constructor(public payload: ResultPayload<JobDetailViewModel>) { }
}

export class addJobFailedAction implements Action {
  public type: string = ADD_JOB_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class editJobAction implements Action {
  public type: string = EDIT_JOB;
  constructor(public payload: EntrancePayload<JobDetailViewModel>) { }
}

export class editJobSuccessAction implements Action {
  public type: string = EDIT_JOB_SUCCESS;
  constructor(public payload: ResultPayload<JobDetailViewModel>) { }
}

export class editJobFailedAction implements Action {
  public type: string = EDIT_JOB_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableJobAction implements Action {
  public type: string = DISABLE_JOB;
  constructor(public payload: EntrancePayload<number>) { }
}
export class disableJobSuccessAction implements Action {
  public type: string = DISABLE_JOB_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class disableJobFailedAction implements Action {
  public type: string = DISABLE_JOB_FAILED;
  constructor(public payload: ResultPayload<string>) { }
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
  addJobAction |
  addJobSuccessAction |
  addJobFailedAction |
  editJobAction |
  editJobSuccessAction |
  editJobFailedAction |
  disableJobAction |
  disableJobFailedAction |
  disableJobSuccessAction;
