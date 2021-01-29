
import { Injectable } from '@angular/core';
import { Menu } from 'src/app/model/master.model';
import { ActivatedRoute } from '@angular/router';
import { AppInjector } from 'src/global';
import { ObjectService } from './object.service';

export const menuSelectServiceFactory = (token: string) => {
  switch (token.toLowerCase()) {

    case "ptc":
      return new MenuSelectServices();

    default:
      return new MenuSelectServices();
  }
}

export interface IMenuSelect {
  /**
   * 比對當前頁面屬於那個功能下
   */
  matchItem(path: string, menu: Menu[]): Menu | null;
}

@Injectable({
  providedIn: 'root'
})
export class MenuSelectServices implements IMenuSelect {


  constructor() { }

  matchItem(path: string, menu: any[]) {
    let matchMenu = null;

    menu.forEach(item => {

      // 依據規則對到path , 回傳menu 選取
      if (this.isMatchName(path, item)) {
        matchMenu = item;
        return;
      }

      // path 對不到 , 就遞迴子成員
      if (item.children && item.children.length > 0) {
        const subMatchMenu = this.matchItem(path, item.children);
        if (subMatchMenu) {
          matchMenu = subMatchMenu;
        }
      }

    });

    return matchMenu;
  }

  isMatchName(path, menuItem): boolean {
    const uri = path.split('/');         // 解析路徑
    const suffix = uri[uri.length - 1];  // 取得字尾
    const link = menuItem.link;          // 取得menu 的連結


    // 判斷執行模式
    return (suffix.toLowerCase().indexOf('-detail') > -1) ?
      link && path.toLowerCase().indexOf(link.toLowerCase()) > -1 :
      link && path.toLowerCase() === link.toLowerCase();
  }

}
