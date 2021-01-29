import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from '../../../base/form-base.component';
import { CaseWarningDetailViewModel } from 'src/app/model/master.model';

import { ActivatedRoute } from '@angular/router';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ActionType } from 'src/app/model/common.model';
import * as fromCaseWarningActions from '../../store/actions/case-warning.actions';
import { State as fromMasterReducer } from '../../store/reducers';
import { Store } from '@ngrx/store';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';
import { takeUntil, skip } from 'rxjs/operators';
import { NumberValidator, PositiveNumberValidator } from 'src/app/shared/data/validator';

const PREFIX = 'CaseWarningComponent';

@Component({
  selector: 'app-case-warning-detail',
  templateUrl: './case-warning-detail.component.html',
  styleUrls: ['./case-warning-detail.component.scss']
})
export class CaseWarningDetailComponent extends FormBaseComponent implements OnInit {

  public form: FormGroup;

  public uiActionType: ActionType;
  public model: CaseWarningDetailViewModel = new CaseWarningDetailViewModel();
  titleTypeString: string = "";

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);

  }

  ngOnInit() {
    this.initializeForm();
    this.subscription();
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromCaseWarningActions.addAction(this.model));
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

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromCaseWarningActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.store
      .select((state: fromMasterReducer) => state.master.caseWarning.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$)
      )
      .subscribe(caseWarning => {
        this.model = { ...caseWarning };
      });
  }

  initializeForm() {
    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      IsEnabled: new FormControl(!(this.model.IsEnabled) ? this.model.IsEnabled = true : this.model.IsEnabled, null),
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      WorkHour: new FormControl(this.model.WorkHour, [
        Validators.required,
        Validators.maxLength(20),
        PositiveNumberValidator
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.model.NodeID = parseInt(params['NodeID']);
        this.store.dispatch(new fromCaseWarningActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromCaseWarningActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromCaseWarningActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

}
