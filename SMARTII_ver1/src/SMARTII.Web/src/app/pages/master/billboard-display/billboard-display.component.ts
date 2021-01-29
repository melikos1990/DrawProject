import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { Store } from '@ngrx/store';
import { State as fromMasterReducers } from '../store/reducers';
import * as fromBillboardActions from '../store/actions/billboard.actions';
import { BillboardSearchViewModel, BillboardListViewModel } from 'src/app/model/master.model';
import { take, skip, takeUntil } from 'rxjs/operators';
import { Observable } from 'rxjs';

const PREFIX = 'BillboardDisplayComponent';

@Component({
  selector: 'app-billboard-display',
  templateUrl: './billboard-display.component.html'
})
export class BillboardDisplayComponent extends BaseComponent implements OnInit {

  public list$: Observable<BillboardListViewModel[]>;
  public model: BillboardSearchViewModel = new BillboardSearchViewModel();

  constructor(
    public store: Store<fromMasterReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscritpion();
    this.store.dispatch(new fromBillboardActions.getOwnListAction(this.model));
    this.store.dispatch(new fromBillboardActions.ClearNotificationAction());
  }

  selectedChange() {
    this.store.dispatch(new fromBillboardActions.getOwnListAction(this.model));
  }




  subscritpion() {

    this.list$ = this.store
      .select(x => x.master.billboard.ownList)
      .pipe(
        skip(1),
        takeUntil(this.destroy$)
      );
      
  }

}
