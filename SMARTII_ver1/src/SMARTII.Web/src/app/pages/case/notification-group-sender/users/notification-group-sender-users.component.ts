import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRootActions from 'src/app/store/actions';
import { State as fromMasterReducer } from "../../../case/store/reducers";
import { takeUntil, filter, skip } from 'rxjs/operators';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcServerTableRequest } from 'ptc-server-table';
import * as fromNotificationGroupSenderActions from '../../store/actions/notification-group-sender.actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { NotificationGroupSenderListViewModel, NotificationGroupUserListViewModel, NotificationGroupSenderExecuteViewModel } from 'src/app/model/master.model';
import { FormBaseComponent } from '../../../base/form-base.component';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { EmailSenderViewModel, NotificationType, EmailReceiveType } from 'src/app/model/shared.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpService } from 'src/app/shared/service/http.service';
import { Observable } from 'rxjs';
import { ConcatableUserViewModel } from 'src/app/model/organization.model';


export const PREFIX = 'NotificationGroupSenderComponent';

@Component({
  selector: 'app-notification-group-sender-users',
  templateUrl: './notification-group-sender-users.component.html',
  styleUrls: ['./notification-group-sender-users.component.scss']
})
export class NotificationGroupSenderUsersComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  @ViewChild('mailsender')
  mailsenderRef: TemplateRef<any>;

  senderList: any[] = [];
  sender: EmailSenderViewModel = new EmailSenderViewModel();

  columns: any[] = [];

  data: NotificationGroupUserListViewModel[] = [];
  model = new NotificationGroupSenderListViewModel();

  constructor(
    public http: HttpService,
    public modalService: NgbModal,
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initializeTable();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
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

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnNoSend() {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否不通知?',
      () => {
        const payload = new EntrancePayload<number>(this.model.GroupID);
        payload.success = this.noSendSuccessHandler.bind(this);
        payload.failed = this.noSendFailedHandler.bind(this);
        this.store.dispatch(new fromNotificationGroupSenderActions.noSendAction(payload));
      }
    )));
  }

  noSendSuccessHandler() {
    this.store.dispatch(new fromNotificationGroupSenderActions.TriggerGetArrivedList());
    this.store.dispatch(new fromNotificationGroupSenderActions.selectChangeAction(this.model));
    this.initializeUIPayload();
  }

  noSendFailedHandler() {
    this.store.dispatch(new fromNotificationGroupSenderActions.selectChangeAction(this.model));
  }


  generatorConcatUserPayload(emailRecvType: EmailReceiveType) {
    return this.data.filter(x => x.NotificationBehavior === this.notificationType.Email.toString() &&
      x.NotificationRemark === emailRecvType.toString());
  }

  generatorSenderPayload(): EmailSenderViewModel {
    const data = new EmailSenderViewModel();

    data.Sender = new ConcatableUserViewModel();
    data.Receiver = this.generatorConcatUserPayload(this.emailReceiveType.Recipient);
    data.Cc = this.generatorConcatUserPayload(this.emailReceiveType.CC);
    data.Bcc = this.generatorConcatUserPayload(this.emailReceiveType.BCC);

    return data;
  }

  onSend($event: EmailSenderViewModel) {
    
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否通知?',
      () => {
        const payload = new EntrancePayload<NotificationGroupSenderExecuteViewModel>();
        payload.data = new NotificationGroupSenderExecuteViewModel($event, this.model.GroupID);
        payload.success = this.sendSuccessHandler.bind(this);
        payload.failed = this.sendFailedHandler.bind(this);
        this.store.dispatch(new fromNotificationGroupSenderActions.sendAction(payload));
      }
    )));

  }



  sendSuccessHandler() {
    this.store.dispatch(new fromNotificationGroupSenderActions.TriggerGetArrivedList());
    this.store.dispatch(new fromNotificationGroupSenderActions.selectChangeAction(this.model));
    this.initializeUIPayload();
    this.modalService.dismissAll();
  }

  sendFailedHandler() {
    this.store.dispatch(new fromNotificationGroupSenderActions.selectChangeAction(this.model));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  render() {
    this.store.dispatch(new fromNotificationGroupSenderActions.getUserListAction(this.model.GroupID));
  }

  subscription() {

    this.store
      .select(x => x.case.notificationGroupSender.selected)
      .pipe(
        takeUntil(this.destroy$),
        skip(1)
      )
      .subscribe(x => {
        this.model = x;
        this.render();
      });
    this.store
      .select(x => x.case.notificationGroupSender.users)
      .pipe(
        takeUntil(this.destroy$),
        skip(1)
      )
      .subscribe(x => {
        this.data = x;
      });


  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.USER_BU_NAME'),
        name: 'BUName',
        disabled: false,
        order: 'BU_NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.USER_JOB_NAME'),
        name: 'JobName',
        disabled: false,
        order: 'JOB_NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.USER_NAME'),
        name: 'UserName',
        disabled: false,
        order: 'USER_NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.USER_NOTIFICATION_RECEIVER_TYPE'),
        name: 'NotificationRemarkName',
        disabled: false,
        order: 'NOTIFICATION_REMARK'
      },

    ];


  }

  initializeUIPayload() {
    this.data = [];
    this.model.GroupID = null;
    this.model.GroupName = null;
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model.GroupID;
  }

}
