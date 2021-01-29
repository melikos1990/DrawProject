import { Component, Injector, Optional, Inject } from '@angular/core';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { BaseComponent } from './base.component';
import { AuthenticationType, Operator, User } from 'src/app/model/authorize.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PREFIX_TOKEN } from 'src/app/shared/injection-token';
import { OrganizationType } from 'src/app/model/organization.model';

@Component({
  selector: 'app-auth-base',
  template: '',
})
export class AuthBaseComponent extends BaseComponent {

  public authService: AuthenticationService = this.injector.get(AuthenticationService);

  constructor(
    public injector: Injector,
    @Optional() @Inject(PREFIX_TOKEN) public prefix?: string) {
    super(injector, prefix);
  }

  findAuth(operator: Operator, feature: string): AuthenticationType {

    if (!operator.Feature) {
      return null;
    }

    const target = operator.Feature.find(x => x.Feature === feature);

    if (target) {
      return target.AuthenticationType;
    }

    return null;
  }

  public hasAuth(operator: Operator, feature: string, authenticationType: AuthenticationType) {
    return this.authService.hasAuthentication(operator, feature, authenticationType);
  }

  public hasAuth$(authenticationType: AuthenticationType): Observable<boolean> {
    return this.getCurrentUser().pipe(
        map(user => this.hasAuth(user, this.featrueName, authenticationType))
    );
  }

  public ishasAuth$(authenticationType: AuthenticationType): Observable<boolean> {
    return this.getCurrentUser().pipe(
      map(user => {
        if (!user) {
          return false;
        }

        const cacheRole = this.authService.getCurrentRole(user);

        const compare: Operator = cacheRole || user;

        return this.authService.hasAuthentication(compare, this.featrueName, authenticationType);
      })
    );
  }

  public hasNodeAuth(nodeID: number, organizationType: OrganizationType): Observable<boolean> {
    return this.getCurrentUser().pipe(
      map(user => {
        return user.JobPosition.some(
          x => x.NodeID == nodeID &&
               x.OrganizationType == organizationType)
       
      })
    );
  }


  public getCurrentUser(): Observable<User> {
    return this.authService.getCompleteUser();
  }


}
