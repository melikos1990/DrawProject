import { Component, OnInit, Input, Injector } from '@angular/core';
import { BillboardWarningType } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as fromNotificationAction from 'src/app/store/actions/notification.actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';

@Component({
  selector: 'app-notify-billboard',
  templateUrl: './notify-billboard.component.html',
  styleUrls: ['./notify-billboard.component.scss']
})
export class NotifyBillboardComponent extends FormBaseComponent implements OnInit {

  
  @Input() model: any;
  billboardWarningType = BillboardWarningType;

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
      url: './pages/master/billboard-display',
      params: {
        id: this.model.ID,
        billboardWarningType: this.model.BillboardWarningType
      }
    }));

    this.modalService.dismissAll();

    
    this.store.dispatch(new fromNotificationAction.removePersonalNotification({ id: this.model.ID }));
  }

}
