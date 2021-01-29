import { Action } from '@ngrx/store';
import { CaseAssignmentVendorSearchViewModel } from 'src/app/model/search.model';
import { ResultPayload, EntrancePayload } from 'src/app/model/common.model';

export const CASE_VENDOR_REPORT = '[CASE_ASSIGNMENT_SEARCH] CASE_VENDOR_REPORT';
export const CASE_VENDOR_REPORT_SUCCESS = '[CASE_ASSIGNMENT_SEARCH] CASE_VENDOR_REPORT_SUCCESS';
export const CASE_VENDOR_REPORT_FAILED = '[CASE_ASSIGNMENT_SEARCH] CASE_VENDOR_REPORT_FAILED';
export const CASE_VENDOR_GETLIST = '[CASE_ASSIGNMENT_SEARCH] CASE_VENDOR_GETLIST';
export const CASE_VENDOR_GETLIST_SUCCESS = '[CASE_ASSIGNMENT_SEARCH] CASE_VENDOR_GETLIST_SUCCESS';
export const CASE_VENDOR_GETLIST_FAILED = '[CASE_ASSIGNMENT_SEARCH] CASE_VENDOR_GETLIST_FAILED';

export class vendorReport implements Action {
  public type: string = CASE_VENDOR_REPORT;
  constructor(public payload: CaseAssignmentVendorSearchViewModel) { }
}


export class vendorReportSuccess implements Action {
  public type: string = CASE_VENDOR_REPORT_SUCCESS;
  constructor(public payload: Blob) { }
}


export class vendorReportFailed implements Action {
  public type: string = CASE_VENDOR_REPORT_FAILED;
  constructor(public payload: string) { }
}


export class vendorGetList implements Action {
  public type: string = CASE_VENDOR_GETLIST;
  constructor(public payload: EntrancePayload<CaseAssignmentVendorSearchViewModel>) { }
}


export class vendorGetListSuccess implements Action {
  public type: string = CASE_VENDOR_GETLIST_SUCCESS;
  constructor(public payload: ResultPayload<any[]>) { }
}


export class vendorGetListFailed implements Action {
  public type: string = CASE_VENDOR_GETLIST_FAILED;
  constructor(public payload: ResultPayload<any>) { }
}


export type Actions =
  vendorReport |
  vendorReportSuccess |
  vendorReportFailed |
  vendorGetList |
  vendorGetListSuccess |
  vendorGetListFailed;
