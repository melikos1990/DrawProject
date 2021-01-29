import { Component, Injector, Optional, SkipSelf, Inject, InjectionToken, inject } from '@angular/core';
import { Translations } from 'src/app/shared/translation';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { CultureService } from '../../shared/service/culture.service';
import { map } from 'rxjs/operators';
import { ActionType } from 'src/app/model/common.model';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { BaseComponent } from './base.component';
import { ObjectService } from 'src/app/shared/service/object.service';
import { MENU_ITEMS } from 'src/app/shared/data/menu';
import { Menu } from 'src/app/model/master.model';
import { AuthBaseComponent } from './auth-base.component';
export const PREFIX_TOKEN = new InjectionToken<string>('PREFIX');


@Component({
  selector: 'app-layout-base',
  template: ''
})
export class LayoutBaseComponent extends AuthBaseComponent {


  public authType = AuthenticationType;
  public actionType = ActionType;

  private objectService: ObjectService = this.injector.get(ObjectService);

  constructor(
    public injector: Injector,
    @Optional() @Inject(PREFIX_TOKEN) public prefix?: string) {
    super(injector, prefix);

  }

  getTranslateMenu(lead?: Menu[]): Menu[] {
    const menuList = this.objectService.jsonDeepClone(lead || MENU_ITEMS);
    menuList.forEach(menu => this.recursive(menu));
    return menuList;
  }

  recursive(menu: Menu) {
    menu.title = this.translateService.instant(menu.title);
    if (menu.children && menu.children.length > 0) {
      menu.children.forEach(x => this.recursive(x));
    }

  }
}
