import { AuthenticationType } from './authorize.model';

export class SystemParameterSearchViewModel {
  ID: string;
  Key: string;
  Value: string;
  Text: string;
}

export class SystemParameterDetailViewModel {
  ID: string;
  Key: string;
  Value: string;
  Text: string;
  NextValue: string;
  ActiveDateTime: string;
  UpdateUserName: string;
  UpdateDateTime: string;
  CreateUserName: string;
  CreateDateTime: string;
}


export class SystemParameterListViewModel {
  ID: string;
  Key: string;
  Value: string;
  Text: string;
  UpdateUserName: string;
  UpdateDateTime: string;
  CreateUserName: string;
  CreateDateTime: string;
}

export class SystemLogListViewModel {
  FeatureName: string;
  FeatureTag: string;
  CreateUserName: string;
  CreateDateTime: string;
  Content: string;
  CreateUserAccount: string;
  Operator?: AuthenticationType;
}

export class SystemLogSearchModel {
  FeatureName: string;
  FeatureTag: string;
  CreateUserName: string;
  CreateDateTime: string;
  Content: string;
  CreateUserAccount: string;
  Operator?: AuthenticationType;
  CreateDateTimeRange: string;
}
