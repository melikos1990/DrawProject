import { Component, OnInit, Injector, Input } from '@angular/core';
import { NgInputBaseComponent } from 'ptc-dynamic-form';
import { CheckInput } from './check-input';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-check-input',
  templateUrl: './check-input.component.html'
})
export class CheckInputComponent extends NgInputBaseComponent implements OnInit {

  translateService: TranslateService;

  @Input() input: CheckInput;

  constructor(public injector: Injector) {
    super(injector);

    this.translateService = injector.get(TranslateService);
  }


  ngOnInit() {

    this.input.validator = JSON.parse((<any>this.input.validator));
    this.setFromControl();

  }

}
