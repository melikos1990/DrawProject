import { Component, OnInit, Injector } from '@angular/core';
import { NgInputBaseComponent } from 'ptc-dynamic-form';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-datetime-input',
  templateUrl: './datetime-input.component.html'
})
export class DatetimeInputComponent extends NgInputBaseComponent implements OnInit {

  translateService: TranslateService;

  constructor(public injector: Injector) {
    super(injector);

    this.translateService = injector.get(TranslateService);
  }

  ngOnInit() {

    this.input.validator = JSON.parse((<any>this.input.validator));
    this.setFromControl();
  }

}
