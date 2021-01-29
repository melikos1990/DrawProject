import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { BillboardDetailViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromBillboardActions from '../../store/actions/billboard.actions';
import { takeUntil } from 'rxjs/operators';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AllNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';
import { UserListViewModel } from 'src/app/model/organization.model';

const PREFIX = 'BillboardComponent';

@Component({
  selector: 'app-billboard-detail',
  templateUrl: './billboard-detail.component.html',
  styleUrls: ['./billboard-detail.component.scss']
})
export class BillboardDetailComponent extends FormBaseComponent implements OnInit {

  public options = {};
  public form: FormGroup;

  public uiActionType: ActionType;
  public model: BillboardDetailViewModel = new BillboardDetailViewModel();

  @ViewChild('allNodeTreeUserSelector') selector: TemplateRef<any>;

  popupRef: NgbModalRef;

  columns = [];
  titleName: string;
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
      .select((state: fromMasterReducer) => state.master.billboard.detail)
      .pipe(
        takeUntil(this.destroy$))
      .subscribe(billboard => {
        this.model = { ...billboard };
        this.setFileOptions();

      });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.store.dispatch(new fromBillboardActions.loadEntryAction());
        this.titleName = this.translateService.instant('APPLICATION.FEATURE.BILLBOARD') + "-" + this.translateService.instant('BILLBOARD.ADD');
        this.columns = this.appendOperator(this.columns);
        break;
      case ActionType.Update:
        this.store.dispatch(new fromBillboardActions.loadDetailAction(payload));
        this.titleName = this.translateService.instant('APPLICATION.FEATURE.BILLBOARD') + "-" + this.translateService.instant('BILLBOARD.EDIT');
        this.columns = this.appendOperator(this.columns);
        break;
      case ActionType.Read:
        this.store.dispatch(new fromBillboardActions.loadDetailAction(payload));
        this.titleName = this.translateService.instant('APPLICATION.FEATURE.BILLBOARD') + "-" + this.translateService.instant('BILLBOARD.READ');
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
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇通知人員")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromBillboardActions.addAction(this.model));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnEdit($event) {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (!this.model.Users || this.model.Users.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇通知人員")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromBillboardActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  closeModel(){
    this.popupRef.dismiss();
  }

  btnSelectUserModal($event) {
    this.popupRef = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

  }

  findIndex = (userID) => this.model.Users.findIndex(x => x.UserID === userID);


  btnAddUser($event: AllNodeTreeUserSelectorComponent) {
    const users: UserListViewModel[] = $event.getValue();

    users
      .filter(user => this.findIndex(user.UserID) === -1)
      .map(user => this.model.Users.push(user));

    this.model.Users = [...this.model.Users]; // clone ref to refresh view....
    this.popupRef.dismiss();
  }

  btnDeleteUser($event: UserListViewModel) {
    const index = this.findIndex($event.UserID);
    this.model.Users.splice(index, 1);
    this.model.Users = [...this.model.Users]; // clone ref to refresh view....
  }



  setFileOptions() {

    const paths = this.model.FilePaths || [];
    const previews = paths.map(path => path.toHostApiUrl())
    const previewConfigs = paths.map(path => {

      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteBillboardFile`.toHostApiUrl(),
        extra: {
          id: this.model.ID,
          key: path
        }
      }
    });

    this.options = {
      preferIconicPreview: true,
      initialPreview: previews,
      initialPreviewConfig: previewConfigs,
      fileActionSettings: {
        showRemove: this.uiActionType == ActionType.Update,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
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

  initializeForm() {
    this.form = new FormGroup({
      Title: new FormControl(this.model.Title, [
        Validators.required,
        Validators.maxLength(256),
      ]),
      Content: new FormControl(this.model.Content, [
        Validators.required,
        Validators.maxLength(1024),
      ]),
      ActiveDateTimeRange: new FormControl(this.model.ActiveDateTimeRange, [
        Validators.required,
      ]),
      BillboardWarningType: new FormControl(this.model.BillboardWarningType, [
        Validators.required,
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

}
