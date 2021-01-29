import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { AuthorizeDirective } from './directive/authorize.directive';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { GlobalInterceptor } from './interceptor/global.interceptor';
import { AuthInterceptor } from './interceptor/auth.interceptor';
import { ValidatorInputComponent } from './component/validator/validator-input/validator-input.component';
import { PtcDynamicFormModule, NgInputBase, DYNAMIC_INPUTS, NgFileInput, ValidatorService } from 'ptc-dynamic-form';
import { AngularDraggableDirective } from './directive/angular-draggable.directive';
import { StopPropagationDirective } from './directive/stop-propagation.directive';
import { Translations, WebpackTranslateLoader } from './translation';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { PtcServerTableModule } from 'ptc-server-table';
import { PtcLocalTableModule } from 'ptc-local-table';
import { PtcSelect2Module } from 'ptc-select2';
import { PtcLoadingModule } from 'ptc-loading';
import { PtcSwalModule } from 'ptc-swal';
import { ServerTableComponent } from './component/table/server-table/server-table.component';
import { FeaturenameSelectComponent } from './component/select/element/featurename-select/featurename-select.component';
import { DaterangepickerDirective } from './directive/datetime-range.directive';
import { FileInputDirective } from './directive/file-input.directive';
import { OpenSelectComponent } from './component/select/atom/open-select/open-select.component';
import { BaseComponent } from '../pages/base/base.component';
import { AuthBaseComponent } from '../pages/base/auth-base.component';
import { FormBaseComponent } from '../pages/base/form-base.component';
import { AuthenticationTreeComponent } from './component/tree/authentication-tree/authentication-tree.component';
import { PtcTreeModule } from './component/tree/ptc-tree/ptc-tree.module';
import { LayoutBaseComponent } from '../pages/base/layout-base.component';
import { RoleSelectComponent } from './component/select/element/role-select/role-select.component';
import { UserSelectComponent } from './component/select/element/user-select/user-select.component';
import { SmartLocalTableComponent } from './component/table/smart-local-table/smart-local-table.component';
import { PasswordStrengthBarComponent } from './component/validator/password-strength-bar/password-strength-bar.component';
import { menuSelectServiceFactory } from './service/menu-select.service';
import { ChangePasswordComponent } from './component/other/change-password/change-password.component';
import { BuSelectComponent } from './component/select/element/bu-select/bu-select.component';
import { VendorSelectComponent } from './component/select/element/vendor-select/vendor-select.component';
import { CCSelectComponent } from './component/select/element/cc-select/cc-select.component';
import { OperatorComponent } from './component/table/smart-local-table/operator/operator.component';
import { OrganizationTreeComponent } from './component/tree/organization-tree/organization-tree.component';
import { ContextMenuComponent } from './component/other/context-menu/context-menu.component';

import { EnterpriseSelectComponent } from './component/select/element/enterprise-select/enterprise-select.component';
import { ClassificationSelectComponent } from './component/select/element/classification-select/classification-select.component';

import { NodeDefinitionSelectComponent } from './component/select/element/node-definition-select/node-definition-select.component';
import { QuestionCategorySelectComponent } from './component/select/element/question-category-select/question-category-select.component';

