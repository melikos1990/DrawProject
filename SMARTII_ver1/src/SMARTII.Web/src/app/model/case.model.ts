import { OrganizationType, ConcatableUserViewModel, OrganizationUserViewModel } from './organization.model';
import { CaseTagListViewModel, CaseFinishDataListViewModel, ItemDetailViewModel } from './master.model';
import { EmailSenderViewModel } from './shared.model';


export enum LoadIntentType {
    UseExist = 0,
    Empty = 1,
}

export enum CaseSourceType {
    Email = 0,
    Phone = 1,
    Other = 2,
    StoreEmail = 3,
}

export enum CaseType {
    Filling = 0,
    Process = 1,
    Finished = 2,
}

export enum CaseAssignmentType {
    Assigned = 0,
    Processed = 1,
    Finished = 2
}

export enum CaseAssignmentWorkType {
    General = 0,
    Accompanied = 1,
}
export enum CaseAssignmentProcessType {
    Notice = 0,
    Invoice = 1,
    Assignment = 2,
    Communication = 3
}

export enum RejectType {
    None = 0,
    FillContent = 1,
    Undo = 2
}

export enum CaseComplainedUserType {
    Notice = 0,
    Responsibility = 1,
}

export enum CaseMutipleTicketType {
    One = 1,
    Multiple = 2,
}

export enum CaseAssignmentComplaintInvoiceType {
    Created = 0,
    Sended = 1,
    Cancel = 2
}

export enum CaseAssignmentComplaintNoticeType {
    Noticed = 0
}

export enum CaseFocusType {
    Assignment = 'Assignment',
    Finished = 'FinishContent'
}

export class CaseItemViewModel {
    CaseID: string;
    ItemID: number;
    JContent: any = {};
    Case: CaseViewModel;
    Item: ItemDetailViewModel;
}

export class CaseSourceViewModel {
    NodeID: number;
    SourceID: string;
    OrganizationType: OrganizationType;
    IsTwiceCall: boolean;
    IsPrevention: boolean;
    IncomingDateTime?: Date = new Date();
    Remark: string;
    RelationCaseIDs: string[] = [];
    VoiceID: string;
    VoiceLocator: string;
    CaseSourceType: CaseSourceType;
    CreateUserName: string;
    CreateDateTime: Date;
    UpdateUserName: string;
    UpdateDateTime?: Date;
    GroupID?: number;
    Cases: CaseViewModel[] = [];
    RelationCaseSourceID: string;
    RelationNodeName: string;
    RelationCaseSource: CaseSourceViewModel;
    User: CaseSourceUserViewModel = new CaseSourceUserViewModel();

    navigate?: CaseFocusType;
    key: string;
    FocusCaseID?: string;

}

export class CaseSourceUserViewModel extends ConcatableUserViewModel {
    SourceID: string;
    //CaseSource: CaseSourceViewModel = new CaseSourceViewModel();
}


export class CaseConcatUserViewModel extends ConcatableUserViewModel {
    ID: number;
    CaseID: string;
    Case: CaseViewModel = new CaseViewModel();

    key: string;
}

export class CaseComplainedUserViewModel extends ConcatableUserViewModel {
    ID: number;
    CaseID: string;
    CaseComplainedUserType?: CaseComplainedUserType;
    Case: CaseViewModel = new CaseViewModel();
    OwnerUserName: string;
    OwnerUserPhone: string;
    OwnerUserEmail: string;
    OwnerJobName: string;
    SupervisorUserName: string;
    SupervisorUserPhone: string;
    SupervisorUserEmail: string;
    SupervisorJobName: string;
    SupervisorNodeName: string;
    StoreTypeName: string;
    key: string;
}

