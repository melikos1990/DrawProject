import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { SubstituteComponent } from './substitute.component';
import { SubstituteRoutingModule } from './substitute-routing.module';
import { CaseApplyComponent } from './case-apply/list/case-apply.component';
import { ApplyUserComponent } from './case-apply/modal/apply-user/apply-user.component';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import * as fromSubstituteEffect from './store/effects';
import * as fromSubstituteReducer from './store/reducers';
import { CaseNoticeComponent } from './case-notice/list/case-notice.component';

const COMPONENTS = [
  SubstituteComponent,
  CaseApplyComponent,
  CaseNoticeComponent,
];

const ENTRY_COMPONENTS = [
  ApplyUserComponent
];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ThemeModule,
    SubstituteRoutingModule,
    EffectsModule.forFeature(fromSubstituteEffect.effects),
    StoreModule.forFeature('substitute', fromSubstituteReducer.reducer),
  ],
  declarations: [
    ...COMPONENTS,
    ...ENTRY_COMPONENTS
  ],
  entryComponents: [...ENTRY_COMPONENTS]
})
export class SubstituteModule { }
