import { Component, OnInit, TemplateRef, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Store } from '@ngrx/store';
import { State as formRootReducers } from '../../store/reducers';
import { skip, takeUntil } from 'rxjs/operators';
import { PtcSwalType } from 'ptc-swal';

import * as fromRootActions from 'src/app/store/actions';

@Component({
  selector: 'app-customer-alert',
  templateUrl: './customer-alert.component.html',
  styleUrls: ['./customer-alert.component.scss']
})
export class CustomerAlertComponent extends BaseComponent implements OnInit {


  type = PtcSwalType;

  option: {
    templateRef: TemplateRef<any>,
    detail: {
      title: string,
      frameClass: string,
      contentClass: string,
      type: PtcSwalType,
      confirm?: () => void,
      cancel?: () => void
    },
    data: any
  }


  constructor(
    private store: Store<formRootReducers>,
    public injector: Injector) {
    super(injector);

  }

  ngOnInit() {
    this.subscription();
  }

  btnConfirm($event) {

    this.option.detail.confirm && this.option.detail.confirm();
    this.store.dispatch(new fromRootActions.AlertActions.CustomerClearAction())
  }

  btnBack($event) {

    this.option.detail.cancel && this.option.detail.cancel();
    this.store.dispatch(new fromRootActions.AlertActions.CustomerClearAction())
  }

  subscription() {
    this.store.select(x => x.alert.customerOpts)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(x => {
        console.log("opt", x);
        this.option = x;
      })
  }

}
