import { CaseRemindType, BillboardWarningType } from './master.model';

export enum PersonalNotificationType{
    NotificationGroup,
    CaseAssign,
    CaseFinish,
    MailAdopt,
    CaseModify,
    MailIncoming,
    Billboard,
    CaseRemind,
    NotificationPPCLife
}


export class OrderPayload {
    ajax: {
        url:string,
        body: any,
        method: "Get" | "Post",
        showBtn: boolean
    };
    
    title: string;
}


export class SystemNotificationViewModel{
    TodayNotification: NotificationPayload;
    BeforeNotification: NotificationPayload;
}

export class NotificationPayload {
    NotificationDatas: any[];
    // PersonalNotificationList: PersonalNotificationListViewModel[];
    // BillboardList: BillboardListViewModel[];
    // CaseRemindList: CaseRemindListViewModel[];
}

export class PersonalNotificationListViewModel{
    ID: number;
    Content: string;
    CreateDateTime: string;
    PersonalNotificationType: PersonalNotificationType;
    Extend: any;
    Count: number;
    
    public parsinExtend<T extends any>(){ return <T>this.Extend; }
}

export class BillboardListViewModel{
    ID: number;
    Content: string;
    Title: string;
    BillboardWarningType: BillboardWarningType;
    BillboardWarningTypeName: string;
    CreateUserName: string;
    CreateDateTime: string;
    ActiveDateTimeRange: string;
    ActiveStartDateTime: string;
    ActiveEndDateTime: string;
    FilePaths: string[];
    PersonalNotificationType: PersonalNotificationType;
}

export class CaseRemindListViewModel{
    ID: number;
    ActiveDateTimeRange: string;
    ActiveEndDateTime: string;
    ActiveStartDateTime: string;
    BuID: number;
    BuName: string;
    CaseID: string;
    Content: string;
    CreateDateTime: string;
    IsConfirm: string;
    AssignmentID: string;
    Level: CaseRemindType;
    LevelName: string;
    PersonalNotificationType: PersonalNotificationType;
}


export class NotficationCalcViewModel {
    SystemNotificationCount: number;
    CaseRemindCount: number;
    OfficialEmailCount: number;
}
