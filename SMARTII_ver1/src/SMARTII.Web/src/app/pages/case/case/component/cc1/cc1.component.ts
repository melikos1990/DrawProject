import { Component, OnInit, Input, Injector, OnChanges, SimpleChanges, ViewChild, Output, EventEmitter, KeyValueDiffer, KeyValueDiffers, ViewChildren } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { CaseViewModel, CaseConcatUserViewModel, CaseComplainedUserViewModel, CaseSourceViewModel } from 'src/app/model/case.model';
import { HashtagComponent } from 'src/app/shared/component/other/hashtag/hashtag.component';

import { takeUntil, filter, take } from 'rxjs/operators';
import { CaseService } from 'src/app/shared/service/case.service';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UcoModalComponent } from '../../modal/uco-modal/uco-modal.component';
import { Store } from '@ngrx/store';
import { State as fromCaseReducers } from '../../../store/reducers'
import { UcmModalComponent } from '../../modal/ucm-modal/ucm-modal.component';
import { UcoComponent } from '../../element/uco/uco.component';
import { UcmComponent } from '../../element/ucm/ucm.component';
import { CcoComponent } from '../cco/cco.component';
import * as fromRootActions from 'src/app/store/actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';

@Component({
  selector: 'app-cc1',
  templateUrl: './cc1.component.html',
  styleUrls: ['./cc1.component.scss']
})
export class Cc1Component extends FormBaseComponent implements OnInit {

  ucoModalRef: NgbModalRef;
  ucmModalRef: NgbModalRef;

  @ViewChild('hashtag') tagRef: HashtagComponent;
  @ViewChild('uco') ucoRef: UcoComponent;
  @ViewChild('ucm') ucmRef: UcmComponent;
  @ViewChild('cco') ccoRef: CcoComponent;

  private _model: CaseViewModel = new CaseViewModel();

  @Input()
  set model(v: CaseViewModel) {
    this._model = v;
  }

  get model(): CaseViewModel {
    this.generatorPayload();
    return this._model;
  }


  @Input() sourcekey: string;

  afterSecondaryConcatUsers: CaseConcatUserViewModel[] = [];
  afterSecondaryCompainedUsers: CaseComplainedUserViewModel[] = [];

  @Input() public form: FormGroup = new FormGroup({});
  ucoform: FormGroup = new FormGroup({});

  tempSource: CaseSourceViewModel = new CaseSourceViewModel();

  hasInput: boolean = false;

  constructor(

    public store: Store<fromCaseReducers>,
    public modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initialPayload();
    this.initializeForm();
    this.subscription();
  }

  initialPayload() {
    this.model.CaseConcatUsers = [
      new CaseConcatUserViewModel()
    ]

    this.model.CaseComplainedUsers = [
      new CaseComplainedUserViewModel()
    ]
  }

  initializeForm() {

    this.form.addControl('IsAttension', new FormControl(this.model.IsAttension, [
      Validators.required,
    ]));
    this.form.addControl('CaseWarningID', new FormControl(this.model.CaseWarningID, [
      Validators.required,
    ]));
    this.form.addControl('IsReport', new FormControl(this.model.IsReport, [
      Validators.required,
    ]));
    this.form.addControl('Content', new FormControl(this.model.Content, [
      Validators.required,
    ]));

    this.form.addControl('child', this.ucoform);
  }

  subscription() {

    this.caseService
      .sorceTempSubject.pipe(
        takeUntil(this.destroy$),
        filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey))
      )
      .subscribe(x => {
        const newer = { ...x[this.sourcekey] };
        this._model.NodeID = newer.NodeID;
        this._model.OrganizationType = newer.OrganizationType;
        this._model.GroupID = newer.GroupID;

      })
  }

  btnRemovePreviewCase(caseID: string) {
    this.deleteRelationCase(caseID);
  }

  deleteRelationCase(caseID: string) {
    this._model.RelationCaseIDs = this.model.RelationCaseIDs.filter(x => x !== caseID);
  }

  generatorPayload() {
    this._model.CaseTagsMark = this.tagRef.tags;
    this.generatorConcatUsers();
    this.generatorCompainedUsers();
  }

  generatorConcatUsers() {
    if (this.ucoRef.onSetUser)
      this._model.CaseConcatUsers = [this.ucoRef.onSetUser, ...this.afterSecondaryConcatUsers];

  }

  generatorCompainedUsers() {
    if (this.ucmRef.onSetUser) {
      this._model.CaseComplainedUsers = [this.ucmRef.onSetUser, ...this.afterSecondaryCompainedUsers];
    }
  }


  btnAddConcatUserModal() {
    if (!this.isFirstUserCheck(this.ucoRef.onSetUser)) return;

    this.ucoModalRef = this.modalService.open(UcoModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = <UcoModalComponent>this.ucoModalRef.componentInstance;
    instance.users = this.afterSecondaryConcatUsers;
    instance.sourcekey = this.sourcekey;
    instance.onCloseModal = (users: CaseConcatUserViewModel[]) => {
      this.afterSecondaryConcatUsers = [...users]
    }
  }
  btnRemoveComplaintUser() {
    this.afterSecondaryCompainedUsers = [];
    this.ucmRef.onSetUser = new CaseComplainedUserViewModel();
    this.ucmRef.onSetUser.UnitType = null;
    this.ucmRef.resetUI();
  }
  btnAddComplaintUserModal() {
    if (!this.isFirstUserCheck(this.ucmRef.onSetUser)) return;

    this.ucmModalRef = this.modalService.open(UcmModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = <UcmModalComponent>this.ucmModalRef.componentInstance;

    console.log("afterSecondaryCompainedUsers => ", this.afterSecondaryCompainedUsers);

    instance.users = this.afterSecondaryCompainedUsers;
    instance.sourcekey = this.sourcekey;
    instance.onCloseModal = (users: CaseComplainedUserViewModel[]) => {
      this.afterSecondaryCompainedUsers = [...users]
    }
  }

  isFirstUserCheck(data: any): boolean {
    if (!data) return false;

    switch (+data.UnitType) {
      case this.unitType.Customer:
        if (!this.checkCustomer(data)) {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請輸入主要反應者姓名")));
          return false;
        }
        break;
      case this.unitType.Store:
        if (!this.checkStore(data)) {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇門市")));
          return false;
        }
        break;
      case this.unitType.Organization:
        if (!this.checkOrganization(data)) {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇單位")));
          return false;
        }
        break;
    }
    return true;
  }

  checkCustomer(data: any): boolean {
    return !data.UserName ? false : true;
  }

  checkStore(data: any): boolean {
    return !data.NodeID ? false : true;
  }

  checkOrganization(data: any): boolean {
    return !data.NodeID ? false : true;
  }

  hasInputChange($event: boolean) {
    this.hasInput = $event ? null : false;
  }
}
