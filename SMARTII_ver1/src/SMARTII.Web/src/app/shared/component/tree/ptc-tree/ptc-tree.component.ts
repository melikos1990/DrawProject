import {
  Component, OnInit, ViewChild, AfterViewInit, ContentChild, TemplateRef,
  AfterContentInit, Output, Input,
  OnChanges, EventEmitter, SimpleChanges, Injector
} from '@angular/core';

import { ITreeOptions, TreeComponent, TREE_ACTIONS, IActionMapping, TreeNode, TreeModel } from 'angular-tree-component';
import { cloneNode } from './common/utility';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';



const defaulNode = {
  name: '',
  nodeAllowDrag: false,
  nodeAllowDrop: false,
  children: null,
  authItems: null,
  isDisable: false
}


const actionMapping: IActionMapping = {
  mouse: {
    dblClick: (tree, node, $event) => {
      if (node.hasChildren) return TREE_ACTIONS.TOGGLE_EXPANDED(tree, node, $event)
    },
    drop: (tree: TreeModel, node: TreeNode, $event: any, { from, to }) => {

      const copyNode = cloneNode(from);
      from.data = copyNode;

      return TREE_ACTIONS.MOVE_NODE(tree, node, $event, { from, to });
    },
    dragStart: (tree: TreeModel, node: TreeNode, $event: any) => {

      //console.log('dragStart')
    },
    drag: (tree: TreeModel, node: TreeNode, $event: any) => {

      // console.log('drag')
    },
    dragEnd: (tree: TreeModel, node: TreeNode, $event: any) => {
      //console.log('dragEnd')
    },
    dragOver: (tree: TreeModel, node: TreeNode, $event: any) => {

      //console.log('dragOver')
    },
    dragLeave: (tree: TreeModel, node: TreeNode, $event: any) => {
      // console.log('dragLeave')
    },
    dragEnter: (tree: TreeModel, node: TreeNode, $event: any) => {
      // console.log('dragEnter')

    },


  }
}

const defaultOptions: ITreeOptions = {
  allowDrag: (node) => {
    return node.data.nodeAllowDrag;
  },
  allowDrop: (node) => {
    return node.data.nodeAllowDrop;
  },
  nodeClass: (node: TreeNode) => {
    return node.hasChildren ? 'hasChilren' : '';
  },
  actionMapping: actionMapping,
};


@Component({
  selector: 'app-ptc-tree',
  templateUrl: './ptc-tree.component.html',
  styleUrls: ['./ptc-tree.component.scss']
})
export class PtcTreeComponent implements OnInit, OnChanges, AfterViewInit, AfterContentInit {

  _searchVal: string;
  subject = new Subject();

  @ContentChild('treeNode') treeNode: TemplateRef<any>;
  @ViewChild('loading') loading: TemplateRef<any>;

  @Input() collapse: boolean = true;
  @Input() customerClass: any = 'text-tree';
  @Input() authItems: any[] = [];
  @Input() searchVal;
  @Input() nodes: [];

  _options: any = {};

  @Input()
  get options(): any {
    return this._options;
  }
  set options(obj: any) {

    if (obj !== null) {
      this._options = { ...defaultOptions, ...obj };

    }
  }

  @Output() onMoveNode: EventEmitter<any> = new EventEmitter();
  @Output() onCopyNode: EventEmitter<any> = new EventEmitter();
  @Output() onNodeActivate: EventEmitter<any> = new EventEmitter();


  @ViewChild(TreeComponent)
  tree: TreeComponent;


  ngOnInit() {

    this.subject.pipe(debounceTime(750)).subscribe((currentValue: any) => {

      console.log("currentValue =>", currentValue);

      const searchVal = currentValue;
      this._searchVal = searchVal;
      const curryFn: (node: TreeNode) => boolean = this.filterCurryFn(this._searchVal);

      console.log("curryFn =>", curryFn);

      this.filterNode(curryFn);
    })
  }

  ngOnChanges(changes: SimpleChanges) {

    // 有搜尋條件時 進行查詢
    if (changes['searchVal'] && !changes['searchVal'].isFirstChange()) {
      this.subject.next(changes['searchVal'].currentValue);
    }

  }

  /**
   * filter 柯里化方法
   * @param searchVal
   */
  private filterCurryFn(searchVal: string) {

    let rootKeyWordNode: TreeNode; //KeyWord節點
    let _searchVal = searchVal || null;

    return (node: TreeNode): boolean => {

      if (_searchVal == null) return true
      let valid = node.data.name.toLowerCase().indexOf(_searchVal.toLowerCase()) >= 0;

      let _rootNode = valid ? node : this.recursivelyParentLeve1(node, _searchVal);

      // 比對到就儲存根節點
      if (valid) rootKeyWordNode = _rootNode;
      // 比對不到 檢查是否為同一個根節點
      else if (!valid && rootKeyWordNode && rootKeyWordNode.id == _rootNode.id) {
        return true;
      }

      return valid;
    }

  }

  /**
   * 過濾特定節點
   */
  filterNode = (filter: (node) => boolean) => this.tree.treeModel.filterNodes(filter);


  ngAfterViewInit(): void {

    // 預先過濾不啟用的節點
    this.filterNode(node => !node.data.isDisable);

    // 全部關閉
    if (this.collapse) {
      this.collapseAll();
    }

  }

  ngAfterContentInit(): void {

    this.tree.treeNodeTemplate = this.treeNode;
    this.tree.loadingTemplate = this.loading;
  }


  /**
   * 刪除節點
   */
  delete(node, tree) {

    const parentNode = node.parent ? node.parent.data : tree.virtualRoot;

    const idx = parentNode.children.findIndex(x => x.id === node.data.id);

    parentNode.children.splice(idx, 1);

    this.treeUpdate();

  }


  /**
   * 儲存節點
   */
  saveNode(input, node) {

    let val = input.value;

    if (!val) return;

    if (node.hasChildren == false) {
      node.data.children = [];
    }

    let mergeVal = { ...defaulNode, ...{ name: val } };

    node.data.children.push(mergeVal);

    this.treeUpdate();

    node.data.adding = !node.data.adding;

  }



  moveNode = (event) => this.onMoveNode.emit(event);

  copyNode = (event) => this.onCopyNode.emit(event);

  focus = (event) => this.onNodeActivate.emit(event);




  /**
   * 取得最上層節點
   * @param node
   */
  private recursivelyParent(node: TreeNode): TreeNode {
    let rootNode: TreeNode;
    if (!node.isRoot) return this.recursivelyParent(node.parent);

    rootNode = node;
    return rootNode;
  }

  /**
 * 取得上層指定關鍵字節點
 * @param node
 */
  private recursivelyParentLeve1(node: TreeNode, _searchVal: string): TreeNode {
    let rootNode: TreeNode;
    if (node.data.name.toLowerCase().indexOf(_searchVal.toLowerCase()) == 1) {
      return this.recursivelyParent(node.parent);
    }
    rootNode = node.parent;
    return rootNode;
  }

  /**
   * 取得節點資料
   *
   * @memberof PtcTreeComponent
   */
  getTreeNode = () => this.tree.treeModel.nodes;

  /**
   * 更新 treeview
   */
  treeUpdate = () => this.tree.treeModel.update();

  /**
   * 關閉所有節點
   */
  collapseAll = () => this.tree.treeModel.collapseAll();

  /**
   * 展開所有節點
   */
  expandAll = () => this.tree.treeModel.expandAll();

}
