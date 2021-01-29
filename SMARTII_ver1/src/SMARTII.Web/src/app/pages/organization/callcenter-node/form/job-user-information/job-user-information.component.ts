import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { Subscription } from 'rxjs';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import { CallCenterNodeDetailViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

const PREFIX = 'CallCenterNodeComponent';

@Component({
  selector: 'app-callcenter-job-user-information',
  templateUrl: './job-user-information.component.html',
})
export class JobUserInformationComponent extends FormBaseComponent implements OnInit {

  @Input() uiActionType: ActionType;

  model$: Subscription;
  currentJob: NodeJobListViewModel = new NodeJobListViewModel();

  public userOptions = {};
  public options = {};
  public model: CallCenterNodeDetailViewModel = new CallCenterNodeDetailViewModel();

  constructor(
    public injector: Injector,
    private store: Store<fromOrganizationReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.callcenterNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
      });
  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }


}