import { DefinitionStepSelectComponent } from './component/select/element/definition-step-select/definition-step-select.component';
import { GeneralInputComponent } from './input/general-input/general-input.component';
import { CheckInputComponent } from './input/check-input/check-input.component';
import { CheckInput } from './input/check-input/check-input';
import { GeneralInput } from './input/general-input/general-input';
import { DatetimeInputComponent } from './input/datetime-input/datetime-input.component';
import { DateTimeInput } from './input/datetime-input/datetime-input';
import { DateTimepickerDirective } from './directive/datetime.directive';
import { BitPipe } from './pipe/bit.pipe';
import { DatetimePipe } from './pipe/datetime.pipe';
import { DYNAMIC_PAYLOADS, MENU_SELECT, PREFIX_TOKEN, DEFAULT_TIMEOUT, DEFAULT_REFESH_TOKEN_URL_CHARATOR, DEFAULT_LOGIN_URL_CHARATOR } from './injection-token';
import { JobSelectorComponent } from './component/modal/job-selector/job-selector.component';
import { UserSelectorComponent } from './component/modal/user-selector/user-selector.component';
import { LocalTableComponent } from './component/table/local-table/local-table.component';
import { ValidatorInputDirective } from './directive/validator-input.directive';
import { AuthenticationService } from './service/authentication.service';
import { PdfPreviewComponent } from './component/other/pdf-preview/pdf-preview.component';
import { DynamicQuestionSelectComponent } from './component/select/component/dynamic-question-select/dynamic-question-select.component';
import { CallcenterNodeUserModalComponent } from './component/modal/callcenter-node-user-modal/callcenter-node-user-modal.component';
import { HeadquarterNodeUserModalComponent } from './component/modal/headquarter-node-user-modal/headquarter-node-user-modal.component';
import { VendorNodeUserModalComponent } from './component/modal/vendor-node-user-modal/vendor-node-user-modal.component';
import { NodeDefinitionJobModalComponent } from './component/modal/node-definition-job-modal/node-definition-job-modal.component';
import { KmTreeComponent } from './component/tree/km-tree/km-tree.component';
import { StoreOwnerSelectorComponent } from './component/modal/store-owner-selector/store-owner-selector.component';
import { HashtagComponent } from './component/other/hashtag/hashtag.component';
import { CounterBallComponent } from './component/other/counter-ball/counter-ball.component';
import { EnumSelectComponent } from './component/select/element/enum-select/enum-select.component';
import { CounterBallGroupComponent } from './component/other/counter-ball-group/counter-ball-group.component';
import { DynamicFinishReasonComponent } from './component/other/dynamic-finish-reason/dynamic-finish-reason.component';
import { CaseWarningSelectComponent } from './component/select/element/case-warning-select/case-warning-select.component';
import { SystemParameterSelectComponent } from './component/select/element/system-parameter-select/system-parameter-select.component';
import { CaseAssignGroupSelectComponent } from './component/select/element/case-assign-group-select/case-assign-group-select.component';
import { CaseAssignGroupUserModalComponent } from './component/modal/case-assign-group-user-modal/case-assign-group-user-modal.component';
import { MailSenderComponent } from './component/other/mail-sender/mail-sender.component';
import { SenderUserCreatorComponent } from './component/other/mail-sender/sender-user-creator.component';
import { CaseTemplateSelectorComponent } from './component/modal/case-template-selector/case-template-selector.component';
import { QuestionClassificationAnswerSelectorComponent } from './component/modal/question-classification-answer-selector/question-classification-answer-selector.component';
import { BuNodeDefinitionLevelSelectorComponent } from './component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { HeaderquarterNodeTreeUserSelectorComponent } from './component/modal/tree-user/headerquarter-node-tree-user-selector/headerquarter-node-tree-user-selector.component';
import { CallcenterNodeTreeUserSelectorComponent } from './component/modal/tree-user/callcenter-node-tree-user-selector/callcenter-node-tree-user-selector.component';
import { VendorNodeTreeUserSelectorComponent } from './component/modal/tree-user/vendor-node-tree-user-selector/vendor-node-tree-user-selector.component';
import { AllNodeTreeUserSelectorComponent } from './component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';
import { JsonFormatterComponent } from './component/other/json-formatter/json-formatter.component';
import { NotificationGroupUserModalComponent } from './component/modal/notification-group-user-modal/notification-group-user-modal.component';
import { NotificationGroupSelectComponent } from './component/select/element/notification-group-select/notification-group-select.component';
import { ItemSelectComponent } from './component/select/element/item-select/item-select.component';
import { WorkProcessSelectComponent } from './component/select/element/work-process-select/work-process-select.component';
import { DualListComponent } from './component/other/dual-list/dual-list.component';
import { AngularDualListBoxModule } from 'angular-dual-listbox';
import { BindPipe } from './pipe/bind.pipe';
import { PreviewNodeTreeUserComponent } from './component/modal/tree-user/preview-node-tree-user/preview-node-tree-user.component';
import { StarsignLableDirective } from './directive/star-sign.directive';
import { CaseFinishedReasonSelectComponent } from './component/select/element/case-finished-reason-select/case-finished-reason-select.component';
import { DraggableListComponent } from './component/other/draggable-list/draggable-list.component';
import { ReplacePipe } from './pipe/replace.pipe';
import { StoreTypeSelectComponent } from './component/select/element/store-type-select/store-type-select.component';
import { StoreSelectComponent } from './component/select/element/store-select/store-select.component';
import { AllNodeTreeSelectorComponent } from './component/modal/tree-selector/all-node-tree-selector/all-node-tree-selector.component';
import { CallcenterNodeTreeSelectorComponent } from './component/modal/tree-selector/callcenter-node-tree-selector/callcenter-node-tree-selector.component';
import { HeaderquarterNodeTreeSelectorComponent } from './component/modal/tree-selector/headerquarter-node-tree-selector/headerquarter-node-tree-selector.component';
import { VendorNodeTreeSelectorComponent } from './component/modal/tree-selector/vendor-node-tree-selector/vendor-node-tree-selector.component';
import { TotopButtonComponent } from './component/other/totop-button/totop-button.component';
import { BottomBarComponent } from './component/other/bottom-bar/bottom-bar.component';
import { NoteListGroupComponent } from './component/other/note-list-group/note-list-group.component';
import { NoteListItemComponent } from './component/other/note-list-group/note-list-item.component';
import { CaseItemTableInputComponent } from './input/ppcLife/case-item-table-input.component';
import { CaseItemTableInput } from './input/ppcLife/case-item-table-input';
import { ItemSelectorComponent } from './component/modal/item-selector/item-selector.component';
import { EnumPipe } from './pipe/enum.pipe';
import { GroupSelectComponent } from './component/select/element/group-select/group-select.component';
import { FavoriteFeatureDndComponent } from './component/other/favorite-feature-dnd/favorite-feature-dnd.component';
import { InvoiceTypeSelectComponent } from './component/select/element/invoice-type-select/invoice-type-select.component';
import { CaseResumeComponent } from './component/other/case-resume/case-resume.component';
import { CaseSourceSelectComponent } from './component/select/element/case-source-select/case-source-select.component';
import { VerificationCodeComponent } from './component/other/verification-code/verification-code.component';
import { AsoCaseItemInputComponent } from './input/aso/aso-case-item-input.component';
import { AsoCaseItemTableInput } from './input/aso/aso-case-item-table-input';
import { IccCaseItemInputComponent } from './input/icc/icc-case-item-input.component';
import { IccCaseItemTableInput } from './input/icc/icc-case-item-table-input';
import { VendorNodeTreeForSearchSelectorComponent } from './component/modal/tree-selector/vendor-node-tree-for-search-selector/vendor-node-tree-for-search-selector.component';
import { AllNodeTreeSelectorForCcComponent } from './component/modal/tree-selector/all-node-tree-selector-for-cc/all-node-tree-selector-for-cc.component';
import { VendorNodeTreeUserForSearchSelectorComponent } from './component/modal/tree-user/vendor-node-tree-user-for-search-selector/vendor-node-tree-user-for-search-selector.component';
import { AllNodeTreeUserSelectorForCcComponent } from './component/modal/tree-user/all-node-tree-user-selector-for-cc/all-node-tree-user-selector-for-cc.component';



