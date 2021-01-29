import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from '../../@theme/theme.module';
import { HomeRoutingModule } from './home-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from 'src/app/shared/shared.module';
// import * as fromHomeEffect from './store/effects';
// import * as fromHomeReducer from './store/reducers';

import { CcHomeComponent } from './home-page/cc-home/cc-home.component';
import { HeaderquaterHomeComponent } from './home-page/headerquater-home/headerquater-home.component';
import { VendorHomeComponent } from './home-page/vendor-home/vendor-home.component';
import { HomePageComponent } from './home-page/home-page.component';
import { HomeComponent } from './home.component';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';


const COMPONENTS = [
  HomeComponent,
  CcHomeComponent, 
  HeaderquaterHomeComponent, 
  VendorHomeComponent,
  HomePageComponent,
];

@NgModule({
  declarations: [...COMPONENTS],
  imports: [
    SharedModule,
    ThemeModule,
    HttpClientModule,
    HomeRoutingModule,
    CommonModule,
    // EffectsModule.forFeature(fromHomeEffect.effects),
    // StoreModule.forFeature('home', fromHomeReducer.reducer),
  ]
})
export class HomeModule { }
