import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEchartsModule } from 'ngx-echarts';
// import { NgxJsonViewerModule } from 'ngx-json-viewer';
import {
  NbActionsModule,
  NbCardModule,
  NbLayoutModule,
  NbMenuModule,
  NbRouteTabsetModule,
  NbSearchModule,
  NbSidebarModule,
  NbTabsetModule,
  NbThemeModule,
  NbUserModule,
  NbCheckboxModule,
  NbPopoverModule,
  NbContextMenuModule,
  NbProgressBarModule,
  // NbCalendarModule,
  // NbCalendarRangeModule,
  // NbStepperModule,
  NbButtonModule,
  NbInputModule,
  NbAccordionModule,
  NbDatepickerModule,
  NbDialogModule,
  NbWindowModule,
  NbListModule,
  NbToastrModule,
  NbAlertModule,
  NbSpinnerModule,
  NbRadioModule,
  NbSelectModule,
  // NbChatModule,
  NbTooltipModule,
  // NbCalendarKitModule,
  NbBadgeModule
} from '@nebular/theme';

import { NbSecurityModule } from '@nebular/security';

import {
  FooterComponent,
  HeaderComponent,
  // SearchInputComponent,
  ThemeSettingsComponent,
  SwitcherComponent,
  //LayoutDirectionSwitcherComponent,
  ThemeSwitcherComponent,
  TinyMCEComponent,
  // ThemeSwitcherListComponent,
  ToggleSettingsButtonComponent,
  ThemeSwitcherListComponent,
  SearchInputComponent,
} from './components';
import {
  CapitalizePipe,
  PluralPipe,
  RoundPipe,
  TimingPipe,
  NumberWithCommasPipe,
  EvaIconsPipe,
} from './pipes';
import {
  SampleLayoutComponent,
} from './layouts';
import { DEFAULT_THEME } from './styles/theme.default';
import { COSMIC_THEME } from './styles/theme.cosmic';
import { CORPORATE_THEME } from './styles/theme.corporate';
import { MatChipsModule, MatListModule, MatRadioModule, MatCheckboxModule, MatIconModule } from '@angular/material';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AceEditorModule } from 'ng2-ace-editor';
import { TranslateModule } from '@ngx-translate/core';
import { Ng2SmartTableModule } from "ng2-smart-table"
import { PdfViewerModule } from 'ng2-pdf-viewer';



const CDK_MODULES = [DragDropModule]

const BASE_MODULES = [CommonModule, FormsModule, ReactiveFormsModule];

const MD_MODULES = [
  MatChipsModule,
  MatListModule,
  MatRadioModule,
  MatCheckboxModule,
  MatIconModule
]

const NB_MODULES = [
  NgxEchartsModule,
  // NgxJsonViewerModule,
  NbCardModule,
  NbLayoutModule,
  NbTabsetModule,
  NbRouteTabsetModule,
  NbMenuModule,
  NbUserModule,
  NbActionsModule,
  // NbSearchModule,
  NbSidebarModule,
  NbCheckboxModule,
  NbPopoverModule,
  NbContextMenuModule,
  NgbModule,
  NbSecurityModule,
  NbProgressBarModule,
  // NbCalendarModule,
  // NbCalendarRangeModule,
  // NbStepperModule,
  NbButtonModule,
  NbListModule,
  // NbToastrModule,
  NbInputModule,
  NbAccordionModule,
  NbDatepickerModule,
  NbDialogModule,
  NbWindowModule,
  // NbAlertModule,
  NbSpinnerModule,
  NbRadioModule,
  NbSelectModule,
  // NbChatModule,
  NbTooltipModule,
  // NbCalendarKitModule,
  Ng2SmartTableModule,
  PdfViewerModule,
  NbBadgeModule
];

const COMPONENTS = [
  SwitcherComponent,
  //LayoutDirectionSwitcherComponent,
  ThemeSwitcherComponent,
  ThemeSwitcherListComponent,
  HeaderComponent,
  FooterComponent,
  SearchInputComponent,
  ThemeSettingsComponent,
  TinyMCEComponent,
  SampleLayoutComponent,
  ToggleSettingsButtonComponent,


];

const ENTRY_COMPONENTS = [
  // ThemeSwitcherListComponent,
];

const PIPES = [
  CapitalizePipe,
  PluralPipe,
  RoundPipe,
  TimingPipe,
  NumberWithCommasPipe,
  EvaIconsPipe,
];

const NB_THEME_PROVIDERS = [
  ...NbThemeModule.forRoot(
    {
      name: 'default',
    },
    [DEFAULT_THEME, COSMIC_THEME, CORPORATE_THEME],
  ).providers,
  ...NbSidebarModule.forRoot().providers,
  ...NbMenuModule.forRoot().providers,
  ...NbDatepickerModule.forRoot().providers,
  ...NbDialogModule.forRoot().providers,
  ...NbWindowModule.forRoot().providers,
  // ...NbToastrModule.forRoot().providers,
  // ...NbChatModule.forRoot({
  //   messageGoogleMapKey: 'AIzaSyA_wNuCzia92MAmdLRzmqitRGvCF7wCZPY',
  // }).providers,
];

@NgModule({
  imports: [...BASE_MODULES, ...NB_MODULES, ...MD_MODULES, ...CDK_MODULES, AceEditorModule,
  TranslateModule.forChild()],
  exports: [...BASE_MODULES, ...NB_MODULES, ...COMPONENTS, ...PIPES, ...MD_MODULES, ...CDK_MODULES, AceEditorModule],
  declarations: [...COMPONENTS, ...PIPES],
  entryComponents: [...ENTRY_COMPONENTS],

})
export class ThemeModule {
  static forRoot(): ModuleWithProviders {
    return <ModuleWithProviders>{
      ngModule: ThemeModule,
      providers: [...NB_THEME_PROVIDERS],
      entryComponents: [...ENTRY_COMPONENTS]
    };
  }
}
