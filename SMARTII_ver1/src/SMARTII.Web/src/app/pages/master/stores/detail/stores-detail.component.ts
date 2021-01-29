import { Component, OnInit, Injector, ViewChild, AfterViewInit } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { StoresDetailViewModel } from 'src/app/model/master.model';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromStoresActions from '../../store/actions/stores.actions';
import { ActivatedRoute } from '@angular/router';
import { NgInputBase } from 'ptc-dynamic-form';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StoreOwnerSelectorComponent } from 'src/app/shared/component/modal/store-owner-selector/store-owner-selector.component';
import { UserListViewModel } from 'src/app/model/organization.model';
import { Guid } from 'guid-typescript';
import { BuNodeDefinitionLevelSelectorComponent } from 'src/app/shared/component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { filter, takeUntil } from 'rxjs/operators';

export const PREFIX = 'StoresComponent';

@Component({
  selector: 'app-stores-detail',
  templateUrl: './stores-detail.component.html',
  styleUrls: ['./stores-detail.component.scss']
})
export class StoresDetailComponent extends FormBaseComponent implements OnInit, AfterViewInit {

  public inputs: NgInputBase[] = [];
  public form: FormGroup;
  public particular = {};
  public uiActionType: ActionType;
  public model: StoresDetailViewModel = new StoresDetailViewModel();
  public options = {};

  @ViewChild('allNodeSelector') allNodeSelector: BuNodeDefinitionLevelSelectorComponent;

  popupOwnerRef: NgbModalRef;
  popupOFCRef: NgbModalRef;

  columnsOwner = [];
  columnsOFC = [];

  model$: Subscription;
  layout: string;
  titleName: string;

  constructor(
    private modalService: NgbModal,
    private active: ActivatedRoute,
    private store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeForm();
  }

