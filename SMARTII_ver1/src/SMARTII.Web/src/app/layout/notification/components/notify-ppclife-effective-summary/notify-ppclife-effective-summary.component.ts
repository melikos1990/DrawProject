import { Component, OnInit, Injector, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromNotificationAction from 'src/app/store/actions/notification.actions';
import { BaseComponent } from 'src/app/pages/base/base.component';

@Component({
  selector: 'app-notify-ppclife-effective-summary',
  templateUrl: './notify-ppclife-effective-summary.component.html',
  styleUrls: ['./notify-ppclife-effective-summary.component.scss']
})
export class NotifyPpclifeEffectiveSummaryComponent extends BaseComponent implements OnInit {


  @Input() model: any;

  constructor(
    public injector: Injector,
    public store: Store<fromRootReducer>,
    private modalService: NgbModal
  ) { 
    super(injector)
  }

  ngOnInit() {
  }

  direct(){
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/case/ppclife-effective-summary',
      params: {}
    }));

    this.modalService.dismissAll();
    
    this.store.dispatch(new fromNotificationAction.removePersonalNotification({ id: this.model.ID }));
  }

}
