import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { UserDetailViewModel, AuthenticationType } from 'src/app/model/authorize.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { State as fromOrganizationReducer } from "../../store/reducers";
import * as fromUserActions from "../../store/actions/user.actions";
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import * as fromRootActions from "src/app/store/actions";
import { NumberValidator, AccountValidator } from 'src/app/shared/data/validator';
import { skip, takeUntil } from 'rxjs/operators';

export const PREFIX = 'UserComponent';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent extends FormBaseComponent implements OnInit, OnDestroy {

  private model$: Subscription;
  public titleTypeString: string;
  public form: FormGroup;

  public validADState?: boolean;
  public uiActionType: ActionType;
  public model: UserDetailViewModel = new UserDetailViewModel();
  public options = {};

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromOrganizationReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  @loggerMethod()
  ngOnInit() {
    this.subscription();
    this.initializeForm();
  }

  @loggerMethod()
  ngAfterViewInit() {
    setTimeout(() => {

      let defalutTime = this.defaultDateTimeRange();

      !(this.model.EnableDateTime) ? this.model.EnableDateTime = defalutTime : this.model.EnableDateTime
    }, 1000);
  }

  //開關重設密碼
  onCheckChangeResetButton(value) {
    this.model.IsAD = value;
  }

  //是否為系統使用者，表單驗證調整
  onCheckChangeResetForm($event) {
    if ($event.target.checked) {
      this.form.controls['Account'].setValidators([
        Validators.required,
        AccountValidator,
        Validators.maxLength(50),
      ]);
    }
    else {
      this.form.controls['Account'].clearValidators();
    }
    this.resetFormControl(this.form);
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!(this.model.IsSystemUser)) {
      this.model.Account = null;
      this.model.RoleIDs = [];
      this.model.EnableDateTime = null;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        var payload = new EntrancePayload<{ ID: string, name: string }>();
        payload.data = {
          ID: null,
          name: this.model.Name
        }
        payload.success = () => {
          this.store.dispatch(new fromUserActions.addAction(this.model));
        }

        this.store.dispatch(new fromUserActions.checkNameAction(payload));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  btnEdit($event) {
    console.log(this.form);
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!(this.model.IsSystemUser)) {
      this.model.Account = null;
      this.model.RoleIDs = [];
      this.model.EnableDateTime = null;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        var payload = new EntrancePayload<{ ID: string, name: string }>();
        payload.data = {
          ID: this.model.UserID,
          name: this.model.Name
        }
        payload.success = () => {
          this.store.dispatch(new fromUserActions.editAction(this.model));
        }

        this.store.dispatch(new fromUserActions.checkNameAction(payload));
      }
    )));
  }


  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  @loggerMethod()
  btnResetPassword() {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
      this.getLoopQuestionMessage(this.translateService.instant('USER.RESET_PASSWORD_QUESTION'),
        () => {
          const payload = new EntrancePayload<string>();
          payload.data = this.model.Account;
          this.store.dispatch(new fromUserActions.resetPasswordAction(payload));
        }
      )));
  }

  subscription() {

    this.active.params.subscribe(this.loadPage.bind(this));

    this.model$ =
      this.store
        .select((state: fromOrganizationReducer) => state.system.user.detail)
        .pipe(
          skip(1),
          takeUntil(this.destroy$)
        )
        .subscribe(user => {
          this.model = { ...user };

          //賦予IsAD值
          const ctrlIsAD = this.form.get('IsAD');
          ctrlIsAD.setValue(this.model.IsAD);

          //若已有帳號且是編輯狀態，則鎖定
          const ctrlAccount = this.form.get('Account');
          (this.uiActionType === ActionType.Update && !!(this.model.Account)) && ctrlAccount.disable();
        });
  }

  initializeForm() {

    this.form = new FormGroup({
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      Telephone: new FormControl(this.model.Telephone, [
        NumberValidator,
        Validators.maxLength(10)
      ]),
      Mobile: new FormControl(this.model.Mobile, [
        NumberValidator,
        Validators.maxLength(10)
      ]),
      Email: new FormControl(this.model.Email, [
        Validators.email,
        Validators.maxLength(255)
      ]),
      IsEnabled: new FormControl(!(this.model.IsEnable) ? this.model.IsEnable = true : this.model.IsEnable, null),
      IsSystemUser: new FormControl(this.model.IsSystemUser, null),
      Account: new FormControl(this.model.Account, null),
      IsAD: new FormControl(this.model.IsAD, null),
      roleIDs: new FormControl(),
      EnableDateTime: new FormControl(this.model.EnableDateTime, null),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
      Ext: new FormControl(this.model.Ext, [
        Validators.maxLength(10)
      ])
    });
  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      UserID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.titleTypeString = this.translateService.instant('USER.TITLE_CREATE');
        this.store.dispatch(new fromUserActions.loadEntryAction());
        break;
      case ActionType.Update:
        this.titleTypeString = this.translateService.instant('USER.TITLE_EDIT');
        this.store.dispatch(new fromUserActions.loadDetailAction(payload));
        break;
      case ActionType.Read:
        this.titleTypeString = this.translateService.instant('USER.TITLE_READ');
        this.store.dispatch(new fromUserActions.loadDetailAction(payload));
        break;
    }

  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }
}