  ngAfterViewInit() {
    this.subscription();
    this.initializeTable();

  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));
    this.model$ = this.store
      .select((state: fromMasterReducer) => state.master.stores.detail)
      .pipe(filter(data => !!(data)), takeUntil(this.destroy$)) //過濾非undefined、null、空字串
      .subscribe(stores => {
        this.model = { ...stores };
        this.particular = stores ? { ...stores.Particular } : null;
        this.layout = stores.DynamicForm ? stores.DynamicForm : "";
        this.deserialize(this.layout);

        console.log("this.model => ", this.model);

        this.allNodeSelector.buID = this.model.BuID;
        this.allNodeSelector.nodeKey = this.model.NodeKey;
        this.allNodeSelector.arrayId = this.model.NodeParentIDPath;
        this.allNodeSelector.getSteps();
      });
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  btnEdit($event) {


    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.model.Particular = this.particular;
    this.model.StoreOpenDateTime === "無" ? this.model.StoreOpenDateTime = null : this.model.StoreOpenDateTime = this.model.StoreOpenDateTime;
    this.model.StoreCloseDateTime === "無" ? this.model.StoreCloseDateTime = null : this.model.StoreCloseDateTime = this.model.StoreCloseDateTime;

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromStoresActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  /*---start 負責人選人---*/
  btnOwnerModal($event) {
    this.popupOwnerRef = this.modalService.open(StoreOwnerSelectorComponent,
      { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<StoreOwnerSelectorComponent>this.popupOwnerRef.componentInstance);
    instance.btnAddOneJob = this.onOwnerGroupSelected.bind(this);
    instance.buID = this.model.BuID;
    instance.nodeID = this.model.NodeID;
    instance.jobKey = "OWNER";
    instance.isTraversing = false;
  }

  onOwnerGroupSelected(data: any) {
    const selectItem = this.popupOwnerRef.componentInstance.table.getSelectItem();

    if (!(selectItem) || selectItem.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('請選取一個職稱')));
      return;
    }

    if (selectItem.length > 1) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('僅能選取一個職稱')));
      return;
    }
    console.log("onOwnerGroupSelected -> data => ", data);
    const payloads = this.generatorPayloads(data);
    this.model.OwnerNodeJobID = Number(data.NodeJobID);
    this.model.OwnerUsers = this.union(this.model.OwnerUsers, payloads);
    console.log("this.model.OwnerNodeJobID => ", this.model.OwnerNodeJobID)
    this.popupOwnerRef.dismiss();
  }
  /*---end 負責人選人---*/

  /*---start OFC選人---*/
  btnOFCModal($event) {
    this.popupOFCRef = this.modalService.open(StoreOwnerSelectorComponent,
      { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<StoreOwnerSelectorComponent>this.popupOFCRef.componentInstance);

    instance.btnAddOneJob = this.onOFCGroupSelected.bind(this);
    instance.buID = this.model.BuID;
    instance.nodeID = this.model.NodeID;
    instance.jobKey = "OFC";
    instance.isTraversing = true;
  }

  onOFCGroupSelected(data: any) {
    const selectItem = this.popupOFCRef.componentInstance.table.getSelectItem();

    if (!(selectItem) || selectItem.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('請選取一個職稱')));
      return;
    }

    if (selectItem.length > 1) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('僅能選取一個職稱')));
      return;
    }

    console.log("data => ", data);
    const payloads = this.generatorPayloads(data);
    this.model.SupervisorNodeJobID = Number(data.NodeJobID);
    this.model.OFCUsers = this.union(this.model.OFCUsers, payloads);
    this.popupOFCRef.dismiss();
  }
  /*---end OFC選人---*/

  //將字串轉進階物件
  deserialize(jsonLayout) {
    this.inputs = [];
    try {

      if (jsonLayout) {
        const objects = <Array<NgInputBase>>JSON.parse(jsonLayout);
        if (objects) {
          this.inputs = objects.map(data => {
            data["disabled"] = this.uiActionType == this.actionType.Read ? true : null;
            return data;
          });
        } else {
          this.inputs = [];
        }
      } else {
        this.inputs = [];
      }
      console.log("inputs => ", this.inputs)
    } catch (e) {
      console.log(e);
    }
  }

  initializeForm() {
    this.allNodeSelector.disabled = true;
    this.form = new FormGroup({
      IsEnabled: new FormControl(this.model.IsEnabled, null),
      NodeID: new FormControl(this.model.BuID, null),
      Code: new FormControl(this.model.Code, [
        Validators.maxLength(50),
      ]),
      Name: new FormControl(this.model.Name, [
        Validators.maxLength(50),
      ]),
      Address: new FormControl(this.model.Address, [
        Validators.maxLength(1000),
      ]),
      Telephone: new FormControl(this.model.Telephone, [
        Validators.maxLength(1000),
      ]),
      Email: new FormControl(this.model.Email, [
        Validators.maxLength(1000),
      ]),
      ServiceTime: new FormControl(this.model.ServiceTime, [
        Validators.maxLength(50),
      ]),
      StoreOpenDateTime: new FormControl(this.model.StoreOpenDateTime, null),
      StoreCloseDateTime: new FormControl(this.model.StoreCloseDateTime, null),
      StoreType: new FormControl(this.model.StoreType, null),
      Memo: new FormControl(this.model.Memo, null),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['Id'],
      OrganizationType: params['OrganizationType']
    };

    switch (this.uiActionType) {
      case ActionType.Update:
        this.store.dispatch(new fromStoresActions.loadDetailAction(payload));
        this.titleName = this.translateService.instant('STORE.TITLE') + "-" + this.translateService.instant('STORE.UPDATE');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromStoresActions.loadDetailAction(payload));
        this.titleName = this.translateService.instant('STORE.TITLE') + "-" + this.translateService.instant('STORE.READ');
        break;
    }
  }

  initializeTable() {
    this.columnsOwner = [
      {
        text: this.translateService.instant('STORE.JOB_NAME'),
        name: 'JobName',
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('STORE.MOBILE'),
        name: 'Mobile',
      },
      {
        text: this.translateService.instant('USER.EMAIL'),
        name: 'Email',
      },
    ];

    this.columnsOFC = [
      {
        text: this.translateService.instant('STORE.NODE_NAME'),
        name: 'NodeName',
      },
      {
        text: this.translateService.instant('STORE.JOB_NAME'),
        name: 'JobName',
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('STORE.MOBILE'),
        name: 'Mobile',
      },
      {
        text: this.translateService.instant('USER.EMAIL'),
        name: 'Email',
      },
    ];
  }

  union(existUsers: UserListViewModel[], newUsers: UserListViewModel[]): UserListViewModel[] {

    const result: UserListViewModel[] = [];

    existUsers = [];

    newUsers.forEach(x => {
      result.push(x);
    });

    existUsers.forEach(x => {
      result.push(x);
    });

    return result;

  }

  getUserIDIfMock(user) {
    return user.UserID || Guid.create().toString();
  }

  generatorPayloads(data: any): UserListViewModel[] {
    return data.Users.map((user: any | UserListViewModel) => {
      const rdata = new UserListViewModel();
      rdata.UserID = this.getUserIDIfMock(user);
      rdata.UserName = user.UserName;
      rdata.Email = user.Email;
      rdata.JobName = data.JobName;
      rdata.Mobile = user.Telephone;
      rdata.NodeName = data.NodeName;
      return rdata;
    });
  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

}

