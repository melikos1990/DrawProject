import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { RoleDetailViewModel, AuthenticationType } from 'src/app/model/authorize.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromRoleActions from "../../store/actions/role.actions";
import { State as fromRoleReducer } from "../../store/reducers";
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from "src/app/store/actions";
import { UserSelectorComponent } from '../../../../shared/component/modal/user-selector/user-selector.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserListViewModel } from 'src/app/model/organization.model';
import { skip, takeUntil } from 'rxjs/operators';

export const PREFIX = 'RoleComponent';

@Component({
  selector: 'app-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrls: ['./role-detail.component.scss']
})
export class RoleDetailComponent extends FormBaseComponent implements OnInit {

  popupRef: NgbModalRef;

  private model$: Subscription;

  public form: FormGroup;

  public uiActionType: ActionType;
  public model: RoleDetailViewModel = new RoleDetailViewModel();

  public options = {};

  public titleName: string;

  constructor(
    private modalService: NgbModal,
    private active: ActivatedRoute,
    private store: Store<fromRoleReducer>,
    public injector: Injector
  ) {
    super(injector, PREFIX);

  }

  ngOnInit() {
    this.initializeTable();
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
        this.store.dispatch(new fromRoleActions.addAction(this.model));
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
        this.store.dispatch(new fromRoleActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  btnUserSelector($event) {

    this.popupRef = this.modalService.open(UserSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<UserSelectorComponent>this.popupRef.componentInstance);
    instance.model.IsEnable = true;
    instance.model.IsSystemUser = true;

    instance.btnAddUser = this.btnAddUser.bind(this);
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnAddUser(data: any) {
    const selectItem = this.popupRef.componentInstance.table.getSelectItem();

    if (!(selectItem) || selectItem.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
        this.getFieldInvalidMessage(this.translateService.instant('ERROR.ATLEAST_ONE_TERM'))));
      return;
    }

    const payloads = this.generatorPayloads(data);
    this.model.Users = this.union(this.model.Users, payloads);
    this.popupRef.dismiss();

  }

  generatorPayloads(users: any[]): UserListViewModel[] {

    return users.map((user: any | UserListViewModel) => {
      const data = new UserListViewModel();
      data.UserID = user.UserID;
      data.UserName = user.UserName;
      data.Account = user.Account;
      data.IsAD = user.IsAD;
      data.IsEnabled = user.IsEnabled;
      return data;
    });
  }

  union(existUsers: UserListViewModel[], newUsers: UserListViewModel[]): UserListViewModel[] {

    const result: UserListViewModel[] = [];

    newUsers.forEach(x => {
      result.push(x);
    });

    existUsers.forEach(x => {
      if (result.filter(g => g.UserID === x.UserID).length === 0) {
        result.push(x);
      }
    });

    return result;

  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnDeleteUser($event) {
    if (this.uiActionType == this.actionType.Read) {
      return;
    }
    const index = this.model.Users.findIndex(x => x.UserID == $event.data.UserID);
    this.model.Users.splice(index, 1);
    $event.confirm.resolve();
  }


  initializeTable() {

    this.options = {
      columns: {
        Account: {
          title: this.translateService.instant('USER.ACCOUNT'),
          width: '20%',
        },
        UserName: {
          title: this.translateService.instant('USER.USER_NAME'),
          width: '20%',
        },
        IsAD: {
          title: this.translateService.instant('USER.IS_AD'),
          width: '20%',
        },
        IsEnabled: {
          title: this.translateService.instant('USER.IS_ENABLED'),
          width: '20%',
        },

      },
    };
  }

  initializeForm() {
    this.form = new FormGroup({
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

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.model$ =
      this.store
        .select((state: fromRoleReducer) => state.system.role.detail)
        .pipe(
          skip(1),
          takeUntil(this.destroy$)
        )
        .subscribe(role => {
          this.model = { ...role };
        });
  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      RoleID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.titleName = this.translateService.instant('ROLE.ADD');
        this.store.dispatch(new fromRoleActions.loadEntryAction());
        break;
      case ActionType.Update:
        this.titleName = this.translateService.instant('ROLE.EDIT');
        this.store.dispatch(new fromRoleActions.loadDetailAction(payload));
        break;
      case ActionType.Read:
        this.titleName = this.translateService.instant('ROLE.READ');
        this.store.dispatch(new fromRoleActions.loadDetailAction(payload));
        break;
    }

  }
}
