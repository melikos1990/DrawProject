import { Action } from '@ngrx/store';
import { CaseSourceViewModel, CaseViewModel, CaseAssignmentViewModel, CaseAssignmentComplaintInvoiceViewModel, CaseAssignmentComplaintNoticeViewModel, CaseAssignmentBaseViewModel, CaseAssignmentCommunicateViewModel, CaseFocusType } from 'src/app/model/case.model';
import { EntrancePayload, ResultPayload } from 'src/app/model/common.model';
import { EmailSenderViewModel } from 'src/app/model/shared.model';

export const INITIAL_SCREEN = '[CASE] INITIAL_SCREEN';

export const CLEAR_ALL = '[CASE] CLEAR ALL'
export const REMOVE_SOURCE_TAB = '[CASE] REMOVE_SOURCE_TAB'
export const ACTIVE_SOURCE_TAB = '[CASE] ACTIVE_SOURCE_TAB'
export const UNACTIVE_SOURCE_TAB = '[CASE] UNACTIVE_SOURCE_TAB'
export const REMOVE_CASE_TAB = '[CASE] REMOVE CASE TAB'
export const ACTIVE_CASE_TAB = '[CASE] ACTIVE CASE TAB'
export const UNACTIVE_CASE_TAB = '[CASE] UNACTIVE_CASE_TAB CASE TAB'
export const LOAD_CASE_SOURCE_ENTRY = '[CASE] LOAD_CASE_SOURCE_ENTRY';
export const LOAD_CASE_SOURCE_CHECK = '[CASE] LOAD_CASE_SOURCE_CHECK';
export const LOAD_CASE_SOURCE = '[CASE] LOAD_CASE_SOURCE';
export const LOAD_CASE_SOURCE_SUCCESS = '[CASE] LOAD_CASE_SOURCE_SUCCESS';
export const LOAD_CASE_SOURCE_FAILED = '[CASE] LOAD_CASE_SOURCE_FAILED';
export const LOAD_CASE_SOURCE_NATIVE = '[CASE] LOAD_CASE_SOURCE_NATIVE';
export const LOAD_CASE_SOURCE_NATIVE_SUCCESS = '[CASE] LOAD_CASE_SOURCE_NATIVE_SUCCESS';
export const LOAD_CASE_SOURCE_NATIVE_FAILED = '[CASE] LOAD_CASE_SOURCE_NATIVE_FAILED';

export const LOAD_CASE_ENTRY = '[CASE] LOAD_CASE_ENTRY';
export const LOAD_CASE = '[CASE] LOAD_CASE';
export const LOAD_CASE_SUCCESS = '[CASE] LOAD_CASE_SUCCESS';
export const LOAD_CASE_FAILED = '[CASE] LOAD_CASE_FAILED';

export const EDIT_CASE_SOURCE = '[CASE] EDIT_CASE_SOURCE';
export const EDIT_CASE_SOURCE_SUCCESS = '[CASE] EDIT_CASE_SOURCE_SUCCESS';
export const EDIT_CASE_SOURCE_FAILED = '[CASE] EDIT_CASE_SOURCE_FAILED';

export const ADD_CASE = '[CASE] ADD_CASE';
export const ADD_CASE_SUCCESS = '[CASE] ADD_CASE_SUCCESS';
export const ADD_CASE_FAILED = '[CASE] ADD_CASE_FAILED';
export const EDIT_CASE = '[CASE] UPDATE_CASE';
export const EDIT_CASE_SUCCESS = '[CASE] UPDATE_CASE_SUCCESS';
export const EDIT_CASE_FAILED = '[CASE] UPDATE_CASE_FAILED';
export const FINISH_CASE = '[CASE] FINISH_CASE';
export const FINISH_CASE_SUCCESS = '[CASE] FINISH_CASE_SUCCESS';
export const FINISH_CASE_FAILED = '[CASE] FINISH_CASE_FAILED';

export const UNLOCK_CASE = '[CASE] UNLOCK_CASE';
export const UNLOCK_CASE_SUCCESS = '[CASE] UNLOCK_CASE_SUCCESS';
export const UNLOCK_CASE_FAILED = '[CASE] UNLOCK_CASE_FAILED';

export const ADD_CASE_SOURCE = '[CASE] ADD_CASE_SOURCE';
export const ADD_CASE_SOURCE_SUCCESS = '[CASE] ADD_CASE_SOURCE_SUCCESS';
export const ADD_CASE_SOURCE_FAILED = '[CASE] ADD_CASE_SOURCE_FAILED';

