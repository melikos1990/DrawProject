import { NbMenuItem } from '@nebular/theme';
import { OrganizationType, ConcatableUserViewModel, UserListViewModel, UnitType } from './organization.model';
import { EmailSenderViewModel } from './shared.model';
import { GenderType, PageAuthFavorite } from './authorize.model';


export enum CaseTemplateType {
  ASSIGNMENT = 'ASSIGNMENT',
  CASE_FINISH= 'CASE_FINISH',
  NOTICE = 'NOTICE',
  Email = 'EMAIL',
  Complaint = 'COMPLAINT',
  CaseFinish = 'CASE_FINISH',
  QC = 'QC'
}

export enum BillboardWarningType {
  General,
  Warning
}

export enum NotificationCalcType {
  ByItem,
  ByQuestion,
  Both,
}

export enum CaseAssignGroupType {
  Normal,
  PPCRepair
}

export enum CaseRemindType {
  General,
  Warning
}

export enum MailProtocolType {
  POP3,
  OFFICE365
}

export enum WorkType {
  WorkOn,
  WorkOff
}

export class Menu extends NbMenuItem {
  component?: string;
  children?: Menu[];
  skip?: boolean = false;
  isFavorite?: boolean = false;
}

export class CaseTemplateParseResultViewModel {
  Content: string;
  EmailTitle: string;
}

export class CaseTemplateParseViewModel {
  NodeID?: number;
  CaseTemplateID?: number;
  ClassificKey?: string;
  IsDefault?: boolean;
  CaseID: string;
  InvoicID: string;
}

export class CaseTemplateSearchViewModel {
  BuID: number;
  Content: string;
  ClassificKey: string;
}

export class CaseTemplateDetailViewModel {
  ID: number;
  BuID: number;
  ClassificKey: string;
  ClassificName: string;
  Title: string;
  Content: string;
  IsDefault: boolean;
  IsFastFinished: boolean;
  EmailTitle: string;
  CreateUserName: string;
  CreateDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;
}


export class CaseTemplateListViewModel {
  ID: number;
  BuID: number;
  BuName: string;
  ClassificKey: string;
  ClassificName: string;
  Title: string;
  Content: string;
  CreateUserName: string;
  CreateDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;
}

export class UserParameterlViewModel {
  UserID: string;
  NavigateOfNewbie: boolean;
  Particular: any = {};
  Picture: File[];
  ImagePath: string;
  FavoriteFeature: PageAuthFavorite[] = [];
}

export class ItemSearchViewModel {
  NodeID?: number;
  Name: string;
  Code: string;
  Particular: any = {};
  IsEnabled: boolean;
  J_Content: string;
}

export class ItemDetailViewModel {
  Description: string;
  ID: number;
  NodeID: number;
  BUName: string;
  Name: string;
  Code: string;
  IsEnabled: boolean;
  Particular: any = {};
  Picture: File[];
  ImagePath: string[];
  CreateUserName: string;
  CreateDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;
  NodeKey: string;
}

export class ItemListViewModel {
  Description: string;
  ID: number;
  NodeID: number;
  BUName: string;
  Name: string;
  Code: string;
  Particular: any;
}

export class ItemExportViewModel {
  BUName: string;
  ID: number;
}

export class KMClassificationNodeViewModel {
  IsRoot: boolean;
  NodeID: number;
  NodeName: string;
  OrganizationType: OrganizationType;
  ClassificationID?: number;
  ClassificationName: string;
  PathName: string;
  Children: KMClassificationNodeViewModel[] = [];
}

export class KMSearchViewModel {
  BuID: number;
  OrganizationType: OrganizationType;
  Keyword: string;
  ClassificationID?: number;

}

export class KMListViewModel {
  Content: string;
  Title: string;
  ClassificationID: number;
  ClassificationName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
}

export class KMDetailViewModel {

  ID: number;
  ClassificationID: number;
  ClassificationName: string;
  Title: string;
  Content: string;
  FilePaths: string[];
  Files: File[]
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  PathName: string;
}



export class FinishReasonDynamicViewModel {
  ClassificationName: string;
  ClassificationID: number;
  FinishReasons: CaseFinishDataDetailViewModel[];
}

export class CaseFinishClassificationDetailViewModel {
  ID: number;
  Title: string;
  IsMultiple: boolean;
  IsEnabled: boolean;
  NodeID: number;
  NodeName: string;
  Order: number;
  OrganizationType: OrganizationType;
  UpdateDateTime: string;
  UpdateUserName: string;
  CreateDateTime: string;
  CreateUserName: string;
  IsRequired: boolean;
}

export class CaseFinishDataListViewModel {
  ClassificationID: number;
  ClassificationName: string;
  ID: number;
  IsEnable: boolean;
  NodeID: number;
  NodeName: string;
  Order: number;
  OrganizationType: OrganizationType;
  Text: string;
  Default: boolean;
}

