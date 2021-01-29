import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseRemindDetailViewModel, CaseRemindListViewModel, NotificationCalcType } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromCaseRemindActions from '../../store/actions/case-remind.actions';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { takeUntil, skip } from 'rxjs/operators';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { CallcenterNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/callcenter-node-tree-user-selector/callcenter-node-tree-user-selector.component';
import { UserListViewModel } from 'src/app/model/organization.model';
import { NumberValidator } from 'src/app/shared/data/validator';
import { Guid } from 'guid-typescript';
import { environment } from 'src/environments/environment';
import { webHostPrefix } from 'src/app.config';

const PREFIX = 'CaseRemindComponent';

@Component({
  selector: 'app-case-remind-detail',
  templateUrl: './case-remind-detail.component.html',
  styleUrls: ['./case-remind-detail.component.scss']
})
export class CaseRemindDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild('selector') selector: TemplateRef<any>;

  public options = {};
  public form: FormGroup;

  public uiActionType: ActionType;
  public model: CaseRemindDetailViewModel = new CaseRemindDetailViewModel();

  editEnable: boolean = false;
  noticePersonEnable: boolean = false;
  titleTypeString: string = "";

  popupUserRef: NgbModalRef;

  columns = [];

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
      .select((state: fromMasterReducer) => state.master.caseRemind.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(item => {
        this.model = { ...item };
        if (this.uiActionType == this.actionType.Update && this.model.IsLock) {
          this.editEnable = false;
          this.noticePersonEnable = false;
          for (let control in this.form.controls) {
            this.form.controls[control].disable();
          };
        };
      });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const caseID = params['caseID'];
    const assignmentID = params['assignmentID'];

    const payload = {
      ID: params['id']
    };

    if (caseID) this.model.CaseID = caseID;
    if (assignmentID) this.model.AssignmentID = assignmentID;

    switch (this.uiActionType) {
      case ActionType.Add:
        this.store.dispatch(new fromCaseRemindActions.loadEntryAction());
        this.noticePersonEnable = true;
        this.columns = this.appendOperator(this.columns);
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromCaseRemindActions.loadDetailAction(payload));
        this.editEnable = true;
        this.noticePersonEnable = true;
        this.columns = this.appendOperator(this.columns);
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromCaseRemindActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
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
        this.store.dispatch(new fromCaseRemindActions.addAction(this.model));
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
        this.store.dispatch(new fromCaseRemindActions.editAction(this.model));
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

  /**
 * 按鈕按下完成
 */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  btnConfirm($event) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('確認完成?',
      () => {
        this.store.dispatch(new fromCaseRemindActions.confirmAction({ ID: this.model.ID }));
      }
    )));
  }

  /**
 * 按鈕按下案件查詢
 */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnCaseSearch($event: any) {
    var payload = new EntrancePayload<{ caseID: string }>();
    payload.data = {
      caseID: this.model.CaseID
    }
    payload.success = () => {
      const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
        actionType: this.actionType.Read,
        caseID: this.model.CaseID
      });
      window.open(url, '_blank');
    }

    this.store.dispatch(new fromCaseRemindActions.checkCaseIDAction(payload));
  }

  btnSelectUserModal($event) {
    this.popupUserRef = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
  }

  onUserSelected(instance: CallcenterNodeTreeUserSelectorComponent) {
    const users: any[] = instance.getValue();

    if (!users || users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇使用者")));
      return;
    }
    const payloads = this.generatorPayloads(instance.getValue());
    this.model.Users = this.union(this.model.Users, payloads);
    this.model.UserIDs = this.model.Users.map(function (item) {
      return item['UserID'];
    });
    this.popupUserRef.dismiss();
  }

  findIndex = (userID) => this.model.Users.findIndex(x => x.UserID === userID);


  btnDeleteUser($event: UserListViewModel) {
    if (this.model.IsLock) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("通知已發送，內容不可修改。")));
      return;
    }
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
      },
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
      CaseID: new FormControl(this.model.CaseID, [
        Validators.required,
        Validators.maxLength(14),
      ]),
      AssignmentID: new FormControl(this.model.AssignmentID, [
        NumberValidator
      ]),
      Level: new FormControl(this.model.Level, [
        Validators.required,
      ]),
      ActiveDateTimeRange: new FormControl(this.model.ActiveDateTimeRange, [
        Validators.required,
      ]),
      Content: new FormControl(this.model.Content, [
        Validators.required,
        Validators.maxLength(1024),
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
      ConfirmUserName: new FormControl(),
      ConfirmDateTime: new FormControl(),
      IsConfirm: new FormControl(),
    });

  }

}

