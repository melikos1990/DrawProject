import { Component, OnInit, Injector, OnDestroy, EventEmitter, Output } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NodeDefinitionDetailViewModel, HeaderQuarterNodeDetailViewModel } from 'src/app/model/organization.model';
import { State as fromOrganizationReducer } from '../../../store/reducers/';
import * as fromNodeDefinitionActions from '../../../store/actions/node-definition.actions';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { takeUntil, skip } from 'rxjs/operators';

const PREFIX = 'NodeDefinitionComponent';
@Component({
  selector: 'app-node-definition-basic-information',
  templateUrl: './basic-information.component.html',
  styleUrls: ['./basic-information.component.scss']
})
export class BasicInformationComponent extends FormBaseComponent implements OnInit, OnDestroy {


  @Output() actionTypeChanged: EventEmitter<ActionType> = new EventEmitter();

  private model$: Subscription;
  public model = new NodeDefinitionDetailViewModel();
  public form: FormGroup;
  public uiActionType: ActionType;

  items: any[] = [
    { id: "BU", text: "企業別" },
    { id: "STORE", text: "門市" },
    { id: "GROUP", text: "服務群組" },
    { id: "VENDOR", text: "廠商" },
    { id: "CALLCENTER", text: "客服中心" },
    { id: "VENDOR_GROUP", text: "廠商服務群組" }
  ]

  constructor(
    private active: ActivatedRoute,
    public store: Store<fromOrganizationReducer>,
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
        this.store.dispatch(new fromNodeDefinitionActions.addAction(this.model));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  btnEdit($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromNodeDefinitionActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/organization/node-definition',
      params: {}
    }));

  }

  //切換組織型態，清空節點
  onChange($event) {
    this.model.Identification = null;
    this.model.IdentificationName = null;
  }

  //補上識別名稱
  onSelectedChange($event: HeaderQuarterNodeDetailViewModel) {
    if (!$event) {
      return;
    }
    this.model.IdentificationName = $event.Name;
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.model$ =
      this.store
        .select((state: fromOrganizationReducer) => state.organization.nodeDefinition.detail)
        .pipe(
          skip(1),
          takeUntil(this.destroy$)
        )
        .subscribe(nodeDefinition => {
          this.model = { ...nodeDefinition };
        });


  }


  initializeForm() {
    this.form = new FormGroup({
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      Level: new FormControl(this.model.Level, [
        Validators.required
      ]),
      OrganizationType: new FormControl(this.model.OrganizationType, [
        Validators.required
      ]),
      IsEnabled: new FormControl(this.model.IsEnabled),
      Key: new FormControl(this.model.Key, null),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    this.actionTypeChanged.emit(this.uiActionType);

    const payload = {
      ID: params['id'],
      OrganizationType: params['organizationType']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.store.dispatch(new fromNodeDefinitionActions.loadEntryAction());
        this.model.IsEnabled = true;
        break;
      case ActionType.Update:
        this.store.dispatch(new fromNodeDefinitionActions.loadDetailAction(payload));
        break;
      case ActionType.Read:
        this.store.dispatch(new fromNodeDefinitionActions.loadDetailAction(payload));
        break;
    }

  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }
}