export const ADD_CASE_SOURCE_COMPLETE = '[CASE] ADD_CASE_SOURCE_COMPLETE';
export const ADD_CASE_SOURCE_COMPLETE_AND_FAST_FINISH = '[CASE] ADD_CASE_SOURCE_COMPLETE_AND_FAST_FINISH';
export const ADD_CASE_SOURCE_COMPLETE_SUCCESS = '[CASE] ADD_CASE_SOURCE_COMPLETE_SUCCESS';
export const ADD_CASE_SOURCE_COMPLETE_FAILED = '[CASE]ADD_CASE_SOURCE_COMPLETE_FAILED';

export const LOAD_CASE_IDS = '[CASE] LOAD_CASE_IDS';
export const LOAD_CASE_IDS_SUCCESS = '[CASE] LOAD_CASE_IDS_SUCCESS';
export const LOAD_CASE_IDS_FAILED = '[CASE] LOAD_CASE_IDS_FAILED';

export const ADD_CASE_ASSIGNMENT = '[CASE] ADD_CASE_ASSIGNMENT';
export const ADD_CASE_ASSIGNMENT_INVOCIE = '[CASE] ADD_CASE_ASSIGNMENT_INVOCIE';
export const ADD_CASE_ASSIGNMENT_NOTICE = '[CASE] ADD_CASE_ASSIGNMENT_NOTICE';
export const ADD_CASE_ASSIGNMENT_COMMUNICATE = '[CASE] ADD_CASE_ASSIGNMENT_NOTICE_COMMUNICATE';
export const ADD_CASE_ASSIGNMENT_AGGREGATE_SUCCESS = '[CASE] ADD_CASE_ASSIGNMENT_AGGREGATE_SUCCESS';
export const ADD_CASE_ASSIGNMENT_AGGREGATE_FAILED = '[CASE] ADD_CASE_ASSIGNMENT_AGGREGATE_FAILED';

export const EDIT_CASE_ASSIGNMENT_INVOICE = '[CASE] EDIT_CASE_ASSIGNMENT_INVOICE'
export const EDIT_CASE_ASSIGNMENT_INVOICE_SUCCESS = '[CASE] EDIT_CASE_ASSIGNMENT_INVOICE_SUCCESS'
export const EDIT_CASE_ASSIGNMENT_INVOICE_FAILED = '[CASE] EDIT_CASE_ASSIGNMENT_INVOICE_FAILED'
export const EDIT_CASE_ASSIGNMENT_NOTICE = '[CASE] EDIT_CASE_ASSIGNMENT_NOTICE'
export const EDIT_CASE_ASSIGNMENT_NOTICE_SUCCESS = '[CASE] EDIT_CASE_ASSIGNMENT_NOTICE_SUCCESS'
export const EDIT_CASE_ASSIGNMENT_NOTICE_FAILED = '[CASE] EDIT_CASE_ASSIGNMENT_NOTICE_FAILED'

export const SEND_CASE_ASSIGNMENT_INVOICE = '[CASE] SEND_CASE_ASSIGMNENT_INVOICE';
export const SEND_CASE_ASSIGNMENT_INVOICE_SUCCESS = '[CASE] SEND_CASE_ASSIGMNENT_INVOICE_SUCCESS';
export const SEND_CASE_ASSIGNMENT_INVOICE_FAILED = '[CASE] SEND_CASE_ASSIGMNENT_INVOICE_FAILED';

export const RESEND_CASE_ASSIGNMENT_INVOICE = '[CASE] RESEND_CASE_ASSIGNMENT_INVOICE';
export const RESEND_CASE_ASSIGNMENT_INVOICE_SUCCESS = '[CASE] RESEND_CASE_ASSIGNMENT_INVOICE_SUCCESS';
export const RESEND_CASE_ASSIGNMENT_INVOICE_FAILED = '[CASE] RESEND_CASE_ASSIGNMENT_INVOICE_FAILED';

export const CASE_FINISHED_REPLY_MAIL = '[CASE] CASE_FINISHED_REPLY_MAIL';
export const CASE_FINISHED_REPLY_MAIL_SUCCESS = '[CASE] CASE_FINISHED_REPLY_MAIL_SUCCESS';
export const CASE_FINISHED_REPLY_MAIL_FAILED = '[CASE] CASE_FINISHED_REPLY_MAIL_FAILED';

