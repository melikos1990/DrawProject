import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SystemLoginComponent } from './system-login/system-login.component';
import { ThemeModule } from '../@theme/theme.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';



const COMPONENT = [
  SystemLoginComponent,
];

@NgModule({
  declarations: [...COMPONENT],
  imports: [
    CommonModule,
    ThemeModule,
    RouterModule,
    SharedModule,
  ],
  exports: [
    ...COMPONENT
  ]
})
export class AuthorizeModule { }