const PIPE = [
  BitPipe,
  DatetimePipe,
  BindPipe,
  ReplacePipe,
  EnumPipe
];


const PTC_MODULES = [
  PtcServerTableModule,
  PtcLocalTableModule,
  PtcLoadingModule,
  PtcSwalModule,
  PtcDynamicFormModule,
  PtcSelect2Module,
  PtcTreeModule
];


const SELECT = [
  RoleSelectComponent,
  OpenSelectComponent,
  FeaturenameSelectComponent,
  UserSelectComponent,
  BuSelectComponent,
  GroupSelectComponent,
  VendorSelectComponent,
  CCSelectComponent,
  EnterpriseSelectComponent,
  NodeDefinitionSelectComponent,
  CCSelectComponent,
  ClassificationSelectComponent,
  QuestionCategorySelectComponent,
  EnumSelectComponent,
  CaseWarningSelectComponent,
  SystemParameterSelectComponent,
  CaseAssignGroupSelectComponent,
  WorkProcessSelectComponent,
  StoreTypeSelectComponent,
  StoreSelectComponent,
  InvoiceTypeSelectComponent,
  CaseSourceSelectComponent
];
const TREE = [
  AuthenticationTreeComponent,
  OrganizationTreeComponent,
  KmTreeComponent
];

