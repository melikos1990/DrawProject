import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C1Component } from './case/component/c1/c1.component';
import { Cs1Component } from './case/component/cs1/cs1.component';
import { Cs2Component } from './case/component/cs2/cs2.component';
import { Cc1Component } from './case/component/cc1/cc1.component';
import { Cc2Component } from './case/component/cc2/cc2.component';
import { Ccm2Component } from './case/component/ccm2/ccm2.component';
import { Ccmi2Component } from './case/component/ccm2/ccmi2/ccmi2.component';
import { Ccml2Component } from './case/component/ccm2/ccml2/ccml2.component';
import { CccComponent } from './case/component/ccc/ccc.component';
import { CcfiComponent } from './case/component/ccfi/ccfi.component';
import { CcfhComponent } from './case/component/ccfh/ccfh.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { CaseRoutingModule } from './case-routing.module';
import { CaseComponent } from './case.component';
import { StoreModule } from '@ngrx/store';
import * as fromCaseReducer from './store/reducers';
import { EffectsModule } from '@ngrx/effects';
import * as fromCaseEffect from '../case/store/effects';
import { PcComponent } from './case/atom/pc/pc.component'
import { PcModalComponent } from './case/modal/pc-modal/pc-modal.component';
import { AcComponent } from './case/atom/ac/ac.component';
import { UsComponent } from './case/element/us/us.component';
import { UcoComponent } from './case/element/uco/uco.component';
import { UcoiComponent } from './case/element/uco/ucoi/ucoi.component';
import { UcotComponent } from './case/element/uco/ucot/ucot.component';
import { UcmComponent } from './case/element/ucm/ucm.component';
import { UcmiComponent } from './case/element/ucm/ucmi/ucmi.component';
import { UmctComponent } from './case/element/ucm/umct/umct.component';
import { UcoModalComponent } from './case/modal/uco-modal/uco-modal.component';
import { UcmModalComponent } from './case/modal/ucm-modal/ucm-modal.component';
import { Cc3Component } from './case/component/cc3/cc3.component';
import { Ccmi2MComponent } from './case/component/ccm2/ccmi2/ccmi2-m.component';
import { Ccmi2PComponent } from './case/component/ccm2/ccmi2/ccmi2-p.component';
import { Ccmi2NComponent } from './case/component/ccm2/ccmi2/ccmi2-n.component';
import { Ccmi2IComponent } from './case/component/ccm2/ccmi2/ccmi2-i.component';
import { AcuModalComponent } from './case/modal/acu-modal/acu-modal.component';
import { CaaComponent } from './case/element/caa/caa.component';
import { CanComponent } from './case/element/can/can.component';
import { CapComponent } from './case/element/cap/cap.component';
import { CamComponent } from './case/element/cam/cam.component';
import { CaaModalComponent } from './case/modal/caa-modal/caa-modal.component';
import { CanModalComponent } from './case/modal/can-modal/can-modal.component';
import { CapModalComponent } from './case/modal/cap-modal/cap-modal.component';

import { RejComponent } from './case/element/rej/rej.component';
import { CffComponent } from './case/element/cff/cff.component';
import { RejModalComponent } from './case/modal/rej-modal/rej-modal.component';
import { CffModalComponent } from './case/modal/cff-modal/cff-modal.component';
import { CamModalComponent } from './case/modal/cam-modal/cam-modal.component';
import { CcoComponent } from './case/component/cco/cco.component';
import { OfficialEmailAdoptComponent } from './official-email-adopt/list/official-email-adopt.component';
import { AutoAssignComponent } from './official-email-adopt/model/auto-assign/auto-assign.component';
import { BatchReplyComponent } from './official-email-adopt/model/batch-reply/batch-reply.component';
import { AdminAssignComponent } from './official-email-adopt/model/admin-assign/admin-assign.component';
import { CaseAssignmentDetailComponent } from './case-assignment/detail/case-assignment-detail.component';
import { NotificationGroupSenderComponent } from './notification-group-sender/notification-group-sender.component';
import { NotificationGroupSenderListComponent } from './notification-group-sender/list/notification-group-sender-list.component';
import { NotificationGroupSenderUsersComponent } from './notification-group-sender/users/notification-group-sender-users.component';
import { NotificationGroupSenderResumeComponent } from './notification-group-sender/resume/notification-group-sender-resume.component';
import { PpclifeEffectiveSummaryComponent } from './ppclife-effective-summary/ppclife-effective-summary.component';
import { PpclifeEffectiveListComponent } from './ppclife-effective-summary/list/ppclife-effective-list.component';
import { PpclifeEffectiveResumeComponent } from './ppclife-effective-summary/resume/ppclife-effective-resume.component';
import { PpclifeEffectiveUsersComponent } from './ppclife-effective-summary/users/ppclife-effective-users.component';
import { RemindModalComponent } from './case/modal/remind-modal/remind-modal.component';




const COMPONENT = [
  CaseComponent,
  C1Component,
  Cs1Component,
  Cs2Component,
  Cc1Component,
  Cc2Component,
  Cc3Component,
  Ccm2Component,
  Ccmi2Component,
  Ccml2Component,
  CccComponent,
  CcoComponent,
  CcfiComponent,
  CcfhComponent,
  PcComponent,
  PcModalComponent,
  AcComponent,
  UsComponent,
  UcoComponent,
  UcoiComponent,
  UcotComponent,
  UcmComponent,
  UcmiComponent,
  Ccmi2MComponent,
  Ccmi2PComponent,
  Ccmi2NComponent,
  Ccmi2IComponent,
  UmctComponent,
  CaaComponent,
  CanComponent,
  CapComponent,
  RejComponent,
  CffComponent,
  CaseAssignmentDetailComponent,
  OfficialEmailAdoptComponent,
  NotificationGroupSenderComponent,
  NotificationGroupSenderListComponent,
  NotificationGroupSenderUsersComponent,
  NotificationGroupSenderResumeComponent,
  OfficialEmailAdoptComponent,
  PpclifeEffectiveSummaryComponent,
  PpclifeEffectiveListComponent,
  PpclifeEffectiveResumeComponent,
  PpclifeEffectiveUsersComponent
]

const ENTRY_COMPONENT = [
  PcComponent,
  PcModalComponent,
  AcComponent,
  UsComponent,
  CaaComponent,
  CanComponent,
  CapComponent,
  CamComponent,
  RejComponent,
  CffComponent,
  UcoModalComponent,
  UcmModalComponent,
  AcuModalComponent,
  CaaModalComponent,
  CanModalComponent,
  CapModalComponent,
  RejModalComponent,
  CffModalComponent,
  CamModalComponent,
  AutoAssignComponent,
  AdminAssignComponent,
  BatchReplyComponent,
  RemindModalComponent
]

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ThemeModule,
    CaseRoutingModule,
    EffectsModule.forFeature(fromCaseEffect.effects),
    StoreModule.forFeature('case', fromCaseReducer.reducer),

  ],
  declarations: [...COMPONENT, ...ENTRY_COMPONENT],
  entryComponents: [...ENTRY_COMPONENT]
})
export class CaseModule { }
