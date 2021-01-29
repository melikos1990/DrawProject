import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { ActionType } from 'src/app/model/common.model';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import * as fromHeaderQuarterNodeActions from '../store/actions/headerquarter-node.action';
import { Store } from '@ngrx/store';
import { AuthBaseComponent } from '../../base/auth-base.component';

const PREFIX = 'HeaderquarterNodeComponent';


@Component({
  selector: 'app-headerquarter-node',
  templateUrl: './headerquarter-node.component.html',
})
export class HeaderquarterNodeComponent extends AuthBaseComponent implements OnInit {


  public uiActionType = ActionType.Update;

  constructor(
    public store: Store<fromOrganizationReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.store.dispatch(new fromHeaderQuarterNodeActions.loadEntryAction());

    this.checkAuthority();

  }

  checkAuthority(){

    this.ishasAuth$(this.authType.Update)
      .subscribe(hasAuth => {
        this.uiActionType = hasAuth ? this.actionType.Update : this.actionType.Read
      })

  }

}
