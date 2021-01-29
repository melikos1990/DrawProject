import { Component, OnInit, Input, Injector, ViewChild, TemplateRef } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CaseAssignmentCommunicateViewModel } from 'src/app/model/case.model';
import { State as fromCaseReducers } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-cam-modal',
  templateUrl: './cam-modal.component.html',
  styleUrls: ['./cam-modal.component.scss']
})
export class CamModalComponent extends FormBaseComponent implements OnInit {

  @Input() uiActionType: ActionType;
  @Input() model: CaseAssignmentCommunicateViewModel

  constructor(
    public modalService: NgbModal,
    public activeModal: NgbActiveModal,
    public store: Store<fromCaseReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }


}
