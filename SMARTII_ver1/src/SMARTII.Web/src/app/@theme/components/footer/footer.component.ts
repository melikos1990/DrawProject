import { Component, Injector } from '@angular/core';
import { BaseComponent } from '../../../pages/base/base.component';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <span class="created-by">{{ translations.APPLICATION.LICENSE | translate }}</span>

  `,
})
export class FooterComponent extends BaseComponent {
  constructor(injector: Injector) {
    super(injector)
  }
}
