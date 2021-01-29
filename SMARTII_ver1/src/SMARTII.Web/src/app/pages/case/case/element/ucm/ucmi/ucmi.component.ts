import { Component, OnInit, Injector, TemplateRef, ViewChild, Input, Output, EventEmitter, SimpleChanges, OnChanges, KeyValueDiffer, KeyValueDiffers } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StoresSearchViewModel, StoresDetailViewModel } from 'src/app/model/master.model';
import { OrganizationDataRangeSearchViewModel, OrganizationNodeViewModel, OrganizationType, HeaderQuarterNodeViewModel, VendorNodeViewModel, CallCenterNodeViewModel, UserListViewModel } from 'src/app/model/organization.model';
import { CaseComplainedUserViewModel, CaseSourceViewModel, CaseComplainedUserType } from 'src/app/model/case.model';
import { Store } from '@ngrx/store';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, filter, take } from 'rxjs/operators';
import { Guid } from 'guid-typescript';

import * as fromRootAction from 'src/app/store/actions';
import { ActionType } from 'src/app/model/common.model';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-ucmi',
  templateUrl: './ucmi.component.html',
  styleUrls: ['./ucmi.component.scss']
})
export class UcmiComponent extends FormBaseComponent implements OnInit {

  @ViewChild('allNodeTreeSelector') selectorRef: TemplateRef<any>;
  treemodel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  organizationRef: NgbModalRef;

  private differ: KeyValueDiffer<CaseComplainedUserViewModel, any>;

  private _user: CaseComplainedUserViewModel;

  @Input() uiActionType: ActionType = this.actionType.Add;
  @Output() uiActionTypeChange = new EventEmitter();
  @Input() sourcekey: string;
  @Input() storeSearchTerm = new StoresSearchViewModel();
  @Input() organizationSearchTerm = new OrganizationDataRangeSearchViewModel();

  @Input()
  set user(v: CaseComplainedUserViewModel) {
    this._user = v;
    this.refillUser(v);
    this.StoreTypeName = v.StoreTypeName;
  }
  get user(): CaseComplainedUserViewModel {
    return this._user;
  }

  @Output() userChange = new EventEmitter();

  storeSelectItems: any[] = [];

  tempStoreInfo: StoresDetailViewModel;
  tempSource: CaseSourceViewModel = new CaseSourceViewModel();
  StoreTypeName: string;

  constructor(
    private differs: KeyValueDiffers,
    public store: Store<any>,
    public modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.subscription();
    this.differ = this.differs.find(this.user).create();
  }


  onUnitTypeChange = ($event) => this.resetUI();

  onStoreChange($event) {
    if(!$event.item){
      this.resetUI()
    }
    else{
      const data: StoresDetailViewModel = !$event.item ? new StoresDetailViewModel() : $event.item.extend;
      this.refillStore(data);
      this.getParentNames(data.NodeID, this.organizationType.HeaderQuarter);
      this.getStoreOwnerUser(data.OwnerNodeJobID);
      this.getStoreSupervisorUser(data.SupervisorNodeJobID);
      this.StoreTypeName = data.StoreTypeName;
    }
  }

  btnOrganizationModal($event) {
    // 需確認立案時須要有甚麼資料範圍
    // this.organizationSearchTerm.NodeID = this.buID;
    // this.organizationSearchTerm.IsStretch = true;
    this.organizationRef = this.modalService.open(this.selectorRef, { size: 'lg', container: 'nb-layout', backdrop: 'static' });

  }

  onSelectOrganizationNode($event) {
    this.refillOrganizationNode($event);
    this.getParentNames($event.ID, $event.OrganizationType);
    this.getNodeOwnerUser($event.ID, $event.OrganizationType);
    this.dismissOrganizationModel();
  }

  dismissOrganizationModel() {
    this.organizationRef && this.organizationRef.dismiss();
  }


  getParentNames(nodeID: number, organizationType: OrganizationType) {
    this.caseService
      .getOrganizationParentPath(nodeID, organizationType)
      .pipe(takeUntil(this.destroy$))
      .subscribe((g: string[]) => {
        this.user.ParentPathName = (g || []).join('/');
      })
  }

  getNodeOwnerUser(nodeID: number, organizationType: OrganizationType) {
    this.caseService
      .getOwnerUserFromNode(nodeID, organizationType)
      .pipe(takeUntil(this.destroy$))
      .subscribe((g: UserListViewModel) => {
        this.user.OwnerUserName = g.UserName;
        this.user.OwnerUserPhone = g.Mobile;
        this.user.OwnerJobName = g.JobName;
        this.user.OwnerUserEmail = g.Email;
      })
  }
  getStoreOwnerUser(nodeJobID: number) {
    this.caseService
      .getOwnerUserFromStore(nodeJobID)
      .pipe(takeUntil(this.destroy$))
      .subscribe((g: UserListViewModel) => {
        this.user.OwnerUserName = g.UserName;
        this.user.OwnerUserPhone = g.Mobile;
        this.user.OwnerJobName = g.JobName;
        this.user.OwnerUserEmail = g.Email;
      })
  }
  getStoreSupervisorUser(nodeJobID: number) {
    this.caseService
      .getOwnerUserFromStore(nodeJobID)
      .pipe(takeUntil(this.destroy$))
      .subscribe((g: UserListViewModel) => {
        this.user.SupervisorUserName = g.UserName;
        this.user.SupervisorUserPhone = g.Mobile;
        this.user.SupervisorJobName = g.JobName;
        this.user.SupervisorNodeName = g.NodeName;
        this.user.SupervisorUserEmail = g.Email;
      })
  }



