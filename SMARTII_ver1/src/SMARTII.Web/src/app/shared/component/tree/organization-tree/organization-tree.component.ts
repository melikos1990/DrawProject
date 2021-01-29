import { Component, OnInit, Input, Injector, Output, EventEmitter, ViewChild } from '@angular/core';
import { ObjectService } from 'src/app/shared/service/object.service';
import { LayoutBaseComponent } from 'src/app/pages/base/layout-base.component';
import { OrganizationNodeViewModel } from 'src/app/model/organization.model';
import { asOrganizationTreeNode } from '../function';
import { Node } from '../ptc-tree/model/node';

import * as fromRootActions from 'src/app/store/actions';
import { State as fromRootReducers } from 'src/app/store/reducers';
import { Store } from '@ngrx/store';
import { PtcTreeComponent } from '../ptc-tree/ptc-tree.component';

@Component({
  selector: 'app-organization-tree',
  templateUrl: './organization-tree.component.html',
  styleUrls: ['./organization-tree.component.scss']
})
export class OrganizationTreeComponent extends LayoutBaseComponent implements OnInit {


  @ViewChild('ptcTree') tree: PtcTreeComponent;

  private _organizationNode: OrganizationNodeViewModel;

  @Input() isSearchable: boolean = true;
  @Input() options: any = {};
  @Output() moveNode: EventEmitter<any> = new EventEmitter();
  @Output() clickNode: EventEmitter<any> = new EventEmitter();
  @Output() selectDisabled: EventEmitter<any> = new EventEmitter();
  @Output() selectAddNode: EventEmitter<any> = new EventEmitter();
  @Input()
  get organizationNode(): OrganizationNodeViewModel {
    return this._organizationNode;
  }
  set organizationNode(value: OrganizationNodeViewModel) {
    if (value !== undefined) {
      this._organizationNode = value;
      this.calcNode();
    }
  }

  public nodes: Node<OrganizationNodeViewModel>[] = new Array<Node<OrganizationNodeViewModel>>();

  constructor(
    public store: Store<fromRootReducers>,
    public objectSercice: ObjectService,
    public injector: Injector) {
    super(injector);

  }

  ngOnInit() {}

  calcNode() {
    this.nodes = [asOrganizationTreeNode(this.organizationNode)];
  }

  onRightClick($event, node) {
    this.store.dispatch(new fromRootActions.AppActions.contextMenuAction({
      display: true,
      position: {
        x: $event.clientX,
        y: $event.clientY
      },
      title: node.data.name,
      cbDist: [
        {
          key: this.translateService.instant('COMMON.CREATE_NODE'),
          value: () => this.selectAddNode.emit(node)
        },
        {
          key: this.translateService.instant('COMMON.DISABLE_NODE'),
          value: () => this.selectDisabled.emit(node)
        },
      ]
    }));
  }

  onClick = ($event, node) => this.clickNode.emit(node);
  onMoveNode = ($event) => this.moveNode.emit($event);

}
