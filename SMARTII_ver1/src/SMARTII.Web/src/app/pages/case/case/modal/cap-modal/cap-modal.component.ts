import { Component, OnInit, Input, Injector, ViewChild, TemplateRef } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CaseAssignmentComplaintInvoiceViewModel } from 'src/app/model/case.model';

import * as formCaseAction from '../../../store/actions/case-creator.actions';
import { State as fromCaseReducers } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import { EmailSenderViewModel, EmailReceiveType } from 'src/app/model/shared.model';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ConcatableUserViewModel } from 'src/app/model/organization.model'; 

@Component({
  selector: 'app-cap-modal',
  templateUrl: './cap-modal.component.html',
  styleUrls: ['./cap-modal.component.scss']
})
export class CapModalComponent extends FormBaseComponent implements OnInit {

  resendSuccess: any;
  editSuccess: any;
  @Input() uiActionType: ActionType;
  @Input() model: CaseAssignmentComplaintInvoiceViewModel


  @ViewChild('mailsender')
  mailsenderRef: TemplateRef<any>;

  senderList: any[] = [];
  sender: EmailSenderViewModel = new EmailSenderViewModel();

  constructor(
    public modalService: NgbModal,
    public activeModal: NgbActiveModal,
    public store: Store<fromCaseReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }


  confirm() {

    var data = new EntrancePayload<CaseAssignmentComplaintInvoiceViewModel>(this.model);
    data.success = (model: CaseAssignmentComplaintInvoiceViewModel) => {
      this.activeModal.close();
      this.editSuccess && this.editSuccess(this.model);
    }

    this.store.dispatch(new formCaseAction.editCaseAssignmentInvoiceAction(data));
  }

  btnSenderModal() {
    this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    this.sender = this.generatorSenderPayload();
    this.senderList = [];
    this.getCurrentUser().subscribe(user => {
      if (user.Email)
        this.senderList = this.senderList.concat({Email: user.Email, UserName: user.Name});
    })
  }
  generatorSenderPayload(): EmailSenderViewModel {
    const data = new EmailSenderViewModel();

    data.Sender = new ConcatableUserViewModel();
    data.Receiver = this.generatorConcatUserPayload(this.emailReceiveType.Recipient);
    data.Cc = this.generatorConcatUserPayload(this.emailReceiveType.CC);
    data.Bcc = this.generatorConcatUserPayload(this.emailReceiveType.BCC);
    return data;
  }

  generatorConcatUserPayload(emailRecvType: EmailReceiveType) {
    return this.model
      .Users
      .filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
        x.NotificationRemark === emailRecvType.toString());
  }

  btnResend() {

    const data = new EntrancePayload<{
      identityID: number,
      model: EmailSenderViewModel
    }>({
      identityID: this.model.ID,
      model: this.sender
    })

    data.success = () => {
      this.resendSuccess && this.resendSuccess(this.model);
    }

    this.store.dispatch(new formCaseAction.resendCaseAssignmentInvoiceAction(data))
  }


}
