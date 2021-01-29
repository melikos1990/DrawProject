import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { Actions, Effect } from '@ngrx/effects';
import * as fromRootAction from '../actions';
import { exhaustMap, debounceTime, switchMap, flatMap, map, catchError, tap, filter } from 'rxjs/operators';
import { _failed$, _success$ } from 'src/app/shared/ngrx/alert.ngrx';
import { _route$ } from 'src/app/shared/ngrx/route.ngrx';
import { _entry$ } from 'src/app/shared/ngrx/common.ngrx';
import { of, concat, iif, forkJoin, throwError, merge, Observable, EMPTY } from 'rxjs';
import { User, Identity, IdentityWrapper, PageAuth, Role, ChangePasswordViewModel, JobPosition, resultBox } from 'src/app/model/authorize.model';
import { AspnetJsonResult, AspnetJsonResultBase, ResultPayload, EntrancePayload } from 'src/app/model/common.model';
import { _httpflow$ } from 'src/app/shared/ngrx/http.ngrx';
import { _loadingWork$, _loading$, _unLoading$ } from 'src/app/shared/ngrx/loading.ngrx';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { SignalRHubService } from 'src/app/shared/service/signalR.hub.service';
import { fromPromise } from 'rxjs/observable/fromPromise';
import { LocalStorage as AsyncStorage } from '@ngx-pwa/local-storage';
import { LocalStorageService } from 'src/app/shared/service/local-storage.service';
import { Menu } from 'src/app/model/master.model';
import { TranslateService } from '@ngx-translate/core';
// import * as fromHomePageActions from '../../pages/home/store/actions/home-page.actions';
import { OrganizationType } from 'src/app/model/organization.model';
import { SessionStorageService } from 'src/app/shared/service/session-storage.service';

@Injectable()
export class AuthEffects {

  constructor(
    private translateService: TranslateService,
    private localStorageService: LocalStorageService,
    private http: HttpService,
    private asyncStorage: AsyncStorage,
    private signalRService: SignalRHubService,
    private authenticationService: AuthenticationService,
    private sessionStorageService: SessionStorageService,
    private actions$: Actions) { }



