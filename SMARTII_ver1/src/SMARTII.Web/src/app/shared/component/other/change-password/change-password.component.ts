import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ChangePasswordViewModel } from 'src/app/model/authorize.model';
import { passwordRule } from 'src/app/shared/data/validator';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent extends FormBaseComponent implements OnInit {


  @Input() data: ChangePasswordViewModel = new ChangePasswordViewModel();
  @Input() form: FormGroup = new FormGroup({});

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeForm();
  }


  initializeForm() {
    this.form.setControl("ConfirmPassword", new FormControl(
      this.data.ConfirmPassword, [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(50)
    ]));
    this.form.setControl("NewPassword", new FormControl(
      this.data.NewPassword, [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(50),
      passwordRule
    ]));
    this.form.setControl("OldPassword", new FormControl(
      this.data.OldPassword, [
      Validators.required,
      Validators.maxLength(50)
    ]));
  }

}