const COMPONENT = [
  BaseComponent,
  AuthBaseComponent,
  FormBaseComponent,
  LayoutBaseComponent,
  ValidatorInputComponent,
  ServerTableComponent,
  LocalTableComponent,
  SmartLocalTableComponent,
  PasswordStrengthBarComponent,
  ChangePasswordComponent,
  ContextMenuComponent,
  DefinitionStepSelectComponent,
  BuNodeDefinitionLevelSelectorComponent,
  HeaderquarterNodeTreeUserSelectorComponent,
  CheckInputComponent,
  GeneralInputComponent,
  DatetimeInputComponent,
  CallcenterNodeTreeUserSelectorComponent,
  DynamicQuestionSelectComponent,
  PdfPreviewComponent,
  VendorNodeTreeUserSelectorComponent,
  AllNodeTreeUserSelectorComponent,
  HashtagComponent,
  CounterBallComponent,
  CounterBallGroupComponent,
  DynamicFinishReasonComponent,
  MailSenderComponent,
  SenderUserCreatorComponent,
  PreviewNodeTreeUserComponent,
  CaseFinishedReasonSelectComponent,
  PreviewNodeTreeUserComponent,
  DraggableListComponent,
  TotopButtonComponent,
  BottomBarComponent,
  NoteListGroupComponent,
  NoteListItemComponent,
  FavoriteFeatureDndComponent,
  CaseResumeComponent,
  VerificationCodeComponent,
  VendorNodeTreeUserForSearchSelectorComponent,
  AllNodeTreeUserSelectorForCcComponent,
];
const DIRECTIVE = [
  DaterangepickerDirective,
  FileInputDirective,
  AngularDraggableDirective,
  StopPropagationDirective,
  AuthorizeDirective,
  DateTimepickerDirective,
  ValidatorInputDirective,
  StarsignLableDirective
];

const ENTRY_COMPONENT = [
  OperatorComponent,
  CheckInputComponent,
  GeneralInputComponent,
  DatetimeInputComponent,
  CaseItemTableInputComponent,
  AsoCaseItemInputComponent,
  IccCaseItemInputComponent,
  ValidatorInputComponent,
  ItemSelectorComponent,
  HeaderquarterNodeTreeUserSelectorComponent,
  CallcenterNodeTreeUserSelectorComponent,
  VendorNodeTreeUserSelectorComponent,
  AllNodeTreeUserSelectorComponent,
  AllNodeTreeSelectorComponent,
  CallcenterNodeTreeSelectorComponent,
  HeaderquarterNodeTreeSelectorComponent,
  VendorNodeTreeSelectorComponent,
  SenderUserCreatorComponent,
  CaseTemplateSelectorComponent,
  QuestionClassificationAnswerSelectorComponent,
  JobSelectorComponent,
  UserSelectorComponent,
  CallcenterNodeUserModalComponent,
  HeadquarterNodeUserModalComponent,
  VendorNodeUserModalComponent,
  NodeDefinitionJobModalComponent,
  StoreOwnerSelectorComponent,
  CaseAssignGroupUserModalComponent,
  JsonFormatterComponent,
  NotificationGroupUserModalComponent,
  NotificationGroupSelectComponent,
  ItemSelectComponent,
  DualListComponent,
  CaseResumeComponent,
  VendorNodeTreeForSearchSelectorComponent,
  AllNodeTreeSelectorForCcComponent,
  VendorNodeTreeUserForSearchSelectorComponent,
  AllNodeTreeUserSelectorForCcComponent,
];