  btnPreviewStore($event) {

    if (!this.user.NodeID) {
      this.store.dispatch(new fromRootAction.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇門市")));
      return;
    }
    
    const url = `${environment.webHostPrefix}/pages/master/stores-detail`.toCustomerUrl({
      actionType: this.actionType.Read,
      Id: this.user.NodeID,
      OrganizationType: this.organizationType.HeaderQuarter,
    })
    window.open(url, '_blank');
  }

  resetUI() {
    this.user = { ...new CaseComplainedUserViewModel(), ...{ UnitType: this.user.UnitType } }
    this.user.ParentPathName = '';
    this.user.key = Guid.create().toString();
    this.user.CaseComplainedUserType = CaseComplainedUserType.Responsibility;
    this.user.Gender = null;
    this.storeSelectItems = [{}];
    this.tempStoreInfo = null;
    this.uiActionType = this.actionType.Add;
    this.uiActionTypeChange.emit(this.uiActionType);
  }

  refillUser(user: CaseComplainedUserViewModel) {

    if (!user) return;

    if (user.UnitType == this.unitType.Store) {
      this.storeSelectItems = [{
        id: user.NodeID,
        text: user.NodeName
      }]
    }
  }

  refillStore(store: StoresDetailViewModel) {
    this.user.NodeID = store.NodeID;
    this.user.OrganizationType = this.organizationType.HeaderQuarter;
    this.user.NodeName = store.Name;
    this.user.UserName = store.Name;
    this.user.BUID = store.BuID
    this.user.BUName = store.BuName;
    this.user.Email = store.Email;
    this.user.Address = store.Address;
    this.user.Telephone = store.Telephone;
    this.user.StoreNo = store.Code;
    // 暫存門市顯示資訊
    this.tempStoreInfo = store;
  }
  refillOrganizationNode(node: OrganizationNodeViewModel) {
    this.tempStoreInfo = null;

    this.user.UserName = node.Name;
    this.user.NodeID = node.ID;
    this.user.OrganizationType = node.OrganizationType;
    this.user.NodeName = node.Name;

    switch (node.OrganizationType) {
      case this.organizationType.Vendor: this.refillVendorNode(<VendorNodeViewModel>node); break;
      case this.organizationType.CallCenter: this.refillCallCenterNode(<CallCenterNodeViewModel>node); break;
      case this.organizationType.HeaderQuarter: this.refillHeaderquarterNode(<HeaderQuarterNodeViewModel>node); break;
    }
  }
  refillHeaderquarterNode(node: HeaderQuarterNodeViewModel) {
    this.user.BUID = node.BUID;
    this.user.BUName = node.BUName;
  }
  refillVendorNode(node: VendorNodeViewModel) {
    // this.user.BUID = node.BUID;
    // this.user.BUName = node.BUName;
  }
  refillCallCenterNode(node: CallCenterNodeViewModel) {
    // this.user.BUID = node.BUID;
    // this.user.BUName = node.BUName;
  }

  onSelecteChange($event) {

    if(!$event) return;

    this.treemodel = new OrganizationDataRangeSearchViewModel();    
    this.treemodel.IsEnabled = true;
    switch ($event.toString()) {
      case OrganizationType.CallCenter.toString():
        this.treemodel.IsSelf = true;
        this.treemodel.Goal = OrganizationType.CallCenter;
        break;

      case OrganizationType.HeaderQuarter.toString():
        this.treemodel.NodeID = this.organizationSearchTerm.NodeID;
        this.treemodel.IsStretch = true;
        this.treemodel.Goal = OrganizationType.HeaderQuarter;
        this.treemodel.NotIncludeDefKey = [this.definitionKey.STORE];
        break;

      case OrganizationType.Vendor.toString():
        this.treemodel.NodeID = this.organizationSearchTerm.NodeID;
        this.treemodel.Goal = OrganizationType.Vendor;
        break;

      default:
        break;
    }
  }

  ngDoCheck(): void {
    const changes = this.differ.diff(this.user);
    if (changes) {
      this.userChange.emit(this.user)
    }
  }

  subscription() {


    this.caseService
      .sorceTempSubject.pipe(
        takeUntil(this.destroy$),
        filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey)),
      )
      .subscribe(x => {
        const newer = { ...x[this.sourcekey] };
        this.tempSource = newer;
        this.storeSearchTerm.BuID = newer.NodeID;
        this.organizationSearchTerm.NodeID = newer.NodeID;
        this.organizationSearchTerm.IsStretch = true;
      })
  }
}
