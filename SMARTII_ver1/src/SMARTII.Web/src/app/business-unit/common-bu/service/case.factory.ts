import { ICaseFactory } from '../..';
import { CaseComplainedUserViewModel } from 'src/app/model/case.model';
import { Injectable } from '@angular/core';
import { CommonBUKeyPair } from 'src/global';


@Injectable()
export class CaseFactory implements ICaseFactory {

    constructor() { }

    key: string = CommonBUKeyPair.NodeKey;

    getNotificationUserName(users: CaseComplainedUserViewModel[]): string {
       return '';
    }

    getNotificationUserMail(users: any): any[] {
      return [];
    }
}
