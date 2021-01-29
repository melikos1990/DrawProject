import { Action } from '@ngrx/store';

export const CHANGE_ROUTE = "[ROUTE] CHANGE";
export const CHANGE_BREADCRUMB = "[ROUTE] CHANGE_BREADCRUMB";

export class changeRouteAction implements Action {
  type: string = CHANGE_ROUTE
  constructor(public payload: any) { }
}

export class changeBreadcrumbAction implements Action {
  type: string = CHANGE_BREADCRUMB
  constructor(public payload: any[]) { }
}

export type Actions = changeRouteAction | changeBreadcrumbAction;
