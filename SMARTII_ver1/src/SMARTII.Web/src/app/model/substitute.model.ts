import { CaseType } from './case.model';

export enum CaseNoticeType {
  OfficialEmail,
  CaseApply
}

export class CaseApplySearchViewModel {
  ApplyUserID: string;
  CaseID: string;
  Value: string;
  CaseType: CaseType;
  CaseWarningID: number;
  NodeID: number;
  CreateDateTimeRange: string;
}

export class CaseApplyListViewModel {
  ApplyUserID: string;
  ApplyUserName: string;
  CaseID: string;
  CaseType: CaseType;
  CreateDateTime: string;
  CaseWarningName: string;
  NodeID: number;
  NodeName: string;
  SourceID?: number;
}

export class CaseApplyCommitViewModel {
  ApplyUserID: string;
  CaseIDs: string[] = []
}

export class CaseNoticeSearchViewModel {
  CaseNoticeType: CaseNoticeType;
}

export class CaseNoticeListViewModel {

}
