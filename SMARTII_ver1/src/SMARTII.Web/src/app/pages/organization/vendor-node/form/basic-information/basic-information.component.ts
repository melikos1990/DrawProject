import { Component, OnInit, Injector, Input, OnDestroy, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import { VendorNodeDetailViewModel } from 'src/app/model/organization.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';

import * as fromRootActions from 'src/app/store/actions';
import * as fromVendorNodeActions from '../../../store/actions/vendor-node.action';
import { DualListAjaxOption, DualListComponent } from 'src/app/shared/component/other/dual-list/dual-list.component';

const PREFIX = 'VendorNodeComponent';


@Component({
  selector: 'app-vendor-basic-information',
  templateUrl: './basic-information.component.html',
  styleUrls: ['./basic-information.component.scss']
})
export class BasicInformationComponent extends FormBaseComponent implements OnInit {

  @ViewChild("dualListComp") listComp: DualListComponent;

  @Input() uiActionType: ActionType;

  orgType: string;
  get isVendorGroupDef() { return this.orgType == this.definitionKey.VENDOR_GROUP }

  get isVendorDef() { return this.model.DefindKey == this.definitionKey.VENDOR }

  model$: Subscription;
  public model: VendorNodeDetailViewModel = new VendorNodeDetailViewModel();
  public form: FormGroup;


  daulListAjaxOpt: DualListAjaxOption = {
    url: `Common/Organization/GetHeaderQuarterRootNodes?typeStyle=${this.organizationType.HeaderQuarter}&isSelf=false`,
    method: 'Post',
    resSelector: (res) => res ? res.items : []
  };

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
        if (!this.isVendorGroupDef) this.model.ServingBu = [];
        const payload = new EntrancePayload<VendorNodeDetailViewModel>(this.model);

        this.store.dispatch(new fromVendorNodeActions.editAction(payload));
      }
    )));
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.vendorNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
        this.orgType = this.model.DefindKey;
      });
  }
  initializeForm() {
    this.model.IsEnabled = true;
    this.form = new FormGroup({
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      IsEnabled: new FormControl(this.model.IsEnabled),
      ServingBu: new FormControl(this.model.ServingBu),
      DefindID: new FormControl(this.model.DefindID, [
        Validators.required
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });
  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

  // 如果 節點不是 VENDOR 就排除VENDOR選項
  nodeDefCompare = (item) => {  
    return (this.isVendorDef || !this.model.VendorID) ? item.extend.Key == this.definitionKey.VENDOR : item.extend.Key != this.definitionKey.VENDOR
  };


  changeOrgDefType = (ev) => this.orgType = ev ? ev.Key : null;
}
