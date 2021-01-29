import { Component, OnInit, Input, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as fromNotificationAction from 'src/app/store/actions/notification.actions';
import { CaseNoticeType } from 'src/app/model/substitute.model';

@Component({
  selector: 'app-notify-mail-adopt',
  templateUrl: './notify-mail-adopt.component.html',
  styleUrls: ['./notify-mail-adopt.component.scss']
})
export class NotifyMailAdoptComponent extends BaseComponent implements OnInit {

  @Input() model: any

  constructor(
    public injector: Injector,
    public store: Store<fromRootReducer>,
    private modalService: NgbModal
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  direct(){
    
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/substitute/case-notice',
      params: {
        caseNoticeType: CaseNoticeType.OfficialEmail
      }
    }));

    this.modalService.dismissAll();
    this.store.dispatch(new fromNotificationAction.removePersonalNotification({ id: this.model.ID }));
  }

}
