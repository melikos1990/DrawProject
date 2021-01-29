import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Injector, ErrorHandler } from '@angular/core';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { ThemeModule } from './@theme/theme.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from './@core/core.module';
import { SharedModule } from './shared/shared.module';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SignalRModule } from 'ng2-signalr';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { reducers } from './store/reducers';
import { AuthorizeModule } from './auth/authorize.module';
import { LayoutModule } from './layout/layout.module';
import { effects } from './store/effects';
import { setAppInjector } from 'src/global';
import { config as signalRConfig } from './shared/signalR';
import { BreadcrumbsModule } from "ng6-breadcrumbs";
import { ErrorHandlerService } from './shared/service/error-handler.service';
import { InitializeService } from './shared/initialize';
import { PpclifeModule } from './business-unit/ppclife/ppclife.module';
import { CommonBuModule } from './business-unit/common-bu/common-bu.module';
import { AsoModule } from './business-unit/aso/aso.module';
import { _21centuryModule } from './business-unit/21century/21century.module';
import { ColdstoneModule } from './business-unit/coldstone/coldstone.module';
import { MisterdountsModule } from './business-unit/misterdounts/misterdounts.module';
import { EshopModule } from './business-unit/eshop/eshop.module';
import { IccModule } from './business-unit/icc/icc.module';
import { OpenPointModule } from './business-unit/openPoint/openpoint.module';
import { environment } from 'src/environments/environment';


declare global {
  interface Window { apiUrl: string; apHost: string; }
}

const BUSINESS_UNIT_MODULES = [
  PpclifeModule,
  CommonBuModule,
  AsoModule,
  _21centuryModule,
  ColdstoneModule,
  MisterdountsModule,
  EshopModule,
  IccModule,
  OpenPointModule,
]

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AuthorizeModule,
    AppRoutingModule,
    LayoutModule,
    CoreModule.forRoot(),
    ThemeModule.forRoot(),
    NgbModule.forRoot(),
    BreadcrumbsModule,
    EffectsModule.forRoot(effects),
    StoreModule.forRoot(reducers),
    SignalRModule.forRoot(signalRConfig),
    SharedModule,
    BUSINESS_UNIT_MODULES,
    StoreDevtoolsModule.instrument({
      maxAge: 10
    }),
  ],
  providers: [
    InitializeService,
    // { provide: APP_INITIALIZER, useFactory: initializeServiceFactory, deps: [InitializeService], multi: true },
    { provide: ErrorHandler, useClass: ErrorHandlerService }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(injector: Injector) {
    setAppInjector(injector);

    window.apiUrl = environment.apiUrl;
    window.apHost = environment.apHost;


  }
}