  @Effect()
  login$ = _entry$<Identity>(this.actions$,
    fromRootAction.AuthActions.LOGIN).pipe(
      switchMap((payload) => {

        const retrieve$ = this.http.post<AspnetJsonResult<string>>('/Account/Login', null, payload);

        // 成功時將呼叫 login$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<IdentityWrapper>) =>
          of(new fromRootAction.AuthActions.loginSuccessAction(result.element));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<IdentityWrapper>) =>
          of(new fromRootAction.AuthActions.loginFailedAction({
            mode: result.element ? result.element.Mode : null,
            message: result.message
          }));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);

      }),
      catchError(err => {

        return of(new fromRootAction.AuthActions.loginFailedAction(err));
      }
      ));

  @Effect()
  loginSuccess$ = _entry$<IdentityWrapper>(this.actions$,
    fromRootAction.AuthActions.LOGIN_SUCCESS)
    .pipe(
      exhaustMap((payload) => {

        // 帶入預設腳色
        if (!!payload.Role && payload.Role.length > 0) {
          this.authenticationService.setCacheRoleID(payload.Role[0].ID);
        }

        // 存放 token
        this.authenticationService.setAccessToken(payload.AccessToken);
        this.authenticationService.setRefreshToken(payload.RefreshToken);

        if (!!this.authenticationService.getAccountIsRememberToken()) {
          this.authenticationService.setAccountIDToken(payload.Account);
        }
        else {
          this.authenticationService.removeAccountID();
        }


        // 存放 menu 、 job...
        const userMenu$ = this.authenticationService.setUserMenu(payload.Feature);
        const role$ = this.authenticationService.setUserRoles(payload.Role);
        const jobPosition$ = this.authenticationService.setUserJonPosition(payload.JobPosition);
        const userFavorite$ = this.authenticationService.setUserFavorite(payload.FavariteFeature);

        const direct$ = _route$('./pages/home/home-page', {});

        // const signalr$ = fromPromise(this.signalRService.initialization(payload.Account))
        //   .pipe(
        //     exhaustMap(x => concat(direct$))
        //   );

        const cache$ = forkJoin(userMenu$, role$, jobPosition$, userFavorite$);

        return cache$.pipe(exhaustMap(() => concat(direct$)));
      }),
      catchError(err => of(new fromRootAction.AuthActions.loginFailedAction(err))
      ));

  @Effect()
  authDeny$ = _entry$<any>(this.actions$,
    fromRootAction.AuthActions.AUTH_DENY).pipe(
      exhaustMap((payload: string) => {
        const clearAuth$ = of(new fromRootAction.AuthActions.clearAuthAction());
        const direct$ = _route$('./login', {});
        const failedPopup$ = _failed$(payload || '無權限');

        this.signalRService.stop();

        return concat(failedPopup$, direct$, clearAuth$);
      }
      )
    );
  @Effect()
  parseAuth$ = _entry$<string>(this.actions$,
    fromRootAction.AuthActions.PARSE_AUTH)
    .pipe(
      exhaustMap(() => {

        // 使用者重新組入
        const accessToken = this.authenticationService.getAccessToken();

        // 解析TOKEN USER
        const user = this.authenticationService.parseTokenUser(accessToken);

        // 將解析後的user 資料放置在store 中
        const tokenMember$ = of(new fromRootAction.AuthActions.tokenMemberAction(user));


        const userMenu$ = this.authenticationService.getUserMenu();
        const roles$ = this.authenticationService.getRoles();
        const jobPosition$ = this.authenticationService.getJobPosition();
        const userFavorite$ = this.authenticationService.getUserFavorite();
        const cache$ = forkJoin(userMenu$, roles$, jobPosition$, userFavorite$);

        const parser$ = cache$.pipe(
          exhaustMap((cache: [PageAuth[], Role[], JobPosition[], PageAuth[]]) => {

            const userMenu = cache[0];
            const roles = cache[1];
            const jobPosition = cache[2];
            const userFavorite = cache[3];
            const nodes = this.cacheMenuNode(userMenu, roles, userFavorite);
            return concat(
              of(new fromRootAction.AuthActions.CacheMenuAction(nodes)),
              of(new fromRootAction.AuthActions.CacheJobPositionAction(jobPosition)));
          }
          ));


        return concat(tokenMember$, parser$);
      })
    );



  @Effect()
  loginFailed$ = _entry$<{ mode?: string, message: string }>(this.actions$,
    fromRootAction.AuthActions.LOGIN_FAILED)
    .pipe(
      flatMap(payload => {
        const data: resultBox = { isSuccess: true, message: payload.message };
        if (payload.mode === 'change') {
          return of(new fromRootAction.AuthActions.changePasswordDisplay(data));
        }

        return _failed$(payload.message);
      })
    );

  @Effect()
  logoff$ = _entry$(this.actions$, fromRootAction.AuthActions.LOGOFF)
    .pipe(
      flatMap(() => {

        const direct$ = _route$('./login', {});
        const clearAuth$ = of(new fromRootAction.AuthActions.clearAuthAction());
        const clearAleart$ = of(new fromRootAction.AlertActions.clearAction());
        this.signalRService.stop();
        return concat(direct$, clearAuth$, clearAleart$);
      })
    );

  @Effect({ dispatch: false })
  clearAuth$ = _entry$(this.actions$, fromRootAction.AuthActions.CLEAR_AUTH)
    .pipe(
      tap(() => {

        this.asyncStorage.clear().subscribe((result) => {
          this.authenticationService.removeAccessToken();
          this.authenticationService.removeRefreshToken();
          this.authenticationService.removeCacheRoleID();
          this.sessionStorageService.clear();
        });


      })
    );

  @Effect({ dispatch: false })
  activateExistIdentity$ = _entry$(this.actions$, fromRootAction.AuthActions.ACTIVATE_EXIST_IDENTITY)
    .pipe(
      exhaustMap((payload) => {

        const accessToken = this.authenticationService.getAccessToken();

        if (!accessToken) {
          return throwError("not found exist accesstoken");
        }

        const user = this.authenticationService.parseTokenUser(accessToken);

        if (!user) {
          return throwError("exist accesstoken parse error");
        }

        const signalr$ = fromPromise(this.signalRService.initialization(user.Account));

        return signalr$;

      }),
      catchError(err => of(new fromRootAction.AuthActions.authDenyAction(err))
      ));


  @Effect()
  resetPassword$ = _entry$<EntrancePayload<ChangePasswordViewModel>>(this.actions$,
    fromRootAction.AuthActions.RESET_PASSWORD).pipe(
      switchMap((payload) => {

        const retrieve$ = this.http.post<AspnetJsonResult<string>>('Account/ResetPassword', null, payload.data);

        // 成功時將呼叫 login$ 進行後續行為
        const handleSuccess = (result: AspnetJsonResult<string>) =>
          of(new fromRootAction.AuthActions.resetPasswordSuccessAction(
            {
              data: { message: result.message },
              cb: payload.success
            }
          ));

        // 失敗時將呼叫 loadDetailFailed$ 進行後續行為
        const handleFailed = (result: AspnetJsonResult<string>) =>
          of(new fromRootAction.AuthActions.resetPasswordFailedAction({
            data: { message: result.message },
            cb: payload.failed
          }));

        // 判斷是否成功或是失敗
        const consider = (result: AspnetJsonResultBase) => result.isSuccess;

        // 實際進行http 行為
        const work$ = _httpflow$(handleSuccess, handleFailed, retrieve$, consider);

        return _loadingWork$(work$);

      }),
      catchError(err => {
        return of(new fromRootAction.AuthActions.resetPasswordFailedAction(err));
      }
      ));


  @Effect()
  resetPasswordSuccess$ = _entry$<ResultPayload<{ message: string }>>(this.actions$,
    fromRootAction.AuthActions.RESET_PASSWORD_SUCCESS).pipe(
      exhaustMap((payload) => {

        payload.cb && payload.cb(payload.data.message);
        // const popup$ = _success$(`${payload.data.message} ,${this.translateService.instant('CHANGE_PASSWORD.RE_LOGIN_HINT')}`);

        // return concat(closeModal$, popup$);

        return EMPTY;
      }),
      catchError(err => {
        return of(new fromRootAction.AuthActions.resetPasswordFailedAction(err));
      }
      ));

  @Effect()
  resetPasswordFailed$ = _entry$<ResultPayload<{ message: string }>>(this.actions$,
    fromRootAction.AuthActions.RESET_PASSWORD_FAILED).pipe(
      switchMap((payload) => {

        return _failed$(payload.data.message);

      }),
      catchError(err => {
        return of(new fromRootAction.AuthActions.resetPasswordFailedAction(err));
      }
      ));

  cacheMenuNode(userMenu, roles: Array<Role>, favorites: Array<PageAuth>): Menu[] {

    // 找到目前存取的currentRole
    const cacheRoleID = this.localStorageService.get('role');

    if (cacheRoleID == null) {
      return this.authenticationService.parseMenuNode(userMenu);
    }

    if (roles == null) {
      return this.authenticationService.parseMenuNode(userMenu);
    }

    const currentRole = roles.find(x => x.ID == cacheRoleID);

    return this.authenticationService.parseMenuNode(currentRole ? currentRole.Feature : null, favorites);

  }

}
