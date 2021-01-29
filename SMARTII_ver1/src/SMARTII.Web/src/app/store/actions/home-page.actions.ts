import { Action } from '@ngrx/store';
import { OrganizationType } from 'src/app/model/organization.model';

export const CHANGE_HOME_TYPE = '[HOME PAGE] CHANGE_HOME_TYPE';


export class changeHomeTypeAction implements Action {
  public type: string = CHANGE_HOME_TYPE;
  constructor(public payload: string) { }
}


export type Actions =
  changeHomeTypeAction;