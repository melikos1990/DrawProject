import { Component, OnInit, Injector, Input, ViewChild, TemplateRef, Output, EventEmitter } from '@angular/core';
import { EmailSenderViewModel, EmailReceiveType } from 'src/app/model/shared.model';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ConcatableUserViewModel, OrganizationDataRangeSearchViewModel, OrganizationType } from 'src/app/model/organization.model';
import { SenderUserCreatorComponent } from './sender-user-creator.component';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseTemplateSelectorComponent } from '../../modal/case-template-selector/case-template-selector.component';
import { CaseTemplateListViewModel, CaseTemplateSearchViewModel, CaseTemplateParseResultViewModel } from 'src/app/model/master.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { AspnetJsonResult } from 'src/app/model/common.model';
import { AllNodeTreeUserSelectorForCcComponent } from '../../modal/tree-user/all-node-tree-user-selector-for-cc/all-node-tree-user-selector-for-cc.component';

@Component({
  selector: 'app-mail-sender',
  templateUrl: './mail-sender.component.html',
  styleUrls: ['./mail-sender.component.scss']
})
export class MailSenderComponent extends FormBaseComponent implements OnInit {

  public form: FormGroup;
  public options = {};

  type: EmailReceiveType;
  selectorRef: NgbModalRef;
  creatorRef: NgbModalRef;
  templeteRef: NgbModalRef;
  isSelectMode: boolean = false;

  operatorInfo: {
    input: any,
    startPos: number,
    endPos: number
  };

  @ViewChild('selector') selector: TemplateRef<any>;
  treemodel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();

  @Input() noDataText: string = '無資料';
  @Input() buID: number;
  @Input() model: EmailSenderViewModel = new EmailSenderViewModel();
  @Output() onSend: EventEmitter<EmailSenderViewModel> = new EventEmitter();
  @Output() onBack: EventEmitter<any> = new EventEmitter();
  @Input() senderList: any[] = [];
  @Input() caseID: string = null;
  @Input() inviceID: string = null;
  @Input() assignmentID: number = null;
  @Input() isAddCaseAttachment: boolean = false;
  constructor(
    public store: Store<any>,
    public modalService: NgbModal,
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeForm();
    this.initFile();
    this.getBUSenderEmail();
    if(this.isAddCaseAttachment)
    {
      this.model.IsAddCaseAttachment = true;
    }
  }
  initFile() {

    let _option: any = {};

    if (!!this.model.Attachments && this.model.Attachments.length > 0) {

      const files = this.model.Attachments || [];
      const previews = files.map(path => path.name);
      const previewConfigs = files.map(file => {
        return {
          caption: file.name,
          type: file.type,
          size: file.size
        }
      });

      _option.initialPreview = previews;
      _option.initialPreviewConfig = previewConfigs;

    }


    _option = {
      ..._option,
      preferIconicPreview: true,
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        showZoom: false,
      }
    }

