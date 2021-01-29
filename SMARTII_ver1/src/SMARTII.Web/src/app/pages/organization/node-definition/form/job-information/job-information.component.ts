import { Component, OnInit, Injector, OnDestroy, Output, EventEmitter, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NodeDefinitionDetailViewModel, JobDetailViewModel } from 'src/app/model/organization.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { Store } from '@ngrx/store';
import { State as fromOrganizationReducer } from '../../../store/reducers/';
import * as fromNodeDefinitionActions from '../../../store/actions/node-definition.actions';
import { Subscription } from 'rxjs';
import * as fromRootActions from 'src/app/store/actions';

const PREFIX = 'NodeDefinitionComponent';

@Component({
  selector: 'app-job-information',
  templateUrl: './job-information.component.html',
  styleUrls: ['./job-information.component.scss']
})
export class JobInformationComponent extends FormBaseComponent implements OnInit, OnDestroy {

  @Output() actionTypeChanged: EventEmitter<ActionType> = new EventEmitter();
  @Input() mainUIActionType?: ActionType;

  private model$: Subscription;
  public model = new NodeDefinitionDetailViewModel();

  public options = {};
  public form: FormGroup;

  public uiActionType?: ActionType;
  public jobModel = new JobDetailViewModel();

  items: any[] = [
    { id: "OFC", text: "OFC" },
    { id: "OWNER", text: "負責人" },
  ]
  titleTypeString: string = "";

  constructor(
    public store: Store<fromOrganizationReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initializeForm();
    this.initializeTable();
    this.considerActionType();
  }


  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @loggerMethod()
  btnLoadAdd($event) {
    this.actionTypeChanged.emit(ActionType.Add);
    this.resetForm();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  @loggerMethod()
  btnLoadEdit($event) {
    this.actionTypeChanged.emit(ActionType.Update);
    this.uiActionType = ActionType.Update;
    this.jobModel = $event.data;
    this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  btnLoadRead($event) {
    this.actionTypeChanged.emit(ActionType.Read);
    this.uiActionType = ActionType.Read;
    this.jobModel = $event.data;
    this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Delete)
  @loggerMethod()
  btnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {

        const payload = new EntrancePayload<number>($event.data.ID);
        payload.success = () => {
          this.resetForm();
          this.reloadData($event.data);
        };

        this.store.dispatch(new fromNodeDefinitionActions.disableJobAction(payload));
      }
    )));
  }

  rowSelect = ($event) => this.uiActionType === ActionType.Read ?
    this.btnLoadRead($event) : this.btnLoadEdit($event)

  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  @loggerMethod()
  btnEdit() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const payload = new EntrancePayload<JobDetailViewModel>(this.jobModel);

    payload.success = (data: JobDetailViewModel) => {

      this.resetForm();
      this.reloadData(data);

    };

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromNodeDefinitionActions.editJobAction(payload));
      }
    )));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @loggerMethod()
  btnAdd() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const payload = new EntrancePayload<JobDetailViewModel>(this.jobModel);

    payload.success = (data: JobDetailViewModel) => {
      this.resetForm();
      this.reloadData(data);

    };

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromNodeDefinitionActions.addJobAction(payload));
      }
    )));
  }


  resetForm() {
    this.uiActionType = ActionType.Add;
    this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
    this.resetModel();
  }

  resetModel() {
    this.jobModel = new JobDetailViewModel();
    this.jobModel.OrganizationType = this.model.OrganizationType;
    this.jobModel.DefinitionID = this.model.ID;
    this.jobModel.IsEnabled = true;
  }

  reloadData(data: JobDetailViewModel) {
    this.store.dispatch(new fromNodeDefinitionActions.loadDetailAction({
      OrganizationType: data.OrganizationType,
      ID: data.DefinitionID
    }));
  }

  considerActionType() {
    if (this.mainUIActionType === ActionType.Read) {
      this.uiActionType = ActionType.Read;
    }
  }

  subscription() {

    this.model$ =
      this.store
        .select((state: fromOrganizationReducer) => state.organization.nodeDefinition.detail)
        .subscribe(nodeDefinition => {
          this.model = { ...nodeDefinition };
        });
  }

  initializeForm() {
    this.form = new FormGroup({
      Name: new FormControl(this.jobModel.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      Level: new FormControl(this.jobModel.Level, [
        Validators.required,
      ]),
      IsEnabled: new FormControl(),
      Key: new FormControl(this.jobModel.Key, null),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  initializeTable() {

    this.options = {
      columns: {
        Name: {
          title: this.translateService.instant('JOB.NAME'),
          width: '20%',
          class: 'text-center'
        },
        IsEnabledName: {
          title: this.translateService.instant('JOB.IS_ENABLED'),
          width: '20%',
          class: 'text-center'
        },
        Level: {
          title: this.translateService.instant('JOB.LEVEL'),
          width: '20%',
          class: 'text-center'
        },
      },
      actions: {
        position: 'right',
        columnTitle: this.translateService.instant('COMMON.ACTION'),
        edit: false,
        add: false,
        delete: this.mainUIActionType === ActionType.Update,
      },
      delete: {
        deleteButtonContent: `<i class="nb-trash fa-xs">${this.translateService.instant('COMMON.BTN_DISABLED')}</i>`,
        confirmDelete: true,
      },

    };
  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }


}
