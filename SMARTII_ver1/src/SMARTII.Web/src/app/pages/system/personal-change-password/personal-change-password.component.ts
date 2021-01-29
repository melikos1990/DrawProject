import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from '../../base/form-base.component';
import { FormGroup } from '@angular/forms';
import { ChangePasswordViewModel, User } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import * as fromRootActions from 'src/app/store/actions';
import { State as fromAuthReducers } from 'src/app/store/reducers/auth.reducer';
import { Store } from '@ngrx/store';
import { Observable, concat, of } from 'rxjs';
import { EntrancePayload } from 'src/app/model/common.model';
import { _success$, successAlertAction } from 'src/app/shared/ngrx/alert.ngrx';

const PREFIX = 'PersonalChangePasswordComponent';

@Component({
  selector: 'app-personal-change-password',
  templateUrl: './personal-change-password.component.html',
  styleUrls: ['./personal-change-password.component.scss']
})
export class PersonalChangePasswordComponent extends FormBaseComponent implements OnInit {


  user: User;

  form: FormGroup = new FormGroup({});
  model: ChangePasswordViewModel = new ChangePasswordViewModel();

  user$: Observable<User>;

  constructor(public injector: Injector,
    public store: Store<fromAuthReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.getCurrentUser().subscribe(user => this.user = user);

  }



  @loggerMethod()
  btnResetPassword(event) {

    if (this.validPassword() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否更換密碼?',
      () => {
        this.model.Account = this.user.Account;

        const payload = new EntrancePayload<ChangePasswordViewModel>(this.model);
        payload.success = (message: string) => {
          this.store.dispatch(successAlertAction(message));
          this.model = new ChangePasswordViewModel();
        };
        this.store.dispatch(new fromRootActions.AuthActions.resetPasswordAction(payload));
      }
    )));
  }
  @loggerMethod()
  validPassword() {

    let vaild = true;
    if (this.validForm(this.form) == false) {
      vaild = false;
    }

    if (this.model.ConfirmPassword !== this.model.NewPassword) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
        this.getFieldInvalidMessage(this.translateService.instant('CHANGE_PASSWORD.CONFIRMPASSWORD_AND_NEWPASSWORD_DIFFERENT'))))
      vaild = false;
    }

    return vaild;
  }


}
