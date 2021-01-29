import { Component, OnInit, Input, Injector } from '@angular/core';
import { ActionType } from 'src/app/model/common.model';
import { Subscription } from 'rxjs';
import { NodeJobListViewModel, VendorNodeDetailViewModel } from 'src/app/model/organization.model';
import { Store } from '@ngrx/store';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { State as fromOrganizationReducers } from '../../../store/reducers';


const PREFIX = 'VendorNodeComponent';

@Component({
  selector: 'app-vendor-job-user-information',
  templateUrl: './job-user-information.component.html',
  styleUrls: ['./job-user-information.component.scss']
})
export class JobUserInformationComponent extends FormBaseComponent implements OnInit {

  @Input() uiActionType: ActionType;

  model$: Subscription;
  currentJob: NodeJobListViewModel = new NodeJobListViewModel();

  public userOptions = {};
  public options = {};
  public model: VendorNodeDetailViewModel = new VendorNodeDetailViewModel();

  constructor(
    public injector: Injector,
    private store: Store<fromOrganizationReducers>) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.vendorNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
      });
  }


  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

}
