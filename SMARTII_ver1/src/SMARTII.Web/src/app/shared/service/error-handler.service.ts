import { Injectable, ErrorHandler, Optional, Injector, Inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { viewChangedError, viewDestroyError } from 'src/app.config';

import { Store } from '@ngrx/store';
import { State as fromRootReducers } from "../../store/reducers";
import * as fromRootActions from '../../store/actions';
import { PtcSwalType } from 'ptc-swal';
import { TranslateService } from '@ngx-translate/core';


@Injectable()
export class ErrorHandlerService implements ErrorHandler {

  constructor(private injector: Injector) { }

  handleError(error: any): void {


    const translateService = this.injector.get<TranslateService>(TranslateService);
    const store = this.injector.get<Store<any>>(Store);

    if (error.toString().indexOf(viewChangedError) >= 0 ||
      error.toString().indexOf(viewDestroyError) >= 0) {
      return;
    }

    if (error instanceof HttpErrorResponse) {
      console.error('[client]  Backend returned status code: ', error.status);
      console.error('[client]  Response body:', error.message);
    } else {
      console.error('[client] An error occurred:', error.stack);
      console.error('[client] An error occurred:', error.message);

      store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
        {
          detail: {
            title: translateService.instant('ERROR.TITLE'),
            text: translateService.instant('ERROR.FAILED', { message: error.message }),
            type: PtcSwalType.error,
            showCancelButton: false,
            confirmButtonText: translateService.instant('COMMON.BTN_CHECK'),
          },
          isLoop: false
        }));
    }
  }
}
