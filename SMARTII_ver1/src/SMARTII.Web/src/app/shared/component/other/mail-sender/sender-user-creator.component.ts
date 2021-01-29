import { Component, OnInit, Injector, Input } from '@angular/core';
import { ConcatableUserViewModel } from 'src/app/model/organization.model';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-sender-user-creator',
  templateUrl: './sender-user-creator.component.html',
  styleUrls: ['./sender-user-creator.component.scss']
})
export class SenderUserCreatorComponent extends FormBaseComponent implements OnInit {

  onBtnAdd: any;
  onBtnBack: any;

  public form: FormGroup;
  model: ConcatableUserViewModel = new ConcatableUserViewModel();

  constructor(
    public store: Store<any>,
    public modalService: NgbModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeForm();

  }

  btnBack() {
    this.onBtnBack && this.onBtnBack();
  }

  btnAdd() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    this.model.UnitType = this.unitType.Customer;

    this.onBtnAdd && this.onBtnAdd(this.model);
  }

  initializeForm() {
    this.form = new FormGroup({
      UserName: new FormControl(this.model.UserName),
      Email: new FormControl(this.model.Email, [
        Validators.required,
        Validators.email,
      ]),
    });
  }

}
