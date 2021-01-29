import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterRoutingModule } from './master-routing.module';
import { ThemeModule } from '../../@theme/theme.module';
import { SharedModule } from '../../shared/shared.module';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import * as fromMasterReducer from './store/reducers';
import * as fromMasterEffect from './store/effects';
import { MasterComponent } from './master.component';
import { CaseTemplateComponent } from './case-template/list/case-template.component';
import { CaseTemplateDetailComponent } from './case-template/detail/case-template-detail.component';
import { ItemComponent } from './item/list/item.component';
import { ItemDetailComponent } from './item/detail/item-detail.component';
import { ColumnGridComponent } from './item/atom/column-grid/column-grid.component';


import { QuestionClassificationAnswerComponent } from './question-classification-answer/list/question-classification-answer.component';
import { QuestionClassificationAnswerDetailComponent } from './question-classification-answer/detail/question-classification-answer-detail.component';
import { QuestionCategoryDetailComponent } from './question-category/detail/question-category-detail.component';
import { QuestionCategoryComponent } from './question-category/list/question-category.component';
import { BillboardDisplayComponent } from './billboard-display/billboard-display.component';
import { BillboardComponent } from './billboard/list/billboard.component';
import { BillboardDisplayItemComponent } from './billboard-display/billboard-display-item/billboard-display-item.component';
import { BillboardDetailComponent } from './billboard/detail/billboard-detail.component';
import { NotificationGroupComponent } from './notification-group/list/notification-group.component';
import { NotificationGroupDetailComponent } from './notification-group/detail/notification-group-detail.component';
import { CaseAssignGroupComponent } from './case-assign-group/list/case-assign-group.component';
import { CaseAssignGroupDetailComponent } from './case-assign-group/detail/case-assign-group-detail.component';
import { StoresComponent } from './stores/list/stores.component';
import { StoresDetailComponent } from './stores/detail/stores-detail.component';
import { CaseTagComponent } from './case-tag/list/case-tag.component';
import { CaseTagDetailComponent } from './case-tag/detail/case-tag-detail.component';
import { CaseFinishedReasonComponent } from './case-finished-reason/list/case-finished-reason.component';
import { CaseFinishedReasonDetailComponent } from './case-finished-reason/detail/case-finished-reason-detail.component';
import { CaseFinishReasonClassificationModalComponent } from './case-finished-reason/modal/case-finished-reason-classification-model/case-finish-reason-classification-modal.component';
import { QuestionOrderComponent } from './question-category/order/question-order.component';
import { CaseFinishedReasonOrderModelComponent } from './case-finished-reason/modal/case-finished-reason-order-model/case-finished-reason-order-model.component';
import { CaseRemindComponent } from './case-remind/list/case-remind.component';
import { CaseRemindDetailComponent } from './case-remind/detail/case-remind-detail.component';
import { OfficialEmailGroupComponent } from './official-email-group/list/official-email-group.component';
import { OfficialEmailGroupDetailComponent } from './official-email-group/detail/official-email-group-detail.component';
import { CaseWarningComponent } from './case-warning/list/case-warning.component';
import { CaseWarningDetailComponent } from './case-warning/detail/case-warning-detail.component';
import { CaseWarningOrderModelComponent } from './case-warning/modal/case-warning-order-model.component';
import { QuestionClassificationGuideComponent } from './question-classification-guide/list/question-classification-guide.component';
import { QuestionClassificationGuideDetailComponent } from './question-classification-guide/detail/question-classification-guide-detail.component';
import { WorkScheduleDetailComponent } from './work-schedule/detail/work-schedule-detail.component';
import { WorkScheduleComponent } from './work-schedule/list/work-schedule.component';



const COMPONENT = [
  MasterComponent,
  CaseTemplateComponent,
  CaseTemplateDetailComponent,
  ColumnGridComponent,
  ItemComponent,
  ItemDetailComponent,
  CaseTemplateDetailComponent,
  QuestionClassificationAnswerComponent,
  QuestionClassificationAnswerDetailComponent,
  QuestionCategoryComponent,
  QuestionCategoryDetailComponent,
  ItemDetailComponent,
  BillboardDisplayComponent,
  BillboardComponent,
  BillboardDetailComponent,
  BillboardDisplayItemComponent,
  NotificationGroupComponent,
  NotificationGroupDetailComponent,
  CaseAssignGroupComponent,
  CaseAssignGroupDetailComponent,
  StoresComponent,
  StoresDetailComponent,
  CaseTagComponent,
  CaseTagDetailComponent,
  CaseFinishedReasonComponent,
  CaseFinishedReasonDetailComponent,
  CaseFinishReasonClassificationModalComponent,
  CaseFinishedReasonOrderModelComponent,
  QuestionOrderComponent,
  CaseRemindComponent,
  CaseRemindDetailComponent,
  OfficialEmailGroupComponent,
  OfficialEmailGroupDetailComponent,
  CaseWarningComponent, 
  CaseWarningDetailComponent,
  CaseWarningOrderModelComponent,
  QuestionClassificationGuideComponent,
  QuestionClassificationGuideDetailComponent,
  QuestionClassificationGuideDetailComponent,
  WorkScheduleComponent,
  WorkScheduleDetailComponent,
  
];

const ENTRY_COMPONENT = [
  BillboardDisplayItemComponent,
  CaseFinishReasonClassificationModalComponent,
  CaseFinishedReasonOrderModelComponent,
  CaseWarningOrderModelComponent
]


@NgModule({
  imports: [
    MasterRoutingModule,
    ThemeModule,
    SharedModule,
    CommonModule,
    EffectsModule.forFeature(fromMasterEffect.effects),
    StoreModule.forFeature('master', fromMasterReducer.reducer),
  ],

  declarations: [...COMPONENT],
  entryComponents: [...ENTRY_COMPONENT]

})
export class MasterModule { }
