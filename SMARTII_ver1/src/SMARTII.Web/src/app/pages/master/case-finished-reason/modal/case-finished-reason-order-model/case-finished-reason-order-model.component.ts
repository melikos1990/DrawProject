import { Component, OnInit, Injector, ViewChild, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Store } from '@ngrx/store';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import * as fromRootActions from 'src/app/store/actions';
import { State as fromMasterReducers } from '../../../store/reducers';
import { DraggableListComponent } from 'src/app/shared/component/other/draggable-list/draggable-list.component';

const PREFIX = 'CaseFinishedReasonComponent';
@Component({
  selector: 'app-case-finished-reason-order-model',
  templateUrl: './case-finished-reason-order-model.component.html',
  styleUrls: ['./case-finished-reason-order-model.component.scss']
})
export class CaseFinishedReasonOrderModelComponent extends FormBaseComponent implements OnInit {

  @ViewChild(DraggableListComponent) dndList: DraggableListComponent;

  @Input() title: string = '';
  @Input() classificationName: string = '';
  @Input() ajaxOpt: any = {};

  sortCompare = (accumulator, currentValue) => accumulator.extend.Order - currentValue.extend.Order;

  public onBtnOrder: (data: any[]) => void;

  constructor(
    public store: Store<fromMasterReducers>,
    public activeModal: NgbActiveModal,
    public injctor: Injector) {
    super(injctor, PREFIX)

  }

  ngOnInit() { }


  btnBack() {
    this.activeModal.close();
  }

  closeModel() {
    this.activeModal.close();
  }

  btnOrder() {

    const data = this.dndList.completeData;

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.onBtnOrder && this.onBtnOrder(data);
      }
    )));
  }


}
