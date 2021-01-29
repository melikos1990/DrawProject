import { UnitType as UnitTypeEnum } from 'src/app/model/organization.model';
import { GenderType as GenderTypeEnum } from 'src/app/model/authorize.model';
import { WorkType as WorkTypeEnum } from 'src/app/model/master.model';
import { CaseAssignmentModeType as CaseAssignmentModeTypeEnum } from 'src/app/model/search.model';
import { CaseType as CaseTypeEnum, CaseSourceType as CaseSourceTypeEnum } from 'src/app/model/case.model';
import { BillboardWarningType as BillboardWarningTypeEnum } from 'src/app/model/master.model';


export const UnitType = {
    [UnitTypeEnum.Customer]: '消費者',
    [UnitTypeEnum.Organization]: '組織',
    [UnitTypeEnum.Store]: '門市',
};

export const GenderType = {
    [GenderTypeEnum.Female]: '小姐',
    [GenderTypeEnum.Male]: '先生',
    [GenderTypeEnum.Other]: '自訂',
}; 

export const WorkType = {
    [WorkTypeEnum.WorkOff]: '休假日',
    [WorkTypeEnum.WorkOn]: '工作日',
}; 

export const CaseAssignmentModeType = {
    [CaseAssignmentModeTypeEnum.Notice]: '一般通知',
    [CaseAssignmentModeTypeEnum.Invoice]: '反應單',
    [CaseAssignmentModeTypeEnum.General]: '一般銷案',
    [CaseAssignmentModeTypeEnum.Accompanied]: '偕同銷案',
}; 


export const CaseType = {
    [CaseTypeEnum.Filling]: "立案",
    [CaseTypeEnum.Process]: "處理中",
    [CaseTypeEnum.Finished]: "結案",
}

export const CaseSourceType = {
    [CaseSourceTypeEnum.Email]: '來信',
    [CaseSourceTypeEnum.Other]: '來電',
    [CaseSourceTypeEnum.Phone]: '其他',
}; 


export const BillboardWarningType = {
    [BillboardWarningTypeEnum.General]: "一般",
    [BillboardWarningTypeEnum.Warning]: "緊急"
}