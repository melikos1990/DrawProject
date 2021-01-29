import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { PtcLoadingComponent } from 'ptc-loading';

import { Observable, Subscription } from 'rxjs';
import { Store } from '@ngrx/store';

import { State as fromRootReducers } from "../../store/reducers";
import { BaseComponent } from 'src/app/pages/base/base.component';
import { skip, takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent extends BaseComponent implements AfterViewInit {



  @ViewChild(PtcLoadingComponent)
  loadingComponent: PtcLoadingComponent;

  constructor(private store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngAfterViewInit(): void {


    this.store.select((state: fromRootReducers) => state.loading.visible)
      .pipe(skip(1),
        takeUntil(this.destroy$))
      .subscribe(visible => {
        visible ? this.loadingComponent.show() : this.loadingComponent.close();
      });
  }






}