export class CaseViewModel {
    CaseID: string;
    NodeID: number;
    OrganizationType: OrganizationType;
    SourceID: string;
    ApplyUserName: string;
    ApplyDateTime?: Date;
    ApplyUserID: string;
    Content: string = '';
    CaseType: CaseType;
    CaseTypeName: string;
    GroupID?: number;
    PromiseDateTime?: Date;
    ExpectDateTime?: Date;
    FilePath: string[];
    Files: File[] = [];
    FinishContent: string;
    FinishFilePath: string[];
    FinishDateTime?: Date;
    FinishEMLFilePath: string;
    FinishReplyDateTime?: Date;
    FinishUserName: string;
    FinishFiles: File[] = [];
    IsReport: boolean = true;
    IsAttension: boolean = false;
    QuestionClassificationID: number;
    CaseWarningID: number;
    CaseWarningName: string;
    RelationCaseIDs: string[];
    CreateDateTime: Date;
    CreateUserName: string;
    UpdateDateTime: Date;
    UpdateUserName: string;
    Particular: any = {};
    JContent: string;
    CaseSource: CaseSourceViewModel = new CaseSourceViewModel();
    CaseTags: CaseTagListViewModel[] = [];
    CaseFinishReasons: CaseFinishDataListViewModel[] = [];
    CaseConcatUsers: CaseConcatUserViewModel[] = [];
    CaseComplainedUsers: CaseComplainedUserViewModel[] = [];
    Items: CaseItemViewModel[] = [];
    LookupUsers: any[];
    CaseTagsMark: any[];   // 案件標籤 (新增時需要)
    QuestionClassificationGroups: any[] = [];
    EditorNodeJobID: number;  // 操作人員組織
    key: string;
    CaseRemindIDs?: number[];
}


export class CaseAssignmentBaseViewModel {
    NodeID: number;
    OrganizationType: OrganizationType;
    CaseID: string;
    CaseAssignmentProcessType: CaseAssignmentProcessType;
    CaseAssignmentProcessTypeName: string;
    FilePath: string[];
    Files: File[];
    Content: string = '';
    NotificationBehaviors: string[];
    NotificationDateTime: Date;
    NotificationUsers: string;
    CreateDateTime: Date;
    CreateUserName: string;
    EMLFilePath: string;
    NoticeUsers: string[];
    EmailPayload: EmailSenderViewModel;
    Case: CaseViewModel = new CaseViewModel();


    ReplyContent: string;      // 廠商or門市回覆
    EditorNodeJobID: number;   // 操作人員組織
}

export class CaseAssignmentViewModel extends CaseAssignmentBaseViewModel {
    AssignmentID: number;
    FinishContent: string;
    FinishFiles: File[];
    FinishFilePath: string[];
    FinishDateTime: Date;
    FinishUserName: string;
    FinishNodeID?: number;
    FinishNodeName: string;
    FinishOrganizationType?: OrganizationType;
    UpdateDateTime: Date;
    UpdateUserName: string;
    CaseAssignmentType: CaseAssignmentType;
    RejectType: RejectType;
    RejectReason: string;
    CaseAssignmentConcatUsers: CaseAssignmentConcatUserViewModel[] = [];
    CaseAssignmentUsers: CaseAssignmentUserViewModel[];
    CaseAssignmentWorkType: CaseAssignmentWorkType;
    CaseAssignmentWorkTypeName: string;
}

export class CaseAssignmentCommunicateViewModel extends CaseAssignmentBaseViewModel {
    ID: number;

}

export class CaseAssignmentComplaintNoticeViewModel extends CaseAssignmentBaseViewModel {
    ID: number;
    CaseAssignmentComplaintNoticeType: CaseAssignmentComplaintNoticeType;
    CaseAssignmentComplaintNoticeTypeName: string;
    Users: CaseAssignmentComplaintNoticeUserViewModel[] = [];

}

