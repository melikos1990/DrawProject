import { Injectable } from '@angular/core';
import { CanActivate, Router, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot, Route, UrlSegment } from '@angular/router';

import { AuthenticationService } from './authentication.service'

import { State as fromRootReducer } from "../../../app/store/reducers"
import * as fromRootActions from "../../../app/store/actions"

import { Store } from '@ngrx/store';
import { LocalStorageService } from './local-storage.service';
import { User } from 'src/app/model/authorize.model';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(
    public store: Store<fromRootReducer>,
    public router: Router,
    public auth: AuthenticationService,
    public storage: LocalStorageService) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.auth.onRouteAuthorization(route);
  }
}
