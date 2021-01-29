import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';
import { PagesComponent } from './pages.component';

const routes: Routes = [
    {
      path: '',
      component : PagesComponent,
      children : [
        {
          path : 'home',
          loadChildren : './home/home.module#HomeModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.HOME'
          }
        },
        {
          path : 'master',
          loadChildren : './master/master.module#MasterModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.MASTER_PARENT'
          }
        },
        {
          path : 'system',
          loadChildren : './system/system.module#SystemModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.SYSTEM_PARENT'
          }
        },
        {
          path : 'organization',
          loadChildren : './organization/organization.module#OrganizationModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.ORGANIZATION_PARENT'
          }
        },
        {
          path : 'develop',
          loadChildren : './develop/develop.module#DevelopModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.DEVELOP_PARENT'
          }
        },
        {
          path : 'substitute',
          loadChildren : './substitute/substitute.module#SubstituteModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.SUBSTITUTE_PARENT'
          }
        },
        {
          path : 'case',
          loadChildren : './case/case.module#CaseModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.CASE_PARENT',
            preload: true
          }
        },
        {
          path : 'search',
          loadChildren : './search/search.module#SearchModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.SEARCH_PARENT'
          }
        },
        {
          path : 'download',
          loadChildren : './download/download.module#DownloadModule',
          data: {
            breadcrumb: 'APPLICATION.FEATURE.DOWNLOAD_PARENT'
          }
        },
      ]
    }
  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule { }
