import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { Subscription } from 'rxjs';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import { HeaderQuarterNodeDetailViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

const PREFIX = 'HeaderquarterNodeComponent';

@Component({ 
  selector: 'app-job-user-information',
  templateUrl: './job-user-information.component.html',
})
export class JobUserInformationComponent extends FormBaseComponent implements OnInit {

  @Input() uiActionType: ActionType;

  model$: Subscription;
  currentJob: NodeJobListViewModel = new NodeJobListViewModel();

  public userOptions = {};
  public options = {};
  public model: HeaderQuarterNodeDetailViewModel = new HeaderQuarterNodeDetailViewModel();

  constructor(
    public injector: Injector,
    private store: Store<fromOrganizationReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.headerQuarterNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
      });
  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }


}