export class CaseAssignmentComplaintInvoiceViewModel extends CaseAssignmentBaseViewModel {
    ID: number;
    InvoiceID: string;
    InvoiceType: string;
    InvoiceTypeName: string;
    CaseAssignmentComplaintInvoiceType: CaseAssignmentComplaintInvoiceType;
    CaseAssignmentComplaintInvoiceTypeName: string;
    IsRecall: boolean;
    Users: CaseAssignmentComplaintInvoiceUserViewModel[] = [];
}

export class CaseAssignmentComplaintInvoiceUserViewModel extends ConcatableUserViewModel {
    ID: number;
    InvoiceID: string;
    CaseID: string;
}

export class CaseAssignmentComplaintNoticeUserViewModel extends ConcatableUserViewModel {
    ID: number;
    NoticeID: number;
    CaseID: string;
}

export class CaseAssignmentConcatUserViewModel extends ConcatableUserViewModel {
    ID: number;
    CaseID: string;
    AssignmentID: number;
    CaseComplainedUserType: CaseComplainedUserType;
    CaseAssignment: CaseAssignmentViewModel = new CaseAssignmentViewModel();
}

export class CaseAssignmentUserViewModel extends OrganizationUserViewModel {
    ID: number;
    CaseID: string;
    AssignmentID: number;
    IsApply: boolean;
    CaseComplainedUserType: CaseComplainedUserType;
    CaseAssignment: CaseAssignmentViewModel = new CaseAssignmentViewModel();

    key: string;
}


export class CaseAssignmentOverviewViewModel {
    Index: number;
    CaseAssignmentProcessType: CaseAssignmentProcessType;
    NoticeDateTime: string;
    FinishedContent: string;
    FinishNodeName: string;
    FinishedUserName: string;
    FinishedDateTime: string;
    InvoiceID: string;
    ComplaintNodeNames: string;
    AssignmentTypeName: string;
    AssignmentType: number;
    CaseID: string;
    NoticeID?: number;
    AssignmentID?: number;
    InvoiceIdentityID?: number;
    CommunicateID?: number;
    CaseRemindIDs?: number[];
    NotificationBehaviors?: string;
}

export class CaseAssignmentResumeViewModel {
    ID: number;
    CaseID: string;
    CaseAssignmentID: number;
    Content: string;
    CaseAssignmentType: CaseAssignmentType;
    CaseAssignmentTypeName: string;
    CreateDateTime: string;
    CreateUserName: string;
    CreateNodeID?: number;
    CreateNodeName: string;
    CreateOrganizationType?: OrganizationType;
    CreateOrganizationTypeName: string;
    IsReply: boolean;
}

export class CaseResumeListViewModel {
    CaseID: string;
    Content: string;
    CreateDateTime: string;
    CaseType: string;
    CreateUserName: string;
}

export class OfficialEmailAutoOrderViewModel {
    EachPersonMail: number;
    UserIDs: string[];
    BuID: number;
    GroupID: number;
}

export class OfficialEmailAdminOrderViewModel {
    MessageIDs: string[];
    BuID: number;
    UserID: string;
    GroupID: number;
}

export class OfficialEmailReplyRengeViewModel {
    QuestionID: number;
    EmailContent: string = '';
    FinishContent: string;
    MessageIDs: string[];
    BuID: number;
    GroupID: number;
}

export class OfficialEmailListViewModel {
    Body: string;
    FromAddress: string;
    FromName: string;
    MessageID: string;
    Subject: string;
    CaseID: string;
    FilePath: string;
    BuID: number;
}

export class OfficialEmailSearchViewModel {
    BuID: number;
    Body: string;
    FromAddress: string;
    FromName: string;
    MessageID: string;
    Subject: string;
    DateRange: string;
    HasPath: boolean;
}

export class OfficialEmailAdoptResult<T> {
    SuccessCount: number;
    FailCount: number;
    Extend: T
}

export class OfficialEmailBatchAdoptViewModel {
    MessageIDs: string[];
    BuID: number;
    GroupID: number;
}

export class OfficialEmailAdoptViewModel{
    MessageID: string;
    BuID: number;
    GroupID: number;
}
