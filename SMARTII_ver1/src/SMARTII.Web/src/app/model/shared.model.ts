import { ConcatableUserViewModel } from './organization.model';


export enum EmailReceiveType {
  Recipient,
  CC,
  BCC
}

export enum NotificationType {
  Email,
  SMS,
  UI,
  WebPush,
  MobilePush,
  Social
}

export class EmailSenderViewModel {

  Sender: ConcatableUserViewModel = new ConcatableUserViewModel();
  Receiver: ConcatableUserViewModel[] = [];
  Cc: ConcatableUserViewModel[] = [];
  Bcc: ConcatableUserViewModel[] = [];
  Attachments: File[] = [];
  FilePaths: string[] = [];
  Title: string = '';
  Content: string = '';
  IsAddCaseAttachment: boolean = false;

}
