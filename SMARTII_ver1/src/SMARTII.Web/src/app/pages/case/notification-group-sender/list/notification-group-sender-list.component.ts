import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions } from 'ptc-server-table';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from "../../../case/store/reducers";
import * as fromNotificationGroupSenderActions from "../../store/actions/notification-group-sender.actions";
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { skip, takeUntil } from 'rxjs/operators';
import { interval } from 'rxjs';

export const REFRESH_TIME = 60000;
export const PREFIX = 'NotificationGroupSenderComponent';

@Component({
  selector: 'app-notification-group-sender-list',
  templateUrl: './notification-group-sender-list.component.html',
  styleUrls: ['./notification-group-sender-list.component.scss']
})
export class NotificationGroupSenderListComponent extends BaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  constructor(
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.subscription();
    this.store.dispatch(new fromNotificationGroupSenderActions.TriggerGetArrivedList());
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  render() {
    setTimeout(() => {
      this.table.render();
    }, 0);
  }

  onRowSelect($event) {
    this.store.dispatch(new fromNotificationGroupSenderActions.selectChangeAction($event));
  }


  subscription() {
    this.store.select(x => x.case.notificationGroupSender.triggerFetch)
      .pipe(
        skip(1),
        takeUntil(this.destroy$)
      ).subscribe(() => this.render());

    interval(REFRESH_TIME).pipe(
      takeUntil(this.destroy$)
    ).subscribe(() => this.render());
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Master/NotificationGroupSender/GetArrivedList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.LIST_BU_NAME'),
        name: 'NodeName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.LIST_GROUP_NAME'),
        name: 'GroupName',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.LIST_TYPE'),
        name: 'CalcMode',
        disabled: false,
        order: 'CALC_MODE'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.LIST_TARGET'),
        name: 'Targets',
        disabled: true,
        customer: true
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.LIST_ACTUAL_COPUNT'),
        name: 'ActualCount',
        disabled: false,
        order: 'ACTUAL_COUNT'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP_SENDER.LIST_ALERT_COUNT'),
        name: 'ExpectCount',
        disabled: false,
        order: 'ALERT_COUNT'
      },
    ];


  }


}