export class CaseFinishDataSearchViewModel {
  ClassificationID?: number;
  NodeID?: number;
  OrganizationType: OrganizationType;
  Text: string;
}

export class CaseFinishClassificationListViewModel {
  ID: number;
  NodeID: number;
  Order: number;
  OrganizationType: OrganizationType;
  Title: string;
  IsEnabled: boolean;
  IsMultiple: boolean;
  CaseFinishDatas: CaseFinishDataListViewModel[];
}

export class CaseFinishClassificationSearchViewModel {
  Text: string;
  NodeID: number;
  ClassificationID?: number;
}


export class CaseFinishDataDetailViewModel {

  ClassificationID: number;
  ClassificationName: string;
  Default: boolean;
  ID: number;
  IsEnabled: boolean;
  NodeID: number;
  NodeName: string;
  Order: number;
  OrganizationType: OrganizationType;
  Text: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  CreateDateTime: string;
  CreateUserName: string;
}

export class NotificationGroupSenderListViewModel {
  NodeID: number;
  NodeName: string;
  GroupID: number;
  GroupName: string;
  CalcMode: string;
  ActualCount: number;
  ExpectCount: number;
  Targets: string[];
}

export class PPCLifeEffectiveListViewModel {
  PPCLifeArriveType: string;
  InternationalBarcode: number;
  ItemName: string;
  BatchNo: string; 
  ArriveCount: number;
  EffectiveID: number;
  Targets: string[];
  Users: PPCLifeEffectiveUserListViewModel[] = [];
  CommodityName: string; 
}

export class NotificationGroupUserListViewModel extends ConcatableUserViewModel {
  ID: number;
  GroupID: number;
}

export class PPCLifeEffectiveUserListViewModel extends ConcatableUserViewModel {
  ID: number;
  GroupID: number;
}

export class PPCLifeEffectiveCaseListViewModel {
  CaseID: number;
  CaseContent: string;
  EffectiveID: number;
  ItemID: number;  
}

export class NotificationGroupSenderResumeSearchViewModel {
  CreateTimeRange: string;
  NodeID?: number;
  GroupID?: number;
}

export class PPCLifeEffectiveResumeSearchViewModel {
  CreateTimeRange: string;
  NodeID?: number;
}

export class NotificationGroupSenderExecuteViewModel {
  constructor(payload: EmailSenderViewModel, groupID: number) {
    this.Payload = payload;
    this.GroupID = groupID;
  }
  Payload: EmailSenderViewModel;
  GroupID: number;
}

export class PPCLifeEffectiveSenderExecuteViewModel {
  constructor(payload: EmailSenderViewModel, effectiveId: number) {
    this.Payload = payload;
    this.EffectiveID = effectiveId;
  }
  Payload: EmailSenderViewModel;
  EffectiveID: number;
}

export class BillboardSearchViewModel {
  Content: string;
  FirstActivateDateTimeRange: string;
  BillboardWarningType?: BillboardWarningType;
}

export class BillboardListViewModel {
  ID: number;
  Title: string;
  Content: string;
  BillboardWarningType: BillboardWarningType;
  BillboardWarningTypeName: string;
  CreateUserName: string;
  ActiveDateTimeRange: string;
  ActiveStartDateTime: string;
  ActiveEndDateTime: string;
  FilePaths: string[];
  ImagePath: string;
}

export class BillboardDetailViewModel {
  ID: number;
  Title: string;
  Content: string;
  Files: File[];
  FilePaths: string[];
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  BillboardWarningType: BillboardWarningType;
  ActiveDateTimeRange: string;
  UserIDs: string[];
  Users: UserListViewModel[] = [];
}

export class CaseAssignGroupSearchViewModel {
  BuID: number;
}

export class CaseAssignGroupListViewModel {
  BuID: number;
  BuName: string;
  ID: number;
  Name: string;
  CaseAssignGroupType: CaseAssignGroupType;
  CaseAssignGroupTypeName: string;
}

export class CaseAssignGroupDetailViewModel {
  ID: number;
  BuID: number;
  Name: string;
  OrganizationType: OrganizationType;
  CaseAssignGroupType: CaseAssignGroupType;
  CreateUserName: string;
  CreateDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;
  Users: CaseAssignGroupUserListViewModel[] = [];
}

export class CaseAssignGroupUserListViewModel extends ConcatableUserViewModel {
  ID: number;
  GroupID: number;
}

export class NotificationGroupSearchViewModel {
  NodeID?: number;
  CalcMode?: NotificationCalcType;
}

export class NotificationGroupListViewModel {
  ID: number;
  NodeID: number;
  NodeName: string;
  CalcModeName: string;
  Name: string;
  TargetNames: string[];
  AlertCycleDay: number;
  AlertCount: number;
}


