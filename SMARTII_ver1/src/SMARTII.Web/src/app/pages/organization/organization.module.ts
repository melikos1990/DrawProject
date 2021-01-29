import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ThemeModule } from 'src/app/@theme/theme.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrganizationRoutingModule } from './organization-routing.module';
import { OrganizationComponent } from './organization.component';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import * as fromOrganizationReducer from './store/reducers';
import * as fromOrganizationEffect from './store/effects';

import { NodeDefinitionComponent } from './node-definition/list/node-definition.component';
import { NodeDefinitionDetailComponent } from './node-definition/detail/node-definition-detail.component';
import { BasicInformationComponent as NodeDefinitionBaseInfoComponent } from './node-definition/form/basic-information/basic-information.component';
import { JobInformationComponent } from './node-definition/form/job-information/job-information.component';

import { HeaderquarterNodeTreeComponent } from './headquarter-node/tree/headerquarter-node-tree.component';
import { HeaderquarterNodeComponent } from './headquarter-node/headerquarter-node.component';
import { HeaderquarterNodeCreateComponent } from './headquarter-node/modal/headerquarter-node-create/headerquarter-node-create.component';
import { BasicInformationComponent as HeaderQuarterBaseInfoComponent } from './headquarter-node/form/basic-information/basic-information.component';
import { JobUserInformationComponent } from './headquarter-node/form/job-user-information/job-user-information.component';
import { JobTableComponent } from './headquarter-node/form/job-user-information/job-table/job-table.component';
import { UserTableComponent } from './headquarter-node/form/job-user-information/user-table/user-table.component';

import { CallCenterNodeComponent } from './callcenter-node/callcenter-node.component';
import { CallCenterNodeTreeComponent } from './callcenter-node/tree/callcenter-node-tree.component';
import { CallCenterNodeCreateComponent } from './callcenter-node/modal/callcenter-node-create/callcenter-node-create.component';
import { BasicInformationComponent as CallCenterBaseInfoComponent } from './callcenter-node/form/basic-information/basic-information.component';
import { JobUserInformationComponent as CallCenterJobUserInfoComponent } from './callcenter-node/form/job-user-information/job-user-information.component';
import { JobTableComponent as CallCenterJobTblComponent } from './callcenter-node/form/job-user-information/job-table/job-table.component';
import { UserTableComponent as CallCenterUserTblComponent } from './callcenter-node/form/job-user-information/user-table/user-table.component';


import { VendorNodeComponent } from './vendor-node/vender-node.component';
import { VenderNodeTreeComponent } from './vendor-node/tree/vender-node-tree.component';
import { VendorNodeCreateComponent } from './vendor-node/modal/vendor-node-create/vendor-node-create.component';
import { BasicInformationComponent as VendorBaseInfoComponent } from './vendor-node/form/basic-information/basic-information.component';
import { JobUserInformationComponent as VendorJobUserInfoComponent } from './vendor-node/form/job-user-information/job-user-information.component';
import { JobTableComponent as VendorJobTblComponent } from './vendor-node/form/job-user-information/job-table/job-table.component';
import { UserTableComponent as VendorUserTblComponent } from './vendor-node/form/job-user-information/user-table/user-table.component';

import { EnterpriseComponent } from './enterprise/list/enterprise.component';
import { EnterpriseDetailComponent } from './enterprise/detail/enterprise-detail.component';

const COMPONENTS = [
  OrganizationComponent,
  NodeDefinitionComponent,
  NodeDefinitionDetailComponent,
  NodeDefinitionBaseInfoComponent,
  JobInformationComponent,
  HeaderquarterNodeComponent,
  HeaderquarterNodeTreeComponent,
  HeaderQuarterBaseInfoComponent,
  JobUserInformationComponent,
  JobTableComponent,
  UserTableComponent,
  CallCenterNodeComponent,
  CallCenterNodeTreeComponent,
  CallCenterBaseInfoComponent,
  CallCenterJobUserInfoComponent,
  CallCenterJobTblComponent,
  CallCenterUserTblComponent,
  VendorNodeComponent,
  VenderNodeTreeComponent,
  VendorJobUserInfoComponent,
  VendorJobTblComponent,
  VendorUserTblComponent,
  VendorBaseInfoComponent,
  EnterpriseComponent,
  EnterpriseDetailComponent
];

const ENTRY_COMPONENTS = [
  HeaderquarterNodeCreateComponent,
  CallCenterNodeCreateComponent,
  VendorNodeCreateComponent
];

@NgModule({
  imports: [
    ThemeModule,
    SharedModule,
    CommonModule,
    OrganizationRoutingModule,
    EffectsModule.forFeature(fromOrganizationEffect.effects),
    StoreModule.forFeature('organization', fromOrganizationReducer.reducer),
  ],
  declarations: [
    ...COMPONENTS,
    ...ENTRY_COMPONENTS,
  ],
  entryComponents: [...ENTRY_COMPONENTS]
})
export class OrganizationModule { }


