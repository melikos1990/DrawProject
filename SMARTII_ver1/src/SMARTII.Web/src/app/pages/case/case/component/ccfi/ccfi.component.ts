import { Component, OnInit, Injector, Input, TemplateRef, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseViewModel } from 'src/app/model/case.model';
import { CaseTemplateSelectorComponent } from 'src/app/shared/component/modal/case-template-selector/case-template-selector.component';
import { CaseTemplateSearchViewModel, CaseTemplateListViewModel, CaseTemplateParseViewModel } from 'src/app/model/master.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConcatableUserViewModel } from 'src/app/model/organization.model';
import { EmailSenderViewModel, EmailReceiveType } from 'src/app/model/shared.model';
import { AcuModalComponent } from '../../modal/acu-modal/acu-modal.component';

import { State as fromCaseReducers } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import * as fromCaseActions from '../../../store/actions';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { CaseService } from 'src/app/shared/service/case.service';
import * as fromRootActions from 'src/app/store/actions';
import { caseTemplateKey } from 'src/global';
import { bind } from '@angular/core/src/render3';
import { Observable } from 'rxjs/internal/Observable';
import { HttpService } from 'src/app/shared/service/http.service';


@Component({
  selector: 'app-ccfi',
  templateUrl: './ccfi.component.html',
  styleUrls: ['./ccfi.component.scss']
})
export class CcfiComponent extends FormBaseComponent implements OnInit {


  @ViewChild('mailsender')
  mailsenderRef: TemplateRef<any>;

  senderList: any[] = [];
  sender: EmailSenderViewModel = new EmailSenderViewModel();

  emailContent: string = '';
  emailSubject: string = '';
  columns = [];
  loading: boolean = false;

  mailoperatorInfo: {
    input: any,
    startPos: number,
    endPos: number
  };

  finishoperatorInfo: {
    input: any,
    startPos: number,
    endPos: number
  };

  @Input() uiActionType: ActionType;
  @Input() users: ConcatableUserViewModel[] = [];
  @Input() fileOptions = {};
  @Input() form: FormGroup;
  @Input() model: CaseViewModel;
  @Input() focusId: string;

  constructor(
    public http: HttpService,
    private caseService: CaseService,
    public store: Store<fromCaseReducers>,
    public modalService: NgbModal,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initializeForm();
    this.initializeTable();
  }

  btnCaseEmailTemplateModal() {
    const ref = this.modalService.open(CaseTemplateSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<CaseTemplateSelectorComponent>ref.componentInstance);
    const model = new CaseTemplateSearchViewModel();
    model.BuID = this.model.NodeID;
    model.ClassificKey = caseTemplateKey.EMAIL;
    instance.model = model;

    instance.btnAdd = (data: CaseTemplateListViewModel) => {

      this.loading = true;
      this.caseService
        .parseTemplate({ CaseTemplateID: data.ID, CaseID: this.model.CaseID } as CaseTemplateParseViewModel)
        .subscribe(x => {
          if (x.isSuccess) {
            this.emailContent = (this.emailContent) ? !!(this.mailoperatorInfo) ? this.insertTag(x.element.Content, this.mailoperatorInfo) : `${this.emailContent}${String.prototype.newLine}${x.element.Content}` : x.element.Content;
            this.emailSubject = x.element.EmailTitle;
          } else {
            this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
          }
          this.loading = false;
        })
      ref.dismiss();
    }
  }

