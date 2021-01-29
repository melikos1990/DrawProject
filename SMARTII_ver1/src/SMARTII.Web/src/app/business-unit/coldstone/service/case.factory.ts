import { ICaseFactory } from '../..';
import { CaseComplainedUserViewModel, CaseComplainedUserType } from 'src/app/model/case.model';
import { Injectable } from '@angular/core';
import {  ColdStoneKeyPair } from 'src/global';
import { UnitType } from 'src/app/model/organization.model';
import { CaseAssignGroupUserListViewModel } from 'src/app/model/master.model';
import { EmailReceiveType, NotificationType } from 'src/app/model/shared.model';
import { Guid } from 'guid-typescript';


@Injectable()
export class CaseFactory implements ICaseFactory {

    constructor() { }

    public key: string = ColdStoneKeyPair.NodeKey

    public getNotificationUserName(users: CaseComplainedUserViewModel[]): string {
        console.log('CS')
        const targets = users.filter(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)

        if (!targets || targets.length == 0)
            return '';


        let str = '';

        /// BU     被反應      通知對象
        /// CS	    門市	    店長
	    /// CS      單位	   單位連絡人
        targets.forEach(target => {

            if (target.UnitType == UnitType.Store) {
                let _str = '';
                if(target.OwnerUserName) _str += target.OwnerUserName;
                if(target.OwnerJobName) _str += ` ${target.OwnerJobName},`;

                str += _str;
            }

            if (target.UnitType == UnitType.Organization) {
                let _str = '';
                if(target.OwnerUserName) _str += target.OwnerUserName;
                if(target.OwnerJobName) _str += ` ${target.OwnerJobName},`;

                str += _str;
            }

        })
        return str.substr(0, str.length - 1);
    }

    public getNotificationUserMail(users: CaseComplainedUserViewModel[]): CaseAssignGroupUserListViewModel[] {
      console.log('CS')
      const targets = users.filter(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)

      if (!targets || targets.length == 0)
          return null;


      var userList: CaseAssignGroupUserListViewModel[] = [];

      //BU:21, COLDSTONE, DONUT
      targets.forEach(target => {
          //案件被反應者是門市，預設發信給門市OFC
          if (target.UnitType == UnitType.Store) {
              let _user: CaseAssignGroupUserListViewModel = new CaseAssignGroupUserListViewModel;

              if(target.SupervisorUserEmail == null) return;

              if(target.SupervisorUserName) _user.UserName = target.SupervisorUserName;

              _user.Email = target.SupervisorUserEmail;
              _user.NotificationRemark = EmailReceiveType.Recipient.toString();
              _user.NotificationBehavior = NotificationType.Email.toString();
              _user.key = Guid.create().toString();
              userList.push(_user);
          }
          //案件被反應者是組織，預設發信給節點OWNER
          if (target.UnitType == UnitType.Organization) {
              let _user: CaseAssignGroupUserListViewModel = new CaseAssignGroupUserListViewModel;

              if(target.OwnerUserEmail == null) return;

              if(target.OwnerUserName) _user.UserName = target.OwnerUserName;

              _user.Email = target.OwnerUserEmail;
              _user.NotificationRemark = EmailReceiveType.Recipient.toString();
              _user.NotificationBehavior = NotificationType.Email.toString();
              _user.key = Guid.create().toString();
              userList.push(_user);
          }
      })

      return userList;

  }

}
