import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SystemRoutingModule } from './system-routing.module';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { SystemComponent } from './system.component';
import { SystemParameterComponent } from './system-parameter/list/system-parameter.component';
import { SystemParameterDetailComponent } from './system-parameter/detail/system-parameter-detail.component';
import { StoreModule } from '@ngrx/store';
import * as fromSystemReducer from './store/reducers';
import * as fromSystemEffect from './store/effects';
import { EffectsModule } from '@ngrx/effects';
import { SystemLogComponent } from './system-log/list/system-log.component';
import { PersonalChangePasswordComponent } from './personal-change-password/personal-change-password.component';
import { UserComponent } from './user/list/user.component';
import { UserDetailComponent } from './user/detail/user-detail.component';
import { AdPasswordComponent } from './user/modal/ad-password/ad-password.component';
import { UserParameterComponent } from './user-parameter/user-parameter.component';
import { RoleDetailComponent } from './role/detail/role-detail.component';
import { RoleComponent } from './role/list/role.component';

import { UserSelectorComponent as RoleUserSelector } from './role/modal/user-selector/user-selector.component';


const COMPONENTS = [
  RoleComponent,
  RoleDetailComponent,
  SystemComponent,
  SystemParameterComponent,
  SystemParameterDetailComponent,
  SystemLogComponent,
  PersonalChangePasswordComponent,
  UserComponent,
  UserDetailComponent,
  UserParameterComponent
];

const ENTRY_COMPONENTS = [
  RoleUserSelector,
  AdPasswordComponent
];

@NgModule({
  declarations: [...COMPONENTS,
    ...ENTRY_COMPONENTS,],
  imports: [
    ThemeModule,
    SharedModule,
    CommonModule,
    SystemRoutingModule,
    EffectsModule.forFeature(fromSystemEffect.effects),
    StoreModule.forFeature('system', fromSystemReducer.reducer),
  ]
})
export class SystemModule { }
