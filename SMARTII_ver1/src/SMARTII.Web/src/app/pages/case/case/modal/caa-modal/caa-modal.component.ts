import { Component, OnInit, Input, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseAssignmentViewModel } from 'src/app/model/case.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';

import * as formCaseAction from '../../../store/actions/case-creator.actions';
import { State as fromCaseReducers } from '../../../store/reducers';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-caa-modal',
  templateUrl: './caa-modal.component.html',
  styleUrls: ['./caa-modal.component.scss']
})
export class CaaModalComponent extends BaseComponent implements OnInit {

  editSuccess: any;
  @Input() uiActionType: ActionType;
  @Input() model: CaseAssignmentViewModel = new CaseAssignmentViewModel();

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




}