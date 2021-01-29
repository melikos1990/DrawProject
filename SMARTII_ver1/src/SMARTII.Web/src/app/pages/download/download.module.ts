import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { AsoDailyReportComponent } from './aso-daily-report/aso-daily-report.component';
import { DailyReportComponent } from './daily-report/daily-report.component';
import { DownloadComponent } from './download.component';
import { DownloadRoutingModule } from './download-routing.module';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import * as downloadEffect from './store/effects';
import * as downloadReducer from './store/reducers';

const COMPONENT = [
  DownloadComponent,
  DailyReportComponent,
  AsoDailyReportComponent
];

@NgModule({
  declarations: [...COMPONENT],
  imports: [
    CommonModule,
    SharedModule,
    ThemeModule,
    DownloadRoutingModule,
    EffectsModule.forFeature(downloadEffect.effects),
    StoreModule.forFeature('search', downloadReducer.reducer),
  ]
})
export class DownloadModule { }
