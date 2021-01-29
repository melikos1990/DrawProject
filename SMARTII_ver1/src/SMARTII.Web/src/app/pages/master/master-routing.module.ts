import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MasterComponent } from './master.component';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';
import { CaseTemplateComponent } from './case-template/list/case-template.component';
import { CaseTemplateDetailComponent } from './case-template/detail/case-template-detail.component';
import { ItemComponent } from './item/list/item.component';
import { ItemDetailComponent } from './item/detail/item-detail.component';
import { QuestionClassificationAnswerComponent } from './question-classification-answer/list/question-classification-answer.component';
import { QuestionClassificationAnswerDetailComponent } from './question-classification-answer/detail/question-classification-answer-detail.component';
import { QuestionClassificationGuideComponent } from './question-classification-guide/list/question-classification-guide.component';
import { QuestionClassificationGuideDetailComponent } from './question-classification-guide/detail/question-classification-guide-detail.component';
import { QuestionCategoryComponent } from './question-category/list/question-category.component';
import { QuestionCategoryDetailComponent } from './question-category/detail/question-category-detail.component';
import { BillboardDisplayComponent } from './billboard-display/billboard-display.component';
import { BillboardComponent } from './billboard/list/billboard.component';
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
import { QuestionOrderComponent } from './question-category/order/question-order.component';
import { CaseRemindComponent } from './case-remind/list/case-remind.component';
import { CaseRemindDetailComponent } from './case-remind/detail/case-remind-detail.component';

import { OfficialEmailGroupComponent } from './official-email-group/list/official-email-group.component';
import { OfficialEmailGroupDetailComponent } from './official-email-group/detail/official-email-group-detail.component';
import { CaseWarningComponent } from './case-warning/list/case-warning.component';
import { CaseWarningDetailComponent } from './case-warning/detail/case-warning-detail.component';
import { WorkScheduleComponent } from './work-schedule/list/work-schedule.component';
import { WorkScheduleDetailComponent } from './work-schedule/detail/work-schedule-detail.component';




const routes: Routes = [{
  path: '',
  component: MasterComponent,
  children: [
    {
      path: 'case-template',
      component: CaseTemplateComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_TEMPLATE',
        feature: 'CaseTemplateComponent'
      },
    },
    {
      path: 'case-template-detail',
      component: CaseTemplateDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_TEMPLATE',
        feature: 'CaseTemplateComponent'
      },
    },
    {
      path: 'item',
      component: ItemComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.ITEM',
        feature: 'ItemComponent'
      },

    },
    {
      path: 'question-classification-answer',
      component: QuestionClassificationAnswerComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CLASSIFICATION_ANSWER',
        feature: 'QuestionClassificationAnswerComponent'
      },
    },
    {
      path: 'question-classification-answer-detail',
      component: QuestionClassificationAnswerDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CLASSIFICATION_ANSWER',
        feature: 'QuestionClassificationAnswerComponent'
      },
    },
    {
      path: 'question-classification-guide',
      component: QuestionClassificationGuideComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CLASSIFICATION_GUIDE',
        feature: 'QuestionClassificationGuideComponent'
      },
    },
    {
      path: 'question-classification-guide-detail',
      component: QuestionClassificationGuideDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CLASSIFICATION_GUIDE',
        feature: 'QuestionClassificationGuideComponent'
      },
    },
    {
      path: 'question-category',
      component: QuestionCategoryComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CATEGORY',
        feature: 'QuestionCategoryComponent'
      },
    },
    {
      path: 'question-category-detail',
      component: QuestionCategoryDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CATEGORY',
        feature: 'QuestionCategoryComponent'
      },
    },
    {
      path: 'question-category-order',
      component: QuestionOrderComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.QUESTION_CATEGORY',
        feature: 'QuestionCategoryComponent'
      },
    },
    {
      path: 'item-detail',
      component: ItemDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.ITEM',
        feature: 'ItemComponent'
      },
    },
    {
      path: 'notification-group',
      component: NotificationGroupComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.NOTIFICATION_GROUP',
        feature: 'NotificationGroupComponent'
      },
    },
    {
      path: 'notification-group-detail',
      component: NotificationGroupDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.NOTIFICATION_GROUP',
        feature: 'NotificationGroupComponent'
      },
    },
    {
      path: 'billboard',
      component: BillboardComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.BILLBOARD',
        feature: 'BillboardComponent'
      },
    },
    {
      path: 'billboard-detail',
      component: BillboardDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.BILLBOARD',
        feature: 'BillboardComponent'
      },
    },
    {
      path: 'billboard-display',
      component: BillboardDisplayComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.BILLBOARD_DISPLAY',
        feature: 'BillboardDisplayComponent'
      },
    },
    {
      path: 'case-assign-group',
      component: CaseAssignGroupComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_ASSIGN_GROUP',
        feature: 'CaseAssignGroupComponent'
      },
    },
    {
      path: 'case-assign-group-detail',
      component: CaseAssignGroupDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_ASSIGN_GROUP',
        feature: 'CaseAssignGroupComponent'
      },
    },
    {
      path: 'stores',
      component: StoresComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.STORES',
        feature: 'StoresComponent'
      },
    },
    {
      path: 'stores-detail',
      component: StoresDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.STORES',
        feature: 'StoresComponent'
      },
    },
    {
      path: 'case-tag',
      component: CaseTagComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_TAG',
        feature: 'CaseTagComponent'
      },
    },
    {
      path: 'case-tag-detail',
      component: CaseTagDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_TAG',
        feature: 'CaseTagComponent'
      },
    },
    {
      path: 'case-finished-reason',
      component: CaseFinishedReasonComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_FINISHED_REASON',
        feature: 'CaseFinishedReasonComponent'
      },

    },
    {
      path: 'case-finished-reason-detail',
      component: CaseFinishedReasonDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_FINISHED_REASON',
        feature: 'CaseFinishedReasonComponent'
      },

    },
    {
      path: 'case-remind',
      component: CaseRemindComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_REMIND',
        feature: 'CaseRemindComponent'
      },

    },
    {
      path: 'case-remind-detail',
      component: CaseRemindDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_REMIND',
        feature: 'CaseRemindComponent'
      },

    },
    {
      path: 'official-email-group',
      component: OfficialEmailGroupComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.OFFICIAL_EMAIL_GROUP',
        feature: 'OfficialEmailGroupComponent'
      },
    },
    {
      path: 'official-email-group-detail',
      component: OfficialEmailGroupDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.OFFICIAL_EMAIL_GROUP',
        feature: 'OfficialEmailGroupComponent'
      },
    },
    {
      path: 'case-warning',
      component: CaseWarningComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_WARNING',
        feature: 'CaseWarningComponent'
      },
    },
    {
      path: 'case-warning-detail',
      component: CaseWarningDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.CASE_WARNING',
        feature: 'CaseWarningComponent'
      },
    },
    {
      path: 'work-schedule',
      component: WorkScheduleComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.WORK_SCHEDULE',
        feature: 'WorkScheduleComponent'
      },
    },
    {
      path: 'work-schedule-detail',
      component: WorkScheduleDetailComponent,
      canActivate: [AuthGuardService],
      data: {
        breadcrumb: 'APPLICATION.FEATURE.WORK_SCHEDULE',
        feature: 'WorkScheduleComponent'
      },
    },    
  ],

}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [
    RouterModule
  ]
})
export class MasterRoutingModule { }