export const FINISH_CASEASSIGNMENT = '[CASE] FINISH_CASEASSIGNMENT';
export const FINISH_CASEASSIGNMENT_SUCCESS = '[CASE]  FINISH_CASEASSIGNMENT_SUCCESS';
export const FINISH_CASEASSIGNMENT_FAILED = '[CASE]  FINISH_CASEASSIGNMENT_FAILED';

export const REJECT_CASEASSIGNMENT = '[CASE] REJECT_CASEASSIGNMENT';
export const REJECT_CASEASSIGNMENT_SUCCESS = '[CASE]  REJECT_CASEASSIGNMENT_SUCCESS';
export const REJECT_CASEASSIGNMENT_FAILED = '[CASE]  REJECT_CASEASSIGNMENT_FAILED';

export const JOIN_ROOM = '[CASE] JOIN_ROOM';

export const SEND_CASE_ASSIGNMENT = '[CASE] SEND_CASE_ASSIGNMENT';
export const SEND_CASE_ASSIGNMENT_SUCCESS = '[CASE] SEND_CASE_ASSIGNMENT_SUCCESS';
export const SEND_CASE_ASSIGNMENT_FAILED = '[CASE] SEND_CASE_ASSIGNMENT_FAILED';

export const CALCEL_INVOICE = '[CASE] CALCEL_INVOICE';

export class initialScreenAction implements Action {
  type: string = INITIAL_SCREEN;
  constructor(public payload = null) { }
}


export class clearAllAction implements Action {
  type: string = CLEAR_ALL;
  constructor(public payload = null) { }
}

export class removeSorceTabAction implements Action {
  type: string = REMOVE_SOURCE_TAB;
  constructor(public payload: string) { }
}

export class activeSourceTabAction implements Action {
  type: string = ACTIVE_SOURCE_TAB;
  constructor(public payload: string) { }
}
export class unActiveSourceTabAction implements Action {
  type: string = UNACTIVE_SOURCE_TAB;
  constructor(public payload = null) { }
}

export class removeCaseTabAction implements Action {
  type: string = REMOVE_CASE_TAB;
  constructor(public payload: {
    sourceID: string,
    caseID: string
  }) { }
}

export class activeCaseTabAction implements Action {
  type: string = ACTIVE_CASE_TAB;
  constructor(public payload: string) { }
}
export class unActiveCaseTabAction implements Action {
  type: string = UNACTIVE_CASE_TAB;
  constructor(public payload = null) { }
}

export class loadCaseSourceEntryAction implements Action {
  type: string = LOAD_CASE_SOURCE_ENTRY;
  constructor(public payload: CaseSourceViewModel = new CaseSourceViewModel()) { }
}
export class loadCaseSourceCheckAction implements Action {
  type: string = LOAD_CASE_SOURCE_CHECK;
  constructor(public payload: EntrancePayload<{ caseID: string , sourceTab: string }>) { }
}
export class loadCaseSourceAction implements Action {
  type: string = LOAD_CASE_SOURCE;
  constructor(public payload: string) { }
}
export class loadCaseSourceSuccessAction implements Action {
  type: string = LOAD_CASE_SOURCE_SUCCESS;
  constructor(public payload: CaseSourceViewModel) { }
}
export class loadCaseSourceFailedAction implements Action {
  type: string = LOAD_CASE_SOURCE_FAILED;
  constructor(public payload: string) { }
}
export class loadCaseSourceNativeAction implements Action {
  type: string = LOAD_CASE_SOURCE_NATIVE;
  constructor(public payload: {
    sourceID: string,
    navigate: CaseFocusType
  }) { }
}
export class loadCaseSourceNativeSuccessAction implements Action {
  type: string = LOAD_CASE_SOURCE_NATIVE_SUCCESS;
  constructor(public payload: CaseSourceViewModel) { }
}
export class loadCaseSourceNativeFailedAction implements Action {
  type: string = LOAD_CASE_SOURCE_NATIVE_FAILED;
  constructor(public payload: string) { }
}

