import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleComponent } from './role/list/role.component';
import { RoleDetailComponent } from './role/detail/role-detail.component';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { SystemParameterComponent } from './system-parameter/list/system-parameter.component';
import { SystemComponent } from './system.component';
import { SystemParameterDetailComponent } from './system-parameter/detail/system-parameter-detail.component';
import { SystemLogComponent } from './system-log/list/system-log.component';
import { PersonalChangePasswordComponent } from './personal-change-password/personal-change-password.component';
import { UserComponent } from './user/list/user.component';
import { UserDetailComponent } from './user/detail/user-detail.component';
import { UserParameterComponent } from './user-parameter/user-parameter.component'


const routes: Routes = [{
  path: '',
  component: SystemComponent,
  children: [
    {
      path: 'role',
      component: RoleComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.ROLE',
        feature: 'RoleComponent'
      },
    },
    {
      path: 'role-detail',
      component: RoleDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.ROLE',
        feature: 'RoleComponent'
      },
    },
    {
      path: 'user',
      component: UserComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.USER',
        feature: 'UserComponent'
      },
    },
    {
      path: 'user-detail',
      component: UserDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.USER',
        feature: 'UserComponent'
      },
    },
    {
      path: 'user-parameter',
      component: UserParameterComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.USER_PARAMETER',
        feature: 'UserParameterComponent'
      },
    },
    {
      path: 'system-parameter',
      component: SystemParameterComponent,
      data: {
        breadcrumb: 'APPLICATION.FEATURE.SYSTEM_PARAMETER',
        feature: 'SystemParameterComponent'
      },
      canActivate: [AuthGuardService],
    },
    {
      path: 'system-parameter-detail',
      component: SystemParameterDetailComponent,
      data: {
        breadcrumb: 'APPLICATION.FEATURE.SYSTEM_PARAMETER_DETAIL',
        feature: 'SystemParameterComponent'
      },
      canActivate: [AuthGuardService],
    },
    {
      path: 'system-log',
      component: SystemLogComponent,
      data: {
        breadcrumb: 'APPLICATION.FEATURE.SYSTEM_LOG',
        feature: 'SystemLogComponent'
      },
      canActivate: [AuthGuardService],
    },
    {
      path: 'personal-change-password',
      component: PersonalChangePasswordComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.PERAONAL_CHANGE_PASSWORD',
        feature: 'PersonalChangePasswordComponent'
      },
    },
  ]
}];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class SystemRoutingModule { }
