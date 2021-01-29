import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { Store } from '@ngrx/store';

export const PREFIX = 'PpclifeEffectiveSummaryComponent';

@Component({
  selector: 'app-ppclife-effective-summary',
  templateUrl: './ppclife-effective-summary.component.html'
})
export class PpclifeEffectiveSummaryComponent extends BaseComponent implements OnInit {

  constructor(
    public store: Store<any>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
  }

}
