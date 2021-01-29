import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { OfficialEmailGroupDetailViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromOfficialEmailGroupActions from '../../store/actions/official-email-group.actions';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { takeUntil, skip } from 'rxjs/operators';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AllNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';
import { UserListViewModel } from 'src/app/model/organization.model';
import { Guid } from 'guid-typescript';
import { NumberValidator, passwordRule } from 'src/app/shared/data/validator';

const PREFIX = 'OfficialEmailGroupComponent';

@Component({
  selector: 'app-official-email-group-detail',
  templateUrl: './official-email-group-detail.component.html',
  styleUrls: ['./official-email-group-detail.component.scss']
})
export class OfficialEmailGroupDetailComponent extends FormBaseComponent implements OnInit {
  @ViewChild('selector') selector: TemplateRef<any>;

  public options = {};
  public form: FormGroup;

  public uiActionType: ActionType;
  public model: OfficialEmailGroupDetailViewModel = new OfficialEmailGroupDetailViewModel();

  popupUserRef: NgbModalRef;

  columns = [];
  isUserEnabled?: boolean = true; //人員清單是否篩選啟用人員
  titleTypeString: string = "";
  constructor(
    private modalService: NgbModal,
    private store: Store<fromMasterReducer>,
    private active: ActivatedRoute,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.subscription();
    this.initializeForm();
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.store
      .select((state: fromMasterReducer) => state.master.officialEmailGroup.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(group => {
        this.model = { ...group };
        console.log(this.model);
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
        this.model.IsEnabled = true;
        this.model.AllowReceive = true;
        this.store.dispatch(new fromOfficialEmailGroupActions.loadEntryAction());
        this.columns = this.appendOperator(this.columns);
        this.titleTypeString = this.translateService.instant('OFFICIAL_EMAIL_GROUP.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromOfficialEmailGroupActions.loadDetailAction(payload));
        this.columns = this.appendOperator(this.columns);
        this.titleTypeString = this.translateService.instant('OFFICIAL_EMAIL_GROUP.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromOfficialEmailGroupActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('OFFICIAL_EMAIL_GROUP.READ');
        break;
    }

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.model.Users || this.model.Users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇通知對象")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromOfficialEmailGroupActions.addAction(this.model));
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

    if (!this.model.Users || this.model.Users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇通知對象")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromOfficialEmailGroupActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  closeModel(){
    this.popupUserRef.dismiss();
  }

  btnSelectUserModal($event) {
    this.popupUserRef = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

  }

  onUserSelected(instance: AllNodeTreeUserSelectorComponent) {
    const users: any[] = instance.getValue();

    if (!users || users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇使用者")));
      return;
    }
    const payloads = this.generatorPayloads(instance.getValue());
    this.model.Users = this.union(this.model.Users, payloads);
    this.popupUserRef.dismiss();
  }

  findIndex = (userID) => this.model.Users.findIndex(x => x.UserID === userID);


  btnDeleteUser($event: UserListViewModel) {
    const index = this.findIndex($event.UserID);
    this.model.Users.splice(index, 1);
    this.model.Users = [...this.model.Users]; // clone ref to refresh view....
  }

  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('USER.ACCOUNT'),
        name: 'Account'
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
      }
    ];
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

  getUserIDIfMock(user) {
    return user.UserID || Guid.create().toString();
  }

  generatorPayloads(users: any[]): UserListViewModel[] {
    return users.map((user: any | UserListViewModel) => {
      const data = new UserListViewModel();
      data.UserID = this.getUserIDIfMock(user);
      data.Account = user.Account;
      data.UserName = user.UserName;
      return data;
    });
  }

  initializeForm() {
    this.form = new FormGroup({
      BuID: new FormControl(this.model.BuID, [
        Validators.required,
      ]),
      Account: new FormControl(this.model.Account, [
        Validators.required,
        Validators.maxLength(50),
      ]),
      Password: new FormControl(this.model.Password, [
        Validators.required,
        Validators.maxLength(50),
        passwordRule
      ]),
      MailAddress: new FormControl(this.model.MailAddress, [
        Validators.required,
        Validators.email,
        Validators.maxLength(100),
      ]),
      Protocol: new FormControl(this.model.MailAddress, [
        Validators.required,
      ]),
      KeepDay: new FormControl(this.model.MailAddress, [
        Validators.required,
        NumberValidator
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
      IsEnabled: new FormControl(),
      HostName: new FormControl(this.model.HostName, [
        Validators.maxLength(50)
      ]),
      MailDisplayName: new FormControl(this.model.MailDisplayName, [
        Validators.required,
        Validators.maxLength(20)
      ]),
      OfficialEmail: new FormControl(this.model.OfficialEmail, [
        Validators.required,
        Validators.email
      ]),
      AllowReceive: new FormControl(this.model.AllowReceive, [
        Validators.required
      ]),
    });

  }



}
