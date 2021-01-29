import { Component, OnInit, Injector, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { PtcTreeComponent } from '../ptc-tree/ptc-tree.component';
import { KMClassificationNodeViewModel } from 'src/app/model/master.model';
import { Node } from '../ptc-tree/model/node';
import { asKMTreeNode } from '../function';
import { State as fromRootReducers } from 'src/app/store/reducers';
import { Store } from '@ngrx/store';
import * as fromRootActions from 'src/app/store/actions';

@Component({
  selector: 'app-km-tree',
  templateUrl: './km-tree.component.html',
  styleUrls: ['./km-tree.component.scss']
})
export class KmTreeComponent extends BaseComponent implements OnInit {


  private _kMNodes: KMClassificationNodeViewModel[];

  @ViewChild('ptcTree') tree: PtcTreeComponent;
  @Output() moveNode: EventEmitter<any> = new EventEmitter();
  @Input() isSearchable: boolean = true;
  @Input() options: any = {};
  @Input()
  get kMNode(): KMClassificationNodeViewModel[] {
    return this._kMNodes;
  }
  set kMNode(value: KMClassificationNodeViewModel[]) {
    if (value !== undefined) {
      this._kMNodes = value;
      this.calcNode();
    }
  }

  @Output() clickNode: EventEmitter<KMClassificationNodeViewModel> = new EventEmitter();
  @Output() selectAddNode: EventEmitter<KMClassificationNodeViewModel> = new EventEmitter();
  @Output() selectDelete: EventEmitter<KMClassificationNodeViewModel> = new EventEmitter();
  @Output() selectRename: EventEmitter<KMClassificationNodeViewModel> = new EventEmitter();

  public nodes: Node<KMClassificationNodeViewModel>[] = new Array<Node<KMClassificationNodeViewModel>>();

  constructor(
    public store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

  calcNode() {
    this.nodes = asKMTreeNode(this._kMNodes);
  }
  onClick = ($event, node) => this.clickNode.emit(node);
  onMoveNode = ($event) => this.moveNode.emit($event);
  onItemRightClick($event, node) {
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
          key: this.translateService.instant('COMMON.RENAME_NODE'),
          value: () => this.selectRename.emit(node)
        },
        {
          key: this.translateService.instant('COMMON.DELETE_NODE'),
          value: () => this.selectDelete.emit(node)
        },
      ]
    }));
  }

  onRootRightClick($event, node) {
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
      ]
    }));
  }

}
