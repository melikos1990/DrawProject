import { Action } from '@ngrx/store';
import { StoresDetailViewModel, StoresListViewModel, WorkScheduleDetailViewModel } from 'src/app/model/master.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { OrganizationType } from 'src/app/model/organization.model';

export const GET_WORK_SCHEDULE_DETAIL = '[STORES] GET_WORK_SCHEDULE_DETAIL';
export const LOAD_DETAIL_ENTRY = '[STORES] LOAD_DETAIL_ENTRY';
export const LOAD_DETAIL = '[STORES] LOAD_DETAIL';
export const LOAD_DETAIL_SUCCESS = '[STORES] LOAD_DETAIL_SUCCESS';
export const LOAD_DETAIL_FAILED = '[STORES] LOAD_DETAIL_FAILED';
export const ADD_WORK_SCHEDULE = '[STORES] ADD_WORK_SCHEDULE';
export const ADD_WORK_SCHEDULE_SUCCESS = '[STORES] ADD_WORK_SCHEDULE_SUCCESS';
export const ADD_WORK_SCHEDULE_FAILED = '[STORES] ADD_WORK_SCHEDULE_FAILED';
export const DELETE_DETAIL = '[STORES] DELETE_DETAIL';
export const DELETE_DETAIL_RANGE = '[STORES] DELETE_DETAIL_RANGE';
export const SUCCESS = '[STORES] SUCCESS';
export const FAILED = '[STORES] FAILED';
export const FAILED_SHOWDETAIL = '[STORES] FAILED_SHOWDETAIL';
export const EDIT_WORK_SCHEDULE = '[STORES] EDIT_WORK_SCHEDULE';
export const EDIT_WORK_SCHEDULE_SUCCESS = '[STORES] EDIT_WORK_SCHEDULE_SUCCESS';

export class addWorkSchedule implements Action {
  type: string = ADD_WORK_SCHEDULE;
  constructor(public payload: EntrancePayload<WorkScheduleDetailViewModel[]>){}
}

export class addWorkScheduleSuccess implements Action {
  type: string = ADD_WORK_SCHEDULE_SUCCESS;
  constructor(public payload: string){}
}


export class addWorkScheduleFailed implements Action {
  type: string = ADD_WORK_SCHEDULE_FAILED;
  constructor(public payload: ResultPayload<any>){}
}

export class loadDetail implements Action {
  type: string = LOAD_DETAIL;
  constructor(public payload: { id: any }){}
}


export class loadDetailSuccess implements Action {
  type: string = LOAD_DETAIL_SUCCESS;
  constructor(public payload: WorkScheduleDetailViewModel){}
}


export class loadDetailFailed implements Action {
  type: string = LOAD_DETAIL_FAILED;
  constructor(public payload: string){}
}


export class loadEntry implements Action {
  type: string = LOAD_DETAIL_ENTRY;
  constructor(public payload: any){}
}

  
export class deleteDetail implements Action {
  type: string = DELETE_DETAIL;
  constructor(public payload: EntrancePayload<WorkScheduleDetailViewModel>){}
}


export class deleteRangeDetail implements Action {
  type: string = DELETE_DETAIL_RANGE;
  constructor(public payload: EntrancePayload<WorkScheduleDetailViewModel[]>){}
}

export class failed implements Action {
  type: string = FAILED;
  constructor(public payload: string){}
}


export class failedShowDetail implements Action {
  type: string = FAILED_SHOWDETAIL;
  constructor(public payload: ResultPayload<any>){}
}


export class success implements Action {
  type: string = SUCCESS;
  constructor(public payload: ResultPayload<any>){}
}

export class editWorkSchedule implements Action {
  type: string = EDIT_WORK_SCHEDULE;
  constructor(public payload: WorkScheduleDetailViewModel){}
}

export class editWorkScheduleSuccess implements Action {
  type: string = EDIT_WORK_SCHEDULE_SUCCESS;
  constructor(public payload: string){}
}

export type Actions = 
    addWorkSchedule | 
    addWorkScheduleSuccess | 
    addWorkScheduleFailed |
    loadDetail |
    loadDetailSuccess |
    loadDetailFailed |
    loadEntry |
    deleteRangeDetail | 
    failedShowDetail | 
    failed |
    success;
