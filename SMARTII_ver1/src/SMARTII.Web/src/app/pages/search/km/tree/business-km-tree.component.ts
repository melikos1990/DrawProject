import { Component, OnInit, Injector, ViewChild, ɵConsole, } from '@angular/core';
import { Store } from '@ngrx/store';

import * as fromKMActions from '../../store/actions/km.actions';
import { State as fromMasterReducers } from '../../store/reducers'
import { takeUntil, skip } from 'rxjs/operators';
import { KMClassificationNodeViewModel } from 'src/app/model/master.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from 'src/app/store/actions';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddClassificationModalComponent } from '../modal/add-classification-modal/add-classification-modal.component';
import { EditClassificationModalComponent } from '../modal/edit-classification-modal/edit-classification-modal.component';
import { KmTreeComponent } from 'src/app/shared/component/tree/km-tree/km-tree.component';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
const PREFIX = 'KmComponent';

@Component({
  selector: 'app-business-km-tree',
  templateUrl: './business-km-tree.component.html',
})
export class BusinessKmTreeComponent extends FormBaseComponent implements OnInit {

  @ViewChild('kmTree') kmTree: KmTreeComponent;
  nodes: KMClassificationNodeViewModel[];

  constructor(
    private modalService: NgbModal,
    public store: Store<fromMasterReducers>,
    public injector: Injector) {
    super(injector, PREFIX)
  }

  ngOnInit() {
    this.initialize();
    this.subscription();
  }

  clickNode($event) {

    const data: KMClassificationNodeViewModel = $event.data.extend;

    if (data.IsRoot) return;

    this.store.dispatch(new fromKMActions.selectItemAction(data));

  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  selectRename($event) {
    const data: KMClassificationNodeViewModel = $event.data.extend;
    const ref = this.modalService.open(EditClassificationModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<EditClassificationModalComponent>ref.componentInstance);
    instance.node = data
    instance.name = data.ClassificationName;
    instance.updateNode = this.btnUpdateNode.bind(this);

  }
  btnUpdateNode(name: string, node: KMClassificationNodeViewModel) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromKMActions.RenameClassificationAction({ ID: node.ClassificationID, name: name }));
        this.modalService.dismissAll();
      }
    )));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  selectDelete($event) {
    const data: KMClassificationNodeViewModel = $event.data.extend;

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        this.store.dispatch(new fromKMActions.deleteClassificationAction({ ID: data.ClassificationID }));
      }
    )));
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Add | AuthenticationType.Read)
  selectAddNode($event) {
    const data: KMClassificationNodeViewModel = $event.data.extend;

    const ref = this.modalService.open(AddClassificationModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<AddClassificationModalComponent>ref.componentInstance);
    instance.node = data
    instance.btnAddNode = this.btnAddNode.bind(this);
  }


  btnAddNode(name: string, node: KMClassificationNodeViewModel) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        if (node.IsRoot == false) {
          this.store.dispatch(new fromKMActions.AddClassificationAction({ parentID: node.ClassificationID, name: name }));
        } else {
          this.store.dispatch(new fromKMActions.AddRootClassificationAction({ nodeID: node.NodeID, name: name }));
        }

        this.modalService.dismissAll();
      }
    )));
  }

  initialize() {
    this.store.dispatch(new fromKMActions.loadTreeAction());
  }

  onMoveNode($event) {
    console.log($event);

    const from = (<KMClassificationNodeViewModel>$event.node.extend);
    const to = (<KMClassificationNodeViewModel>$event.to.parent.extend);

    this.store.dispatch(new fromKMActions.DragClassificationAction({ ID: from.ClassificationID, parentID: to.ClassificationID }));
  }

  subscription() {
    this.store.select(x => x.mySearch.km.tree)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(tree => {
        this.nodes = [...tree]
      });
  }
}
