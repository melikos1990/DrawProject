import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { Store } from '@ngrx/store';
import * as fromNotificationGroupSenderActions from '../store/actions/notification-group-sender.actions';

export const PREFIX = 'NotificationGroupSenderComponent';

@Component({
  selector: 'app-notification-group-sender',
  templateUrl: './notification-group-sender.component.html',
})
export class NotificationGroupSenderComponent extends BaseComponent implements OnInit {

  constructor(
    public store: Store<any>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    // this.store.dispatch(new fromNotificationGroupSenderActions.ClearNotification());
  }

}
