
import { Injectable } from '@angular/core';
import { ICaseFactory } from '../..';
import { CaseConcatUserViewModel } from 'src/app/model/case.model';
import { OpenPointKeyPair } from 'src/global';
import { UnitType } from 'src/app/model/organization.model';

@Injectable()
export class CaseFactory implements ICaseFactory {

    key: string = OpenPointKeyPair.NodeKey;


    getNotificationUserName(users: CaseConcatUserViewModel[]): string {

        console.log('openPoint');

        const targets = users.filter(x => x.UnitType == UnitType.Customer);

        if (!targets || targets.length == 0)
            return '';


        let str = '';

        targets.forEach(target => {

            str += target.UserName;

        })


        return str.substr(0, str.length - 1);

    }

    public getNotificationUserMail(users: any): any[] {
      return [];
    }
}
