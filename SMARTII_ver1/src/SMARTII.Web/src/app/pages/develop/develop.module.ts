import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { DevelopComponent } from './develop.component';
import { DevelopRoutingModule } from './develop-routing.module';
import { SettingModalComponent } from './dynamic-form/modal/setting-modal.component';
import { SummaryComponent } from './summary/summary.component';


const COMPONENT = [
  DevelopComponent,
  DynamicFormComponent,
  SummaryComponent
];

const ENTRY_COMPONENT = [
  SettingModalComponent
];

@NgModule({
  declarations: [...COMPONENT, ...ENTRY_COMPONENT],
  entryComponents: [...ENTRY_COMPONENT],
  imports: [
    CommonModule,
    SharedModule,
    ThemeModule,
    DevelopRoutingModule
  ]
})
export class DevelopModule { }