export class NotificationGroupDetailViewModel {
  ID: number;
  NodeID: number;
  Name: string;
  AlertCycleDay: number;
  AlertCount: number;
  CalcMode: NotificationCalcType;
  QuestionClassificationID?: number;
  QuestionClassificationName: string;
  QuestionClassificationGroups: any[] = [];
  Users: NotificationGroupUserListViewModel[] = [];
  ItemID?: number;
  ItemName: string;
  CreateUserName: string;
  CreateDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;

}

export class CaseTagSearchViewModel {
  BuID: number;
}


export class CaseTagDetailViewModel {

  BuID: number;
  ID: number;
  IsEnabled: boolean;
  Name: string;
  OrganizationType: OrganizationType;
  CreateUserName: string;
  CreateDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;

}

export class CaseTagListViewModel {
  BuID: number;
  BuName: string;
  ID: number;
  Name: string;
  IsEnabled: string;
}

export class StoresSearchViewModel {
  BuID: number;
  StoreCloseDateTime: string;
  LeftBoundary: number;
  Name: string;
  NodeID?: number;
  StoreOpenDateTime: string;
  RightBoundary: number;
  Particular: any = {};
  
  IsEnable: boolean = true;
}

export class StoresListViewModel {
  NodeID: number;
  BUName: string;
  IsEnabled: string;
  Code: string;
  Name: string;
  Address: string;
  Telephone: string;
  StoreOpenDateTime: string;
  StoreCloseDateTime: string;
}

export class StoresDetailViewModel {
  NodeID: number;
  BuID: number;
  BuName: string;
  Address: string;
  IsEnabled: boolean;
  BehaviorType: number;
  StoreCloseDateTime: string;
  Code: string;
  CreateDateTime: string;
  CreateUserName: string;
  Email: string;
  Name: string;
  StoreOpenDateTime: string;
  StoreTypeName: string;
  ServiceTime: string;
  StoreType: number;
  Telephone: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  Memo: string;
  OwnerNodeJobID: number;
  SupervisorNodeJobID: number;
  Particular: any = {};
  NodeKey: string;
  OwnerUsers: UserListViewModel[] = [];
  OFCUsers: UserListViewModel[] = [];
  NodeParentIDPath: number[] = [];
  DynamicForm: string;
}

export class CaseRemindSearchViewModel {
  CaseID: string;
  CaseAssignmentID: number;
  IsConfirm: boolean;
  ActiveDateTimeRange: string;
  UserIDs: string;
  Level: CaseRemindType;
  CreateDateTimeRange: string;
  CreateUserID: string;
  NodeID: string;
  Content: string;
}

export class CaseRemindListViewModel {
  ActiveDateTimeRange: string;
  ActiveEndDateTime: string;
  ActiveStartDateTime: string;
  BuID: number;
  BuName: string;
  CaseID: string;
  Content: string;
  CreateDateTime: string;
  ID: number;
  Isconfirm: boolean;
  AssignmentID: number;
  LevelName: string;
}

export class CaseRemindDetailViewModel {
  ID: number;
  CaseID: string;
  AssignmentID: number;
  Content: string;
  IsConfirm: boolean;
  Level: CaseRemindType;
  ConfirmUserID: string;
  ConfirmDateTime: string;
  ConfirmUserName: string;
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  ActiveDateTimeRange: string;
  UserIDs: string[] = [];
  Users: UserListViewModel[] = [];
  IsLock: boolean;
}


export class OfficialEmailGroupSearchViewModel {
  BuID: number;
}

export class OfficialEmailGroupListViewModel {
  BuID: number;
  BuName: string;
  ID: number;
  Account: string;
  UserNames: string[] = [];
}

export class OfficialEmailGroupDetailViewModel {
  BuID: number;
  BuName: string;
  ID: number;
  KeepDay: number;
  MailAddress: string;
  MailDisplayName: string;
  OfficialEmail: string;
  Account: string;
  Password: string;
  Protocol: MailProtocolType;
  UpdateDateTime: string;
  UpdateUserName: string;
  CreateDateTime: string;
  CreateUserName: string;
  IsEnabled: boolean;
  Users: UserListViewModel[] = [];
  HostName: string;
  AllowReceive: boolean;
}

export class CaseWarningSearchViewModel {
  NodeID: number;
}

export class CaseWarningListViewModel {
  ID: number;
  Name: string;
  NodeID: number;
  NodeName: string;
  OrganizationType: OrganizationType;
  WorkHour: number;
  Order: number;
  IsEnabled: string;
}

export class CaseWarningDetailViewModel {
  ID: number;
  Name: string;
  NodeID: number;
  OrganizationType: OrganizationType;
  WorkHour: number;
  IsEnabled: boolean;
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
}


export class WorkScheduleSearchViewModel {
  BuID: number;
  YearTime: string;
}

export class WorkScheduleDetailViewModel {
  ID: number;
  DateStr: string;
  Title: string;
  WorkType: WorkType;
  WorkTypeName: string;
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  OrganizationType: OrganizationType;
  BuID: any;
  BuName: string;
}
