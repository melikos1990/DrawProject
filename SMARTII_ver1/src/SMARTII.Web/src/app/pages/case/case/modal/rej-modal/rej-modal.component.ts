import { Component, OnInit, Injector, inject, Input } from '@angular/core';
import { CaseAssignmentViewModel } from 'src/app/model/case.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRootActions from 'src/app/store/actions';

@Component({
  selector: 'app-rej-modal',
  templateUrl: './rej-modal.component.html',
  styleUrls: ['./rej-modal.component.scss']
})
export class RejModalComponent extends FormBaseComponent implements OnInit {


  btnConfirm: any
  form: FormGroup;
  @Input() model: CaseAssignmentViewModel

  constructor(
    public store: Store<any>,
    public modalService: NgbModal,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initializeForm();
  }


  confirm() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    
    if(!this.model.RejectType)
    {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.btnConfirm && this.btnConfirm(this.model);

  }

  initializeForm() {
    this.form = new FormGroup({
      RejectType: new FormControl(this.model.RejectType, [
        Validators.required,
      ]),
      RejectReason: new FormControl(this.model.RejectReason, [
        Validators.required,
        Validators.maxLength(256),
      ]),
    })
  }
}
