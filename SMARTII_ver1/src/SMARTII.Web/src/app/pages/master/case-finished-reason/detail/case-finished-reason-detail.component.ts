import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { CaseFinishDataDetailViewModel } from 'src/app/model/master.model';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';

import * as fromRootActions from 'src/app/store/actions';
import * as fromCaseFinishedReasonActions from '../../store/actions/case-finished-reason.actions';
import { skip, takeUntil } from 'rxjs/operators';
import { OrganizationType } from 'src/app/model/organization.model';

const PREFIX = 'CaseFinishedReasonComponent';

@Component({
  selector: 'app-case-finished-reason-detail',
  templateUrl: './case-finished-reason-detail.component.html',
  styleUrls: ['./case-finished-reason-detail.component.scss']
})
export class CaseFinishedReasonDetailComponent extends FormBaseComponent implements OnInit {

  public form: FormGroup;

  public uiActionType: ActionType;
  public model: CaseFinishDataDetailViewModel = new CaseFinishDataDetailViewModel();
  defaultCheck: boolean;
  titleTypeString:string;

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);

  }

  @loggerMethod()
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
        var payload = new EntrancePayload<{ ID: number }>();
        payload.data = { ID: this.model.ClassificationID }
        payload.success = () => {
          this.store.dispatch(new fromCaseFinishedReasonActions.addAction(this.model));
        }

        (this.model.Default) ?
          this.store.dispatch(new fromCaseFinishedReasonActions.checkSingleAction(payload)) :
          this.store.dispatch(new fromCaseFinishedReasonActions.addAction(this.model))
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
        var payload = new EntrancePayload<{ ID: number }>();
        payload.data = { ID: this.model.ClassificationID }
        payload.success = () => {
          this.store.dispatch(new fromCaseFinishedReasonActions.editAction(this.model));
        }

        (this.model.Default && !this.defaultCheck) ?
          this.store.dispatch(new fromCaseFinishedReasonActions.checkSingleAction(payload)) :
          this.store.dispatch(new fromCaseFinishedReasonActions.editAction(this.model))
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
      .select((state: fromMasterReducer) => state.master.caseFinishedReason.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$)
      )
      .subscribe(caseFinishedReason => {
        this.model = { ...caseFinishedReason };
        this.defaultCheck = this.model.Default;
      });
  }

  initializeForm() {
    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      ClassificationID: new FormControl(this.model.ClassificationID, [
        Validators.required,
      ]),
      Text: new FormControl(this.model.Text, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      IsEnabled: new FormControl(!(this.model.IsEnabled) ? this.model.IsEnabled = true : this.model.IsEnabled, null),
      Default: new FormControl(this.model.Default, null),
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
        this.model.NodeID = parseInt(params['nodeID']);
        this.model.OrganizationType = OrganizationType.HeaderQuarter;
        this.store.dispatch(new fromCaseFinishedReasonActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromCaseFinishedReasonActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromCaseFinishedReasonActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

}
