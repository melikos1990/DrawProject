
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './search.component';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { CallCenterCaseSearchComponent } from './call-center-case-search/list/call-center-case-search.component';
import { HeaderQurterStoreCaseSearchComponent } from './headerqurter-store-case-search/list/headerqurter-store-case-search.component';
import { HeaderQurterBUCaseSearchComponent } from './headerqurter-bu-case-search/list/headerqurter-bu-case-search.component';
import { CallCenterAssignmentSearchComponent } from './call-center-assignment-search/list/call-center-assignment-search.component';
import { HeaderqurterStoreAssignmentSearchComponent } from './headerqurter-store-assignment-search/list/headerqurter-store-assignment-search.component';
import { HeaderqurterBUAssignmentSearchComponent } from './headerqurter-bu-assignment-search/list/headerqurter-bu-assignment-search.component';
import { VendorAssignmentSearchComponent } from './vendor-assignment-search/list/vendor-assignment-search.component';
import { KmComponent } from './km/km.component';
import { KmDetailComponent } from './km/detail/km-detail.component';

const routes: Routes = [{
    path: '',
    component: SearchComponent,
    children: [
        {
            path: 'call-center-case-search',
            component: CallCenterCaseSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.CALLCENTER_CASE_SEARCH',
                feature: 'CallCenterCaseSearchComponent'
            },
        },
        {
            path: 'headerqurter-store-case-search',
            component: HeaderQurterStoreCaseSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.HEADQUARTERS_STORE_CASE_SEARCH',
                feature: 'HeaderQurterNodeStoreCaseSearchComponent'
            },
        },
        {
            path: 'headerqurter-bu-case-search',
            component: HeaderQurterBUCaseSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.HEADQUARTERS_BU_CASE_SEARCH',
                feature: 'HeaderQurterNodeBUCaseSearchComponent'
            },
        },
        {
            path: 'call-center-assignment-search',
            component: CallCenterAssignmentSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.CALLCENTER_STORE_ASSIGNMENT_SEARCH',
                feature: 'CallCenterAssignmentSearchComponent'
            },
        },
        {
            path: 'headerqurter-store-assignment-search',
            component: HeaderqurterStoreAssignmentSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.HEADQUARTERS_STORE_ASSIGNMENT_SEARCH',
                feature: 'HeaderqurterStoreAssignmentSearchComponent'
            },
        },
        {
            path: 'headerqurter-bu-assignment-search',
            component: HeaderqurterBUAssignmentSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.HEADQUARTERS_BU_ASSIGNMENT_SEARCH',
                feature: 'HeaderqurterBUAssignmentSearchComponent'
            },
        },
        {
            path: 'vendor-assignment-search',
            component: VendorAssignmentSearchComponent,
            canActivate: [AuthGuardService],
            data: {
                breadcrumb: 'APPLICATION.FEATURE.VENDOR_ASSIGNMENT_SEARCH',
                feature: 'VendorAssignmentSearchComponent'
            },
        },
        {
            path: 'km',
            component: KmComponent,
            canActivate: [AuthGuardService],
            data: {
              breadcrumb: 'APPLICATION.FEATURE.KM',
              feature: 'KmComponent'
            },
      
          },
          {
            path: 'km-detail',
            component: KmDetailComponent,
            canActivate: [AuthGuardService],
            data: {
              breadcrumb: 'APPLICATION.FEATURE.KM',
              feature: 'KmComponent'
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
export class SearchRoutingModule{
    constructor(){}
}
