import { InjectionToken } from '@angular/core';
import { CaseComplainedUserViewModel } from '../model/case.model';
import { CaseAssignGroupUserListViewModel } from '../model/master.model';


export const MASTER_ITEM_LIST_COLUMN_GRID = new InjectionToken<{ key: string, component: any }[]>('DYNAMIC_MASTER_ITEM_LIST_COLUMN_GRIDINPUTS');
export const CASE_FACTORY = new InjectionToken<ICaseFactory>('CASE_FACTORY');

export interface ICaseFactory {
  key: string;
  getNotificationUserName(users: CaseComplainedUserViewModel[]): string
  getNotificationUserMail(users: CaseComplainedUserViewModel[]): CaseAssignGroupUserListViewModel[]
}
