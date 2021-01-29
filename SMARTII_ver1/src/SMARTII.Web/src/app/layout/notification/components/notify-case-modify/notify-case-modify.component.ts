import { Component, OnInit, Input, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as fromNotificationAction from 'src/app/store/actions/notification.actions';
import { ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-notify-case-modify',
  templateUrl: './notify-case-modify.component.html',
  styleUrls: ['./notify-case-modify.component.scss']
})
export class NotifyCaseModifyComponent extends BaseComponent implements OnInit {

  @Input() model: any;

  constructor(
    public injector: Injector,
    public store: Store<fromRootReducer>,
    private modalService: NgbModal,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  direct(){
    
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/case/case-create',
      params: {
        actionType: ActionType.Update,
        caseID: this.model.Extend.CaseID
      }
    }));

    this.modalService.dismissAll();
    this.store.dispatch(new fromNotificationAction.removePersonalNotification({ id: this.model.ID }));
  }

}
