import { Component, OnInit, Input, Injector, ViewChild } from '@angular/core';
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
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromCaseCreatorAction from '../../../store/actions/case-creator.actions';
import * as fromRootActions from 'src/app/store/actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { UcoComponent } from '../../element/uco/uco.component';
import { UcmComponent } from '../../element/ucm/ucm.component';
import { CcoComponent } from '../cco/cco.component';




@Component({
  selector: 'app-cc3',
  templateUrl: './cc3.component.html',
  styleUrls: ['./cc3.component.scss']
})
export class Cc3Component extends FormBaseComponent implements OnInit {

  ucoModalRef: NgbModalRef;
  ucmModalRef: NgbModalRef;

  @ViewChild('hashtag') tagRef: HashtagComponent;
  @ViewChild('uco') ucoRef: UcoComponent;
  @ViewChild('ucm') ucmRef: UcmComponent;
  @ViewChild('cco') ccoRef: CcoComponent;
  @Input() caseKey: string;
  @Input() sourcekey: string;

  afterSecondaryConcatUsers: CaseConcatUserViewModel[] = [];
  afterSecondaryCompainedUsers: CaseComplainedUserViewModel[] = [];

  @Input() public form: FormGroup = new FormGroup({});
  ucoform: FormGroup = new FormGroup({});

  model: CaseViewModel = new CaseViewModel();
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
        this.tempSource = { ...x[this.sourcekey] };
      })

    this.store.select(`case`, `caseCreator`, `${this.caseKey}`)
      .pipe(
        takeUntil(this.destroy$),
        filter(x => !!x),
        take(1)
      )
      .subscribe(x => {
        this.model = x;
        this.model.SourceID = this.tempSource.SourceID;
        this.model.NodeID = this.tempSource.NodeID;
        this.model.GroupID = this.tempSource.GroupID;
        this.model.OrganizationType = this.tempSource.OrganizationType;

        this.initialPayload();
      })

  }


  btnSaveCase() {

    this.generatorPayload();

    if (this.isCompleteValid() == false) return;
    
    const data = new EntrancePayload<CaseViewModel>(this.model);

    data.success = (model: CaseViewModel) => {

      this.store.dispatch(new fromCaseCreatorAction.loadCaseIDsAction(new EntrancePayload(this.sourcekey)));
      this.store.dispatch(new fromCaseCreatorAction.removeCaseTabAction({
        sourceID: model.SourceID,
        caseID: model.CaseID
      }));
    }

    this.store.dispatch(new fromCaseCreatorAction.addCaseAction(data));
  }

  btnRemovePreviewCase(caseID: string) {
    this.deleteRelationCase(caseID);
  }

  deleteRelationCase(caseID: string) {
    this.model.RelationCaseIDs = this.model.RelationCaseIDs.filter(x => x !== caseID);
  }



  generatorConcatUsers() {
    if (this.ucoRef.onSetUser) {
      this.model.CaseConcatUsers = [this.ucoRef.onSetUser, ...this.afterSecondaryConcatUsers];
    }
  }

  generatorCompainedUsers() {

    if (this.ucmRef.onSetUser) {
      if (this.ucmRef.onSetUser.UnitType == undefined || this.ucmRef.onSetUser.UnitType == null) {
        this.model.CaseComplainedUsers = []
      } else {
        this.model.CaseComplainedUsers = [this.ucmRef.onSetUser, ...this.afterSecondaryCompainedUsers];

      }
    }
  }
  btnRemoveComplaintUser() {
    this.afterSecondaryCompainedUsers = [];
    this.ucmRef.onSetUser = new CaseComplainedUserViewModel();
    this.ucmRef.onSetUser.UnitType = null;
    this.ucmRef.resetUI();
  }


  generatorPayload() {

    this.model.CaseTagsMark = this.tagRef.tags;
    this.generatorConcatUsers();
    this.generatorCompainedUsers();
    this.model.JContent = JSON.stringify(this.model.Particular);
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
  btnAddComplaintUserModal() {
    if (!this.isFirstUserCheck(this.ucmRef.onSetUser)) return;

    this.ucmModalRef = this.modalService.open(UcmModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = <UcmModalComponent>this.ucmModalRef.componentInstance;
    instance.users = this.afterSecondaryCompainedUsers;
    instance.sourcekey = this.sourcekey;
    instance.onCloseModal = (users: CaseComplainedUserViewModel[]) => {
      this.afterSecondaryCompainedUsers = [...users]
    }
  }

  isCompleteValid() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    if (!this.model.QuestionClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_NOTICE.CASE_CATEGORY_NEED'))));
      return;
    }

    if (!this.model.CaseConcatUsers || this.model.CaseConcatUsers.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    // 判斷 反應者項目
    if (this.validConcatAndComplained(this.model.CaseConcatUsers, this.ucoRef.valid.bind(this.ucoRef)) == false) {
      return false;
    }

    if (this.model.CaseComplainedUsers && this.model.CaseComplainedUsers.length > 0) {
      // 判斷 被反應者項目
      let validor = function (user: any) {
        return this.ucmRef.valid(user, this.model.CaseComplainedUsers);
      }.bind(this);

      if (this.validConcatAndComplained(this.model.CaseComplainedUsers, validor) == false) {
        return false;
      }
    }


  }

  validConcatAndComplained<T>(datas: T[], validor: (data: T) => boolean) {

    let valid = true;
    datas.forEach(user => {

      // 上一筆驗證通過就繼續, 否則不繼續驗證
      valid == true && (valid = validor(user));
    })

    return valid;
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
