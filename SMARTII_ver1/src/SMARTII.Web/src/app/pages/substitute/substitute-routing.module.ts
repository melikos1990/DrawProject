import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { SubstituteComponent } from './substitute.component';
import { CaseApplyComponent } from './case-apply/list/case-apply.component';
import { CaseNoticeComponent } from './case-notice/list/case-notice.component';


const routes: Routes = [{
  path: '',
  component: SubstituteComponent,
  children: [
    {
      path: 'case-apply',
      component: CaseApplyComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_APPLY',
        feature: 'CaseApplyComponent'
      },
    },
    {
      path: 'case-notice',
      component: CaseNoticeComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_NOTICE',
        feature: 'CaseNoticeComponent'
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
export class SubstituteRoutingModule { }