    this.options = { ..._option };

  }


  /**
   * 開啟選擇人員彈出視窗 , 並記錄目前所在UI位置(TYPE)
   * 於其他component 已實作
   * @param $event
   */
  btnSelectorModal($event) {
    this.type = $event;
    this.openSelectorModal();
  }

  /**
   * 開啟建立人員彈出視窗, 並記錄目前所在UI位置(TYPE)
   * @param $event
   */
  btnCreatorModal($event) {
    this.type = $event;
    this.openCreateModal();
  }

  /**
   * 當新增人員時
   * @param user
   */
  onCreate(user: ConcatableUserViewModel) {

    this.creatorRef.dismiss();
    this.push([user]);
  }


  onFocusout() {
    // 防止 onSelect 併發
    setTimeout(() => {
      this.isSelectMode = false;
    }, 350);

  }
  onfocus() {
    setTimeout(() => {
      this.isSelectMode = true;
    }, 350);

  }
  /**
   * 當選擇人員時
   * @param instance
   */
  onSelected(instance: AllNodeTreeUserSelectorForCcComponent) {
    const users: any[] = instance.getValue();

    if (!users || users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇使用者")));
      return;
    }

    this.selectorRef.dismiss();
    this.push(instance.getValue());
  }

  onSelectedSender($event) {

    this.model.Sender.Email = $event.Email
    this.model.Sender.UserName = $event.OfficialDisplayName || $event.UserName; // 有多種類型物件, 官網來信, 使用者
    this.isSelectMode = false
  }



  onSelectCaseTemplate(caseTemplates: CaseTemplateListViewModel) {

    if (!caseTemplates) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇範本")));
      return;
    }

    var payload = { CaseTemplateID: caseTemplates.ID, CaseID: this.caseID, InvoicID: this.inviceID, assignmentID: this.assignmentID }

    this.http.post('Common/Master/ParseCaseTemplate/', null, payload)
      .subscribe((res: AspnetJsonResult<CaseTemplateParseResultViewModel>) => {

        if (!res.isSuccess) {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(res.message)));
          return;
        }
        this.model.Content = (this.model.Content) ? !!(this.operatorInfo) ? this.insertTag(res.element.Content, this.operatorInfo) : `${this.model.Content}${String.prototype.newLine}${res.element.Content}` : res.element.Content;
        this.model.Title += (res.element.EmailTitle + String.prototype.newLine);

        this.templeteRef.dismiss();
      })

  }

  /**
   * 根據不同UI位置 , 進行刪除
   * @param type
   * @param $event
   * @param idx
   */
  onCancelUser(type, $event, idx) {
    switch (type) {
      case this.emailReceiveType.Recipient:
        this.model.Receiver.splice(idx, 1);
        break;
      case this.emailReceiveType.CC:
        this.model.Cc.splice(idx, 1);
        break;
      case this.emailReceiveType.BCC:
        this.model.Bcc.splice(idx, 1);
        break;
    }
  }



  /**
   * 開啟選擇人員彈出視窗
   */
  openSelectorModal() {
    this.selectorRef = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<AllNodeTreeUserSelectorForCcComponent>this.selectorRef.componentInstance);
  }

  onSelecteChange($event) {

    if (!$event) return;

    this.treemodel = new OrganizationDataRangeSearchViewModel();
    this.treemodel.IsEnabled = true;
    switch ($event.toString()) {
      case OrganizationType.CallCenter.toString():
        this.treemodel.IsSelf = true;
        this.treemodel.Goal = OrganizationType.CallCenter;
        break;

      case OrganizationType.HeaderQuarter.toString():
        this.treemodel.NodeID = this.buID;
        this.treemodel.IsStretch = true;
        this.treemodel.Goal = OrganizationType.HeaderQuarter;
        break;

      case OrganizationType.Vendor.toString():
        this.treemodel.NodeID = this.buID;
        this.treemodel.Goal = OrganizationType.Vendor;
        break;

      default:
        break;
    }
  }

  /**
   * 開啟新增人員彈出視窗
   */
  openCreateModal() {
    this.creatorRef = this.modalService.open(SenderUserCreatorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<SenderUserCreatorComponent>this.creatorRef.componentInstance);
    instance.onBtnBack = () => {
      this.creatorRef.dismiss();
    };
    instance.onBtnAdd = (user) => {
      this.onCreate(user);
    };
  }

  /**
   * 範本主檔
   */
  openCaseTemplateModal() {
    this.templeteRef = this.modalService.open(CaseTemplateSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseTemplateSelectorComponent>this.templeteRef.componentInstance);
    const searchTerm = new CaseTemplateSearchViewModel();
    searchTerm.BuID = this.buID;
    searchTerm.ClassificKey = this.caseTemplateType.Email;
    instance.model = searchTerm;
    instance.btnAdd = this.onSelectCaseTemplate.bind(this);

  }

  /**
   * 根據傳入之物件 , 產生 ConcatableUser.cs
   * 傳入的有可能物件形狀不同 , 因此用 any
   */
  generatorPayloads(users: any[]): ConcatableUserViewModel[] {
    return users.map((user: any | ConcatableUserViewModel) => {
      const data = new ConcatableUserViewModel();
      data.BUID = user.BUID;
      data.BUName = user.BUName;
      data.UserID = user.UserID;
      data.UserName = user.UserName;
      data.Email = user.Email;
      data.JobName = user.JobName;
      data.JobID = user.JobID;
      data.Mobile = user.Mobile;
      data.NodeID = user.NodeID;
      data.NodeName = user.NodeName;
      data.Address = user.Address;
      data.Email = user.Email;
      data.Telephone = user.Telephone;
      data.OrganizationType = user.OrganizationType;
      data.UnitType = this.unitType.Organization;
      data.Mobile = user.Mobile;
      data.NotificationBehavior = this.notificationType.Email.toString();
      data.NotificationRemark = this.type.toString();
      return data;
    });
  }

  /**
   * 準備新增人員
   * @param users
   */
  push(users: any[] | ConcatableUserViewModel[]) {

    const payloads = this.generatorPayloads(users);
    this.refill(payloads);
  }

  /**
   * 依照新傳入的使用者 , 取得聯集
   * @param existUsers
   * @param newUsers
   */
  union(existUsers: ConcatableUserViewModel[], newUsers: ConcatableUserViewModel[]): ConcatableUserViewModel[] {

    const result: ConcatableUserViewModel[] = [];

    newUsers.forEach(x => {
      result.push(x);
    });

    existUsers.forEach(x => {
      if (result.filter(g => g.Email === x.Email).length === 0) {
        result.push(x);
      }
    });

    return result;

  }

  /**
   *
   * @param payloads 回填畫面資訊
   */
  refill(payloads: ConcatableUserViewModel[]) {
    switch (this.type) {
      case this.emailReceiveType.Recipient:
        this.model.Receiver = this.union(this.model.Receiver, payloads);
        break;
      case this.emailReceiveType.CC:
        this.model.Cc = this.union(this.model.Cc, payloads);
        break;
      case this.emailReceiveType.BCC:
        this.model.Bcc = this.union(this.model.Bcc, payloads);
        break;
    }
  }


  /**
   * 該頁面按下寄送時
   */
  btnSend() {
    if (!this.model.Sender.Email) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('EMAIL.NEED_SENDER'))));
      return;
    }
    if (this.model.Receiver.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('EMAIL.NEED_RECIVE'))));
      return;
    }

    if (this.vaildMsgUser()) return;

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.onSend.emit(this.model);
  }

  /**
   * 該頁面按下返回時
   */
  btnBack() {
    this.onBack.emit();
  }

  btnClosepop() {
    this.selectorRef.dismiss();
  }

  /**
   * 初始化form
   */
  initializeForm() {
    this.form = new FormGroup({
      SenderEmail: new FormControl(this.model.Sender.Email, [
        Validators.required,
        Validators.email
      ]),
      Content: new FormControl(this.model.Content, [
        Validators.required,
      ]),
      Title: new FormControl(this.model.Title, [
        Validators.required,
      ]),
    });
  }

  /**
   * 驗證收件者、CC、BCC
   */
  vaildMsgUser(): boolean {

    let re: boolean = false;
    re = this.model.Receiver.some(x => {
      return !x.Email ? true : false;
    });
    if (re) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請確認收件者信箱")));
      return true;
    }

    let cc: boolean = false;
    if (!!this.model.Cc && this.model.Cc.length > 0) {
      cc = this.model.Cc.some(x => {
        return !x.Email ? true : false;
      });
    }
    if (cc) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請確認副本信箱")));
      return true;
    }

    let bcc: boolean = false;
    if (!!this.model.Bcc && this.model.Bcc.length > 0) {
      bcc = this.model.Bcc.some(x => {
        if (!x.Email) {
          return true;
        }
      });
    }
    if (bcc) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請確認密件副本信箱")));
      return true;
    }

    return false;
  }


  getBUSenderEmail() {

    this.http.get<any>('/Common/Notification/GetOfficialWebMailList', { buID: this.buID })
      .subscribe(emails => {
        if (emails && emails.length > 0) {
          this.senderList = this.senderList.concat(emails);
          this.model.Sender.Email = emails[0].Email;
          this.model.Sender.UserName = emails[0].OfficialDisplayName;
        }
      });
  }

  clickInput = (el) => {
    this.operatorInfo = this.resetInputInfo(el);
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
