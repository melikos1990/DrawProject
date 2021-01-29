import { Component, OnInit, Injector, OnDestroy, ViewChild, Input } from '@angular/core';
import { CallCenterNodeViewModel, CallCenterNodeDetailViewModel, OrganizationNodeViewModel } from 'src/app/model/organization.model';
import { State as fromOrganizationReducers } from 'src/app/pages/organization/store/reducers';
import * as fromCallCenterNodeActions from '../../store/actions/callcenter-node.action';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CallCenterNodeCreateComponent } from '../modal/callcenter-node-create/callcenter-node-create.component';
import { OrganizationTreeComponent } from 'src/app/shared/component/tree/organization-tree/organization-tree.component';
import { Node } from 'src/app/shared/component/tree/ptc-tree/model/node';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from 'src/app/store/actions';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
const PREFIX = 'CallCenterNodeComponent';

@Component({
  selector: 'app-callcenter-node-tree',
  templateUrl: './callcenter-node-tree.component.html',
})
export class CallCenterNodeTreeComponent extends FormBaseComponent implements OnInit {

  @Input() uiActionType: ActionType;
  @ViewChild('organizationTree') organizationTree: OrganizationTreeComponent;

  private model$: Subscription;
  public model: CallCenterNodeViewModel;

  public options: any = {
    idField: 'guid', allowDrag: (node) => {
      return this.hasAuths;
    },
    allowDrop: (node) => {
      return this.hasAuths;
    }
  };

  public hasAuths: boolean = true;

  constructor(
    public authService: AuthenticationService,
    private modalService: NgbModal,
    public injector: Injector,
    private store: Store<fromOrganizationReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initialize();

    this.authService.onMethodAuthorization(PREFIX, AuthenticationType.Update).subscribe(hasAuth => {
      this.hasAuths = hasAuth
    });
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  @loggerMethod()
  selectDisabled($event) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<number>($event.data.id);
        this.store.dispatch(new fromCallCenterNodeActions.disabledAction(payload));
      }
    )));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Add | AuthenticationType.Read)
  @loggerMethod()
  selectAddNode($event) {

    const ref = this.modalService.open(CallCenterNodeCreateComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CallCenterNodeCreateComponent>ref.componentInstance);
    instance.prefixNode = $event.data;
    instance.btnAddOrgniazationNode = this.btnAddOrgniazationNode.bind(this);

  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  clickNode($event) {
    console.log("點擊節點 =====>", $event);
    if ($event.data.extend.NodeTypeKey == "ROOT") {
      return;
    }
    const payload = new EntrancePayload<number>($event.data.extend.ID);
    this.store.dispatch(new fromCallCenterNodeActions.loadDetailAction(payload));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  @loggerMethod()
  moveNode($event) {

    const from = (<CallCenterNodeViewModel>$event.node.extend);
    const to = (<CallCenterNodeViewModel>$event.to.parent.extend);


    const fullNodes = this.organizationTree.tree.getTreeNode();
    const fullNodesRef = fullNodes[0].extend;
    this.calcNodeRef([fullNodesRef], from, to);


    const data: CallCenterNodeViewModel = fullNodesRef;
    const payload = new EntrancePayload<CallCenterNodeViewModel>(data);

    this.store.dispatch(new fromCallCenterNodeActions.updateTreeAction(payload));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Add | AuthenticationType.Read)
  @loggerMethod()
  btnAddOrgniazationNode(formData: CallCenterNodeDetailViewModel, prefixNode: Node<CallCenterNodeViewModel>) {

    // 組合 node 物件
    const node = this.getOrganizationPayload(formData, prefixNode.extend);
    prefixNode.extend.Children.push(node);

    // 完整的節點結構
    const fullNodes = this.organizationTree.tree.getTreeNode();

    if (!fullNodes || fullNodes.length === 0) { return; }

    const data: CallCenterNodeViewModel = fullNodes[0].extend;
    const payload = new EntrancePayload<CallCenterNodeViewModel>(data);
    payload.success = () => {
      this.modalService.dismissAll();
    };

    this.store.dispatch(new fromCallCenterNodeActions.addAction(payload));

  }

  getOrganizationPayload(formData: CallCenterNodeDetailViewModel, prefixNode: CallCenterNodeViewModel): CallCenterNodeViewModel {
    const data = new CallCenterNodeViewModel();
    data.IsEnabled = formData.IsEnabled;
    data.Name = formData.Name;
    data.Target = true;
    return data;
  }

  calcNodeRef(tree: OrganizationNodeViewModel[], from: OrganizationNodeViewModel, to: OrganizationNodeViewModel) {

    tree.forEach(node => {

      if (node.ID === to.ID) {
        node.Children.push(from);
        return;
      }
      if (node && node.Children && node.Children.length > 0) {
        node.Children = node.Children.filter(x => x.ID !== from.ID);
        this.calcNodeRef(node.Children, from, to);
      }
    });

  }

  initialize() {
    const payload = new EntrancePayload<any>();
    this.store.dispatch(new fromCallCenterNodeActions.loadAction(payload));
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.callcenterNode.tree)
      .subscribe(tree => {
        this.model = { ...tree };

        setTimeout(() => {
          this.organizationTree.tree.treeUpdate();
          this.organizationTree.tree.collapseAll();
        }, 0);
      });

  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

}
