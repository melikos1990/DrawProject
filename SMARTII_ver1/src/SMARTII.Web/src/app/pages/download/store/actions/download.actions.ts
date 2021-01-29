import { Action } from '@ngrx/store';

export const DOWNLOAD_REPORT = '[DOWNLOAD] DOWNLOAD_REPORT';
export const DOWNLOAD_ASO_REPORT = '[DOWNLOAD] DOWNLOAD_ASO_REPORT';
export const DOWNLOAD_REPORT_FAILED = '[DOWNLOAD] DOWNLOAD_REPORT_FAILED';
export const DOWNLOAD_REPORT_SUCCESS = '[DOWNLOAD] DOWNLOAD_REPORT_SUCCESS';

export class downloadReport implements Action {
    public type: string = DOWNLOAD_REPORT;
    constructor(public payload: any) { }
}

export class downloadAsoReport implements Action {
    public type: string = DOWNLOAD_ASO_REPORT;
    constructor(public payload: any) { }
}

export class downloadReportFailed implements Action {
    public type: string = DOWNLOAD_REPORT_FAILED;
    constructor(public payload: string) { }
}

export class downloadReportSuccess implements Action {
    public type: string = DOWNLOAD_REPORT_SUCCESS;
    constructor(public payload: string) { }
}


export type Actions = downloadReport | downloadAsoReport | downloadReportFailed | downloadReportSuccess;
