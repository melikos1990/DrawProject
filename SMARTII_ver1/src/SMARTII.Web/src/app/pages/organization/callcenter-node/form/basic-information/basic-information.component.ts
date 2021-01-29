import { Component, OnInit, Injector, Input, OnDestroy } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import { CallCenterNodeDetailViewModel } from 'src/app/model/organization.model';
import { FormGroup, FormControl, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import * as fromCallCenterNodeActions from '../../../store/actions/callcenter-node.action';

import * as fromRootActions from 'src/app/store/actions';
import { DualListAjaxOption } from 'src/app/shared/component/other/dual-list/dual-list.component';

const PREFIX = 'CallCenterNodeComponent';

@Component({
  selector: 'app-callcenter-node-basic-information',
  templateUrl: './basic-information.component.html',
  styleUrls: ['./basic-information.component.scss']
})
export class BasicInformationComponent extends FormBaseComponent implements OnInit, OnDestroy {


  @Input() uiActionType: ActionType;


  daulListAjaxOpt: DualListAjaxOption = {
    url: `Common/Organization/GetHeaderQuarterRootNodes?typeStyle=${this.organizationType.HeaderQuarter}&isSelf=false`,
    method: 'Post',
    resSelector: (res) => res ? res.items : []
  };
  isDisabled: boolean = false;
  orgType: string;
  model$: Subscription;
  public model: CallCenterNodeDetailViewModel = new CallCenterNodeDetailViewModel();
  public form: FormGroup;
  public groupForm: FormGroup;

  get isGroupOrgDef() { return this.orgType == this.definitionKey.GROUP; }

  get isCallCenterOrgDef() { return this.model.DefindKey == this.definitionKey.CALLCENTER; }

  constructor(
    public injector: Injector,
    private store: Store<fromOrganizationReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initializeForm();
    this.authVerification();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnEdit($event) {

    let groupVaild = this.isGroupOrgDef ? this.validForm(this.groupForm) : true;

    if (this.validForm(this.form) === false || groupVaild == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.isGroupOrgDef) {
      this.model.WorkProcessType = null;
      this.model.ServingBu = [];
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        const payload = new EntrancePayload<CallCenterNodeDetailViewModel>(this.model);

        this.store.dispatch(new fromCallCenterNodeActions.editAction(payload));
      }
    )));
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.callcenterNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
        this.orgType = this.model.DefindKey;
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
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

    this.groupForm = new FormGroup({
      WorkProcessType: new FormControl(this.model.WorkProcessType, [
        Validators.required
      ]),
      IsWorkProcessNotice: new FormControl(this.model.IsWorkProcessNotice),
      ServingBu: new FormControl(this.model.ServingBu),
    })

  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

  // 如果 節點不是 CALLCENTER 就排除CALLCENTER選項
  nodeDefCompare = (item) => {
    return (this.isCallCenterOrgDef || !this.model.CallCenterID) ? item.extend.Key == this.definitionKey.CALLCENTER : item.extend.Key != this.definitionKey.CALLCENTER
  };


  changeOrgDefType = (ev) => this.orgType = ev ? ev.Key : null;

  authVerification() {
    this.ishasAuth$(this.authType.Update).subscribe(reuslt => {
      this.isDisabled = !reuslt;
    })
  }


}
