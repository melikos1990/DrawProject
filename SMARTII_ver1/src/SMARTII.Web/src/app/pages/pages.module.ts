import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesComponent } from './pages.component';
import { PagesRoutingModule } from './pages-routing.module';
import { ThemeModule } from '../@theme/theme.module';
import { SharedModule } from '../shared/shared.module';
import { MasterModule } from './master/master.module';



@NgModule({
  declarations: [PagesComponent],
  imports: [
    SharedModule,
    PagesRoutingModule,
    ThemeModule,
    CommonModule,
    // MasterModule
  ],

})
export class PagesModule { }
