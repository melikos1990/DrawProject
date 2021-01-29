import { Injectable } from '@angular/core';

import { JwtHelperService } from '@auth0/angular-jwt';
import { User, PageAuth, IdentityWrapper, Operator, Role, JobPosition } from 'src/app/model/authorize.model';
import { MENU_ITEMS } from '../data/menu';
import { Menu } from 'src/app/model/master.model';
import { ObjectService } from './object.service';

import { State as fromRootReducers } from "src/app/store/reducers"
import * as fromRootActions from "src/app/store/actions";
import { Store } from '@ngrx/store';
import { LocalStorageService } from './local-storage.service';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of, throwError, iif, concat, forkJoin } from 'rxjs';
import { HttpService } from './http.service';
import { AspnetJsonResult } from 'src/app/model/common.model';
import { exhaustMap, catchError, switchMap, map, pluck, flatMap, mergeMap, mapTo, tap } from 'rxjs/operators';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { globalLang } from 'src/app.config';
import { LocalStorage as AsyncLocalStorage } from "@ngx-pwa/local-storage";
import { HttpRequest } from '@angular/common/http';
import { OrganizationType } from 'src/app/model/organization.model';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private jwtHelper: JwtHelperService = new JwtHelperService();

  constructor(private http: HttpService,
    private asyncLocalStorage: AsyncLocalStorage,
    private store: Store<fromRootReducers>,
    private storageService: LocalStorageService,
    private objectService: ObjectService) {

  }

  onRouteAuthorization(route: ActivatedRouteSnapshot): Observable<boolean> {

    const v$ = of(route).pipe(
      exhaustMap(route => {

        const children = route.routeConfig.children;
        const feature = route.data.feature;

        const main$ = this.getCompleteUser().pipe(
          exhaustMap(user => {

            if (!user) {
              return of(false);
            }

            if (children && children.length > 0) {
              let hasAuth = false;

              const cacheRole = this.getCurrentRole(user);

              const compare: Operator = cacheRole || user;

              for (const pageAuth of compare.Feature) {
                if (children.some(x => compare.Feature.includes(pageAuth))) {
                  hasAuth = true;
                }
              }

              return of(hasAuth);

            } else {

              return this.onClassAuthorization(feature as string);
            }
          }));

        return main$;
      })
    );


    v$.subscribe(result => {

      if (result === false) {
        this.store.dispatch(new fromRootActions.AuthActions.authDenyAction('無功能權限'));
      }
    });


    return v$;

  }

  onClassAuthorization(feature: string): Observable<boolean> {

    const v$ = this.getCompleteUser().pipe(
      map(user => {
        if (!user) {
          return false;
        }
        const cacheRole = this.getCurrentRole(user);

        const compare: Operator = cacheRole || user;

        const type = this.findFeatureAuthentication(compare, feature);

        return type != null;
      })
    );
    return v$;

  }

  onMethodAuthorization(feature: string, authenticationType: AuthenticationType): Observable<boolean> {


    const v$ = this.getCompleteUser().pipe(
      map(user => {
        if (!user) {
          return false;
        }

        const cacheRole = this.getCurrentRole(user);

        const compare: Operator = cacheRole || user;

        return this.hasAuthentication(compare, feature, authenticationType);
      })
    );
    return v$;
  }

  hasAuthentication(operator: Operator, feature: string, authenticationType: AuthenticationType): boolean {

    const userPageAuth = this.findFeatureAuthentication(operator, feature);

    if (userPageAuth == null) return false;

    return (userPageAuth & authenticationType) === authenticationType;
  }

  findFeatureAuthentication(operator: Operator, feature: string) {


    if (!operator.Feature || operator.Feature.length === 0) {
      return null;
    }

    const result = operator.Feature.find(x => `${x.Feature}Component` === feature);

    return result ? result.AuthenticationType : null;
  }

  parseMenuNode(auths: PageAuth[], favorites: PageAuth[] = []): Menu[] {

    const result: Menu[] = [];
    const fullMenuNode = this.objectService.jsonDeepClone(MENU_ITEMS);

    if (!auths || auths.length === 0) {
      return [];
    }

    fullMenuNode.forEach(node => {
      result.push(this.nestedCombinationMenuNode(node, auths));
    })

    // 使用者的所有功能列
    var allFeature = result.filter(x => (!x.children || x.children.length > 0));
    
    // 依據功能列 比對 快速功能清單
    var allFavorite = allFeature.filter(x => x.children && x.children.length > 0)
                      .toFlatten<Menu>(x => x.children)
                      .filter(x => favorites.some(g => `${g.Feature}Component` == x.component))
                      .map(x => ( { title: x.title, link: x.link, component: x.component } as Menu ));

    // 插入 快速功能列   
    allFeature.splice(allFeature.findIndex(x => x.isFavorite) + 1, 0, ...allFavorite);

    return allFeature;
  }

  nestedCombinationMenuNode(menu: Menu, auths: PageAuth[]): Menu {

    const filtered = [];

    if (!menu.children) {
      return menu;
    }

    menu.children.forEach(node => {

      if (node.component) {

        // 檢核節點是否存在
        const exist = auths.some(x => `${x.Feature}Component` === node.component);

        // 如果節點存在 , 加入遞迴
        if (exist) {
          filtered.push(this.nestedCombinationMenuNode(node, auths));
        }

      } else {
        filtered.push(node);
      }

    });
    menu.children = filtered;

    return menu;
  }

  

  getCurrentRole(user: User): Role {

    // 須考慮到cache
    const cacheRoleID = this.getCacheRoleID();

    if (user.Role == null) {
      return null;
    }

    if (cacheRoleID != null) {
      return user.Role.find(x => x.ID == cacheRoleID);
    }

    return null;
  }

  getCompleteUser(): Observable<User> {

    const v$ = of(this.getAccessToken()).pipe(
      switchMap(token => {
        const tokenUser = this.parseTokenUser(token);

        // 取得使用者 menu , role
        const cache$ = forkJoin(this.getUserMenu(), this.getRoles(), this.getJobPosition());

        const result$ = cache$.pipe(
          map((data: [PageAuth[], Role[], JobPosition[]]) => {

            // 取得後回填物件
            if (!tokenUser) {
              return null;
            }
            tokenUser.JobPosition = data[2];
            tokenUser.Role = data[1];
            tokenUser.Feature = data[0];
            return tokenUser;
          }));
        return result$;
      })
    );

    return v$;
  }

  parseTokenUser(token: string): User {

    try {

      if (!token) {
        throw new Error('parse error (00)');
      }

      const jwtDescript = this.jwtHelper.decodeToken(token);

      if (!jwtDescript.user) {
        throw new Error('parse error (01)');
      }
      const user = JSON.parse(jwtDescript.user);

      if (!user) {
        throw new Error('parse error (02)');
      }

      return user;

    } catch (error) {
      console.log(error);
      return null;
    }

  }

  refreshToken$(): Observable<IdentityWrapper> {

    const refreshToken = this.getRefreshToken();

    return this.http.get<AspnetJsonResult<IdentityWrapper>>("Account/RefreshToken", {
      refreshToken: refreshToken
    }).pipe(
      exhaustMap(payload => {

        // 回填 token
        this.setAccessToken(payload.element.AccessToken);
        this.setRefreshToken(payload.element.RefreshToken);

        if (payload.isSuccess === false) {
          return throwError(payload.message);
        }

        // 存放 menu 、 job...
        const userMenu$ = this.setUserMenu(payload.element.Feature);
        const role$ = this.setUserRoles(payload.element.Role);
        const jobPosition$ = this.setUserJonPosition(payload.element.JobPosition);

        const cache$ = forkJoin(userMenu$, role$, jobPosition$).pipe(exhaustMap(() => of(payload.element)));

        return cache$;
      }),
      catchError((error) => {
        return throwError(error);
      })
    );

  }

  appendRequest(request: HttpRequest<any>): Observable<any> {

    return of(request).pipe(exhaustMap(x => {

      const position$ = this.getJobPosition();

      return concat(position$).pipe(map(c => {

        // 取得 accesstoken
        const accessToken = this.getAccessToken();
        const token = (!accessToken) ? '' : 'Bearer ' + accessToken;

        // 取得語系
        const lang = this.getCacheLangKey();

        const position = c ? c.map(y => this.position(y)) : [];
        return x.clone({
          setHeaders: {
            JobPosition: JSON.stringify(position),
            Culture: lang || globalLang,
            Authorization: token,
           
          }
        });
      }));

    }));


  }

  position = (jobPosition: JobPosition) => {

    return {
      ID: jobPosition.ID,
      OrganizationType: jobPosition.OrganizationType,
      
      // 防止 http request too long . 
      // IdentificationID: jobPosition.BUID,
      // LeftBoundary: jobPosition.Left,
      // RightBoundary: jobPosition.Right,
      
    };

  }

  getLanguages = (key: string) => this.storageService.get<any>(key);
  getCacheLangKey = (): string => this.storageService.get<string>('lang');
  getCacheRoleID = (): number => this.storageService.get<number>('role');
  getAccessToken = (): string => this.storageService.get<string>('ngToken');
  getRefreshToken = (): string => this.storageService.get<string>('ngRefreshToken');
  getAccountIDToken = (): string => this.storageService.get<string>('accountID');
  getAccountIsRememberToken = (): boolean => this.storageService.get<boolean>('accountRemember');
  removeAccessToken = () => this.storageService.remove('ngToken');
  removeRefreshToken = () => this.storageService.remove('ngRefreshToken');
  removeCacheRoleID = () => this.storageService.remove('role');
  removeAccountID = () => this.storageService.remove('accountID');
  setLanguages = (key: string, value: any) => this.storageService.set(key, value);
  setCacheLangKey = (key: string) => this.storageService.set('lang', key);
  setCacheRoleID = (id: number) => this.storageService.set('role', id);
  setAccessToken = (token: string) => this.storageService.set('ngToken', token);
  setRefreshToken = (token: string) => this.storageService.set('ngRefreshToken', token);
  setAccountIDToken = (token: string) => this.storageService.set('accountID', token);
  setAccountIsRememberToken = (token: boolean) => this.storageService.set('accountRemember', token);
  setUserMenu = (feature: PageAuth[]): Observable<boolean> => this.asyncLocalStorage.setItem('user-menu', feature);
  setUserJonPosition = (jobPosition: JobPosition[]): Observable<boolean> => this.asyncLocalStorage.setItem('job-position', jobPosition);
  setUserRoles = (roles: Array<Role>): Observable<boolean> => this.asyncLocalStorage.setItem('role', roles);
  setUserFavorite = (feature: PageAuth[]): Observable<boolean> => this.asyncLocalStorage.setItem('user-favorite', feature);
  

  getUserMenu = (): Observable<PageAuth[]> => this.asyncLocalStorage.getItem('user-menu');
  getRoles = (): Observable<Array<Role>> => this.asyncLocalStorage.getItem<Array<Role>>('role');
  getJobPosition = (): Observable<Array<JobPosition>> => this.asyncLocalStorage.getItem<Array<JobPosition>>('job-position');
  getUserFavorite = (): Observable<PageAuth[]> => this.asyncLocalStorage.getItem('user-favorite');
}
