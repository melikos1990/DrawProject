import { Component, OnInit, Input, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseAssignmentComplaintNoticeViewModel } from 'src/app/model/case.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';

import * as formCaseAction from '../../../store/actions/case-creator.actions';
import { State as fromCaseReducers } from '../../../store/reducers';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-can-modal',
  templateUrl: './can-modal.component.html',
  styleUrls: ['./can-modal.component.scss']
})
export class CanModalComponent extends BaseComponent implements OnInit {

  editSuccess: any;
  @Input() uiActionType: ActionType;
  @Input() model: CaseAssignmentComplaintNoticeViewModel = new CaseAssignmentComplaintNoticeViewModel();

  constructor(
    public store: Store<fromCaseReducers>,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

  closeModel() {
    this.activeModal.close();
  }
  confirm() {
    var data = new EntrancePayload<CaseAssignmentComplaintNoticeViewModel>(this.model);
    data.success = (model: CaseAssignmentComplaintNoticeViewModel) => {
      this.activeModal.close();
      this.editSuccess && this.editSuccess(this.model);
    }

    this.store.dispatch(new formCaseAction.editCaseAssignmentNoticeAction(data));
  }



}