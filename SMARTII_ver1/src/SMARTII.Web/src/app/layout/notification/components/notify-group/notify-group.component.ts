import { Component, OnInit, Input, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Store } from '@ngrx/store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromNotificationAction from 'src/app/store/actions/notification.actions';

@Component({
  selector: 'app-notify-group',
  templateUrl: './notify-group.component.html',
  styleUrls: ['./notify-group.component.scss']
})
export class NotifyGroupComponent extends BaseComponent implements OnInit {

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
      url: './pages/case/notification-group-sender',
      params:{
      }
    }));

    this.modalService.dismissAll();
    
    this.store.dispatch(new fromNotificationAction.removePersonalNotification({ id: this.model.ID }));
  }
}
