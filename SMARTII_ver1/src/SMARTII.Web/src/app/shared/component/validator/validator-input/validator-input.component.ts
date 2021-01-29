import { Component, Injector, ContentChild } from '@angular/core';
import { FormControlName } from '@angular/forms';
import { BaseComponent } from 'src/app/pages/base/base.component';


@Component({
  selector: 'app-validator-input',
  templateUrl: './validator-input.component.html',
  styleUrls: ['./validator-input.component.scss']
})
export class ValidatorInputComponent extends BaseComponent {

  @ContentChild(FormControlName) controlName: any;
  constructor(injector: Injector) {
    super(injector);
  }

}
