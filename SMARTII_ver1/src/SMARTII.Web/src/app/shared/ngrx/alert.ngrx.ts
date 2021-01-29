import * as fromRootAction from '../../store/actions'
import { of } from "rxjs";
import { PtcSwalType } from 'ptc-swal';
import { AppInjector } from 'src/global';
import { TranslateService } from '@ngx-translate/core';
import { ItemExportViewModel } from 'src/app/model/master.model';
import { LocalTableComponent } from '../component/table/local-table/local-table.component';
import { TemplateRef } from '@angular/core';


export const successAlertAction = (message, confirm?: (res: any) => void, cancel?: () => void) => {

  const translateService = AppInjector.get<TranslateService>(TranslateService);

  return new fromRootAction.AlertActions.alertOpenAction(
    {
      detail: {
        title: translateService.instant('SUCCESS.TITLE'),
        text: translateService.instant('SUCCESS.SUCCESSED', { message }),
        type: PtcSwalType.success,
      },
      isLoop: false,
      confirm: confirm,
      cancel: cancel

    });
}

export const failedAlertAction = (message: string, confirm?: (res: any) => void, cancel?: () => void) => {

  const translateService = AppInjector.get<TranslateService>(TranslateService);

  return new fromRootAction.AlertActions.alertOpenAction(
    {
      detail: {
        title: translateService.instant('ERROR.TITLE'),
        text: translateService.instant('ERROR.FAILED', { message }),
        type: PtcSwalType.error,
      },
      isLoop: false,
      confirm: confirm,
      cancel: cancel

    }
  );
}

export const failedExportAlertAction = (message: string, tem: TemplateRef<any>, data?: ItemExportViewModel[]) => {

  const translateService = AppInjector.get<TranslateService>(TranslateService);
  return new fromRootAction.AlertActions.CustomerAlertOpenAction({
    detail: {
      title: message || translateService.instant('ERROR.EXPORT_ERROR'),
      type: PtcSwalType.error,
      cancel: () => {}
    },
    data,
    templateRef: tem,
  });
}

export const questionAlertAction = (message: string, confirm?: (res: any) => void, cancel?: () => void) => {

  const translateService = AppInjector.get<TranslateService>(TranslateService);

  return new fromRootAction.AlertActions.alertOpenAction(
    {
      detail: {
        title: translateService.instant('WARNING.TITLE'),
        text: message,
        type: PtcSwalType.question,
        showCancelButton: true
      },
      isLoop: false,
      confirm: confirm,
      cancel: cancel

    }
  );
}
let _success$ = (message: string, confirm?: (res: any) => void, cancel?: () => void) => {

  return of(successAlertAction(message, confirm, cancel));
}
let _failed$ = (message: string, confirm?: (res: any) => void, cancel?: () => void) => {

  return of(failedAlertAction(message, confirm, cancel));
}
let _failedExport$ = (message: string, tem: TemplateRef<any>, data?: ItemExportViewModel[]) => {

  return of(failedExportAlertAction(message, tem, data));
}

let _prompt$ = (message: string, confirm?: (res: any) => void, cancel?: () => void) => {

  return of(questionAlertAction(message, confirm, cancel));
}

export { _success$, _failed$, _prompt$, _failedExport$ };
