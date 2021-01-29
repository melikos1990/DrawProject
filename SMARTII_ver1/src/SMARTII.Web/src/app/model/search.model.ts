import { CaseType, CaseSourceType, CaseAssignmentType, CaseAssignmentComplaintInvoiceType, CaseAssignmentComplaintNoticeType, CaseAssignmentProcessType, CaseAssignmentUserViewModel } from './case.model';
import { UnitType, OrganizationNodeViewModel } from './organization.model';


export enum CaseAssignmentModeType {
    Notice,
    Invoice,
    General,
    Accompanied
}


export class SearchBase {
    NodeID: number;
    NodeName: string;
    CaseConcatUnitType: UnitType;
    ConcatName: string;
    ConcatTelephone: string;
    ConcatEmail: string;
    ConcatStoreName: string;
    ConcatStoreNo: string;
    ConcatNodeName: string;
    CaseComplainedUnitType: UnitType;
    CaseComplainedStoreName: string;
    CaseComplainedStoreNo: string;
    CaseComplainedNodeName: string;
    ClassificationID: number;
    CaseID: string;
}

export class CaseSearchBase extends SearchBase{
    CaseSourceType: CaseSourceType;
    CaseType: CaseType;
    CaseWarningID: number;
    CaseWarningName: string;
    CreateTimeRange: string;
    InvoiceID: string;
    CaseTagList: string[];
    CaseTagIDList: number[];
    CaseContent: string; 
    FinishContent: string;
    ApplyUserID: string;
    ReasonIDs: any[] = [];
    ReasonNames: any[] = [];

    ConcatNode: OrganizationNodeViewModel[];
    ComplainedNode: OrganizationNodeViewModel[];
} 

export type CaseAssigmentState = CaseAssignmentType | CaseAssignmentComplaintInvoiceType | CaseAssignmentComplaintNoticeType;
export class CaseAssigmentSearchBase extends SearchBase{
    CaseSourceType: CaseSourceType;
    CaseType: CaseType;
    CreateTimeRange: string;
    NoticeContent: string;
    NoticeDateTimeRange: string;
    AssignmentType: CaseAssignmentProcessType;
    Type: CaseAssigmentState; // 依據模式不同 有不同狀態

    ConcatNode: OrganizationNodeViewModel[];
    ComplainedNode: OrganizationNodeViewModel[];

    AssignmentUsers: CaseAssignmentUserViewModel[]
    InvoiceID: string;
    InvoiceType: string;
    CaseContent: string;
} 

export class CaseCallCenterSearchViewModel extends CaseSearchBase{
    ExpectDateTimeRange: string;
    ApplyUserName: string;    
}

export class CaseHeaderqurterStoreSearchViewModel extends CaseSearchBase {
  IsBusinessAll: boolean;// 是否是否顯示該BU所有案件(同客服查詢結果，不inner join被反應者Table -(BU查詢功能))
}

export class CaseAssignmentCallCenterSearchViewModel extends CaseAssigmentSearchBase {
}

export class CaseAssignmentHeaderqurterStoreSearchViewModel extends CaseAssigmentSearchBase {
  IsBusinessAll: boolean;// 是否是否顯示該BU所有案件(同客服查詢結果，不inner join被反應者Table-(BU查詢功能))
}

export class CaseAssignmentVendorSearchViewModel extends CaseAssigmentSearchBase {
}
