import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { CaseComponent } from './case.component';
import { CaseAssignmentDetailComponent } from './case-assignment/detail/case-assignment-detail.component';
import { C1Component } from './case/component/c1/c1.component';
import { OfficialEmailAdoptComponent } from './official-email-adopt/list/official-email-adopt.component';
import { PpclifeEffectiveSummaryComponent } from './ppclife-effective-summary/ppclife-effective-summary.component';
import { NotificationGroupSenderComponent } from './notification-group-sender/notification-group-sender.component';

const routes: Routes = [{
    path: '',
    component: CaseComponent,
    children: [
        {
            path: 'case-create',
            component: C1Component,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.CASE_CREATOR',
                feature: 'C1Component'
            },
        },
        {
            path: 'case-assignment-detail',
            component: CaseAssignmentDetailComponent,
            //canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.CASE_ASSIGNMENT',
                feature: 'CaseAssignmentDetailComponent'
            },
        },
        {
          path: 'official-email-adopt',
          component: OfficialEmailAdoptComponent,
          canActivate: [AuthGuardService],
          data: {
            breadcrumb: 'APPLICATION.FEATURE.OFFICIAL_EMAIL_ADOPT',
            feature: 'OfficialEmailAdoptComponent'
          },
        },
        {
            path: 'notification-group-sender',
            component: NotificationGroupSenderComponent,
            canActivate: [AuthGuardService],
            data: {
              breadcrumb: 'APPLICATION.FEATURE.NOTIFICATION_GROUP_SENDER',
              feature: 'NotificationGroupSenderComponent'
            },
          },
        {
            path: 'ppclife-effective-summary',
            component: PpclifeEffectiveSummaryComponent,
            canActivate: [AuthGuardService],
            data: {
              breadcrumb: 'APPLICATION.FEATURE.PPCLIFE_EFFECTIVE_SUMMARY',
              feature: 'PpclifeEffectiveSummaryComponent'
            },
          },
    ]
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [
        RouterModule
    ]
})
export class CaseRoutingModule { }
