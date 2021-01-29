import { Injector } from '@angular/core';

/**
 * Allows for retrieving singletons using `AppInjector.get(MyService)` (whereas
 * `ReflectiveInjector.resolveAndCreate(MyService)` would create a new instance
 * of the service).
 */
export let AppInjector: Injector;

/**
 * Helper to set the exported {@link AppInjector}, needed as ES6 modules export
 * immutable bindings (see http://2ality.com/2015/07/es6-module-exports.html) for
 * which trying to make changes after using `import {AppInjector}` would throw:
 * "TS2539: Cannot assign to 'AppInjector' because it is not a variable".
 */
export function setAppInjector(injector: Injector) {
  if (AppInjector) {
    // Should not happen
    console.error('Programming error: AppInjector was already set');
  }
  else {
    AppInjector = injector;
  }
}


export const caseTemplateKey = {
  EMAIL: 'EMAIL',
  CASE_FINISH: 'CASE_FINISH',
  NOTICE: 'NOTICE',
  ASSIGNMENT: 'ASSIGNMENT',
  COMPLAINT: 'COMPLAINT',
  COMMUNICATION:'COMMUNICATION'
}



export const _21CenturyKeyPair = { BuName: '21CENTURY', NodeKey: '001' };
export const MisterDonutKeyPair = { BuName: 'MisterDonut', NodeKey: '002' };
export const ColdStoneKeyPair = { BuName: 'ColdStone', NodeKey: '003' };
export const eShopKeyPair = { BuName: 'eShop', NodeKey: '004' };
export const PPCLIFEKeyPair = { BuName: 'PPCLIFE', NodeKey: '005' };
export const ICCKeyPair = { BuName: 'ICC', NodeKey: '006' };
export const ASOKeyPair = { BuName: 'ASO', NodeKey: '007' };
export const OpenPointKeyPair = { BuName: 'OpenPoint', NodeKey: '008' };
export const FORTUNEKeyPair = { BuName: 'FORTUNE', NodeKey: '009' };
export const CommonBUKeyPair = { BuName: 'COMMON_BU', NodeKey: '000' };

export const buProviderList = [
  // _21CenturyKeyPair,
  // MisterDonutKeyPair,
  //ColdStoneKeyPair,
  //eShopKeyPair,
  PPCLIFEKeyPair,
  // ICCKeyPair,
  // ASOKeyPair,
  // OpenPointKeyPair,
  // CommonBUKeyPair,
  // FORTUNEKeyPair
]
export const buReportList = [
  _21CenturyKeyPair,
  MisterDonutKeyPair,
  ColdStoneKeyPair,
  eShopKeyPair,
  PPCLIFEKeyPair,
  ICCKeyPair,
  ASOKeyPair,
  OpenPointKeyPair,
  CommonBUKeyPair
]
export const buStoreList = [
  // _21CenturyKeyPair,
  // MisterDonutKeyPair,
  //ColdStoneKeyPair,
  //eShopKeyPair,
  //PPCLIFEKeyPair,
  // ICCKeyPair,
  // ASOKeyPair,
  // OpenPointKeyPair,
  // CommonBUKeyPair,
  FORTUNEKeyPair
]
export const commonBu = 'COMMON_BU';
export const definitionKey = {
  BU: "BU",
  GROUP: "GROUP",
  VENDOR: "VENDOR",
  CALLCENTER: "CALLCENTER",
  STORE: "STORE",
  VENDOR_GROUP: "VENDOR_GROUP"
};

export const caseTemplate = {
  CaseFinish: "CASE_FINISH"
}

export const caseTemplateTag = {
  COMPLAINTED_USER: "{{ComplaintedUser}}",
  CONCAT_USER_NAME: "{{ConcatUserName}}",
  CONCAT_USER_PHONE: "{{ConcatUserPhone}}",
  COMPLAINTED_USER_INFO: "{{ComplaintedUserInfo}}",
  CONCAT_USER: "{{ConcatUser}}",
  ASSIGNMENT_NOTIFY_USERS: "{{AssignmentNotifyUsers}}",
  COMPLAINT_INVOICE_USERS: "{{ComplaintInvoiceUsers}}",
  COMPLAINT_NOTICE_USERS: "{{ComplaintNoticeUsers}}",
  CASE_ID: "{{CaseID}}",
  CASE_CONTENT: "{{CaseContent}}",
  INVOICE_ID: "{{InvoiceID}}",
  ASSIGNMENT_CONTENT: "{{AssignmentContent}}",
  SOURCE_EMAIL_TITLE: "{{SourceEmailTitle}}",
  ASSIGNMENT_USER: "{{AssignmentUsers}}",
  NOTIFY_TIME: "{{NotifyTime}}", // 通知時間 *Template 解析時的當下時間*
  UNIT_CODE: "{{UnitCode}}", // 單位編號 *付錢只有門市會用到*
  INVOICE_TITLE_21_NODE_NAME: "{{InvoiceTitle21NodeName}}",
  INVOICE_TITLE_NODE_NAME: "{{InvoiceTitleNodeName}}",
  CREATE_USER_NAME: "{{CreateUserName}}",
  CREATE_DATE_TIME: "{{CreateDateTime}}",
  QUESTION_CLASSIFICATION: "{{QuestionClassifiction}}",
  QUESTION_CLASSIFICATION_LEVEL1:"{{QuestionClassifictionLevel1}}",
  QUESTION_CLASSIFICATION_LEVEL2:"{{QuestionClassifictionLevel2}}",
  QUESTION_CLASSIFICATION_LEVEL3:"{{QuestionClassifictionLevel3}}",
  QUESTION_CLASSIFICATION_LEVEL4:"{{QuestionClassifictionLevel4}}"
};

export function tryGetProviderKey(key: string) {
  const providerKey = buProviderList.find(x => x.NodeKey === key);
  if (!providerKey) return commonBu;
  return providerKey.BuName;
}

export function tryGetReportKey(key: string) {
  const providerKey = buReportList.find(x => x.NodeKey === key);
  if (!providerKey) return commonBu;
  return providerKey.BuName;
}

export function tryGetStoreKey(key: string) {
  const providerKey = buStoreList.find(x => x.NodeKey === key);
  if (!providerKey) return commonBu;
  return providerKey.BuName;
}

