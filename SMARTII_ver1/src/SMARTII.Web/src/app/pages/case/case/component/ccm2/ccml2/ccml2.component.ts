import { Component, OnInit, Input, Injector, ViewChild, TemplateRef } from '@angular/core';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, finalize } from 'rxjs/operators';
import { CaseViewModel, CaseAssignmentOverviewViewModel, CaseAssignmentComplaintNoticeViewModel, CaseAssignmentComplaintInvoiceViewModel, CaseAssignmentViewModel, CaseAssignmentCommunicateViewModel, CaseAssignmentProcessType, CaseType } from 'src/app/model/case.model';
import { Store } from '@ngrx/store';

import * as fromCaseActions from '../../../../store/actions';
import { State as fromCaseReducers } from '../../../../store/reducers';
import { EmailSenderViewModel, EmailReceiveType } from 'src/app/model/shared.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaaModalComponent } from '../../../modal/caa-modal/caa-modal.component';
import { CanModalComponent } from '../../../modal/can-modal/can-modal.component';
import { CapModalComponent } from '../../../modal/cap-modal/cap-modal.component';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { ConcatableUserViewModel } from 'src/app/model/organization.model';
import { RejModalComponent } from '../../../modal/rej-modal/rej-modal.component';
import { CamModalComponent } from '../../../modal/cam-modal/cam-modal.component';
import { RemindModalComponent } from '../../../modal/remind-modal/remind-modal.component';
import * as fromRootAction from '../../../../../../store/actions';
import * as fromRootActions from '../../../../../../store/actions';
import { environment } from 'src/environments/environment';
import { webHostPrefix } from 'src/app.config';
import { successAlertAction, failedAlertAction } from 'src/app/shared/ngrx/alert.ngrx';
import { PtcSwalType } from 'ptc-swal';

@Component({
  selector: 'app-ccml2',
  templateUrl: './ccml2.component.html',
  styleUrls: ['./ccml2.component.scss']
})
export class Ccml2Component extends FormBaseComponent implements OnInit {

  senderList: any[] = [];
  sender: EmailSenderViewModel = new EmailSenderViewModel();
  reportSrc;

  @Input() uiActionType: ActionType;

  @ViewChild('mailsender')
  mailsenderRef: TemplateRef<any>;
  @ViewChild('excelViewer') excelViewerRef: TemplateRef<any>;
  data = [];
  columns = [];
  loading: boolean = false;


  currentInvocie: CaseAssignmentComplaintInvoiceViewModel;

  currentAssignment: CaseAssignmentViewModel;

  currentAssignmentProcessType: CaseAssignmentProcessType;

  private _case: CaseViewModel;


  @Input()
  public set case(v: CaseViewModel) {
    this._case = v;
    if (v && !!v.CaseID) {
      this.getList();
    }
  }
  public get case(): CaseViewModel {
    return this._case
  }


  constructor(
    public store: Store<fromCaseReducers>,
    public modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
  }



