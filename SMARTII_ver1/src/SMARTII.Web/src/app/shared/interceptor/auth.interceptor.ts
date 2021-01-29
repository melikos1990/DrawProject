import { Inject, Injectable, InjectionToken } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, filter, take, switchMap, exhaustMap } from 'rxjs/operators';
import { AuthenticationService } from '../service/authentication.service';
import { State as fromRootReducer } from "src/app/store/reducers"
import * as fromRootAction from "src/app/store/actions"
import { Store } from '@ngrx/store';
import { IdentityWrapper } from 'src/app/model/authorize.model';
import { DEFAULT_REFESH_TOKEN_URL_CHARATOR, DEFAULT_LOGIN_URL_CHARATOR } from '../injection-token';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private refreshTokenSubject: BehaviorSubject<any>;
  private refreshTokenInProgress;

  constructor(private auth: AuthenticationService,
    private store: Store<fromRootReducer>,
    @Inject(DEFAULT_REFESH_TOKEN_URL_CHARATOR) protected defaultRefreshTokenUrl: string,
    @Inject(DEFAULT_LOGIN_URL_CHARATOR) protected defaultLoginUrl: string) {

      this.refreshTokenSubject = new BehaviorSubject<any>(null);
      this.refreshTokenInProgress = false;

  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(req).pipe(

      catchError((error) => {
        // 如果不是401 錯誤 , 不嘗試刷新token
        if (error.status !== 401) {
          return throwError(error);
        }

        if (req.url.includes(this.defaultRefreshTokenUrl) ||
          req.url.includes(this.defaultLoginUrl)) {
          if (req.url.includes(this.defaultRefreshTokenUrl)) {
            this.store.dispatch(new fromRootAction.AuthActions.authDenyAction());
            return throwError(error);
          }

          // 回傳錯誤obeservable
          return throwError(error);
        }

        if (this.refreshTokenInProgress) {

          // 在token更新過程中 , 阻塞原來的observable
          const tokenSubject$ = this.refreshTokenSubject.pipe(
            filter(result => result !== null),
            take(1),
            switchMap(() => this.auth.appendRequest(req).pipe(exhaustMap(x => next.handle(x))))
          );

          return tokenSubject$;

        } else {

          // 嘗試refresh token
          this.refreshTokenInProgress = true;

          this.refreshTokenSubject.next(null);

          let refreshToken$ = this.auth.refreshToken$()
            .pipe(
              switchMap((wrapper: IdentityWrapper) => {

                this.refreshTokenInProgress = false;
                this.refreshTokenSubject.next(wrapper.AccessToken);

                // refresh token 成功後 , 再次發送requrest
                return this.auth.appendRequest(req).pipe(exhaustMap(x => next.handle(x)));

              }),
              catchError((error) => {
                this.refreshTokenInProgress = false;
                this.store.dispatch(new fromRootAction.AuthActions.authDenyAction('TOKEN 交換異常'));
                return throwError('TOKEN 交換異常' + error);
              })
            )

          return refreshToken$;

        }
      })

    )
  }
}