const INTERCEPTOR = [
  { provide: HTTP_INTERCEPTORS, useClass: GlobalInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
];

const DYNAMIC_FORM = [
  { provide: DYNAMIC_PAYLOADS, useValue: { key: 'CaseItemTableInput', payload: new CaseItemTableInput() }, multi: true },
  { provide: DYNAMIC_PAYLOADS, useValue: { key: 'AsoCaseItemTableInput', payload: new AsoCaseItemTableInput() }, multi: true },
  { provide: DYNAMIC_PAYLOADS, useValue: { key: 'IccCaseItemTableInput', payload: new IccCaseItemTableInput() }, multi: true },
  { provide: DYNAMIC_INPUTS, useValue: { key: 'CaseItemTableInputComponent', component: CaseItemTableInputComponent }, multi: true },
  { provide: DYNAMIC_INPUTS, useValue: { key: 'AsoCaseItemInputComponent', component: AsoCaseItemInputComponent }, multi: true },
  { provide: DYNAMIC_INPUTS, useValue: { key: 'IccCaseItemInputComponent', component: IccCaseItemInputComponent }, multi: true },
  { provide: DYNAMIC_PAYLOADS, useValue: { key: 'CheckInput', payload: new CheckInput() }, multi: true },
  { provide: DYNAMIC_INPUTS, useValue: { key: 'CheckInputComponent', component: CheckInputComponent }, multi: true },
  { provide: DYNAMIC_PAYLOADS, useValue: { key: 'GeneralInput', payload: new GeneralInput() }, multi: true },
  { provide: DYNAMIC_INPUTS, useValue: { key: 'GeneralInputComponent', component: GeneralInputComponent }, multi: true },
  { provide: DYNAMIC_PAYLOADS, useValue: { key: 'DateTimeInput', payload: new DateTimeInput() }, multi: true },
  { provide: DYNAMIC_INPUTS, useValue: { key: 'DatetimeInputComponent', component: DatetimeInputComponent }, multi: true },
];



@NgModule({
  declarations: [
    ...COMPONENT,
    ...SELECT,
    ...TREE,
    ...DIRECTIVE,
    ...ENTRY_COMPONENT,
    ...PIPE,

 
  ],
  imports: [
    ThemeModule,
    CommonModule,
    AngularDualListBoxModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: WebpackTranslateLoader,
        deps: [AuthenticationService]
      },
    }),
    ...PTC_MODULES
  ],
  entryComponents: [
    ...ENTRY_COMPONENT,
  ],
  exports: [
    ...COMPONENT,
    ...DIRECTIVE,
    ...SELECT,
    ...TREE,
    ...PTC_MODULES,
    ...ENTRY_COMPONENT,
    ...PIPE,
    TranslateModule
  ],
  providers: [
    ValidatorService,
    Translations,
    ...DYNAMIC_FORM,
    ...INTERCEPTOR,
    { provide: 'MENU_SELECT_TOKEN', useValue: 'PTC' },
    { provide: MENU_SELECT, useFactory: menuSelectServiceFactory, deps: ['MENU_SELECT_TOKEN'] },
    { provide: DEFAULT_TIMEOUT, useValue: 30000 },
    { provide: PREFIX_TOKEN, useValue: '' },
    { provide: DEFAULT_REFESH_TOKEN_URL_CHARATOR, useValue: `refreshToken` },
    { provide: DEFAULT_LOGIN_URL_CHARATOR, useValue: `login` },

  ],
})
export class SharedModule {

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
    };
  }
}