  getList() {
    this.loading = true;
    this.caseService.getAssignmentAggregate(this._case.CaseID)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.data = x.element;
        this.loading = false;
      })
  }

  getIdentity(row: CaseAssignmentOverviewViewModel) {

    switch (row.CaseAssignmentProcessType) {
      case this.caseAssignmentProcessType.Assignment: return row.AssignmentID;
      case this.caseAssignmentProcessType.Invoice: return row.InvoiceIdentityID;
      case this.caseAssignmentProcessType.Notice: return row.NoticeID;
      case this.caseAssignmentProcessType.Communication: return row.CommunicateID;
    }
  }

  toEditUrl = (actionType: ActionType, row: CaseAssignmentOverviewViewModel) => {
    return `${environment.webHostPrefix}/pages/case/case-assignment-detail`.toCustomerUrl({
      actionType,
      ID: this.getIdentity(row),
      type: row.CaseAssignmentProcessType,
      caseID: row.CaseID
    })
  }

  btnDetail(row: CaseAssignmentOverviewViewModel) {
    switch (+row.CaseAssignmentProcessType) {
      case +this.caseAssignmentProcessType.Assignment:
        this.assignmentDetailModal(row);
        break;
      case +this.caseAssignmentProcessType.Notice:
        this.noticeModal(row);
        break;
      case +this.caseAssignmentProcessType.Invoice:
        this.invoiceModal(row);
        break;
      case +this.caseAssignmentProcessType.Communication:
        this.communicateModal(row);
        break;
    }

  }

  btnFinish(row: CaseAssignmentOverviewViewModel) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否客服銷案?',
      () => {
        this.caseService
          .getAssignment(row.CaseID, row.AssignmentID)
          .subscribe(x => {
            const data = new EntrancePayload<CaseAssignmentViewModel>(x.element);
            data.success = () => {
              this.getList();
              this.modalService.dismissAll();
            }
            this.store.dispatch(new fromCaseActions.CaseCreatorActions.finishCaseAssignmentAction(data))
          })
      }
    )));
  }

  rejectModal(row: CaseAssignmentOverviewViewModel) {
    this.caseService
      .getAssignment(row.CaseID, row.AssignmentID)
      .subscribe(x => {
        const ref = this.modalService.open(RejModalComponent, { container: 'nb-layout', backdrop: 'static', windowClass: 'modal-md' });
        const instance = (<RejModalComponent>ref.componentInstance);

        instance.model = x.element;
        instance.btnConfirm = (model: CaseAssignmentViewModel) => {

          const data = new EntrancePayload<CaseAssignmentViewModel>(model);
          data.success = () => {
            this.getList();
            this.modalService.dismissAll();
          }

          this.store.dispatch(new fromCaseActions.CaseCreatorActions.rejectCaseAssignmentAction(data))
        }
      })
  }
  //派工 -> 派工按鈕事件
  btnAssignmentModal(model: CaseAssignmentOverviewViewModel) {
    const url = this.toEditUrl(this.actionType.Read, model);
    window.open(url, '_blank');
  }
  //派工 -> 明細按鈕事件
  assignmentDetailModal(model: CaseAssignmentOverviewViewModel) {
    this.caseService
      .getAssignment(model.CaseID, model.AssignmentID)
      .subscribe(x => {
        const ref = this.modalService.open(CaaModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
        const instance = (<CaaModalComponent>ref.componentInstance);
        instance.model = x.element;
      })
  }

  noticeModal(model: CaseAssignmentOverviewViewModel) {

    this.caseService
      .getAssignmentNotice(model.NoticeID)
      .subscribe(x => {
        const ref = this.modalService.open(CanModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
        const instance = (<CanModalComponent>ref.componentInstance);
        instance.model = x.element;
        instance.editSuccess = (data: CaseAssignmentComplaintNoticeViewModel) => {
          this.getList();
          this.modalService.dismissAll();
        }
      })

  }

  invoiceModal(model: CaseAssignmentOverviewViewModel) {

    this.caseService
      .getAssignmentIvoice(model.InvoiceIdentityID)
      .subscribe(x => {
        const ref = this.modalService.open(CapModalComponent, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl', backdrop: 'static' });
        const instance = (<CapModalComponent>ref.componentInstance);
        instance.model = x.element;
        instance.resendSuccess = (data: CaseAssignmentComplaintInvoiceViewModel) => {
          this.getList();
          this.modalService.dismissAll();
        }
        instance.editSuccess = (data: CaseAssignmentComplaintInvoiceViewModel) => {
          this.getList();
          this.modalService.dismissAll();
        }
      })

  }

  communicateModal(model: CaseAssignmentOverviewViewModel) {
    this.caseService
      .getAssignmentCommunicate(model.CommunicateID)
      .subscribe(x => {
        const ref = this.modalService.open(CamModalComponent, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl', backdrop: 'static' });
        const instance = (<CamModalComponent>ref.componentInstance);
        instance.model = x.element;
      })

  }

  btnInvoiceSenderModal(model: CaseAssignmentOverviewViewModel) {

    this.caseService
      .getAssignmentIvoice(model.InvoiceIdentityID)
      .subscribe(x => {

        this.currentAssignmentProcessType = this.caseAssignmentProcessType.Invoice;
        this.currentInvocie = x.element;
        this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl', backdrop: 'static' });
        this.senderList = [];
        this.sender = this.generatorSenderPayload();
        this.getCurrentUser().subscribe(user => {
          if (user.Email)
            this.senderList = this.senderList.concat({ Email: user.Email, UserName: user.Name });
        })
      })
  }

  btnOnBack() {
    this.modalService.dismissAll();
  }

  btnSendInvoice($event: EmailSenderViewModel) {

    const data = new EntrancePayload<{
      identityID: number,
      model: EmailSenderViewModel
    }>({
      identityID: this.currentInvocie.ID,
      model: $event,
    });
    data.success = () => {

      this.modalService.dismissAll();
      this.getList();
    }
    this.store.dispatch(new fromCaseActions.CaseCreatorActions.sendCaseAssignmentInvoiceAction(data));

  }

  btnSendAssignment($event: EmailSenderViewModel) {

    const payload = new EntrancePayload<{
      assignmentID: number,
      caseID: string,
      model: EmailSenderViewModel
    }>({
      assignmentID: this.currentAssignment.AssignmentID,
      caseID: this.case.CaseID,
      model: $event,
    });

    payload.success = () => {
      this.modalService.dismissAll();

      this.currentAssignment = null;

    }

    this.store.dispatch(new fromCaseActions.CaseCreatorActions.sendCaseAssignmentAction(payload));

  }

  btnSend($event: EmailSenderViewModel) {


    switch (this.currentAssignmentProcessType) {
      case this.caseAssignmentProcessType.Invoice:
        this.btnSendInvoice($event);
        break;
      case this.caseAssignmentProcessType.Assignment:
        this.btnSendAssignment($event);
        break;
    }

  }

  btnPreviewInvoiceModal(model: CaseAssignmentOverviewViewModel) {


    this.caseService
      .getAssignmentIvoice(model.InvoiceIdentityID)
      .subscribe(x => {
        this.currentInvocie = x.element;
        //開遮罩
        this.store.dispatch(new fromRootAction.LoadingActions.visibleLoadingAction())

        //取得反應單PDF
        const work$ = this.caseService
          .getPreviewComplaintInvoice(this.currentInvocie.NodeID, this.currentInvocie.CaseID, this.currentInvocie.InvoiceID);

        //處理回應
        work$.pipe(
          finalize(() => {
            this.store.dispatch(new fromRootAction.LoadingActions.invisibleLoadingAction());
            return;
          })).
          subscribe(x => {
            this.reportSrc = x
          }, (error) => {
            this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_COMMON.GET_INOVICE_ERROR'))))
          },
            () => {
              this.modalService.open(this.excelViewerRef, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl', backdrop: 'static' });
            });
      })

  }

  btnAssignmentSenderModal(model: CaseAssignmentOverviewViewModel) {


    this.caseService
      .getAssignment(model.CaseID, model.AssignmentID)
      .subscribe(x => {
        this.currentAssignmentProcessType = this.caseAssignmentProcessType.Assignment;
        this.currentAssignment = x.element;
        this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl', backdrop: 'static' });
        this.senderList = [];
        this.sender = this.generatorSenderPayload();
        this.getCurrentUser().subscribe(user => {
          if (user.Email)
            this.senderList = this.senderList.concat({ Email: user.Email, UserName: user.Name });
        })
      })


  }

  btnInvoiceCancel(model: CaseAssignmentOverviewViewModel) {

    if (this.case.CaseType == CaseType.Finished) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage( '案件已經結案，不可取消',
                                                                                                        false,
                                                                                                        null,
                                                                                                        null,
                                                                                                        PtcSwalType.warning
                                                                                                      )));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否取消反應單?',
      () => {
        const payload = new EntrancePayload<{
          InvoiceIdentityID: number,
        }>
          ({
            InvoiceIdentityID: model.InvoiceIdentityID,
          });
        payload.success = (message: string) => {
          this.store.dispatch(successAlertAction(message));
          this.getList();
        };
        payload.failed = (message: string) => {
          this.store.dispatch(failedAlertAction(message));
        };

        this.store.dispatch(new fromCaseActions.CaseCreatorActions.CancelInvoiceAction(payload));
      }
    )));
  }


  generatorSenderPayload(): EmailSenderViewModel {
    const data = new EmailSenderViewModel();

    const { content, users } = this.getDifferentInfo();

    data.Content = content;
    data.Sender = new ConcatableUserViewModel();
    data.Receiver = this.generatorConcatUserPayload(this.emailReceiveType.Recipient, users);
    data.Cc = this.generatorConcatUserPayload(this.emailReceiveType.CC, users);
    data.Bcc = this.generatorConcatUserPayload(this.emailReceiveType.BCC, users);
    //data.Attachments.push = [...(this.assignment.Files || [])]
    return data;
  }


  generatorConcatUserPayload(emailRecvType: EmailReceiveType, users: ConcatableUserViewModel[]) {
    return users.filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
      x.NotificationRemark === emailRecvType.toString());
  }

  getDifferentInfo() {
    let content = this.currentAssignmentProcessType == this.caseAssignmentProcessType.Assignment ?
      this.currentAssignment.Content :
      this.currentInvocie.Content;

    let users = this.currentAssignmentProcessType == this.caseAssignmentProcessType.Assignment ?
      this.currentAssignment.CaseAssignmentConcatUsers :
      this.currentInvocie.Users;

    return { content, users }
  }

  btnAddCaseRemind(row: CaseAssignmentOverviewViewModel) {
    let url = `${environment.webHostPrefix}/pages/master/case-remind-detail`.toCustomerUrl({
      actionType: this.actionType.Add,
      caseID: row.CaseID,
      assignmentID: row.AssignmentID
    })

    window.open(url, '_blank');
  }

  btnReadCaseRemind(row: CaseAssignmentOverviewViewModel) {
    let modal = this.modalService.open(RemindModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    let instance = <RemindModalComponent>modal.componentInstance;

    instance.caseRemindIDs = row.CaseRemindIDs;

  }

  initializeTable() {
    this.columns = [
      {//項次
        text: this.translateService.instant('CASE_COMMON.TABLE.INDEX'),
        name: 'Index'
      },
      {//案件提醒
        text: this.translateService.instant('CASE_COMMON.TABLE.REMIND'),
        name: 'remind',
        customer: true
      },
      {//序號
        text: this.translateService.instant('CASE_COMMON.CASE_ASSIGNMENT_ID'),
        name: 'Identifier',
      },
      {//通知時間
        text: this.translateService.instant('CASE_COMMON.NOTIFY_DATETIME'),
        name: 'NoticeDateTime',
      },
      {//歷程模式
        text: this.translateService.instant('CASE_COMMON.ASSIGNMENT_MODE'),
        name: 'CaseAssignmentProcessTypeName',
      },
      {//處理單位
        text: this.translateService.instant('CASE_COMMON.TABLE.PROCESS_UNIT'),
        name: 'ComplaintNodeNames',
      },
      {//狀態
        text: this.translateService.instant('CASE_COMMON.TYPE'),
        name: 'AssignmentTypeName',
      },
      {//銷案內容
        text: this.translateService.instant('CASE_COMMON.CLOSE_CONTENT'),
        name: 'FinishedContent',
        customer: true
      },
      {//銷案單位
        text: this.translateService.instant('CASE_COMMON.TABLE.CLOSE_UNIT'),
        name: 'FinishNodeName',
      },
      {//銷案人
        text: this.translateService.instant('CASE_COMMON.TABLE.CLOSE_USER'),
        name: 'FinishedUserName',
      },
      {//銷案時間
        text: this.translateService.instant('CASE_COMMON.TABLE.CLOSE_TIME'),
        name: 'FinishedDateTime',
      },
      {//通知內容
        text: this.translateService.instant('CASE_COMMON.NOTIFY_CONTENT'),
        name: 'NotifyContent',
        customer: true
      },
      {//操作
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'operator',
        customer: true
      },
    ];

  }

  isShowCaseAssignmentFinish($event) {
    return this.uiActionType != this.actionType.Read &&
      +$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment &&
      +$event.AssignmentType != this.caseAssignmentType.Finished
  }

  isShowCaseAssignmentReject($event) {
    return this.uiActionType != this.actionType.Read &&
      +$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment &&
      +$event.AssignmentType == this.caseAssignmentType.Processed
  }

  isShowIvoiceSender($event) {
    return this.uiActionType != this.actionType.Read &&
      +$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Invoice &&
      (+$event.AssignmentType != this.caseAssignmentInvoiceType.Sended && +$event.AssignmentType != this.caseAssignmentInvoiceType.Cancel)
  }
  isShowIvoicePreview($event) {
    return this.uiActionType != this.actionType.Read &&
      +$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Invoice &&
      +$event.AssignmentType != this.caseAssignmentInvoiceType.Sended
  }
  isShowInvoiceCancel($event) {
    return this.uiActionType != this.actionType.Read &&
      (+$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Invoice &&
        +$event.AssignmentType != this.caseAssignmentInvoiceType.Cancel)
  }

  isShowAssignmentSender($event) {
    return this.uiActionType != this.actionType.Read &&
      +$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment &&
      +$event.NotificationBehaviors.some(x => x == this.notificationType.Email)
  }

  isAssignment($event) {
    return +$event.CaseAssignmentProcessType == this.caseAssignmentProcessType.Assignment
  }
}
