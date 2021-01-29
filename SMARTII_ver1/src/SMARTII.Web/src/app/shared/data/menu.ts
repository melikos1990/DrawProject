import { Menu } from 'src/app/model/master.model';


export const MENU_ITEMS: Menu[] = [
  {
    title: 'APPLICATION.FEATURE.HOME',
    icon: 'nb-home',
    link: '/pages/home/home-page',
    home: true,
    component: 'HomePageComponent'
  },
  {
    title: 'APPLICATION.FEATURE.FAVORITE_FEATURE_LIST_PARENT',
    group: true,
    isFavorite: true
  },
  {
    title: 'APPLICATION.FEATURE.FEATURE_LIST_PARENT',
    group: true,
  },
  {
    title: 'APPLICATION.FEATURE.MASTER_PARENT',
    icon: 'nb-star',
    children: [
      {
        title: 'APPLICATION.FEATURE.CASE_TEMPLATE',
        link: '/pages/master/case-template',
        component: 'CaseTemplateComponent'
      },
      {
        title: 'APPLICATION.FEATURE.STORES',
        link: '/pages/master/stores',
        component: 'StoresComponent'
      },
      {
        title: 'APPLICATION.FEATURE.ITEM',
        link: '/pages/master/item',
        component: 'ItemComponent'
      },
      {
        title: 'APPLICATION.FEATURE.QUESTION_CLASSIFICATION_ANSWER',
        link: '/pages/master/question-classification-answer',
        component: 'QuestionClassificationAnswerComponent'
      },
      {
        title: 'APPLICATION.FEATURE.QUESTION_CLASSIFICATION_GUIDE',
        link: '/pages/master/question-classification-guide',
        component: 'QuestionClassificationGuideComponent'
      },
      {
        title: 'APPLICATION.FEATURE.QUESTION_CATEGORY',
        link: '/pages/master/question-category',
        component: 'QuestionCategoryComponent'
      },
      {
        title: 'APPLICATION.FEATURE.NOTIFICATION_GROUP',
        link: '/pages/master/notification-group',
        component: 'NotificationGroupComponent'
      },
      {
        title: 'APPLICATION.FEATURE.BILLBOARD',
        link: '/pages/master/billboard',
        component: 'BillboardComponent'
      },
      {
        title: 'APPLICATION.FEATURE.BILLBOARD_DISPLAY',
        link: '/pages/master/billboard-display',
        component: 'BillboardDisplayComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CASE_ASSIGN_GROUP',
        link: '/pages/master/case-assign-group',
        component: 'CaseAssignGroupComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CASE_TAG',
        link: '/pages/master/case-tag',
        component: 'CaseTagComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CASE_FINISHED_REASON',
        link: '/pages/master/case-finished-reason',
        component: 'CaseFinishedReasonComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CASE_REMIND',
        link: '/pages/master/case-remind',
        component: 'CaseRemindComponent'
      },
      {
        title: 'APPLICATION.FEATURE.OFFICIAL_EMAIL_GROUP',
        link: '/pages/master/official-email-group',
        component: 'OfficialEmailGroupComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CASE_WARNING',
        link: '/pages/master/case-warning',
        component: 'CaseWarningComponent'
      },
      {
        title: 'APPLICATION.FEATURE.WORK_SCHEDULE',
        link: '/pages/master/work-schedule',
        component: 'WorkScheduleComponent'
      },
    ],
  },
  {
    title: 'APPLICATION.FEATURE.SYSTEM_PARENT',
    icon: 'nb-edit',
    children: [
      {
        title: 'APPLICATION.FEATURE.ROLE',
        link: '/pages/system/role',
        component: 'RoleComponent'
      },
      {
        title: 'APPLICATION.FEATURE.USER',
        link: '/pages/system/user',
        component: 'UserComponent'
      },
      {
        title: 'APPLICATION.FEATURE.USER_PARAMETER',
        link: '/pages/system/user-parameter',
        component: 'UserParameterComponent'
      },
      {
        title: 'APPLICATION.FEATURE.SYSTEM_PARAMETER',
        link: '/pages/system/system-parameter',
        component: 'SystemParameterComponent'
      },
      {
        title: 'APPLICATION.FEATURE.SYSTEM_LOG',
        link: '/pages/system/system-log',
        component: 'SystemLogComponent'
      },
      {
        title: 'APPLICATION.FEATURE.PERAONAL_CHANGE_PASSWORD',
        link: '/pages/system/personal-change-password',
        component: 'PersonalChangePasswordComponent'
      },
    ],
  },
  {
    title: 'APPLICATION.FEATURE.CASE_PARENT',
    icon: 'nb-email',
    children: [
      {
        title: 'APPLICATION.FEATURE.CASE_CREATOR',
        link: '/pages/case/case-create',
        component: 'C1Component'
      },
      {
        title: 'APPLICATION.FEATURE.OFFICIAL_EMAIL_ADOPT',
        link: '/pages/case/official-email-adopt',
        component: 'OfficialEmailAdoptComponent'
      },
      {
        title: 'APPLICATION.FEATURE.NOTIFICATION_GROUP_SENDER',
        link: '/pages/case/notification-group-sender',
        component: 'NotificationGroupSenderComponent'
      },
      {
        title: 'APPLICATION.FEATURE.PPCLIFE_EFFECTIVE_SUMMARY',
        link: '/pages/case/ppclife-effective-summary',
        component: 'PpclifeEffectiveSummaryComponent'
      },
    ],
  },
  {
    title: 'APPLICATION.FEATURE.ORGANIZATION_PARENT',
    icon: 'nb-person',
    children: [

     
      {
        title: 'APPLICATION.FEATURE.NODE_DEFINITION',
        link: '/pages/organization/node-definition',
        component: 'NodeDefinitionComponent'
      },
      {
        title: 'APPLICATION.FEATURE.HEADERQUARTER_NODE',
        link: '/pages/organization/headerquarter-node',
        component: 'HeaderquarterNodeComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CALLCENTER_NODE',
        link: '/pages/organization/callcenter-node',
        component: 'CallCenterNodeComponent'
      },
      {
        title: 'APPLICATION.FEATURE.VENDOR_NODE',
        link: '/pages/organization/vendor-node',
        component: 'VendorNodeComponent'
      },
      {
        title: 'APPLICATION.FEATURE.ENTERPRISE',
        link: '/pages/organization/enterprise',
        component: 'EnterpriseComponent'
      },

    ],
  },
  {
    title: 'APPLICATION.FEATURE.DEVELOP_PARENT',
    icon: 'nb-star',
    children: [
      {
        title: 'APPLICATION.FEATURE.DYNAMIC_FORM',
        link: '/pages/develop/dynamic-form',
        component: 'DynamicFormComponent'
      },
      {
        title: '測試區',
        link: '/pages/develop/summary',
        component: 'SummaryComponent'
      },
    ],
  },
  {
    title: 'APPLICATION.FEATURE.SUBSTITUTE_PARENT',
    icon: 'nb-audio',
    children: [
      {
        title: 'APPLICATION.FEATURE.CASE_APPLY',
        link: '/pages/substitute/case-apply',
        component: 'CaseApplyComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CASE_NOTICE',
        link: '/pages/substitute/case-notice',
        component: 'CaseNoticeComponent'
      },
    ],
  },
  {
    title: 'APPLICATION.FEATURE.SEARCH_PARENT',
    icon: 'nb-star',
    children: [
      {
        title: 'APPLICATION.FEATURE.CALLCENTER_CASE_SEARCH',
        link: '/pages/search/call-center-case-search',
        component: 'CallCenterCaseSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.HEADQUARTERS_STORE_CASE_SEARCH',
        link: '/pages/search/headerqurter-store-case-search',
        component: 'HeaderQurterNodeStoreCaseSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.HEADQUARTERS_BU_CASE_SEARCH',
        link: '/pages/search/headerqurter-bu-case-search',
        component: 'HeaderQurterNodeBUCaseSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.CALLCENTER_STORE_ASSIGNMENT_SEARCH',
        link: '/pages/search/call-center-assignment-search',
        component: 'CallCenterAssignmentSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.HEADQUARTERS_STORE_ASSIGNMENT_SEARCH',
        link: '/pages/search/headerqurter-store-assignment-search',
        component: 'HeaderqurterStoreAssignmentSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.HEADQUARTERS_BU_ASSIGNMENT_SEARCH',
        link: '/pages/search/headerqurter-bu-assignment-search',
        component: 'HeaderqurterBUAssignmentSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.VENDOR_ASSIGNMENT_SEARCH',
        link: '/pages/search/vendor-assignment-search',
        component: 'VendorAssignmentSearchComponent'
      },
      {
        title: 'APPLICATION.FEATURE.OLD_CASE_SEARCH',
        url: 'https://scweb.ptc-nec.com.tw/B2CWeb/',
        target: '_blank',
        component: ''
      },
      {
        title: 'APPLICATION.FEATURE.KM',
        link: '/pages/search/km',
        component: 'KmComponent'
      },
    ],
  },
  {
    title: 'APPLICATION.FEATURE.DOWNLOAD_PARENT',
    icon: 'nb-star',
    children: [
      {
        title: 'APPLICATION.FEATURE.DAILY_REPORT',
        link: '/pages/download/daily-report',
        component: 'DailyReportComponent'
      },
      {
        title: 'APPLICATION.FEATURE.ASO_DAILY_REPORT',
        link: '/pages/download/aso-daily-report',
        component: 'AsoDailyReportComponent'
      },
    ],
  },
];
