import { Component, OnInit, Injector, Input, OnDestroy } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import { HeaderQuarterNodeDetailViewModel } from 'src/app/model/organization.model';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';

import * as fromRootActions from 'src/app/store/actions';
import * as fromHeaderQuarterNodeActions from '../../../store/actions/headerquarter-node.action';

const PREFIX = 'HeaderquarterNodeComponent';

@Component({
  selector: 'app-headerquarter-node-basic-information',
  templateUrl: './basic-information.component.html',
  styleUrls: ['./basic-information.component.scss']
})
export class BasicInformationComponent extends FormBaseComponent implements OnInit, OnDestroy {


  @Input() uiActionType: ActionType;

  orgType: string;
  get isBuOrgDef() { return this.model.DefindKey == this.definitionKey.BU }

  model$: Subscription;
  public model: HeaderQuarterNodeDetailViewModel = new HeaderQuarterNodeDetailViewModel();
  public form: FormGroup;

  constructor(
    public injector: Injector,
    private store: Store<fromOrganizationReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initializeForm();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnEdit($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        const payload = new EntrancePayload<HeaderQuarterNodeDetailViewModel>(this.model);

        // console.log("this.model => ", this.model);
        this.store.dispatch(new fromHeaderQuarterNodeActions.editAction(payload));
      }
    )));
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.headerQuarterNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
        this.orgType = this.model.DefindKey; // 重整狀態

      });
  }
  initializeForm() {
    this.form = new FormGroup({
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      IsEnabled: new FormControl(this.model.IsEnabled),
      DefindID: new FormControl(this.model.DefindID, [
        Validators.required
      ]),
      StoreName: new FormControl(),
      StoreCode: new FormControl(),
      EnterpriseID: new FormControl(),
      BUkey: new FormControl(),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });


  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

  // 如果 節點不是 BU 就排除BU選項
  nodeDefCompare = (item) => {
    return (this.isBuOrgDef || !this.model.BUID) ? item.extend.Key == this.definitionKey.BU : item.extend.Key != this.definitionKey.BU
  };

  changeOrgDefType(ev) {

    this.orgType = ev ? ev.Key : null;

    this.setValid(this.orgType);

  }

  setValid(key: string) {
    this.form.controls["StoreName"].setValidators([]);
    this.form.controls["StoreCode"].setValidators([]);
    this.form.controls["BUkey"].setValidators([]);

    switch (key) {
      case this.definitionKey.STORE:
        this.form.controls["StoreName"].setValidators([Validators.required, Validators.maxLength(50)]);
        this.form.controls["StoreCode"].setValidators([Validators.required, Validators.maxLength(50)]);
        break;

      case this.definitionKey.BU:
        this.form.controls["BUkey"].setValidators([Validators.required, Validators.maxLength(3)]);
        break;

      default:
        break;
    }

  }

}
