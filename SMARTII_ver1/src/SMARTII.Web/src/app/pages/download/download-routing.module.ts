import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { AsoDailyReportComponent } from './aso-daily-report/aso-daily-report.component';
import { DailyReportComponent } from './daily-report/daily-report.component';
import { DownloadComponent } from './download.component';


const routes: Routes = [{
  path: '',
  component: DownloadComponent,
  children: [
    {
      path: 'aso-daily-report',
      component: AsoDailyReportComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.ASO_DAILY_REPORT',
        feature: 'AsoDailyReportComponent'
      },
    },
    {
      path: 'daily-report',
      component: DailyReportComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.DAILY_REPORT',
        feature: 'DailyReportComponent'
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
export class DownloadRoutingModule { }
