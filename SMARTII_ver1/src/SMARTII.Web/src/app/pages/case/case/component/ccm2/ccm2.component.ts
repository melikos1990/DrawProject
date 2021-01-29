import { Component, OnInit, Injector, Input, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseViewModel, CaseAssignmentBaseViewModel, CaseAssignmentComplaintNoticeViewModel, CaseAssignmentViewModel, CaseAssignmentComplaintInvoiceViewModel, CaseAssignmentCommunicateViewModel } from 'src/app/model/case.model';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { Ccml2Component } from './ccml2/ccml2.component';
import { State as fromCaseReducer } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import * as fromCaseActions from '../../../store/actions';
import * as fromRootActions from '../../../../../store/actions';
import * as fromRootAction from '../../../../../store/actions';
import { Ccmi2Component } from './ccmi2/ccmi2.component';
import { EmailSenderViewModel, EmailReceiveType } from 'src/app/model/shared.model';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ConcatableUserViewModel, BusinesssUnitParameters } from 'src/app/model/organization.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, concatMap, concatMapTo, finalize } from 'rxjs/operators';
import { HttpService } from 'src/app/shared/service/http.service';
import { MailSenderComponent } from 'src/app/shared/component/other/mail-sender/mail-sender.component';
import { Observable, of } from 'rxjs';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import { HttpResponse } from '@angular/common/http';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'app-ccm2',
  templateUrl: './ccm2.component.html',
  styleUrls: ['./ccm2.component.scss']
})
export class Ccm2Component extends FormBaseComponent implements OnInit {

  @Input() focusId: string;
  senderList: any[] = [];
  sender: EmailSenderViewModel = new EmailSenderViewModel();
  businessParameter: BusinesssUnitParameters;
  reportSrc;
  newInvoiceID: string;

  @ViewChild('mailsender')
  mailsenderRef: TemplateRef<any>;

  @ViewChild('ccml2') ccml2Ref: Ccml2Component;
  @ViewChild('ccmi2') ccmi2Ref: Ccmi2Component;
  @ViewChild('excelViewer') excelViewerRef: TemplateRef<any>;

  @Input() uiActionType: ActionType;
  @Input() sourcekey: string;
  @Input() model: CaseViewModel
  @Input() assignment: CaseAssignmentBaseViewModel = new CaseAssignmentBaseViewModel();

  public form: FormGroup = new FormGroup({});

  get assignmentID() {
    let assignmentID = (<CaseAssignmentViewModel>this.assignment).AssignmentID;
    return assignmentID ? assignmentID : null;
  }

  constructor(
    public http: HttpService,
    public caseService: CaseService,
    public modalService: NgbModal,
    public store: Store<fromCaseReducer>,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initialize();
  }


