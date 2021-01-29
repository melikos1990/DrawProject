import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { DevelopComponent } from './develop.component';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { SummaryComponent } from './summary/summary.component';


const routes: Routes = [{
  path: '',
  component: DevelopComponent,
  children: [
    {
      path: 'dynamic-form',
      component: DynamicFormComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.USER',
        feature: 'DynamicFormComponent'
      },
    },
    {
      path: 'summary',
      component: SummaryComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: '測試區',
        feature: 'SummaryComponent'
      },
    }

  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [
    RouterModule
  ]
})
export class DevelopRoutingModule { }
