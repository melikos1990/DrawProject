import { Component, OnInit, Input, Injector } from '@angular/core';
import { NgInputBaseComponent } from 'ptc-dynamic-form';
import { GeneralInput } from './general-input';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-general-input',
  templateUrl: './general-input.component.html',
})
export class GeneralInputComponent extends NgInputBaseComponent implements OnInit {

  translateService: TranslateService;
  @Input() input: GeneralInput;

  constructor(public injector: Injector) {
    super(injector);

    this.translateService = injector.get(TranslateService);
  }


  ngOnInit() {

    this.input.validator = JSON.parse((<any>this.input.validator));
    this.setFromControl();

  }

}
