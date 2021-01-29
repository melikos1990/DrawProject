import { GenderType } from './authorize.model';
import { CaseMutipleTicketType } from './case.model';
import { CaseFinishClassificationListViewModel } from './master.model';

export enum OrganizationType {
  HeaderQuarter,
  CallCenter,
  Vendor
}

export enum UnitType {
  Customer,
  Store,
  Organization
}

export enum WorkProcessType {
  Individual,
  Accompanied
}

export class BusinessLayouts {
  StoreQueryLayout : string;
  StoreDetailLayout : string;
  ItemQueryLayout : string;
  ItemDetailLayout : string;
  CaseOtherLayout : string;
  CaseFinishReasonClassifications : CaseFinishClassificationListViewModel[] = []
}

export class BusinesssUnitParameters {
  NodeKey : string;
  BuID: number;
  CanFastFinished: boolean;
  CanFinishReturn: boolean;
  CanGetAssignmentUser: boolean;
  MutipleTicketType: CaseMutipleTicketType
  CaseComplaintInvoiceTypes: { id: string, text: string }[];
}

export class OrganizationDataRangeSearchViewModel {
  Goal: OrganizationType;
  IsSelf: boolean = false;
  NodeID?: number;
  DefKey?: string;
  NotIncludeDefKey?: string[];
  IsStretch: boolean = false;
  IsEnabled: boolean;
}

export class UserListViewModel {
  UserID: string;
  IsEnabled: string;
  IsAD: string;
  IsSystemUser: string;
  UserName: string;
  Account: string;
  CreateUserName: string;
  CreateDateTime: string;
  RoleNames: string[];
  Email: string;
  Mobile: string;
  Telephone: string;
  Address: string;
  JobName: string;
  JobID?: string;
  NodeJobID: number;
  Level?: number;
  NodeID: number;
  NodeName: string;
  OrganizationType: OrganizationType;
}

export class VendorUserListViewModel extends UserListViewModel {

  VendorName: string;
  VendorID?: number;

}

export class CallCenterUserListViewModel extends UserListViewModel {

  CallCenterName: string;
  CallCenterID?: number;
}

export class HeaderQuarterUserListViewModel extends UserListViewModel {

  BUName: string;
  BUID?: number;
}

export class NodeDefinitionSearchViewModel {
  Name: string;
  IdentificationName: string;
  Identification?: number;
  OrganizationType?: OrganizationType;
  IsEnabled: boolean;
}

export class NodeDefinitionListViewModel {

  ID: number;
  OrganizationType: OrganizationType;
  OrganizationTypeName: string;
  Name: string;
  CreateDateTime: string;
  CreateUserName: string;
  IsEnabled: string;
  IdentificationName: string;

}

export class NodeDefinitionDetailViewModel {
  ID: number;
  OrganizationType: OrganizationType;
  Name: string;
  Level: number;
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  Identification?: number;
  IdentificationName: string;
  IsEnabled: boolean;
  Key: string;
  Jobs: Array<JobListViewModel> = [];
}

export class JobSearchViewModel {
  Name: string;
  DefinitionID?: number;
  OrganizationType?: OrganizationType;
}

export class JobListViewModel {
  Name: string;
  OrganizationType?: OrganizationType;
  ID: number;
  DefinitionID: number;
  IsEnabled: string;
  Level: number;
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  Key: string;
}

export class JobDetailViewModel {
  Name: string;
  OrganizationType?: OrganizationType;
  ID: number;
  DefinitionID: number;
  IsEnabled: boolean;
  Level: number;
  CreateDateTime: string;
  CreateUserName: string;
  UpdateDateTime: string;
  UpdateUserName: string;
  Key: string;
}

export class EnterpriseDetailViewModel {
  Name: string;
  IsEnabled: boolean;
  UpdateUserName: string;
  UpdateDateTime: string;
  CreateUserName: string;
  CreateDateTime: string;
}

export class EnterpriseSearchViewModel {
  Name: string;
}



export class NodeJobListViewModel {
  ID: number;
  NodeJobID: number;
  Level: string;
  NodeID: number;
  IsEnableName: string;
  Name: string;
  Users: NodeJobUserListViewModel[] = [];
}


export class NodeJobUserListViewModel {
  UserID: string;
  IsEnabled: string;
  UserName: string;
  IsSystemUser: string;
}

export class AddUserViewModel {
  NodeJobID: number;
  UserIDs: string[] = []
}

export class AddJobViewModel {
  NodeID: number;
  JobIDs: number[] = []
}

export class OrganizationNodeDetailViewModel {
  ID: number;
  Name: string;
  DefindID?: number;
  DefindName: string;
  DefindKey: string;
  IsEnabled: boolean;
  UpdateUserName: string;
  UpdateDateTime: string;
  CreateUserName: string;
  CreateDateTime: string;
  NodeTypeKey: string;
  Jobs: NodeJobListViewModel[] = [];
}

export class VendorNodeDetailViewModel extends OrganizationNodeDetailViewModel {
  VendorID: number;
  ServingBu: number[];
  VendorName: string;
}

export class CallCenterNodeDetailViewModel extends OrganizationNodeDetailViewModel {
  WorkProcessType: WorkProcessType;
  IsWorkProcessNotice: boolean = false;
  ServingBu: number[];
  CallCenterID: number;
  CallCenterName: string;
}

export class HeaderQuarterNodeDetailViewModel extends OrganizationNodeDetailViewModel {
  BUID?: number;
  BUkey: string;
  BUName: string;
  EnterpriseID?: number;
  Jobs: NodeJobListViewModel[] = [];
  StoreName: string;
  StoreCode: string;
  CanModifyBuCode: boolean;
  NodeKey: string;
}

export class OrganizationNodeViewModel {
  ID: number;
  Name: string;
  DefindID?: number;
  DefindName: string;
  LeftBoundary: number;
  RightBoundary: number;
  OrganizationType: OrganizationType;
  Level: number;
  IsPresist: boolean;
  Children: Array<OrganizationNodeViewModel> = [];
  IsEnabled: boolean;
  Target: boolean;  
}

export class VendorNodeViewModel extends OrganizationNodeViewModel {
  VendorID?: number;
  VendorName: string;
}
export class CallCenterNodeViewModel extends OrganizationNodeViewModel {
  CallCenterID?: number;
  CallCenterName: string;
}

export class HeaderQuarterNodeViewModel extends OrganizationNodeViewModel {
  EnterpriseID?: number;
  BUID?: number;
  BUName: string;
}

export class SummaryTargetViewModel {
  TargetFrom: string;
  TargetName: string;
  TargetID: number;
}

export class OrganizationUserViewModel {

  UserID: string;
  UserName: string;
  NodeName: string;
  NodeID?: number;
  JobID?: number;
  JobName: string;
  StoreNo : string;
  BUID?: number;
  BUName: string;
  OrganizationType?: OrganizationType;
  UnitType?: UnitType;
  OrganizationTypeName: string;
  ParentPathName: string;
}

export class ConcatableUserViewModel extends OrganizationUserViewModel {

  NotificationBehavior: string;
  NotificationKind: string;
  NotificationRemark: string;
  Email: string;
  Mobile: string;
  Telephone: string;
  TelephoneBak: string;
  Address: string;
  Gender?: GenderType;

  key: string;
}

export class NotificationGroupUser extends ConcatableUserViewModel {
  ID: number;
  GroupID: number;

}