  btnDefaultCaseTemplateModal() {
    const data = new CaseTemplateParseViewModel();
    data.NodeID = this.model.NodeID;
    data.CaseID = this.model.CaseID;
    data.IsDefault = true;
    data.ClassificKey = caseTemplateKey.EMAIL;
    this.loading = true;
    this.caseService
      .parseTemplate(data)
      .subscribe(x => {
        if (x.isSuccess) {
          this.emailContent = (this.emailContent) ? !!(this.mailoperatorInfo) ? this.insertTag(x.element.Content, this.mailoperatorInfo) : `${this.emailContent}${String.prototype.newLine}${x.element.Content}` : x.element.Content;
          this.emailSubject = x.element.EmailTitle;
        } else {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
        }
        this.loading = false;
      })

  }
  btnCaseFinishTemplateModal() {
    const ref = this.modalService.open(CaseTemplateSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<CaseTemplateSelectorComponent>ref.componentInstance);
    const model = new CaseTemplateSearchViewModel();
    model.BuID = this.model.NodeID;
    model.ClassificKey = caseTemplateKey.CASE_FINISH;
    instance.model = model;

    instance.btnAdd = (data: CaseTemplateListViewModel) => {

      this.loading = true;
      this.caseService
        .parseTemplateUseExist(data.Content, this.model.CaseID)
        .subscribe(x => {
          if (x.isSuccess) {
            this.model.FinishContent = (this.model.FinishContent) ? !!(this.finishoperatorInfo) ? this.insertTag(x.element, this.finishoperatorInfo) : `${this.model.FinishContent}${String.prototype.newLine}${x.element}` : x.element;
          } else {
            this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
          }
          this.loading = false;

        })
      ref.dismiss();
    }
  }

  initializeForm() {
    this.form.addControl('FinishContent', new FormControl(this.model.FinishContent, [
      Validators.required,
      Validators.maxLength(4000),
    ]));
  }

  btnDeleteUser(user: ConcatableUserViewModel) {
    const index = this.users.findIndex(x => x.key === user.key);
    this.users.splice(index, 1);
    this.users = [...this.users];
  }

  btnAddUserModal() {
    const ref = this.modalService.open(AcuModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' })
    const instance = (<AcuModalComponent>ref.componentInstance);
    instance.btnAddUser = (user: ConcatableUserViewModel) => {
      this.users = [user, ...this.users]
      ref.dismiss();
    }
  }

  btnSenderModal() {
    this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    this.sender = this.generatorSenderPayload();
    this.senderList = [];

    this.getCurrentUser().subscribe(user => {
      if (user.Email)
        this.senderList = this.senderList.concat({ Email: user.Email, UserName: user.Name });
    });

  }

  generatorSenderPayload(): EmailSenderViewModel {
    const data = new EmailSenderViewModel();

    data.Sender = new ConcatableUserViewModel();
    data.Receiver = this.generatorConcatUserPayload(this.emailReceiveType.Recipient);
    data.Cc = this.generatorConcatUserPayload(this.emailReceiveType.CC);
    data.Bcc = this.generatorConcatUserPayload(this.emailReceiveType.BCC);
    data.Content = this.emailContent;
    data.Title = this.emailSubject;

    return data;
  }

  generatorConcatUserPayload(emailRecvType: EmailReceiveType) {
    return this.users.filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
      x.NotificationRemark === emailRecvType.toString());
  }

  btnSend($event: EmailSenderViewModel) {

    const data = new EntrancePayload<{ caseID: string, model: EmailSenderViewModel }>({
      caseID: this.model.CaseID,
      model: $event
    });
    data.success = () => {
      // this.store.dispatch(new fromCaseActions.CaseCreatorActions.loadCaseAction(this.model.CaseID));
      this.modalService.dismissAll();
    }
    this.store.dispatch(new fromCaseActions.CaseCreatorActions.caseFinishedReplyMailAction(data))
  }



  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CUSTOMER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.EMAIL_RECEIVER_TYPE'),
        name: 'NotificationRemark',
        customer: true,
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.EMAIL'),
        name: 'Email',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
      }
    ];

  }

  mailclickInput = (el) => {
    this.mailoperatorInfo = this.resetInputInfo(el);
  }

  finishclickInput = (el) => {
    this.finishoperatorInfo = this.resetInputInfo(el);
  }

  insertTag(tag: string, el: any) {
    if (!el) return;

    let { input, startPos } = el;

    let oldValue: string = input.value;

    let newValue = oldValue.slice(0, startPos) + tag + oldValue.slice(startPos);

    return newValue;

  }

  private resetInputInfo = (el) => ({
    input: el,
    startPos: el.selectionStart,
    endPos: el.selectionEnd
  })



}
