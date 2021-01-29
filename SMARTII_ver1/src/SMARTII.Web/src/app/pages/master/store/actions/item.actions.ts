import { Action } from '@ngrx/store';
import { ItemDetailViewModel, ItemListViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';

export const GET_ITEM_DETAIL_TEMPLATE = '[ITEM] GET ITEM DETAIL TEMPLATE';
export const GET_ITEM_DETAIL_TEMPLATE_SUCCESS = '[ITEM] GET ITEM DETAIL TEMPLATE SUCCESS';
export const GET_ITEM_DETAIL_TEMPLATE_FAIL = '[ITEM] GET ITEM DETAIL TEMPLATE FAILED';
export const GET_ITEM_LIST_TEMPLATE = '[ITEM] GET ITEM LIST TEMPLATE';
export const GET_ITEM_LIST_TEMPLATE_SUCCESS = '[ITEM] GET ITEM LIST TEMPLATE SUCCESS';
export const GET_ITEM_LIST_TEMPLATE_FAIL = '[ITEM] GET ITEM LIST TEMPLATE FAILED';
export const ADD = '[ITEM] ADD';
export const ADD_SUCCESS = '[ITEM] ADD SUCCESS';
export const ADD_FAILED = '[ITEM] ADD FAILED';
export const EDIT = '[ITEM] EDIT';
export const EDIT_SUCCESS = '[ITEM] EDIT SUCCESS';
export const EDIT_FAILED = '[ITEM] EDIT FAILED';
export const DISABLE = '[ITEM] DISABLE';
export const DISABLE_SUCCESS = '[ITEM] DISABLE SUCCESS';
export const DISABLE_FAILED = '[ITEM] DISABLE FAILED';
export const DISABLE_RANGE = '[ITEM] DISABLE RANGE';
export const DISABLE_RANGE_SUCCESS = '[ITEM] DISABLE RANGE SUCCESS';
export const DISABLE_RANGE_FAILED = '[ITEM] DISABLE RANGE FAILED';
export const LOAD_ENTRY = '[ITEM] LOAD ENTRY';
export const LOAD_DETAIL = '[ITEM] LOAD DETAIL';
export const LOAD_DETAIL_SUCCESS = '[ITEM] LOAD DETAIL SUCCESS';
export const LOAD_DETAIL_FAILED = '[ITEM] LOAD DETAIL FAILED';
export const UPLOAD = '[ITEM] UPLOAD';
export const UPLOAD_SUCCESS = '[ITEM] UPLOAD SUCCESS';
export const UPLOAD_FAILED = '[ITEM] UPLOAD FAILED';



export class getItemDetailTemplateAction implements Action {
  public type: string = GET_ITEM_DETAIL_TEMPLATE;
  constructor(public payload: { nodeKey: string }) { }
}

export class getItemDetailTemplateSuccessAction implements Action {
  public type: string = GET_ITEM_DETAIL_TEMPLATE_SUCCESS;
  constructor(public payload: string) { }
}

export class getItemDetailTemplateFailedAction implements Action {
  public type: string = GET_ITEM_DETAIL_TEMPLATE_FAIL;
  constructor(public payload: string) { }
}

export class getItemListTemplateAction implements Action {
  public type: string = GET_ITEM_LIST_TEMPLATE;
  constructor(public payload: { nodeKey: string }) { }
}
export class getItemListTemplateSuccessAction implements Action {
  public type: string = GET_ITEM_LIST_TEMPLATE_SUCCESS;
  constructor(public payload: string) { }
}

export class getItemListTemplateFailedAction implements Action {
  public type: string = GET_ITEM_LIST_TEMPLATE_FAIL;
  constructor(public payload: string) { }
}

export class loadEntryAction implements Action {
  public type: string = LOAD_ENTRY;
  constructor(public payload: ItemDetailViewModel = new ItemDetailViewModel()) { }
}

export class loadDetailAction implements Action {
  public type: string = LOAD_DETAIL;
  constructor(public payload: { ID?: number }) { }
}

export class loadDetailSuccessAction implements Action {
  public type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: ItemDetailViewModel) { }
}

export class loadDetailFailedAction implements Action {
  public type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string) { }
}

export class addAction implements Action {
  public type: string = ADD
  constructor(public payload: ItemDetailViewModel) { }
}

export class addSuccessAction implements Action {
  public type: string = ADD_SUCCESS
  constructor(public payload: string) { }
}

export class addFailedAction implements Action {
  public type: string = ADD_FAILED
  constructor(public payload: string) { }
}

export class editAction implements Action {
  public type: string = EDIT
  constructor(public payload: ItemDetailViewModel) { }
}

export class editSuccessAction implements Action {
  public type: string = EDIT_SUCCESS
  constructor(public payload: string) { }
}

export class editFailedAction implements Action {
  public type: string = EDIT_FAILED
  constructor(public payload: string) { }
}


export class disableAction implements Action {
  public type: string = DISABLE
  constructor(public payload: EntrancePayload<{ ID?: number }>) { }
}

export class disableSuccessAction implements Action {
  public type: string = DISABLE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class disableFailedAction implements Action {
  public type: string = DISABLE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}


export class disableRangeAction implements Action {
  public type: string = DISABLE_RANGE;
  constructor(public payload: EntrancePayload<Array<ItemListViewModel>>) { }
}

export class disableRangeSuccessAction implements Action {
  public type: string = DISABLE_RANGE_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class disableRangeFailedAction implements Action {
  public type: string = DISABLE_RANGE_FAILED
  constructor(public payload: ResultPayload<string>) { }
}


export class uploadAction implements Action {
  public type: string = UPLOAD;
  constructor(public payload: EntrancePayload<FormData>) { }
}

export class uploadSuccessAction implements Action {
  public type: string = UPLOAD_SUCCESS
  constructor(public payload: ResultPayload<string>) { }
}

export class uploadFailedAction implements Action {
  public type: string = UPLOAD_FAILED
  constructor(public payload: ResultPayload<any>) { }
}



export type Actions =
  getItemDetailTemplateAction |
  getItemDetailTemplateSuccessAction |
  getItemDetailTemplateFailedAction |
  getItemListTemplateAction |
  getItemListTemplateSuccessAction |
  getItemListTemplateFailedAction |
  loadDetailAction |
  loadDetailSuccessAction |
  loadDetailFailedAction |
  addAction |
  addSuccessAction |
  addFailedAction |
  loadEntryAction |
  disableAction |
  disableFailedAction |
  disableSuccessAction |
  disableRangeAction |
  disableRangeFailedAction |
  disableRangeSuccessAction |
  uploadAction |
  uploadFailedAction |
  uploadSuccessAction;
