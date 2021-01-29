import { Component, OnInit, Injector, ViewChild, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Store } from '@ngrx/store';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { State as fromMasterReducers } from '../../../master/store/reducers';
import { DraggableListComponent } from 'src/app/shared/component/other/draggable-list/draggable-list.component';
import * as fromRootActions from 'src/app/store/actions';

const PREFIX = 'CaseWarningComponent';

@Component({
  selector: 'app-case-warning-order-model',
  templateUrl: './case-warning-order-model.component.html',
  styleUrls: ['./case-warning-order-model.component.scss']
})
export class CaseWarningOrderModelComponent extends FormBaseComponent implements OnInit {

  @ViewChild(DraggableListComponent) dndList: DraggableListComponent;

  @Input() title: string = '';
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

