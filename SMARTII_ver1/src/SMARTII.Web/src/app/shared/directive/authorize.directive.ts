import { Directive, Input, OnInit, HostBinding, ViewContainerRef } from '@angular/core';
import { AuthenticationService } from '../service/authentication.service';
import { LocalStorageService } from '../service/local-storage.service';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { BaseComponent } from 'src/app/pages/base/base.component';

@Directive({
  selector: '[authorize]'
})
export class AuthorizeDirective implements OnInit {

  @HostBinding('style.visibility')
  display: string = 'inherit';

  @Input()
  authorize: AuthenticationType;

  constructor(
    private viewContainerRef: ViewContainerRef,
    private authentication: AuthenticationService) { }

  ngOnInit(): void {

    const view = this.viewContainerRef['_view'];

    if (!view) {
      this.display = 'hidden';
      return;
    }

    const hostComponent = <BaseComponent>view.component;

    const jwtToken = this.authentication.getAccessToken();
    const user = this.authentication.parseTokenUser(jwtToken);

    if (!user || !hostComponent) {
      this.display = 'hidden';
      return;
    }

    this.authentication.onMethodAuthorization(hostComponent.featrueName, this.authorize).subscribe(hasAuth => {
      this.display = hasAuth ? 'inherit' : 'hidden';
    });

  }
}
