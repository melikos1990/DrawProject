import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from '../@theme/theme.module';
import { LoadingComponent } from './loading/loading.component';
import { StoreModule } from '@ngrx/store';
import { AlertComponent } from "./alert/alert.component";
import { SharedModule } from '../shared/shared.module';
import { CustomerAlertComponent } from './customer-alert/customer-alert.component';
import { NotificationLayoutComponent } from './notification/notification-layout/notification-layout.component';
import { NotifyGroupComponent } from './notification/components/notify-group/notify-group.component';
import { NotifyBillboardComponent } from './notification/components/notify-billboard/notify-billboard.component';
import { NotifyCaseAssignComponent } from './notification/components/notify-case-assign/notify-case-assign.component';
import { NotifyCaseFinishedComponent } from './notification/components/notify-case-finished/notify-case-finished.component';
import { NotifyCaseModifyComponent } from './notification/components/notify-case-modify/notify-case-modify.component';
import { NotifyMailAdoptComponent } from './notification/components/notify-mail-adopt/notify-mail-adopt.component';
import { NotifyCaseRemindComponent } from './notification/components/notify-case-remind/notify-case-remind.component';
import { NotifyMailIncomingComponent } from './notification/components/notify-mail-incoming/notify-mail-incoming.component';
import { NotifyPpclifeEffectiveSummaryComponent } from './notification/components/notify-ppclife-effective-summary/notify-ppclife-effective-summary.component';

const COMPONENTS = [LoadingComponent, AlertComponent, CustomerAlertComponent]

const NOTIFY_COMPOENNTS = [
  NotificationLayoutComponent,
  NotifyGroupComponent,
  NotifyBillboardComponent,
  NotifyCaseAssignComponent,
  NotifyCaseFinishedComponent,
  NotifyCaseModifyComponent,
  NotifyMailAdoptComponent,
  NotifyCaseRemindComponent,
  NotifyMailIncomingComponent,
  NotifyPpclifeEffectiveSummaryComponent
]

@NgModule({
  declarations: [...COMPONENTS, ...NOTIFY_COMPOENNTS],
  imports: [
    CommonModule,
    ThemeModule,
    StoreModule,
    SharedModule
  ],
  exports: [
    ...COMPONENTS,
    ...NOTIFY_COMPOENNTS
  ],
  entryComponents: [...NOTIFY_COMPOENNTS]

})
export class LayoutModule { }
