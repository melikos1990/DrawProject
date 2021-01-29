import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { State as fromCaseReducers } from '../../store/reducers';
import { Store } from '@ngrx/store';
import * as fromRootActions from 'src/app/store/actions';
import * as fromPPCLifeEffectiveActions from '../../store/actions/ppclife-effective.actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { PtcServerTableRequest, PtcAjaxOptions } from 'ptc-server-table';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { EmailSenderViewModel, EmailReceiveType } from 'src/app/model/shared.model';
import { PPCLifeEffectiveUserListViewModel, PPCLifeEffectiveListViewModel, PPCLifeEffectiveSenderExecuteViewModel, PPCLifeEffectiveCaseListViewModel } from 'src/app/model/master.model';
import { AllNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';
import { CaseAssignGroupUserModalComponent } from 'src/app/shared/component/modal/case-assign-group-user-modal/case-assign-group-user-modal.component';
import { EntrancePayload, AspnetJsonResult } from 'src/app/model/common.model';
import { takeUntil, skip } from 'rxjs/operators';
import { ConcatableUserViewModel, HeaderQuarterNodeDetailViewModel, BusinesssUnitParameters } from 'src/app/model/organization.model';
import { CaseAssignGroupDetailViewModel, CaseAssignGroupUserListViewModel, CaseAssignGroupListViewModel } from 'src/app/model/master.model';
import { Observable } from 'rxjs';
import { Guid } from 'guid-typescript';
import { PPCLIFEKeyPair } from 'src/global';

export const PREFIX = 'PpclifeEffectiveSummaryComponent';

@Component({
  selector: 'app-ppclife-effective-users',
  templateUrl: './ppclife-effective-users.component.html',
  styleUrls: ['./ppclife-effective-users.component.scss']
})
export class PpclifeEffectiveUsersComponent extends FormBaseComponent implements OnInit {

  constructor(
    public http: HttpService,
    public modalService: NgbModal,
    public store: Store<fromCaseReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }
  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  @ViewChild('selector') selector: TemplateRef<any>;

  @ViewChild('table')
  table: ServerTableComponent;

  @ViewChild('caseTable')
  caseTable: ServerTableComponent;

  @ViewChild('mailsender')
  mailsenderRef: TemplateRef<any>;

  popupUserRef: NgbModalRef;
  popupGroupRef: NgbModalRef;

  isUserEnabled?: boolean = true; //人員清單是否篩選啟用人員
  senderList: any[] = [];
  sender: EmailSenderViewModel = new EmailSenderViewModel();

  columns: any[] = [];
  caseColums: any[] = [];

  emailData: PPCLifeEffectiveUserListViewModel[] = [];
  caseData: PPCLifeEffectiveCaseListViewModel[] = [];
  model = new PPCLifeEffectiveListViewModel();
  sysParam: BusinesssUnitParameters;

  ngOnInit() {
    this.subscription();
    this.initializeLocalTable();
  }

  getBuModel(nodeKey: string): Observable<BusinesssUnitParameters> {
    return this.http.get('Common/Organization/GetBUParametersByNodeKey', { NodeKey: nodeKey });
  }

  btnDeleteUser($event: PPCLifeEffectiveUserListViewModel) {
    const index = this.findIndex($event.UserID);
    this.emailData.splice(index, 1);
    this.emailData = [...this.emailData]; // clone ref to refresh view....
  }
  findIndex = (userID) => this.emailData.findIndex(x => x.UserID === userID);

  /**
   * 通知事件
   */
  @loggerMethod()
  btnSend() {

    this.modalService.open(this.mailsenderRef, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    this.sender = this.generatorSenderPayload();
    this.senderList = [];

    this.getCurrentUser().subscribe(user => {
      if (user.Email)
        this.senderList = this.senderList.concat({Email: user.Email, UserName: user.Name});
    })

  }
  /**
   * 不通知事件
   */
  @loggerMethod()
  btnNoSend() {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否不通知?',
      () => {
        const payload = new EntrancePayload<number>(this.model.EffectiveID);
        payload.success = this.successHandler.bind(this);
        payload.failed = this.failedHandler.bind(this);
        this.store.dispatch(new fromPPCLifeEffectiveActions.noSendAction(payload));
      }
    )));
  }

  /**
   * 無視事件
   */
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnNoIgnore() {
    const filterCase = this.caseTable.getSelectItem()
    if (filterCase.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先勾選案件")));
      return;
    }
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否無視案件?',
      () => {
        //勾選的案件集合
        const payload = new EntrancePayload<PPCLifeEffectiveCaseListViewModel[]>(filterCase);
        payload.success = this.successHandler.bind(this);
        payload.failed = this.failedHandler.bind(this);
        this.store.dispatch(new fromPPCLifeEffectiveActions.ignoreAction(payload));
      }
    )));
  }

  successHandler() {
    this.store.dispatch(new fromPPCLifeEffectiveActions.saveSelectChangeAction(this.model));
    this.store.dispatch(new fromPPCLifeEffectiveActions.refreshEffectiveSummary());
    this.initializeUIPayload();
    this.modalService.dismissAll();
  }

  failedHandler() {
    this.store.dispatch(new fromPPCLifeEffectiveActions.saveSelectChangeAction(this.model));
    this.store.dispatch(new fromPPCLifeEffectiveActions.refreshEffectiveSummary());

  }

  generatorConcatUserPayload(emailRecvType: EmailReceiveType) {
    return this.emailData.filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
      x.NotificationRemark === emailRecvType.toString());
  }

  /**
   * 傳入Modal的資訊
   */
  generatorSenderPayload(): EmailSenderViewModel {
    const data = new EmailSenderViewModel();

    data.Sender = new ConcatableUserViewModel();
    data.Receiver = this.generatorConcatUserPayload(this.emailReceiveType.Recipient);
    data.Cc = this.generatorConcatUserPayload(this.emailReceiveType.CC);
    data.Bcc = this.generatorConcatUserPayload(this.emailReceiveType.BCC);

    return data;
  }

  /**
   * popup 按鈕事件
   * @param $event 
   */
  onSend($event: EmailSenderViewModel) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否通知?',
      () => {
        const payload = new EntrancePayload<PPCLifeEffectiveSenderExecuteViewModel>();
        payload.data = new PPCLifeEffectiveSenderExecuteViewModel($event, this.model.EffectiveID);
        payload.success = this.successHandler.bind(this);
        payload.failed = this.failedHandler.bind(this);
        this.store.dispatch(new fromPPCLifeEffectiveActions.sendAction(payload));
      }
    )));

  }

  /**
   * 取得達標資訊後 查詢案件清單
   */
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  refreshCaseList($event: PPCLifeEffectiveListViewModel) {
    this.store.dispatch(new fromPPCLifeEffectiveActions.getCaseListAction($event.EffectiveID));
  }

  subscription() {
    //取得傳入的大量叫修資訊
    this.store
      .select(x => x.case.ppclifeEffectiveSender.selected)
      .pipe(
        takeUntil(this.destroy$),
        skip(1)
      )
      .subscribe((x: PPCLifeEffectiveListViewModel) => {
        console.log("統藥大量叫修內容", x)
        this.model = x;
        if (this.model !== null) {
          this.refreshCaseList(this.model);
        }
      });

    //監聽caselist 從store取得資訊 填入案件清單Data中
    this.store
      .select(x => x.case.ppclifeEffectiveSender.caselist)
      .pipe(
        takeUntil(this.destroy$),
        skip(1)
      )
      .subscribe(x => {
        this.caseData = x;
      });

    //監聽clearSelected = true 清空Table
    this.store
      .select(x => x.case.ppclifeEffectiveSender.refreshList)
      .subscribe(x => {
        console.log("清空TABLE", x);
        this.caseData = [];

      });

    //因為是統藥專用 所以NodeKey寫死005
    this.getBuModel(PPCLIFEKeyPair.NodeKey).subscribe(sysParam => {
      this.sysParam = sysParam;
    });
  }


  initializeLocalTable() {
    this.columns = [
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.USER_BU_NAME'),
        name: 'BUName',
        disabled: false,
        order: 'BU_NAME'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.USER_JOB_NAME'),
        name: 'JobName',
        disabled: false,
        order: 'JOB_NAME'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.USER_NAME'),
        name: 'UserName',
        disabled: false,
        order: 'USER_NAME'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.USER_NOTIFICATION_RECEIVER_TYPE'),
        name: 'NotificationRemark',
        disabled: false,
        customer: true,
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
      }

    ];
    this.caseColums = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.CASE_ID'),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.CASE_CONTENT'),
        name: 'CaseContent',
        disabled: false,
        order: 'CASE_CONTENT',
      },
    ];
  }

  initializeUIPayload() {
    this.emailData = [];
    this.model = new PPCLifeEffectiveListViewModel();
  }


  /*---start 組織選人---*/
  btnSelectUserModal($event) {
    this.popupUserRef = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

  }

  onUserSelected(instance: AllNodeTreeUserSelectorComponent) {
    const users: any[] = instance.getValue();

    if (!users || users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇使用者")));
      return;
    }
    const payloads = this.generatorPayloads(instance.getValue());
    console.log("信件內容 => ", payloads);
    this.emailData = this.union(this.emailData, payloads);
    this.popupUserRef.dismiss();
  }

  /*---end 組織選人---*/

  /*---start 群組拷貝---*/
  btnCopyGroupModal($event) {
    this.popupGroupRef = this.modalService.open(CaseAssignGroupUserModalComponent,
      { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

    this.popupGroupRef.componentInstance.btnAddUser = this.onGroupSelected.bind(this);
    this.popupGroupRef.componentInstance.nodeID = this.sysParam.BuID;
    this.popupGroupRef.componentInstance.type = this.caseAssignGroupType.PPCRepair;
  }

  onGroupSelected(data: CaseAssignGroupListViewModel[]) {

    const payloads = this.generatorPayloads(data);

    this.emailData = this.union(this.emailData, payloads);
    this.popupGroupRef.dismiss();
  }
  /*---end 群組拷貝---*/

  getUserIDIfMock(user) {
    return user.UserID || Guid.create().toString();
  }

  generatorPayloads(users: any[]): CaseAssignGroupUserListViewModel[] {
    return users.map((user: any | CaseAssignGroupUserListViewModel) => {
      const data = new CaseAssignGroupUserListViewModel();
      data.BUID = user.BUID;
      data.BUName = user.BUName;
      data.UserID = this.getUserIDIfMock(user);
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
      data.OrganizationTypeName = user.OrganizationTypeName;
      data.UnitType = this.unitType.Organization;
      data.Mobile = user.Mobile;
      data.NotificationBehavior = this.notificationType.Email.toString();
      data.NotificationRemark = !(user.NotificationRemark) ? user.NotificationRemark = "0" : user.NotificationRemark;
      //data.GroupID = this.model.GroupID;
      return data;
    });
  }

  union(existUsers: CaseAssignGroupUserListViewModel[], newUsers: CaseAssignGroupUserListViewModel[]): CaseAssignGroupUserListViewModel[] {

    const result: CaseAssignGroupUserListViewModel[] = [];

    newUsers.forEach(x => {
      result.push(x);
    });

    existUsers.forEach(x => {
      if (result.filter(g => g.UserID === x.UserID).length === 0) {
        result.push(x);
      }
    });

    return result;

  }
}