  btnSaveAssignment() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.assignment['CaseAssignmentUsers'] || this.assignment['CaseAssignmentUsers'].length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_COMMON.CHOSE_TRANSFER'))))
      return false;
    }

    if (!this.assignment['CaseAssignmentUsers'].some(x => x.CaseComplainedUserType == this.caseComplainedUserType.Responsibility)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("只少要有一個權責單位")))
      return;
    }

    const model = new EntrancePayload<CaseAssignmentViewModel>(<CaseAssignmentViewModel>this.assignment);
    model.success = (data: CaseAssignmentViewModel) => {

      // 沒有選擇 Email 通知行為就不寄信
      if (data.NotificationBehaviors.some(x => x == this.notificationType.Email.toString()) == false) {
        this.clearPayload();
        return;
      }

      this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
      this.sender = this.generatorSenderPayload();
      this.senderList = [];

      this.getCurrentUser().subscribe(user => {
        if (user.Email)
          this.senderList = this.senderList.concat({Email: user.Email, UserName: user.Name});
      });

      (<CaseAssignmentViewModel>this.assignment).AssignmentID = data.AssignmentID;
    }

    this.store.dispatch(new fromCaseActions.CaseCreatorActions.addCaseAssignmentAction(model));

  }
  btnPreviewInvoiceModal() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.assignment['InvoiceType']) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_COMMON.CHOSE_INOVICE_TYPE'))));
      return;
    }

    if (!this.model.CaseComplainedUsers || this.model.CaseComplainedUsers.length < 1) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先填寫被反應者並點選儲存")));
      return;
    }

    //開遮罩
    this.store.dispatch(new fromRootAction.LoadingActions.visibleLoadingAction())

    //取得反應單PDF
    const work$ = this.caseService
      .getPreviewComplaintInvoice(this.model.NodeID, this.model.CaseID, null);

    //處理回應
    work$.pipe(
      finalize(() => {
        this.store.dispatch(new fromRootAction.LoadingActions.invisibleLoadingAction());
        return;
      })).
      subscribe(x => {
        this.reportSrc = x
      }, (error: HttpResponse<any>) => {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(error.statusText.decoding())))
      },
        () => {
          this.modalService.open(this.excelViewerRef, { container: 'nb-layout', backdrop: 'static', windowClass: 'ccm2-modal' });
        });
  }

  btnOpenInvoice() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const model = new EntrancePayload<CaseAssignmentComplaintInvoiceViewModel>(<CaseAssignmentComplaintInvoiceViewModel>this.assignment);
    model.success = (model: CaseAssignmentComplaintInvoiceViewModel) => {
      (<CaseAssignmentComplaintInvoiceViewModel>this.assignment).InvoiceID = model.InvoiceID;
      (<CaseAssignmentComplaintInvoiceViewModel>this.assignment).ID = model.ID;
      this.newInvoiceID = model.InvoiceID;
      this.modalService.dismissAll();

      // 通知行為 選擇 Email 才開啟寄信畫面
      if((<CaseAssignmentBaseViewModel>this.assignment).NotificationBehaviors.some((x: any) => x == this.notificationType.Email)){
        this.btnSenderModal();
      }
      else{
        this.clearPayload();
      }

    }
    this.store.dispatch(new fromCaseActions.CaseCreatorActions.addCaseAssignmentInvoiceAction(model));
  }

  btnSenderModal() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!!this.assignment.NotificationBehaviors && this.assignment.NotificationBehaviors.length > 0 && this.assignment.NotificationBehaviors.includes("0")) {
      this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
      this.sender = this.generatorSenderPayload();
      this.senderList = [];

      this.getCurrentUser().subscribe(user => {
        if (user.Email)
          this.senderList = this.senderList.concat({Email: user.Email, UserName: user.Name});
      })

    }
    else {
      this.btnSend(null);
    }
  }

  btnOnBack() {
    this.modalService.dismissAll();
    this.clearPayload();
  }


  btnSendInvoice($event: EmailSenderViewModel) {

    const data = new EntrancePayload<{
      identityID: number,
      model: EmailSenderViewModel
    }>({
      identityID: (<CaseAssignmentComplaintInvoiceViewModel>this.assignment).ID,
      model: $event,
    });
    data.success = () => {
      this.modalService.dismissAll();
      this.clearPayload();
    }

    this.store.dispatch(new fromCaseActions.CaseCreatorActions.sendCaseAssignmentInvoiceAction(data));
  }

  btnSendNotice($event: EmailSenderViewModel) {

    console.log("$event => ", $event);

    this.assignment.EmailPayload = $event;
    const model = new EntrancePayload<CaseAssignmentComplaintNoticeViewModel>(<CaseAssignmentComplaintNoticeViewModel>this.assignment);
    model.success = () => {
      this.modalService.dismissAll();
      this.clearPayload();
    }

    this.store.dispatch(new fromCaseActions.CaseCreatorActions.addCaseAssignmentNoticeAction(model));
  }

  btnSendAssignment($event: EmailSenderViewModel) {

    const payload = new EntrancePayload<{
      assignmentID: number,
      caseID: string,
      model: EmailSenderViewModel
    }>({
      assignmentID: (<CaseAssignmentViewModel>this.assignment).AssignmentID,
      caseID: (<CaseAssignmentViewModel>this.assignment).CaseID,
      model: $event,
    });

    payload.success = () => {
      this.modalService.dismissAll();
      this.clearPayload();
      (<CaseAssignmentViewModel>this.assignment).AssignmentID = null;
    }

    this.store.dispatch(new fromCaseActions.CaseCreatorActions.sendCaseAssignmentAction(payload));
  }

  btnSaveCommunicate() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const model = new EntrancePayload<CaseAssignmentCommunicateViewModel>(<CaseAssignmentCommunicateViewModel>this.assignment);
    model.success = () => {
      this.clearPayload();
    }
    this.store.dispatch(new fromCaseActions.CaseCreatorActions.addCaseAssignmentCommunicateAction(model));

  }


  btnSend($event: EmailSenderViewModel) {


    switch (+this.assignment.CaseAssignmentProcessType) {
      case this.caseAssignmentProcessType.Notice:
        this.btnSendNotice($event);
        break;
      case this.caseAssignmentProcessType.Invoice:
        this.btnSendInvoice($event);
        break;
      case this.caseAssignmentProcessType.Assignment:
        this.btnSendAssignment($event);
        break;
    }
  }

  generatorSenderPayload(): EmailSenderViewModel {
    const data = new EmailSenderViewModel();
    data.Content = this.assignment.Content;
    data.Sender = new ConcatableUserViewModel();
    data.Receiver = this.generatorConcatUserPayload(this.emailReceiveType.Recipient);
    data.Cc = this.generatorConcatUserPayload(this.emailReceiveType.CC);
    data.Bcc = this.generatorConcatUserPayload(this.emailReceiveType.BCC);
    data.Receiver = data.Receiver.concat(this.getNoSettingUsers());
    data.Attachments = this.assignment.Files;
    return data;
  }

  getNoSettingUsers() {
    return this.getConcatUser(this.assignment)
      .filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
        !x.NotificationRemark);
  }

  generatorConcatUserPayload(emailRecvType: EmailReceiveType) {
    return this.getConcatUser(this.assignment)
      .filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
        x.NotificationRemark === emailRecvType.toString());
  }


  getConcatUser(assignment: CaseAssignmentBaseViewModel): ConcatableUserViewModel[] {
    switch (+assignment.CaseAssignmentProcessType) {
      case this.caseAssignmentProcessType.Assignment:
        return (<CaseAssignmentViewModel>assignment).CaseAssignmentConcatUsers;
      case this.caseAssignmentProcessType.Notice:
        return (<CaseAssignmentComplaintNoticeViewModel>assignment).Users;
      case this.caseAssignmentProcessType.Invoice:
        return (<CaseAssignmentComplaintInvoiceViewModel>assignment).Users;

    }

  }
  initialize() {

    this.caseService
      .getBUParameters(this.model.NodeID)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => this.businessParameter = x)
  }

  clearPayload() {
    this.ccml2Ref.getList();
    this.ccmi2Ref.refeshPayload();
    this.ccmi2Ref.fileOpts = {};
  }

  btnRefresh() {
    this.ccml2Ref.getList();
  }

}
