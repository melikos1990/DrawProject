import { Inject, Injectable, InjectionToken } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { timeout,exhaustMap } from 'rxjs/operators';
import { AuthenticationService } from '../service/authentication.service';
import { DEFAULT_TIMEOUT } from '../injection-token';



@Injectable()
export class GlobalInterceptor implements HttpInterceptor {
  constructor(private auth: AuthenticationService,
    @Inject(DEFAULT_TIMEOUT) protected defaultTimeout: number) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {


    const value = Number(req.headers.get('timeout')) || this.defaultTimeout;

    const load$ = this.auth.appendRequest(req).pipe(
      exhaustMap(x => next.handle(x)),
      timeout(value));

    return load$;

  }
}
