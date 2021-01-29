import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { AuthenticationType, User } from 'src/app/model/authorize.model';
import { EnterpriseDetailViewModel } from 'src/app/model/organization.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromEnterpriseActions from "../../store/actions/enterprise.actions";
import { State as fromOrganizationReducer } from "../../store/reducers";
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from "src/app/store/actions";
import { takeUntil } from 'rxjs/operators';

export const PREFIX = 'EnterpriseComponent';

@Component({
  selector: 'app-enterprise-detail',
  templateUrl: './enterprise-detail.component.html',
  styleUrls: ['./enterprise-detail.component.scss']
})
export class EnterpriseDetailComponent extends FormBaseComponent implements OnInit {

  model$: Subscription;

  form: FormGroup;
  titleTypeString: string = "";

  public uiActionType: ActionType;
  public model: EnterpriseDetailViewModel = new EnterpriseDetailViewModel();

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromOrganizationReducer>,
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
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        if (this.validForm(this.form) === false) {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
          return;
        }
        this.store.dispatch(new fromEnterpriseActions.addAction(this.model));
      }
    )));
    

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  btnEdit($event) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        if (this.validForm(this.form) === false) {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
          return;
        }
        this.store.dispatch(new fromEnterpriseActions.editAction(this.model));
      }
    )));

  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  initializeForm() {
    this.form = new FormGroup({
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(50),
      ]),
      IsEnabled: new FormControl(this.model.IsEnabled, null),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.model$ =
      this.store
        .select((state: fromOrganizationReducer) => state.organization.enterprise.detail)
        .pipe(
          takeUntil(this.destroy$)
        )
        .subscribe(enterprise => {
          this.model = { ...enterprise };
        });
  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      EnterpriseID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.store.dispatch(new fromEnterpriseActions.loadEntryAction());
        this.titleTypeString=this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromEnterpriseActions.loadDetailAction(payload));
        this.titleTypeString=this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromEnterpriseActions.loadDetailAction(payload));
        this.titleTypeString=this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

}

