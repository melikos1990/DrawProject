import { Component, OnInit, Injector, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ConcatableUserViewModel } from 'src/app/model/organization.model';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from 'src/app/store/reducers';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-acu-modal',
  templateUrl: './acu-modal.component.html',
  styleUrls: ['./acu-modal.component.scss']
})
export class AcuModalComponent extends FormBaseComponent implements OnInit {
  public form: FormGroup;

  btnAddUser: any;
  @Input() model: ConcatableUserViewModel = new ConcatableUserViewModel();

  constructor(
    private store: Store<fromMasterReducer>,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeForm();
    this.model.NotificationRemark = this.emailReceiveType.Recipient.toString();
    this.model.NotificationBehavior = this.notificationType.Email.toString();
  }

  closeModel() {
    this.activeModal.close();
  }

  addUser() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.model.UserID = Guid.create().toString();
    this.model.UnitType = this.unitType.Customer;
    this.model.NotificationBehavior = this.notificationType.Email.toString();
    this.btnAddUser && this.btnAddUser(this.model);
  }

  initializeForm() {
    this.form = new FormGroup({
      UserName: new FormControl(this.model.UserName, [
        Validators.required,
        Validators.maxLength(30)
      ]),
      Email: new FormControl(this.model.Email, [
        Validators.required,
        Validators.email,
        Validators.maxLength(255)
      ]),
      NotificationRemark: new FormControl(this.model.NotificationRemark, [
        Validators.required,
      ]),
    });

  }
}
