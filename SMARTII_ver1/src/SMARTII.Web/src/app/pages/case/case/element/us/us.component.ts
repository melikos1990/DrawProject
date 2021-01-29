import { Component, OnInit, Injector, Input, SimpleChanges, OnChanges, Output, EventEmitter, ViewChild, TemplateRef, KeyValueDiffer, KeyValueDiffers } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseSourceUserViewModel, CaseSourceViewModel } from 'src/app/model/case.model';
import { StoresSearchViewModel, StoresDetailViewModel } from 'src/app/model/master.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, filter } from 'rxjs/operators';
import { OrganizationDataRangeSearchViewModel, CallCenterNodeViewModel, OrganizationNodeViewModel, HeaderQuarterNodeViewModel, VendorNodeViewModel, OrganizationType } from 'src/app/model/organization.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectDataItem } from 'ptc-select2';
import { ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Store } from '@ngrx/store';
import * as fromRootActions from "src/app/store/actions";

@Component({
  selector: 'app-us',
  templateUrl: './us.component.html',
  styleUrls: ['./us.component.scss'],

})
export class UsComponent extends FormBaseComponent implements OnInit {

  @ViewChild('allNodeTreeSelector') selectorRef: TemplateRef<any>;
  treemodel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();

  private differ: KeyValueDiffer<CaseSourceUserViewModel, any>;

  @Input() form: FormGroup = new FormGroup({});

  @Input() user: CaseSourceUserViewModel = new CaseSourceUserViewModel();
  @Output() userChange = new EventEmitter();

  @Input() uiActionType: ActionType;
  @Input() sourcekey: string;
  @Input() storeSearchTerm = new StoresSearchViewModel();
  @Input() organizationSearchTerm = new OrganizationDataRangeSearchViewModel();

  refillStoreItem: SelectDataItem[] = [];
  tempSource: CaseSourceViewModel = new CaseSourceViewModel();

  constructor(
    private differs: KeyValueDiffers,
    public modalService: NgbModal,
    private store: Store<any>,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }



  ngOnInit() {
    this.initializeForm();
    this.subscription();
    this.tryRefill();
    this.differ = this.differs.find(this.user).create();
  }

  onStoreChange($event) {
    if(!$event.item){
      this.resetUI()
    }
    else{
      const data: StoresDetailViewModel = !$event.item ? new StoresDetailViewModel() : $event.item.extend;
      this.refillStore(data);
      this.getParentNames(data.NodeID, this.organizationType.HeaderQuarter)
    }
  }

  onOrganizationModal($event) {
    // 需確認立案時須要有甚麼資料範圍
    // this.organizationSearchTerm.NodeID = this.buID;
    // this.organizationSearchTerm.IsStretch = true;
    this.modalService.open(this.selectorRef, { size: 'lg', container: 'nb-layout', backdrop: 'static' });

  }

  onSelectOrganizationNode($event: OrganizationNodeViewModel) {
    this.refillOrganizationNode($event);
    this.getParentNames($event.ID, $event.OrganizationType)
    this.modalService.dismissAll();
  }

  onUnitTypeChange = ($event) => this.resetUI();

  getParentNames(nodeID: number, organizationType: OrganizationType) {
    this.caseService
      .getOrganizationParentPath(nodeID, organizationType)
      .pipe(takeUntil(this.destroy$))
      .subscribe((g: string[]) => {
        this.user.ParentPathName = (g || []).join('/');
      })
  }

  resetUI() {
    this.user = { ...new CaseSourceUserViewModel(), ...{ UnitType: this.user.UnitType } }
    this.refillStoreItem = null;
    this.resetFormControl(this.form);
  }
  refillStore(store: StoresDetailViewModel) {
    this.user.NodeID = store.NodeID;
    this.user.OrganizationType = this.organizationType.HeaderQuarter;
    this.user.NodeName = store.Name;
    this.user.BUID = store.BuID
    this.user.BUName = store.BuName;
    this.user.StoreNo = store.Code;
  }
  refillOrganizationNode(node: OrganizationNodeViewModel) {
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

  tryRefill() {
    if (this.user.UnitType && this.user.NodeID) {
      this.getParentNames(this.user.NodeID, this.user.OrganizationType);
      this.refillStoreItem = [{ id: this.user.NodeID, text: `${this.user.StoreNo}-${this.user.NodeName}` }];

    }
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
      console.log('change us')
      this.userChange.emit(this.user)
    }
  }

  subscription() {

    this.caseService.sorceTempSubject.pipe(
      takeUntil(this.destroy$),
      filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey)))
      .subscribe(x => {
        this.tempSource = x[this.sourcekey];
        this.storeSearchTerm.BuID = this.tempSource.NodeID;
        this.organizationSearchTerm.NodeID = this.tempSource.NodeID;
        this.organizationSearchTerm.IsStretch = true;

      })
  }

  valid(user: CaseSourceUserViewModel) {

    let result = true, payload;

    if (user.UnitType == null || user.UnitType == undefined) {
      payload = this.getFieldInvalidMessage("[來源反應者]請選擇反應者類型");
      result = false;
    }

    switch (user.UnitType) {
      case this.unitType.Customer:

        if (!this.user.UserName){
          payload = this.getFieldInvalidMessage("[來源反應者]請輸入反應者名稱");
          result = false;
        } 

        if (this.user.Gender == null || this.user.Gender == undefined) {
          payload = this.getFieldInvalidMessage("[來源反應者]請選擇反應者性別");
          result = false;
        }

        break;

      case this.unitType.Store:

        if (!this.user.NodeID){
          payload = this.getFieldInvalidMessage("[來源]請選擇門市");
          result = false;
        }

        break;

      case this.unitType.Organization:

        if (!this.user.NodeID){
          payload = this.getFieldInvalidMessage("[來源]請選擇單位");
          result = false;
        } 

        break;

    }

    
    !!(payload) && this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(payload));
    return result;
  }


  initializeForm() {

    this.form.addControl('UserName', new FormControl(this.user.UserName, [
      Validators.maxLength(30),
    ]));
    this.form.addControl('Mobile', new FormControl(this.user.Mobile, [
      Validators.maxLength(20),
    ]));
    this.form.addControl('Telephone', new FormControl(this.user.Telephone, [
      Validators.maxLength(20),
    ]));
    this.form.addControl('Email', new FormControl(this.user.Email, [
      Validators.maxLength(40),
      Validators.email
    ]));
  }
}
