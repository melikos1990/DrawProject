import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { loggerClass, loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ActionType } from 'src/app/model/common.model';
import { SystemParameterDetailViewModel } from 'src/app/model/system.model';
import * as fromSystemParameterActions from '../../store/actions/system-parameter.actions';
import { State as fromSystemReducer } from "../../store/reducers";
import { Store } from '@ngrx/store';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';
import { skip, takeUntil } from 'rxjs/operators';


export const PREFIX = 'SystemParameterComponent';

@Component({
  selector: 'app-system-parameter-detail',
  templateUrl: './system-parameter-detail.component.html',
  styleUrls: ['./system-parameter-detail.component.scss']
})
@loggerClass()
export class SystemParameterDetailComponent extends FormBaseComponent implements OnInit, OnDestroy {

  isCheck: boolean = false;
  titleTypeString: string = "";

  public form: FormGroup;

  public uiActionType: ActionType;
  public model: SystemParameterDetailViewModel = new SystemParameterDetailViewModel();

  model$: Subscription;

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromSystemReducer>,
    public injector: Injector) {
    super(injector, PREFIX);

  }

  @loggerMethod()
  ngOnInit() {
    this.subscription();
    this.initializeForm();
  }

  onCheckChange($event) {
    //切換預先設定
    if ($event) {
      this.form.controls['NextValue'].setValidators([Validators.required]);
      this.form.controls['ActiveDateTime'].setValidators([Validators.required]);
      this.form.controls['Value'].clearValidators();
    }
    else {
      this.form.controls['NextValue'].clearValidators();
      this.form.controls['ActiveDateTime'].clearValidators();
      this.form.controls['Value'].setValidators([Validators.required]);
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

    if (!(this.isCheck)) {
      this.model.NextValue = null;
      this.model.ActiveDateTime = null;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromSystemParameterActions.addAction(this.model));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  btnEdit($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!(this.isCheck)) {
      this.model.NextValue = null;
      this.model.ActiveDateTime = null;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromSystemParameterActions.editAction(this.model));
      }
    )));
  }


  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  subscription() {

    this.active.params.subscribe(this.loadPage.bind(this));

    this.model$ =
      this.store
        .select((state: fromSystemReducer) => state.system.systemParameter.detail)
        .pipe(
          skip(1),
          takeUntil(this.destroy$)
        )
        .subscribe(systemParameter => {
          this.model = { ...systemParameter };
          !(this.model.NextValue) ? this.isCheck = false : this.isCheck = true;

          this.onCheckChange(this.isCheck);

        });
  }

  initializeForm() {
    this.form = new FormGroup({
      ID: new FormControl(this.model.ID, [
        Validators.required,
        Validators.maxLength(50),
      ]),
      Key: new FormControl(this.model.Key, [
        Validators.required,
        Validators.maxLength(50),
      ]),
      Text: new FormControl(this.model.Text, [
        Validators.required,
      ]),
      Value: new FormControl(this.model.Value, [
        Validators.required,
      ]),
      IsCheck: new FormControl(),
      NextValue: new FormControl(this.model.NextValue, null),
      ActiveDateTime: new FormControl(this.model.ActiveDateTime, null),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  loadPage(params) {

    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id'],
      Key: params['key']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.store.dispatch(new fromSystemParameterActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromSystemParameterActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromSystemParameterActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

}
