import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from '../../../base/form-base.component';
import { CaseTagDetailViewModel } from 'src/app/model/master.model';

import { ActivatedRoute } from '@angular/router';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ActionType } from 'src/app/model/common.model';
import * as fromCaseTagActions from '../../store/actions/case-tag.actions';
import { State as fromMasterReducer } from '../../store/reducers';
import { Store } from '@ngrx/store';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';
import { takeUntil, skip } from 'rxjs/operators';

const PREFIX = 'CaseTagComponent';

@Component({
  selector: 'app-case-tag-detail',
  templateUrl: './case-tag-detail.component.html',
  styleUrls: ['./case-tag-detail.component.scss']
})
export class CaseTagDetailComponent extends FormBaseComponent implements OnInit {

  public form: FormGroup;

  public uiActionType: ActionType;
  public model: CaseTagDetailViewModel = new CaseTagDetailViewModel();
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
        this.store.dispatch(new fromCaseTagActions.addAction(this.model));
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
        this.store.dispatch(new fromCaseTagActions.editAction(this.model));
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
      .select((state: fromMasterReducer) => state.master.caseTag.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$)
      )
      .subscribe(caseTag => {
        this.model = { ...caseTag };
      });
  }

  initializeForm() {
    this.form = new FormGroup({
      BuID: new FormControl(this.model.BuID, [
        Validators.required,
      ]),
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      IsEnabled: new FormControl(!(this.model.IsEnabled) ? this.model.IsEnabled = true : this.model.IsEnabled, null),
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
        this.model.BuID = parseInt(params['BuID']);
        this.store.dispatch(new fromCaseTagActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromCaseTagActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromCaseTagActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

}
