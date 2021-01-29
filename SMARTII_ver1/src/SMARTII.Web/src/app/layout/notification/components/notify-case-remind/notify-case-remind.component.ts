import { Component, OnInit, Input, Injector } from '@angular/core';
import { CaseRemindType } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-notify-case-remind',
  templateUrl: './notify-case-remind.component.html',
  styleUrls: ['./notify-case-remind.component.scss']
})
export class NotifyCaseRemindComponent extends BaseComponent implements OnInit {

  @Input() model: any;

  caseRemindType = CaseRemindType;

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
      url: './pages/master/case-remind-detail',
      params: {
        actionType: this.actionType.Update,
        id: this.model.ID
      }
    }));

    this.modalService.dismissAll();
  }

}
