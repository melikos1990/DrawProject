export enum HQHomeSearchType {
  UnFinishCase,
  UnFinishRejectCase,
  UnFinishUnderCase,
  UnCloseCase
}

export class CcHomeSearchViewModel {
  BuID: number;
  IsAttention: boolean;
  IsSelf: boolean;
}
export class HQHomeSearchViewModel {
  HQHomeSearchType: HQHomeSearchType;
}
export class VendorHomeSearchViewModel {
  HQHomeSearchType: HQHomeSearchType;
}

export class CcHomeListViewModel {
  BuID: number;
  CaseSourceID: number;
  CreateDateTime: string;
  CaseID: number;
  CaseTypeName: string;
  CaseWarningName: string;
  UnitTypeName: string;
  ConcatUserName: string;
  ComplainedUserName: string;
  Content: string;
  ApplyUserName: string;
  UnCloseAssigmentCaseCount: number;
  PromiseDateTime: string;
}