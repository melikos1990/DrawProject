import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { ActionType } from 'src/app/model/common.model';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import * as fromCallCenterNodeActions from '../store/actions/callcenter-node.action';
import { Store } from '@ngrx/store';

const PREFIX = 'CallCenterNodeComponent';

@Component({
  selector: 'app-callcenter-node',
  templateUrl: './callcenter-node.component.html',
})
export class CallCenterNodeComponent extends BaseComponent implements OnInit {

  public uiActionType = ActionType.Update;

  constructor(
    public store: Store<fromOrganizationReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.store.dispatch(new fromCallCenterNodeActions.loadEntryAction());
  }



}