export class editCaseSourceAction implements Action {
  type: string = EDIT_CASE_SOURCE;
  constructor(public payload: EntrancePayload<CaseSourceViewModel>) { }
}
export class editCaseSourceSuccessAction implements Action {
  type: string = EDIT_CASE_SOURCE_SUCCESS;
  constructor(public payload: ResultPayload<CaseSourceViewModel>) { }
}
export class editCaseSourceFailedAction implements Action {
  type: string = EDIT_CASE_SOURCE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class addCaseSourceAction implements Action {
  type: string = ADD_CASE_SOURCE;
  constructor(public payload: EntrancePayload<CaseSourceViewModel>) { }
}

export class addCaseSourceSuccessAction implements Action {
  type: string = ADD_CASE_SOURCE_SUCCESS;
  constructor(public payload: ResultPayload<CaseSourceViewModel>) { }
}
export class addCaseSourceFailedAction implements Action {
  type: string = ADD_CASE_SOURCE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class editCaseAction implements Action {
  type: string = EDIT_CASE;
  constructor(public payload: EntrancePayload<{CaseViewModel:CaseViewModel,roleID:number}>) { }
}

export class editCaseSuccessAction implements Action {
  type: string = EDIT_CASE_SUCCESS;
  constructor(public payload: ResultPayload<CaseViewModel>) { }
}
export class editCaseFailedAction implements Action {
  type: string = EDIT_CASE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class finishCaseAction implements Action {
  type: string = FINISH_CASE;
  constructor(public payload: EntrancePayload<CaseViewModel>) { }
}

export class finishCaseSuccessAction implements Action {
  type: string = FINISH_CASE_SUCCESS;
  constructor(public payload: ResultPayload<CaseViewModel>) { }
}
export class finishCaseFailedAction implements Action {
  type: string = FINISH_CASE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class addCaseAction implements Action {
  type: string = ADD_CASE;
  constructor(public payload: EntrancePayload<CaseViewModel>) { }
}

export class addCaseSuccessAction implements Action {
  type: string = ADD_CASE_SUCCESS;
  constructor(public payload: ResultPayload<CaseViewModel>) { }
}
export class addCaseFailedAction implements Action {
  type: string = ADD_CASE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class addCaseSourceCompleteAction implements Action {
  type: string = ADD_CASE_SOURCE_COMPLETE;
  constructor(public payload: EntrancePayload<CaseSourceViewModel>) { }
}
export class addCaseSourceCompleteAndFastFinishAction implements Action {
  type: string = ADD_CASE_SOURCE_COMPLETE_AND_FAST_FINISH;
  constructor(public payload: EntrancePayload<CaseSourceViewModel>) { }
}
export class addCaseSourceCompleteSuccessAction implements Action {
  type: string = ADD_CASE_SOURCE_COMPLETE_SUCCESS;
  constructor(public payload: ResultPayload<CaseSourceViewModel>) { }
}
export class addCaseSourceCompleteFailedAction implements Action {
  type: string = ADD_CASE_SOURCE_COMPLETE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class loadCaseEntryAction implements Action {
  type: string = LOAD_CASE_ENTRY;
  constructor(public payload: {
    model: CaseViewModel,
    sourceKey: string,
  }) { }
}

export class loadCaseAction implements Action {
  type: string = LOAD_CASE;
  constructor(public payload: string) { }
}
export class loadCaseSuccessAction implements Action {
  type: string = LOAD_CASE_SUCCESS;
  constructor(public payload: CaseViewModel) { }

}
export class loadCaseFailedAction implements Action {
  type: string = LOAD_CASE_FAILED;
  constructor(public payload: string) { }
}

export class loadCaseIDsAction implements Action {
  type: string = LOAD_CASE_IDS;
  constructor(public payload: EntrancePayload<string>) { }
}
export class loadCaseIDsSuccessAction implements Action {
  type: string = LOAD_CASE_IDS_SUCCESS;
  constructor(public payload: ResultPayload<{
    sorceID: string,
    caseIDs: string[]
  }>) { }
}
export class loadCaseIDsFailedAction implements Action {
  type: string = LOAD_CASE_IDS_FAILED;
  constructor(public payload: string) { }
}

export class addCaseAssignmentAction implements Action {
  type: string = ADD_CASE_ASSIGNMENT;
  constructor(public payload: EntrancePayload<CaseAssignmentViewModel>) { }
}
export class addCaseAssignmentInvoiceAction implements Action {
  type: string = ADD_CASE_ASSIGNMENT_INVOCIE;
  constructor(public payload: EntrancePayload<CaseAssignmentComplaintInvoiceViewModel>) { }
}
export class addCaseAssignmentNoticeAction implements Action {
  type: string = ADD_CASE_ASSIGNMENT_NOTICE;
  constructor(public payload: EntrancePayload<CaseAssignmentComplaintNoticeViewModel>) { }
}
export class addCaseAssignmentCommunicateAction implements Action {
  type: string = ADD_CASE_ASSIGNMENT_COMMUNICATE;
  constructor(public payload: EntrancePayload<CaseAssignmentCommunicateViewModel>) { }
}
export class addCaseAssignmentAggregateSuccessAction implements Action {
  type: string = ADD_CASE_ASSIGNMENT_AGGREGATE_SUCCESS;
  constructor(public payload: ResultPayload<CaseAssignmentBaseViewModel>) { }
}
export class addCaseAssignmentAggregateFailedAction implements Action {
  type: string = ADD_CASE_ASSIGNMENT_AGGREGATE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class sendCaseAssignmentInvoiceAction implements Action {
  type: string = SEND_CASE_ASSIGNMENT_INVOICE;
  constructor(public payload: EntrancePayload<{
    identityID: number,
    model: EmailSenderViewModel
  }>) { }
}

export class sendCaseAssignmentInvoiceSuccessAction implements Action {
  type: string = SEND_CASE_ASSIGNMENT_INVOICE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class sendCaseAssignmentInvoiceFailedAction implements Action {
  type: string = SEND_CASE_ASSIGNMENT_INVOICE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class resendCaseAssignmentInvoiceAction implements Action {
  type: string = RESEND_CASE_ASSIGNMENT_INVOICE;
  constructor(public payload: EntrancePayload<{
    identityID: number,
    model: EmailSenderViewModel
  }>) { }
}

export class resendCaseAssignmentInvoiceSuccessAction implements Action {
  type: string = RESEND_CASE_ASSIGNMENT_INVOICE_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class resendCaseAssignmentInvoiceFailedAction implements Action {
  type: string = RESEND_CASE_ASSIGNMENT_INVOICE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class caseFinishedReplyMailAction implements Action {
  type: string = CASE_FINISHED_REPLY_MAIL;
  constructor(public payload: EntrancePayload<{
    caseID: string,
    model: EmailSenderViewModel
  }>) { }
}

export class caseFinishedReplyMailSuccessAction implements Action {
  type: string = CASE_FINISHED_REPLY_MAIL_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class caseFinishedReplyMailFailedAction implements Action {
  type: string = CASE_FINISHED_REPLY_MAIL_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class editCaseAssignmentInvoiceAction implements Action {
  type: string = EDIT_CASE_ASSIGNMENT_INVOICE;
  constructor(public payload: EntrancePayload<CaseAssignmentComplaintInvoiceViewModel>) { }
}
export class editCaseAssignmentInvoiceSuccessAction implements Action {
  type: string = EDIT_CASE_ASSIGNMENT_INVOICE_SUCCESS;
  constructor(public payload: ResultPayload<CaseAssignmentComplaintInvoiceViewModel>) { }
}
export class editCaseAssignmentInvoiceFailedAction implements Action {
  type: string = EDIT_CASE_ASSIGNMENT_INVOICE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}
export class editCaseAssignmentNoticeAction implements Action {
  type: string = EDIT_CASE_ASSIGNMENT_NOTICE;
  constructor(public payload: EntrancePayload<CaseAssignmentComplaintNoticeViewModel>) { }
}
export class editCaseAssignmentNoticeSuccessAction implements Action {
  type: string = EDIT_CASE_ASSIGNMENT_NOTICE_SUCCESS;
  constructor(public payload: ResultPayload<CaseAssignmentComplaintNoticeViewModel>) { }
}
export class editCaseAssignmentNoticeFailedAction implements Action {
  type: string = EDIT_CASE_ASSIGNMENT_NOTICE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class finishCaseAssignmentAction implements Action {
  type: string = FINISH_CASEASSIGNMENT;
  constructor(public payload: EntrancePayload<CaseAssignmentViewModel>) { }
}

export class finishCaseAssignmentSuccessAction implements Action {
  type: string = FINISH_CASEASSIGNMENT_SUCCESS;
  constructor(public payload: ResultPayload<CaseAssignmentViewModel>) { }
}

export class finishCaseAssignmentFailedAction implements Action {
  type: string = FINISH_CASEASSIGNMENT_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}


export class rejectCaseAssignmentAction implements Action {
  type: string = REJECT_CASEASSIGNMENT;
  constructor(public payload: EntrancePayload<CaseAssignmentViewModel>) { }
}

export class rejectCaseAssignmentSuccessAction implements Action {
  type: string = REJECT_CASEASSIGNMENT_SUCCESS;
  constructor(public payload: ResultPayload<CaseAssignmentViewModel>) { }
}

export class rejectCaseAssignmentFailedAction implements Action {
  type: string = REJECT_CASEASSIGNMENT_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class unLockAction implements Action {
  type: string = UNLOCK_CASE;
  constructor(public payload: EntrancePayload<CaseViewModel>) { }
}

export class unLockSuccessAction implements Action {
  type: string = UNLOCK_CASE_SUCCESS;
  constructor(public payload: ResultPayload<CaseViewModel>) { }
}

export class unLockFailedAction implements Action {
  type: string = UNLOCK_CASE_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

export class joinRoomAction implements Action {
  type: string = JOIN_ROOM;
  constructor(public payload: string) { }
}



export class sendCaseAssignmentAction implements Action {
  type: string = SEND_CASE_ASSIGNMENT;
  constructor(public payload: EntrancePayload<{
    assignmentID: number,
    caseID: string,
    model: EmailSenderViewModel
  }>) { }
}

export class sendCaseAssignmentSuccessAction implements Action {
  type: string = SEND_CASE_ASSIGNMENT_SUCCESS;
  constructor(public payload: ResultPayload<string>) { }
}

export class sendCaseAssignmentFailedAction implements Action {
  type: string = SEND_CASE_ASSIGNMENT_FAILED;
  constructor(public payload: ResultPayload<string>) { }
}

//CancelInvoice
export class CancelInvoiceAction implements Action {
  type: string = CALCEL_INVOICE;
  constructor(public payload:  EntrancePayload<{
    InvoiceIdentityID : number
  }>) { }
}

export type Actions =
  initialScreenAction |
  clearAllAction |
  removeSorceTabAction |
  activeSourceTabAction |
  loadCaseSourceEntryAction |
  loadCaseSourceCheckAction |
  loadCaseSourceAction |
  loadCaseSourceSuccessAction |
  loadCaseSourceFailedAction |
  loadCaseSourceNativeAction |
  loadCaseSourceNativeSuccessAction |
  loadCaseSourceNativeFailedAction |
  editCaseSourceAction |
  editCaseSourceSuccessAction |
  editCaseSourceFailedAction |
  addCaseSourceAction |
  addCaseSourceSuccessAction |
  addCaseSourceFailedAction |
  addCaseSourceCompleteAction |
  addCaseSourceCompleteAndFastFinishAction |
  addCaseSourceCompleteSuccessAction |
  addCaseSourceCompleteFailedAction |
  loadCaseEntryAction |
  loadCaseAction |
  loadCaseSuccessAction |
  loadCaseFailedAction |
  loadCaseIDsAction |
  loadCaseIDsSuccessAction |
  loadCaseIDsFailedAction |
  removeCaseTabAction |
  addCaseAction |
  addCaseSuccessAction |
  addCaseFailedAction |
  editCaseAction |
  editCaseSuccessAction |
  editCaseFailedAction |
  addCaseAssignmentAction |
  addCaseAssignmentInvoiceAction |
  addCaseAssignmentNoticeAction |
  addCaseAssignmentCommunicateAction |
  addCaseAssignmentAggregateSuccessAction |
  addCaseAssignmentAggregateFailedAction |
  sendCaseAssignmentInvoiceAction |
  sendCaseAssignmentInvoiceSuccessAction |
  sendCaseAssignmentInvoiceFailedAction |
  resendCaseAssignmentInvoiceAction |
  resendCaseAssignmentInvoiceSuccessAction |
  resendCaseAssignmentInvoiceFailedAction |
  caseFinishedReplyMailAction |
  caseFinishedReplyMailSuccessAction |
  caseFinishedReplyMailFailedAction |
  editCaseAssignmentInvoiceAction |
  editCaseAssignmentInvoiceSuccessAction |
  editCaseAssignmentInvoiceFailedAction |
  editCaseAssignmentNoticeAction |
  editCaseAssignmentNoticeSuccessAction |
  editCaseAssignmentNoticeFailedAction |
  finishCaseAssignmentAction |
  finishCaseAssignmentSuccessAction |
  finishCaseAssignmentFailedAction |
  rejectCaseAssignmentAction |
  rejectCaseAssignmentSuccessAction |
  rejectCaseAssignmentFailedAction |
  unLockAction |
  unLockSuccessAction |
  unLockFailedAction |
  unActiveCaseTabAction |
  unActiveSourceTabAction |
  joinRoomAction |
  sendCaseAssignmentAction |
  sendCaseAssignmentSuccessAction |
  sendCaseAssignmentFailedAction |
  CancelInvoiceAction;
