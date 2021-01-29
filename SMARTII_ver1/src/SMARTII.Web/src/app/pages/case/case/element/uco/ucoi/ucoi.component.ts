import { Component, OnInit, Injector, Input, TemplateRef, ViewChild, Output, EventEmitter, SimpleChanges, OnChanges, KeyValueDiffer, KeyValueDiffers, ChangeDetectionStrategy } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseConcatUserViewModel, CaseSourceViewModel, CaseSourceUserViewModel } from 'src/app/model/case.model';
import { StoresSearchViewModel, StoresDetailViewModel } from 'src/app/model/master.model';
import { OrganizationType, OrganizationDataRangeSearchViewModel, OrganizationNodeViewModel, VendorNodeViewModel, CallCenterNodeViewModel, HeaderQuarterNodeViewModel } from 'src/app/model/organization.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, filter, take } from 'rxjs/operators';

import * as fromRootAction from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';


import { Guid } from 'guid-typescript';
import { ActionType } from 'src/app/model/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { webHostPrefix } from 'src/app.config';
import { TelephoneNumberValidator } from 'src/app/shared/data/validator';


@Component({
  selector: 'app-ucoi',
  templateUrl: './ucoi.component.html',
  styleUrls: ['./ucoi.component.scss'],
})
export class UcoiComponent extends FormBaseComponent implements OnInit {

  @Input() form: FormGroup = new FormGroup({});

  @ViewChild('allNodeTreeSelector') selectorRef: TemplateRef<any>;
  treemodel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();

  organizationRef: NgbModalRef;
  private differ: KeyValueDiffer<CaseConcatUserViewModel, any>;

  @Input() uiActionType: ActionType = this.actionType.Add;
  @Output() uiActionTypeChange = new EventEmitter();

  @Input() sourcekey: string;
  @Input() storeSearchTerm = new StoresSearchViewModel();
  @Input() organizationSearchTerm = new OrganizationDataRangeSearchViewModel();
  @Input() displayWithConcatUser: boolean = true;

  private _user: CaseConcatUserViewModel = new CaseConcatUserViewModel();

  @Input()
  set user(v: CaseConcatUserViewModel) {
    this._user = v;
    this.refillUser(v);
  }
  get user(): CaseConcatUserViewModel {
    return this._user;
  }

  @Output() userChange = new EventEmitter();

  sameOfSourceUser: boolean = false;
  storeSelectItems: any[] = [];

  tempSource: CaseSourceViewModel = new CaseSourceViewModel();

  constructor(
    private differs: KeyValueDiffers,
    public store: Store<any>,
    public modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }



  ngOnInit() {
    this.initializeForm();
    this.subscription();
    this.differ = this.differs.find(this.user).create();

  }

  onUnitTypeChange = ($event) => this.resetUI();

  onStoreChange($event) {
    if(!$event.item){
      this.resetUI()
    }
    else{
      const data: StoresDetailViewModel = $event.item.extend;
      this.refillStore(data);
      this.getParentNames(data.NodeID, this.organizationType.HeaderQuarter)
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
    this.getParentNames($event.ID, $event.OrganizationType)
    this.dismissOrganizationModel();
  }

  dismissOrganizationModel() {
    this.organizationRef && this.organizationRef.dismiss();
  }


  btnSameOfSourceUser($event) {
    if (this.sameOfSourceUser === true && this.tempSource) {
      this.refillSameOfSourceUser(this.tempSource.User)
    }
  }


  getParentNames(nodeID: number, organizationType: OrganizationType) {
    this.caseService
      .getOrganizationParentPath(nodeID, organizationType)
      .pipe(takeUntil(this.destroy$))
      .subscribe((g: string[]) => {
        this.user.ParentPathName = (g || []).join('/');
      })
  }



  btnPreviewStore($event) {

    if (!this.user.NodeID) {
      this.store.dispatch(new fromRootAction.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇門市")));
      return;
    }

    let url = `${environment.webHostPrefix}/pages/master/stores-detail`.toCustomerUrl({
      actionType: this.actionType.Read,
      Id: this.user.NodeID,
      OrganizationType: this.organizationType.HeaderQuarter
    })


    window.open(url, '_blank');
  }


  resetUI() {
    this.user = { ...new CaseConcatUserViewModel(), ...{ UnitType: this.user.UnitType } }
    this.user.ParentPathName = '';
    this.user.key = Guid.create().toString();
    this.user.Gender = null;
    this.uiActionType = this.actionType.Add;
    this.storeSelectItems = [{}];
    this.uiActionTypeChange.emit(this.uiActionType);
    this.resetFormControl(this.form);
  }

  refillSameOfSourceUser(user: CaseSourceUserViewModel) {
    this.user = { ...new CaseConcatUserViewModel(), ...user }

    if ((this.user.UnitType == this.unitType.Organization ||
      this.user.UnitType == this.unitType.Store) &&
      this.user.NodeID) {
      this.getParentNames(this.user.NodeID, this.user.OrganizationType)

    }
    
    this.storeSelectItems = [{
      id: this.user.NodeID,
      text: `${this.user.StoreNo}-${this.user.NodeName}`
    }]

  }

  refillUser(user: CaseConcatUserViewModel) {

    if (!user) return;

    if (user.UnitType == this.unitType.Store) {
      this.storeSelectItems = [{
        id: user.NodeID,
        text: `${this.user.StoreNo}-${this.user.NodeName}`
      }]
    }
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

  initializeForm() {

    this.form.addControl('UserName', new FormControl(this.user.UserName, [
      Validators.maxLength(30),
    ]));
    this.form.addControl('Mobile', new FormControl(this.user.Mobile, [
      TelephoneNumberValidator,
    ]));
    this.form.addControl('Telephone', new FormControl(this.user.Telephone, [
      Validators.maxLength(20),
    ]));
    this.form.addControl('TelephoneBak', new FormControl(this.user.TelephoneBak, [
      Validators.maxLength(20),
    ]));
    this.form.addControl('Email', new FormControl(this.user.Email, [
      Validators.maxLength(40),
      Validators.email
    ]));
    this.form.addControl('Address', new FormControl(this.user.Address, [
      Validators.maxLength(40),
    ]));
    this.form.addControl('OrgConcatUser', new FormControl(this.user.UserName, [
      Validators.maxLength(30),
    ]));
  }
}
