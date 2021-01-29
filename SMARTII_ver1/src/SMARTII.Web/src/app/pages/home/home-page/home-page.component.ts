import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from '../../base/form-base.component';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from '../../../store/reducers/index';
import { skip, takeUntil, filter } from 'rxjs/operators';
import { OrganizationType } from 'src/app/model/organization.model';
import * as fromHomePageActions from '../../../store/actions/home-page.actions';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';


export const PREFIX = 'HomePageComponent';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent extends FormBaseComponent implements OnInit {

  homeId: string;

  constructor(
    private store: Store<fromRootReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
  }

  subscription() {
    this.store
      .select((state: fromRootReducer) => state.home.homeID)
      .pipe(
        filter(data => !!data),
        takeUntil(this.destroy$)
      )
      .subscribe(item => {
        this.homeId = item;
      });
  }
}
