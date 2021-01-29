import { Component, Injector, Optional, SkipSelf, Inject, InjectionToken, inject, OnDestroy } from '@angular/core';
import { Translations } from 'src/app/shared/translation';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { CultureService } from '../../shared/service/culture.service';
import { CacheService } from '../../shared/service/cache.service';
import { map, takeUntil } from 'rxjs/operators';
import { ActionType } from 'src/app/model/common.model';
import { AuthenticationType, GenderType } from 'src/app/model/authorize.model';
import { OrganizationType, UnitType, WorkProcessType } from '../../model/organization.model';
import { PREFIX_TOKEN } from 'src/app/shared/injection-token';

import { Subject } from 'rxjs';
import { EmailReceiveType, NotificationType } from 'src/app/model/shared.model';
import { CaseTemplateType, BillboardWarningType, NotificationCalcType, CaseAssignGroupType } from 'src/app/model/master.model';
import { definitionKey as DefinitionKey, caseTemplateTag as CaseTemplateTag, caseTemplate as CaseTemplate } from 'src/global';
import { CaseComplainedUserType, CaseAssignmentProcessType, CaseType, CaseAssignmentType, CaseAssignmentComplaintInvoiceType, CaseAssignmentComplaintNoticeType, RejectType, CaseFocusType } from 'src/app/model/case.model';
import { Guid } from 'guid-typescript';
import { PtcAjaxOptions } from 'ptc-server-table';
import { HQHomeSearchType } from 'src/app/model/home.model';
import { SessionStorageService } from 'src/app/shared/service/session-storage.service';
import { CacheConfig } from 'src/app/shared/decorator/searchCache.decorator';
import { FormGroup, FormArray } from '@angular/forms';


@Component({
  selector: 'app-base',
  template: ''
})
export class BaseComponent implements OnDestroy {

  public featrueName: string;
  public destroy$: Subject<number>;

  public cacheService: CacheService;
  public translateService: TranslateService;
  public translations: Translations;
  public sessionStorageService: SessionStorageService;

  public billboardWarningType = BillboardWarningType;
  public caseTemplateType = CaseTemplateType;
  public notificationType = NotificationType;
  public notificationCalcType = NotificationCalcType;
  public emailReceiveType = EmailReceiveType;
  public organizationType = OrganizationType;
  public authType = AuthenticationType;
  public actionType = ActionType;
  public unitType = UnitType;
  public caseAssignGroupType = CaseAssignGroupType;
  public genderType = GenderType;
  public caseComplainedUserType = CaseComplainedUserType;
  public definitionKey = DefinitionKey;
  public caseAssignmentProcessType = CaseAssignmentProcessType;
  public caseType = CaseType;
  public caseAssignmentType = CaseAssignmentType;
  public caseAssignmentInvoiceType = CaseAssignmentComplaintInvoiceType;
  public caseAssignmentNoticeType = CaseAssignmentComplaintNoticeType;
  public rejectType = RejectType;
  public workProcessType = WorkProcessType;
  public guid = Guid;
  public caseTemplateTag = CaseTemplateTag;
  public caseTemplate = CaseTemplate;
  public hQHomeSearchType = HQHomeSearchType; 
  public caseFocusType = CaseFocusType;

  constructor(
    public injector: Injector,
    @Optional() @Inject(PREFIX_TOKEN) public prefix?: string) {

    this.cacheService = this.injector.get(CacheService);
    this.translateService = this.injector.get(TranslateService);
    this.translations = this.injector.get(Translations);
    this.sessionStorageService = this.injector.get(SessionStorageService);
    this.featrueName = prefix;
    const cultureService = this.injector.get(CultureService);
    this.destroy$ = new Subject();

    cultureService.language$.pipe(
      map((language: LangChangeEvent) => language.lang))
      .subscribe(lang => this.translateService.use(lang));
  }

  ngOnDestroy(): void {
    this.destroy$.next(1);
  }

  clearCache() {
    this.sessionStorageService.clear();
  }

  getCache<T>(keys: string[]): T {
    
    let result: any = {};
    
    keys.forEach(key => {
      result[key] = this.prefix ? this.sessionStorageService.get(`${this.prefix}-${key}`) : this.sessionStorageService.get(key);
      
    })

    return result;
  }

  protected resetFormControl(form) {
    for (let control in form.controls) {
      let controlItem = form.controls[control]
      if (
        controlItem instanceof FormGroup ||
        controlItem instanceof FormArray) {
        this.resetFormControl(controlItem);
      }
      form.controls[control].updateValueAndValidity();
    }
  }


}
