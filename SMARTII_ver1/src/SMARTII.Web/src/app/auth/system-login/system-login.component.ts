import { Component, OnInit, Injector, OnDestroy, ViewChild, TemplateRef, NgModuleRef, ContentChild } from '@angular/core';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import * as fromRootAction from 'src/app/store/actions';
import { State as fromRootReducers } from 'src/app/store/reducers';
import { Store } from '@ngrx/store';
import { User, ChangePasswordViewModel, resultBox } from 'src/app/model/authorize.model';
import * as fromAppActions from "../../store/actions/app.actions";
import * as fromRootActions from "src/app/store/actions";
import { Subscription, concat, of } from 'rxjs';
import { globalLang } from 'src/app.config';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { EntrancePayload, AspnetJsonResultBase } from 'src/app/model/common.model';
import * as fromNotificationActions from "../../store/actions/notification.actions";
import { _success$, successAlertAction } from 'src/app/shared/ngrx/alert.ngrx';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { now } from 'moment';
import { environment } from 'src/environments/environment';
import { HttpService } from 'src/app/shared/service/http.service';
import { VerificationCodeComponent } from 'src/app/shared/component/other/verification-code/verification-code.component';
import { filter } from 'rxjs/operators';
import { PtcSwalType } from 'ptc-swal';
export const PREFIX = 'SystemLoginComponent';

@Component({
  selector: 'app-login',
  templateUrl: './system-login.component.html',
  styleUrls: ['./system-login.component.scss']
})
export class SystemLoginComponent extends FormBaseComponent implements OnInit, OnDestroy {

  @ViewChild(VerificationCodeComponent) verificationCode: VerificationCodeComponent;

  @ViewChild("resetPasswordModel") resetPasswordRef: TemplateRef<any>;
  modelRef: NgbModalRef;

  form: FormGroup;
  resetPasswordform: FormGroup = new FormGroup({});

  culture = globalLang;

  culture$: Subscription;
  modal$: Subscription;

  isRemember: boolean;

  captchaUrl: string = ("Account/VerificationCode?test=" + Date.now()).toHostApiUrl();
  public user = new User();
  public passwordModel = new ChangePasswordViewModel();

  constructor(
    private modalService: NgbModal,
    private store: Store<fromRootReducers>,
    private authenticationService: AuthenticationService,
    public http: HttpService,
    public injector: Injector) {
    super(injector, PREFIX);
  }


  @loggerMethod()
  ngOnInit(): void {
    this.setLang();

    this.initializeForm();

    this.culture$ = this.store.select((state: fromRootReducers) => state.app.cluture)
      .subscribe(this.setLang.bind(this));

    this.modal$ = this.store.select((state: fromRootReducers) => state.auth.changePasswordDisplay).pipe(filter(x => x != null))
      .subscribe(x => (x.isSuccess === true) ? this.displayPasswordModal(x.message) : this.closePasswordModel());

    this.isRemember = this.authenticationService.getAccountIsRememberToken();
    this.user.Account = this.authenticationService.getAccountIDToken();
    console.log("environmentType =====>");
    console.log(environment);
  }


  @loggerMethod()
  btnLogin($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    else if (this.verificationCode.isValid() == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("驗證碼不通過")));
      return;
    }

    this.store.dispatch(new fromRootAction.AuthActions.loginAction({ ...this.user, Type: environment.webType }));

    this.authenticationService.setAccountIsRememberToken(this.isRemember);

  }

  /**
   * 重設密碼 Popup
   */
  @loggerMethod()
  displayPasswordModal(message: string) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
      this.getFieldInvalidMessage(message,
        false,
        () => {

          this.modelRef = this.modalService.open(this.resetPasswordRef, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
        }, null, PtcSwalType.warning
      )));


  }

  @loggerMethod()
  closePasswordModel() {
    this.passwordModel = new ChangePasswordViewModel();
    this.modelRef && this.modelRef.close();
  }
  @loggerMethod()
  closeModel() {
    const data: resultBox = { isSuccess: false, message: "" };

    this.store.dispatch(new fromRootAction.AuthActions.changePasswordDisplay(data));
  }

  initializeForm() {
    this.form = new FormGroup({
      Account: new FormControl(this.user.Account, [
        Validators.required,
      ]),
      Password: new FormControl(this.user.Password, [
        Validators.required,
      ])
    });
  }


  onSelectLanguage($event) {
    this.store.dispatch(new fromAppActions.changeCultureAction(this.culture));
  }

  setLang(lang?: string) {
    if (lang) {
      this.culture = lang;
    } else {
      this.culture = this.authService.getCacheLangKey() || 'zh-tw';
    }
    this.authService.setCacheLangKey(this.culture);
    this.translateService.use(this.culture);
  }


  @loggerMethod()
  btnResetPassword(event) {

    if (this.validPassword() === false) {
      return;
    }

    this.passwordModel.Account = this.user.Account;

    let payload = new EntrancePayload<ChangePasswordViewModel>(this.passwordModel);
    payload.success = (message: string) => {
      const data: resultBox = { isSuccess: false, message: "" };

      this.store.dispatch(new fromRootAction.AuthActions.changePasswordDisplay(data));
      this.store.dispatch(successAlertAction(`${message} ,${this.translateService.instant('CHANGE_PASSWORD.RE_LOGIN_HINT')}`));

    };

    this.store.dispatch(new fromRootActions.AuthActions.resetPasswordAction(payload));

  }
  @loggerMethod()
  validPassword() {
    let vaild = true;
    if (this.validForm(this.resetPasswordform) == false) {
      vaild = false;
    }
    if (this.passwordModel.OldPassword !== this.user.Password) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
        this.getFieldInvalidMessage(this.translateService.instant('CHANGE_PASSWORD.ORIGINPASSWORD_AND_OLDPASSWORD_DIFFERENT'))))
      return false;
    }
    if (this.passwordModel.ConfirmPassword !== this.passwordModel.NewPassword) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
        this.getFieldInvalidMessage(this.translateService.instant('CHANGE_PASSWORD.CONFIRMPASSWORD_AND_NEWPASSWORD_DIFFERENT'))))
      vaild = false;
    }

    return vaild;
  }

  @loggerMethod()
  ngOnDestroy(): void {
    this.culture$ && this.culture$.unsubscribe();
    this.modal$ && this.modal$.unsubscribe();
  }

}
