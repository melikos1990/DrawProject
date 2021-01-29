import { Component, OnInit, Input, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as fromNotificationAction from 'src/app/store/actions/notification.actions';
import { ActionType } from 'src/app/model/common.model';
import * as moment from 'moment';

@Component({
  selector: 'app-notify-case-finished',
  templateUrl: './notify-case-finished.component.html',
  styleUrls: ['./notify-case-finished.component.scss']
})
export class NotifyCaseFinishedComponent extends BaseComponent implements OnInit {

  @Input() model: any;
  finishDate: string;

  constructor(
    public injector: Injector,
    public store: Store<fromRootReducer>,
    private modalService: NgbModal,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.finishDate = !this.model.Extend.FinishDateTime ? 
                        this.translateService.instant('ERROR.NAN') : moment(this.model.Extend.FinishDateTime).format('YYYY-MM-DD HH:mm');
  }


  direct() {
    console.log("model => ", this.model);
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
