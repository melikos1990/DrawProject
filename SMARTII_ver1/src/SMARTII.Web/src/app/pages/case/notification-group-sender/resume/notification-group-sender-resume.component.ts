import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { NotificationGroupSenderResumeSearchViewModel, NotificationGroupSenderListViewModel } from 'src/app/model/master.model';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../../case/store/reducers';
import { takeUntil, skip } from 'rxjs/operators';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';

export const PREFIX = 'NotificationGroupSenderComponent';

@Component({
  selector: 'app-notification-group-sender-resume',
  templateUrl: './notification-group-sender-resume.component.html',
  styleUrls: ['./notification-group-sender-resume.component.scss']
})
export class NotificationGroupSenderResumeComponent extends BaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  model = new NotificationGroupSenderListViewModel();

  searchTerm = new NotificationGroupSenderResumeSearchViewModel();
  isEnable: boolean = false;
  constructor(
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.subscription();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  btnRender() {
    this.isEnable = true;
    setTimeout(() => {
      this.table.render();
    }, 0);
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.searchTerm; 
    ($event.direction == undefined || $event.direction == null) && ($event.direction = "desc");
  }

  initializeTable() {

    this.ajax.url = 'Master/NotificationGroupSender/GetResumeList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.RESUME_CREATE_DATETIME'),
        name: 'CreateDateTime',
        disabled: false,
        order: 'CREATE_DATETIME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.RESUME_BU_NAME'),
        name: 'BUName',
        disabled: false,
        order: 'NODE_NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.RESUME_GROUP_NAME'),
        name: 'GroupName',
        disabled: false,
        order: 'NOTIFICATION_GROUP.NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.RESUME_TARGET'),
        name: 'Targets',
        disabled: true,
        customer: true
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.RESUME_CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.RESUME_RESULT_TYPE'),
        name: 'Type',
        disabled: false,
        order: 'TYPE'
      },
      {
        text: this.translateService.instant('COMMON.BTN_DOWNLOAD'),
        name: 'FilePath',
        disabled: true,
        customer: true
      },
    ];


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
        this.searchTerm.NodeID = x.NodeID;
        this.btnRender();
      });
  }

}
