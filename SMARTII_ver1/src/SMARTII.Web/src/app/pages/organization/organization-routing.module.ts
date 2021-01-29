import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { OrganizationComponent } from './organization.component';
import { NodeDefinitionComponent } from './node-definition/list/node-definition.component';
import { NodeDefinitionDetailComponent } from './node-definition/detail/node-definition-detail.component';
import { HeaderquarterNodeComponent } from './headquarter-node/headerquarter-node.component';
import { CallCenterNodeComponent } from './callcenter-node/callcenter-node.component';
import { VendorNodeComponent } from './vendor-node/vender-node.component';
import { EnterpriseComponent } from './enterprise/list/enterprise.component';
import { EnterpriseDetailComponent } from './enterprise/detail/enterprise-detail.component';

const routes: Routes = [{
  path: '',
  component: OrganizationComponent,
  children: [
    {
      path: 'node-definition',
      component: NodeDefinitionComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.NODE_DEFINITION',
        feature: 'NodeDefinitionComponent'
      },
    },
    {
      path: 'node-definition-detail',
      component: NodeDefinitionDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.NODE_DEFINITION',
        feature: 'NodeDefinitionComponent'
      }
    },
    {
      path: 'headerquarter-node',
      component: HeaderquarterNodeComponent,
      canActivate : [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.HEADERQUARTER_NODE',
        feature: 'HeaderquarterNodeComponent'
      },
    },
    {
      path: 'callcenter-node',
      component: CallCenterNodeComponent,
      canActivate : [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CALLCENTER_NODE',
        feature: 'CallCenterNodeComponent'
      },
    },{
      path: 'vendor-node',
      component: VendorNodeComponent,
      canActivate : [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.VENDOR_NODE',
        feature: 'VendorNodeComponent'
      },
    },
    {
      path: 'enterprise',
      component: EnterpriseComponent,
      canActivate : [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.ENTERPRISE',
        feature: 'EnterpriseComponent'
      },
    },
    {
        path: 'enterprise-detail',
        component: EnterpriseDetailComponent,
        canActivate : [AuthGuardService],
        data: {
          breadcrumb: 'APPLICATION.FEATURE.ENTERPRISE',
          feature: 'EnterpriseComponent'
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
export class OrganizationRoutingModule { }
