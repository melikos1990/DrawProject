import { Component, OnInit, OnDestroy, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { State as fromOrganizationReducers } from '../../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromCallCenterNodeActions from '../../../../store/actions/callcenter-node.action';
import { Store } from '@ngrx/store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { NodeJobListViewModel, AddUserViewModel, CallCenterNodeDetailViewModel } from 'src/app/model/organization.model';
import { Subscription } from 'rxjs';
import { UserSelectorComponent } from 'src/app/shared/component/modal/user-selector/user-selector.component';

const PREFIX = 'CallCenterNodeComponent';
@Component({
  selector: 'app-callcenter-user-table',
  templateUrl: './user-table.component.html',
})
export class UserTableComponent extends FormBaseComponent implements OnInit, OnDestroy {

  @Input() uiActionType: ActionType;
  public options = {};

  model$: Subscription;
  main$: Subscription;

  public main: CallCenterNodeDetailViewModel = new CallCenterNodeDetailViewModel();
  public model: NodeJobListViewModel = new NodeJobListViewModel();


  constructor(
    private modalService: NgbModal,
    private store: Store<fromOrganizationReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeUserTable();
    this.subscription();
  }


  subscription() {

    this.model$ = this.store.select(x => x.organization.callcenterNode.job)
      .subscribe(job => {
        this.model = { ...job };
      });
    this.main$ = this.store.select(x => x.organization.callcenterNode.detail)
      .subscribe(detail => {
        this.main = { ...detail };
        setTimeout(() => {
          this.store.dispatch(new fromCallCenterNodeActions.selectJobAction(this.getCurrentJob()));
        }, 0);
      });

  }


  getCurrentJob = () => {
    if (!this.main || !this.main.Jobs) {
      return new NodeJobListViewModel();
    }
    const job = this.main.Jobs.find(x => x.NodeJobID === this.model.NodeJobID);
    return job || new NodeJobListViewModel();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnUserSelector($event) {
    const ref = this.modalService.open(UserSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<UserSelectorComponent>ref.componentInstance);

    instance.btnAddUser = this.btnAddUser.bind(this);
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnAddUser($event) {

    const existIDs = this.model.Users.map(x => x.UserID);
    const newIDs = $event.map(x => x.UserID);

    const intersection = existIDs.filter(v => newIDs.includes(v));

    if (intersection.length > 0) {
      let itemName = $event.filter(x => intersection.includes(x.UserID)).map(x => x.UserName).join("/");
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(itemName + this.translateService.instant("HEADERQUARTER_NODE.USER_REPEAT"))));
      return;
    }


    if (newIDs.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const payload = new EntrancePayload<AddUserViewModel>();
    payload.data = new AddUserViewModel();
    payload.data.NodeJobID = this.model.NodeJobID;
    payload.data.UserIDs = newIDs;

    payload.success = () => {
      this.store.dispatch(new fromCallCenterNodeActions.loadDetailAction(new EntrancePayload<number>(this.model.NodeID)));
      this.modalService.dismissAll();
    };

    this.store.dispatch(new fromCallCenterNodeActions.addUserAction(payload));
  }



  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnDeleteUser($event) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ nodeJobID: number, userID: string }>();
        payload.data = {
          nodeJobID: this.model.NodeJobID,
          userID: $event.data.UserID
        };
        payload.success = () => {
          this.store.dispatch(new fromCallCenterNodeActions.loadDetailAction(new EntrancePayload<number>(this.model.NodeID)));
          this.modalService.dismissAll();
        };
        this.store.dispatch(new fromCallCenterNodeActions.deleteUserAction(payload));
      }
    )));
  }


  initializeUserTable() {
    this.options = {
      columns: {
        UserName: {
          title: this.translateService.instant('USER.USER_NAME'),
          width: '20%',
        },
        IsSystemUser: {
          title: this.translateService.instant('USER.IS_SYSTEM_USER'),
          width: '20%',
        },
        Account: {
          title: this.translateService.instant('USER.ACCOUNT'),
          width: '20%',
        },
        IsEnabled: {
          title: this.translateService.instant('USER.IS_ENABLED'),
          width: '20%',
        },
      },
      actions: {
        position: 'right',
        columnTitle: this.translateService.instant('COMMON.ACTION'),
        edit: false,
        add: false,
        delete: this.uiActionType === ActionType.Update,
      },
      delete: {
        deleteButtonContent: '<i class="nb-trash"></i>',
        confirmDelete: true,
      },
    };


  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
    this.main$ && this.main$.unsubscribe();
  }
}

