import { ICaseFactory } from '../..';
import { CaseComplainedUserViewModel, CaseComplainedUserType } from 'src/app/model/case.model';
import { Injectable } from '@angular/core';
import { eShopKeyPair } from 'src/global';
import { UnitType } from 'src/app/model/organization.model';


@Injectable()
export class CaseFactory implements ICaseFactory {

    constructor() { }

    public key: string = eShopKeyPair.NodeKey

    public getNotificationUserName(users: CaseComplainedUserViewModel[]): string {
        console.log('es')
        const targets = users.filter(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)

        if (!targets || targets.length == 0)
            return '';


        let str = '';

        /// BU     被反應      通知對象
        /// es	    門市	   門市負責人
        /// es      單位	   單位連絡人
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

    public getNotificationUserMail(users: any): any[] {
      return [];
    }
}
