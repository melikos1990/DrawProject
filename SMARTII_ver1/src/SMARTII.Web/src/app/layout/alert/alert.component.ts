import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy, ComponentFactoryResolver, Injector, ElementRef } from '@angular/core';
import { PtcSwalComponent, PtcSwalOption, PtcSwalType } from 'ptc-swal';

import { Store } from '@ngrx/store';
import { State as fromRootReducers } from "../../store/reducers"
import { filter,  takeUntil } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { CounterBallComponent } from 'src/app/shared/component/other/counter-ball/counter-ball.component';
import { ItemSelectComponent } from 'src/app/shared/component/select/element/item-select/item-select.component';
import { CallcenterNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/callcenter-node-tree-user-selector/callcenter-node-tree-user-selector.component';
import { BaseComponent } from 'src/app/pages/base/base.component';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
export class AlertComponent extends BaseComponent implements AfterViewInit {

  @ViewChild("swal") swal: PtcSwalComponent;

  opts$: Subscription;

  opts: PtcSwalOption = {}


  constructor(
    public resolver: ComponentFactoryResolver,
    public injector: Injector,
    public store: Store<fromRootReducers>) {
    super(injector);

  }

  ngAfterViewInit(): void {
    this.store.select((state: fromRootReducers) => state.alert.opts)
      .pipe(filter(x => !!(x)), takeUntil(this.destroy$))
      .subscribe(opts => {
        this.opts = { ...opts.detail };
        this.swal.options = { ...opts.detail };
        this.swal.show(opts.isLoop, opts.confirm, opts.cancel);
      });
  }


}
