import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { ItemSelectComponent } from 'src/app/shared/component/select/element/item-select/item-select.component';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators, NgModel } from '@angular/forms';
import { NotificationGroupDetailViewModel, NotificationGroupUserListViewModel, NotificationGroupListViewModel, NotificationCalcType, ItemSearchViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromNotificationGroupActions from '../../store/actions/notification-group.actions';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { takeUntil, skip } from 'rxjs/operators';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AllNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/all-node-tree-user-selector/all-node-tree-user-selector.component';
import { UserListViewModel } from 'src/app/model/organization.model';
import { PositiveNumberValidator } from 'src/app/shared/data/validator';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { Guid } from 'guid-typescript';
import { NotificationGroupUserModalComponent } from 'src/app/shared/component/modal/notification-group-user-modal/notification-group-user-modal.component';
import { ItemSelectorComponent } from 'src/app/shared/component/modal/item-selector/item-selector.component';



const PREFIX = 'NotificationGroupComponent';

@Component({
  selector: 'app-notification-group-detail',
  templateUrl: './notification-group-detail.component.html',
  styleUrls: ['./notification-group-detail.component.scss']
})
export class NotificationGroupDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild('dynamicSelect') dynamicSelect: DynamicQuestionSelectComponent;
  @ViewChild('selector') selector: TemplateRef<any>;
  @ViewChild('itemSelect') itemSelect: ItemSelectComponent;

  public options = {};
  public form: FormGroup;

  public uiActionType: ActionType;
  public model: NotificationGroupDetailViewModel = new NotificationGroupDetailViewModel();


  popupUserRef: NgbModalRef;
  popupGroupRef: NgbModalRef;
  popupItemref: NgbModalRef;

  columns = [];
  isFirst: boolean = true;
  titleTypeString: string;

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
      .select((state: fromMasterReducer) => state.master.notificationGroup.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(group => {
        this.model = { ...group };
        this.dynamicSelect.updateView();
      });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.model.NodeID = parseInt(params['BuID']);
        this.store.dispatch(new fromNotificationGroupActions.loadEntryAction());
        this.columns = this.appendOperator(this.columns);
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromNotificationGroupActions.loadDetailAction(payload));
        this.columns = this.appendOperator(this.columns);
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromNotificationGroupActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {


    // 回填問題分類的key
    this.model.QuestionClassificationID = this.dynamicSelect.lastHasValue;


    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (this.advancedValid() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.ValidErrorMessage())));
      return;
    }

    if (this.vaildNotificationRemark() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('NOTIFICATION_GROUP.EMAIL_RECEIVER_TYPE_ERROR'))));
      return;
    }

    if (this.vaildEmail() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('NOTIFICATION_GROUP.EMAIL_ERROR'))));
      return;
    }

    if (this.model.CalcMode == NotificationCalcType.ByQuestion) {
      this.model.ItemID = null;
    }
    else if (this.model.CalcMode == NotificationCalcType.ByItem) {
      this.model.QuestionClassificationID = null;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromNotificationGroupActions.addAction(this.model));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  btnEdit($event) {


    // 回填問題分類的key
    this.model.QuestionClassificationID = this.dynamicSelect.lastHasValue;

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (this.advancedValid() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.ValidErrorMessage())));
      return;
    }

    if (this.vaildNotificationRemark() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('NOTIFICATION_GROUP.EMAIL_RECEIVER_TYPE_ERROR'))));
      return;
    }

    if (this.vaildEmail() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('NOTIFICATION_GROUP.EMAIL_ERROR'))));
      return;
    }

    if (this.model.CalcMode == NotificationCalcType.ByQuestion) {
      this.model.ItemID == null;
    }
    else if (this.model.CalcMode == NotificationCalcType.ByItem) {
      this.model.QuestionClassificationID == null;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromNotificationGroupActions.editAction(this.model));
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
  
  onBuChange($event) {
    if (this.itemSelect !== undefined) {
      this.itemSelect.getList($event);
    }
  }

  btnCopyGroupModal($event) {
    this.popupGroupRef = this.modalService.open(NotificationGroupUserModalComponent,
      { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });

    this.popupGroupRef.componentInstance.btnAddUser = this.onGroupSelected.bind(this);
    this.popupGroupRef.componentInstance.nodeID = this.model.NodeID;
  }

  btnItemModal($event) {
    if (this.model.NodeID == null) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("必須先選擇企業別")));
      return;
    }
    this.popupItemref = this.modalService.open(ItemSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    this.popupItemref.componentInstance.model = { NodeID: this.model.NodeID } as ItemSearchViewModel;
    this.popupItemref.componentInstance.btnAddItem = this.onItemSelected.bind(this);


  }

  onGroupSelected(data: NotificationGroupListViewModel[]) {

    const payloads = this.generatorPayloads(data);
    this.model.Users = this.union(this.model.Users, payloads);
    this.popupGroupRef.dismiss();
  }

  onItemSelected(data: NotificationGroupDetailViewModel) {
    this.model.ItemID = data.ID;
    this.model.ItemName = data.Name;
    this.popupItemref.dismiss();
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
        text: this.translateService.instant('NOTIFICATION_GROUP.ORGANIZATION_TYPE'),
        name: 'OrganizationTypeName'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.USER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.EMAIL'),
        name: 'Email',
        customer: true,
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.EMAIL_RECEIVER_TYPE'),
        name: 'NotificationRemark',
        customer: true,
      },
    ];
  }

  union(existUsers: NotificationGroupUserListViewModel[], newUsers: NotificationGroupUserListViewModel[]): NotificationGroupUserListViewModel[] {

    const result: NotificationGroupUserListViewModel[] = [];

    newUsers.forEach(x => {
      result.push(x);
    });

    existUsers.forEach(x => {
      if (result.filter(g => g.UserID === x.UserID).length === 0) {
        result.push(x);
      }
    });

    const resultOrder = result.sort(function (a, b) {
      return a.NotificationRemark > b.NotificationRemark ? 1 : -1;
    });

    return resultOrder;

  }

  // getUserIDIfMock(user) {
  //   return user.UserID || Guid.create().toString();
  // }

  generatorPayloads(users: any[]): NotificationGroupUserListViewModel[] {
    return users.map((user: any | NotificationGroupUserListViewModel) => {
      const data = new NotificationGroupUserListViewModel();
      data.BUID = user.BUID;
      data.BUName = user.BUName;
      data.UserID = user.UserID;
      data.UserName = user.UserName;
      data.Email = user.Email;
      data.JobName = user.JobName;
      data.JobID = user.JobID;
      data.Mobile = user.Mobile;
      data.NodeID = user.NodeID;
      data.NodeName = user.NodeName;
      data.Address = user.Address;
      data.Email = user.Email;
      data.Telephone = user.Telephone;
      data.OrganizationType = user.OrganizationType;
      data.OrganizationTypeName = user.OrganizationTypeName;
      data.UnitType = this.unitType.Organization;
      data.Mobile = user.Mobile;
      data.NotificationBehavior = this.notificationType.Email.toString();
      data.NotificationRemark = !(user.NotificationRemark) ? user.NotificationRemark = "0" : user.NotificationRemark;
      data.GroupID = this.model.ID;
      return data;
    });
  }

  advancedValid() {

    console.log(this.model.CalcMode);
    console.log(this.model.QuestionClassificationID)
    switch (this.model.CalcMode.toString()) {
      case this.notificationCalcType.ByQuestion.toString():
        return !!this.model.QuestionClassificationID;
      case this.notificationCalcType.ByItem.toString():
        return !!this.model.ItemID;
      case this.notificationCalcType.Both.toString():
        return !!this.model.ItemID && !!this.model.QuestionClassificationID;
    }
    return false;
  }

  ValidErrorMessage() {
    switch (this.model.CalcMode.toString()) {
      case this.notificationCalcType.ByQuestion.toString():
        return "請選擇第一層問題分類";
      case this.notificationCalcType.ByItem.toString():
        return "請選取商品名稱";
      case this.notificationCalcType.Both.toString():
        if (!!this.model.ItemID == false && !!this.model.QuestionClassificationID == false) {
          return "請選取第一層問題分類以及商品名稱";
        }
        else if (!!this.model.ItemID == false) {
          return "請選取商品名稱";
        }
        else if (!!this.model.QuestionClassificationID == false) {
          return "請選擇第一層問題分類";
        }
    }
    return "";
  }

  initializeForm() {
    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      AlertCycleDay: new FormControl(this.model.AlertCycleDay, [
        Validators.required,
        PositiveNumberValidator,
      ]),
      AlertCount: new FormControl(this.model.AlertCount, [
        Validators.required,
        PositiveNumberValidator,
      ]),
      CalcMode: new FormControl(this.model.CalcMode, [
        Validators.required,
      ]),
      ItemID: new FormControl(this.model.ItemID, []),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
      ItemName: new FormControl(),
    });

  }

  vaildNotificationRemark() {
    console.log(this.model.Users);
    if (!this.model.Users) return true;

    if (this.model.Users.some(x => !x.NotificationRemark)) {
      return false;
    }

    return true;
  }

  vaildEmail() {
    console.log(this.model.Users);
    if (!this.model.Users) return true;

    if (this.model.Users.some(x => !x.Email)) {
      return false;
    }

    return true;
  }


}
