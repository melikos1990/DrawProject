import { Component, OnInit, Injector, Input, OnDestroy } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { VendorNodeDetailViewModel, JobListViewModel, AddJobViewModel, NodeDefinitionSearchViewModel, NodeJobListViewModel } from 'src/app/model/organization.model';
import { Subscription } from 'rxjs';
import { State as fromOrganizationReducers } from '../../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromHeaderVendorActions from '../../../../store/actions/vendor-node.action';
import { Store } from '@ngrx/store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { JobSelectorComponent } from 'src/app/shared/component/modal/job-selector/job-selector.component';
import { failedAlertAction } from 'src/app/shared/ngrx/alert.ngrx';
import { VendorNodeUserModalComponent } from 'src/app/shared/component/modal/vendor-node-user-modal/vendor-node-user-modal.component';


const PREFIX = 'VendorNodeComponent';

@Component({
  selector: 'app-vendor-job-table',
  templateUrl: './job-table.component.html',
})
export class JobTableComponent extends FormBaseComponent implements OnInit, OnDestroy {

  @Input() uiActionType: ActionType;

  public options = {};
  model$: Subscription;
  public model: VendorNodeDetailViewModel = new VendorNodeDetailViewModel();


  constructor(
    private modalService: NgbModal,
    private store: Store<fromOrganizationReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initializeTable();
  }


  jobRowSelect = ($event) => this.store.dispatch(new fromHeaderVendorActions.selectJobAction($event.data));


  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnDeleteJob($event) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ nodeJobID: number }>();
        payload.data = { nodeJobID: $event.data.NodeJobID };
        payload.success = () => {

          this.store.dispatch(new fromHeaderVendorActions.loadDetailAction(new EntrancePayload<number>(this.model.ID)));
          this.modalService.dismissAll();
        };
        this.store.dispatch(new fromHeaderVendorActions.deleteJobAction(payload));
      }
    )));

  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnJobSelector($event) {

    if (!this.model.DefindID) {
      this.store.dispatch(failedAlertAction("組織定義必填欄位"));
      return;
    }

    const ref = this.modalService.open(JobSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<JobSelectorComponent>ref.componentInstance);

    instance.main = this.model;
    instance.btnAddJob = this.btnAddJob.bind(this);
  }


  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  @loggerMethod()
  btnAddJob($event: JobListViewModel[]) {

    const existIDs = this.model.Jobs.map(x => x.ID);
    const newIDs = $event.map(x => x.ID);

    const intersection = existIDs.filter(v => newIDs.includes(v));


    if (intersection.length > 0) {
      let itemName = $event.filter(x => intersection.includes(x.ID)).map(x => x.Name).join("/");
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(itemName + this.translateService.instant("HEADERQUARTER_NODE.JOB_REPEAT"))));
      return;
    }

    if (newIDs.length === 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const payload = new EntrancePayload<AddJobViewModel>();
    payload.data = new AddJobViewModel();
    payload.data.NodeID = this.model.ID;
    payload.data.JobIDs = newIDs;
    payload.success = () => {

      this.store.dispatch(new fromHeaderVendorActions.loadDetailAction(new EntrancePayload<number>(this.model.ID)));
      this.modalService.dismissAll();
    };
    this.store.dispatch(new fromHeaderVendorActions.addJobAction(payload));
  }

  subscription() {

    this.model$ = this.store.select(x => x.organization.vendorNode.detail)
      .subscribe(detail => {
        this.model = { ...detail };
      });
  }

  initializeTable() {

    this.options = {
      columns: {
        Name: {
          title: this.translateService.instant('JOB.NAME'),
          width: '20%',
        },
        
        Level: {
          title: this.translateService.instant('JOB.LEVEL'),
          width: '20%',
        },

      },
      actions: {
        position: 'right',
        columnTitle: this.translateService.instant('COMMON.ACTION'),
        edit: false,
        add: false,
        delete: this.uiActionType === ActionType.Update,
      },
      delete: {
        deleteButtonContent: '<i class="nb-trash"></i>',
        confirmDelete: true,
      },
    };
  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }

  
  @loggerMethod()
  btnUserPreview($event) {

    const ref = this.modalService.open(VendorNodeUserModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<VendorNodeUserModalComponent>ref.componentInstance);

    instance.main = this.model;
    instance.nodeID = this.model.ID;

  }

}
