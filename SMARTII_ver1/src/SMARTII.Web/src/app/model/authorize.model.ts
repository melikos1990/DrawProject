import { OrganizationType, UserListViewModel } from './organization.model';

export enum AuthenticationType {
  None = 0,
  Read = 1 << 0,
  Add = 1 << 1,
  Delete = 1 << 2,
  Update = 1 << 3,
  Report = 1 << 4,
  Admin = 1 << 5,

  All = Read | Add | Delete | Update | Report | Admin
}

export enum GenderType {
  Female = 0,
  Male = 1,
  Other = 2
}

export namespace AuthenticationType {
  export function toArray(n: AuthenticationType) {
    const values: string[] = [];
    while (n) {
      const bit = n & (~n + 1);
      values.push(AuthenticationType[bit]);
      n ^= bit;
    }
    return values;
  }
}

export enum WebType {
  Internet,
  Extranet
}


export type Identity = {

  Account: string;
  Password: string;
  Type: WebType;
};

export type IdentityWrapper = {
  Account: string;
  UserName: string;
  AccessToken: string;
  RefreshToken: string;
  Feature: PageAuth[];
  Role: Role[];
  JobPosition: JobPosition[];
  FavariteFeature: PageAuth[];
  Mode: string;
}


export class Operator {
  Feature: PageAuth[] = []
}

export class resultBox {
  isSuccess: boolean;
  message: string;
}

export class User extends Operator {
  UserID: string;
  Account: string;
  Password: string;
  Name: string;
  Email: string;
  Phone: string;
  IsEnable: boolean;
  IsAD: boolean;
  Feature: PageAuth[] = [];
  Role: Role[] = [];
  JobPosition: JobPosition[] = [];
  ImagePath: string;
  DownProviderBUDist: Map<OrganizationType, number[]> = new Map<OrganizationType, number[]>();
  VerificationCode: string;
}

export class Role extends Operator {
  ID: number;
  Name: string;
  IsEnable: boolean;
  Feature: PageAuth[] = [];
}

export class JobPosition {
  OrganizationType: OrganizationType;
  BUID?: number;
  BUName: string;
  CallCenterID: number;
  CallCenterName: string;
  VendorID: number;
  VendorName: string;
  JobName: string;
  NodeID: number;
  NodeName: string;
  Left: number;
  Right: number;
  ID: number;
}


export class ChangePasswordViewModel {
  Account: string;
  NewPassword: string;
  OldPassword: string;
  ConfirmPassword: string;
}


export class UserSearchViewModel {
  Account: string;
  Name: string;
  IsEnable?: boolean;
  IsAD?: boolean;
  IsSystemUser?: boolean;
  RoleIDs: number[];
  RoleNames: string[];
}


export class RoleSearchViewModel {
  Name: string;
  IsEnable?: boolean;
}

export class RoleDetailViewModel extends Operator {
  Name: string;
  IsEnabled: boolean;
  Feature: PageAuth[] = [];
  UpdateUserName: string;
  UpdateDateTime: string;
  CreateUserName: string;
  CreateDateTime: string;
  Users: UserListViewModel[] = [];
}


export class UserDetailViewModel extends Operator {
  UserID: string;
  Account: string;
  Password: string;
  Name: string;
  Email: string;
  Telephone: string;
  Mobile: string;
  IsEnable: boolean;
  IsAD: boolean;
  Feature: PageAuth[] = [];
  RoleIDs: number[] = [];
  UpdateUserName: string;
  UpdateDateTime: string;
  CreateUserName: string;
  CreateDateTime: string;
  Picture: File;
  ImagePath: string;
  Ext: string;
  IsSystemUser: boolean;
  EnableDateTime: string;
}


export class PageAuth {
  Feature: string;
  AuthenticationType: AuthenticationType;
}


export class PageAuthFavoritePairCollection {
  Left: PageAuthFavorite[] = [];
  Right: PageAuthFavorite[] = [];
}

export class PageAuthFavorite {
  Order: number;
  Feature: string;
  AuthenticationType?: AuthenticationType = AuthenticationType.All;
  Title: string;
}

