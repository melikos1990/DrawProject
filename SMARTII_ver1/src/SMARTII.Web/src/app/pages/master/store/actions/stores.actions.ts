import { Action } from '@ngrx/store';
import { StoresDetailViewModel, StoresListViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { OrganizationType } from 'src/app/model/organization.model';

export const GET_STORES_LIST_TEMPLATE = '[STORES] GET STORES LIST TEMPLATE';
export const GET_STORES_LIST_TEMPLATE_SUCCESS = '[STORES] GET STORES LIST TEMPLATE SUCCESS';
export const GET_STORES_LIST_TEMPLATE_FAIL = '[STORES] GET STORES LIST TEMPLATE FAILED';
export const EDIT = '[STORES] EDIT';
export const EDIT_SUCCESS = '[STORES] EDIT SUCCESS';
export const EDIT_FAILED = '[STORES] EDIT FAILED';
export const LOAD_ENTRY = '[STORES] LOAD ENTRY';
export const LOAD_DETAIL = '[STORES] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[STORES] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[STORES] LOAD DETAIL FAILED';



export class getStoresListTemplateAction implements Action {
  public type: string = GET_STORES_LIST_TEMPLATE;
  constructor(public payload: { nodeKey: string }) { }
}
export class getStoresListTemplateSuccessAction implements Action {
  public type: string = GET_STORES_LIST_TEMPLATE_SUCCESS;
  constructor(public payload: string) { }
}

export class getStoresListTemplateFailedAction implements Action {
  public type: string = GET_STORES_LIST_TEMPLATE_FAIL;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: StoresDetailViewModel = new StoresDetailViewModel()) { }
}

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number, OrganizationType?: OrganizationType }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: StoresDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT
  constructor(public payload: StoresDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS
  constructor(public payload: string) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED
  constructor(public payload: string) { }
}


export type Actions =
  getStoresListTemplateAction |
  getStoresListTemplateSuccessAction |
  getStoresListTemplateFailedAction |
  loadDetailAction |
  loadDetailSuccessAction |
  loadDetailFailedAction |
  loadEntryAction;
