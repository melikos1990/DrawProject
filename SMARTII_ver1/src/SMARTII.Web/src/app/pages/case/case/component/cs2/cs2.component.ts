import { Component, OnInit, Injector, Input, KeyValueDiffer, KeyValueDiffers, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { BusinesssUnitParameters, CallCenterNodeDetailViewModel } from 'src/app/model/organization.model';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { CaseSourceViewModel, CaseViewModel } from 'src/app/model/case.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseService } from 'src/app/shared/service/case.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { State as fromCaseReducers } from '../../../store/reducers';
import { filter, takeUntil, skip } from 'rxjs/operators';
import { Guid } from 'guid-typescript';
import * as fromCaseCreatorAction from '../../../store/actions/case-creator.actions';
import * as fromRootActions from 'src/app/store/actions';
import { PcModalComponent } from '../../modal/pc-modal/pc-modal.component';
import { AcComponent } from '../../atom/ac/ac.component';
import { interval } from 'rxjs';
import { GroupSelectComponent } from 'src/app/shared/component/select/element/group-select/group-select.component';
import { UsComponent } from '../../element/us/us.component';


const PREFIX = 'C1Component';


@Component({
  selector: 'app-cs2',
  templateUrl: './cs2.component.html',
  styleUrls: ['./cs2.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Cs2Component extends FormBaseComponent implements OnInit {


  @ViewChild('tabs') tabsRef: any;
  @ViewChild('ac') accomponentRef: AcComponent;
  @ViewChild('groupSelect') groupSelect: GroupSelectComponent;
  @ViewChild('us') usRef: UsComponent;


  @Input() public uiActionType: ActionType;
  @Input() public model: CaseSourceViewModel = new CaseSourceViewModel();
  @Input() public caseKeyPairs: any[] = [];
  @Input() public key: string;
  @Input() public focusCaseID: string;
  @Input() sourceTab: any[];;

  public usform: FormGroup;
  public form: FormGroup;
  public businessParameter: BusinesssUnitParameters;
  public checkableCaseID: string;
  public loading: boolean = true;
  public subLoading: boolean = true;

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    public caseService: CaseService,
    public modalService: NgbModal,
    public store: Store<fromCaseReducers>,
    public injector: Injector) {
    super(injector);
  }

  isNew = (id: string) => id === 'new';

  ngOnInit() {
    this.initializeForm();
    this.initializePayload();
    this.subscription();

  }


  btnPreventionCase() {

    if (!this.model.NodeID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請輸入企業別代號")));
      return;
    }

    const ref = this.modalService.open(PcModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<PcModalComponent>ref.componentInstance);
    instance.sourcekey = this.model.key;
    instance.btnRelated = this.btnRelatedPrevention.bind(this);
  }

  btnRelatedPrevention($event) {
    this.model.RelationCaseSourceID = $event.SourceID;
    this.model.RelationNodeName = $event.NodeName;
    this.modalService.dismissAll();
  }

  btnRelatedCase($event) {
    this.appendRelationCase($event.CaseID);
  }

  btnLookupCase($event) {
    const sourceTab = JSON.stringify(this.sourceTab);
    const payload = new EntrancePayload<{ caseID: string, sourceTab: string }>();
    payload.data = {
      caseID: $event.CaseID,
      sourceTab: sourceTab,
    };
    this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceCheckAction(payload))
  }
  btnPreviewCase(caseID: string) {
    this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceAction(caseID))
  }
  btnRemovePreviewCase(caseID: string) {
    this.deleteRelationCase(caseID);
  }
  btnRemoveSource() {
    this.model.RelationCaseSourceID = null;
    this.model.RelationNodeName = null;
  }

  btnCheckCase() {
    if (!this.model.NodeID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇企業別")));
      return;
    }

    this.caseService.checkCase(this.checkableCaseID, this.model.NodeID, this.model.SourceID).subscribe(x => {
      if (x.isSuccess) {
        this.appendRelationCase(x.element);
        this.checkableCaseID = '';
      } else {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
      }
    })
  }



  btnSaveCaseSource() {

    if (this.validForm(this.form) === false || this.validForm(this.usform) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    else if (this.validConcatAndComplained([this.usRef.user], this.usRef.valid.bind(this.usRef)) == false) {
      return;
    }

    const data = new EntrancePayload<CaseSourceViewModel>(this.model);

    data.success = (source: CaseSourceViewModel) => {
      this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceNativeAction({
        sourceID: source.SourceID,
        navigate: null
      }))
    }
    this.store.dispatch(new fromCaseCreatorAction.editCaseSourceAction(data));
  }

  btnCreateCase() {
    this.loadEntry();
  }

  closeCase($event: { sourceKey: string, caseKey: string }) {
    this.store.dispatch(new fromCaseCreatorAction.removeCaseTabAction({
      caseID: $event.caseKey,
      sourceID: $event.sourceKey
    }))
  }

  deleteRelationCase(caseID: string) {
    this.model.RelationCaseIDs = this.model.RelationCaseIDs.filter(x => x !== caseID);
  }

  appendRelationCase(caseID: string) {
    const set = new Set([caseID, ...this.model.RelationCaseIDs]);
    this.model.RelationCaseIDs = Array.from(set);
  }
  btnRefresh() {

    if (this.model.NodeID == undefined || this.model.NodeID == null) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇企業別")))
      return;
    }

    this.accomponentRef.getList();
  }


  initializeUI() {

    if (Guid.isGuid(this.model.key)) {
      this.initializePayload();
    } else {
      this.loadCaseIDs();
    }


    this.groupSelect && this.groupSelect.getList(this.model.NodeID);
  }

  loadEntry() {
    this.store.dispatch(new fromCaseCreatorAction.loadCaseEntryAction({
      model: new CaseViewModel(),
      sourceKey: this.key
    }))
  }

  loadCaseIDs() {

    let payload = new EntrancePayload(this.key);

    payload.success = function () {
      this.store.dispatch(new fromCaseCreatorAction.activeCaseTabAction(this.focusCaseID));
    }.bind(this);

    this.store.dispatch(new fromCaseCreatorAction.loadCaseIDsAction(payload));
  }

  initializePayload() {
    this.model.Cases = [new CaseViewModel()]
  }

  initializeForm() {
    this.usform = new FormGroup({});
    this.form = new FormGroup({
      GroupID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      CaseSourceType: new FormControl(this.model.CaseSourceType, [
        Validators.required,
      ]),
      IncomingDateTime: new FormControl(this.model.IncomingDateTime, [
        Validators.required,
      ]),
      IsTwiceCall: new FormControl(),
      IsPrevention: new FormControl(),

    });
  }

  pushTemp() {
    const temp = {};
    temp[this.model.key] = {
      NodeID: this.model.NodeID,
      OrganizationType: this.model.OrganizationType,
      SourceID: this.model.SourceID,
      User: this.model.User,
      GroupID: this.model.GroupID,
      navigate: this.model.navigate
    };

    // console.log("CS2 pushTemp => ", temp);
    this.caseService.sorceTempSubject.next({ ...temp });
  }

  considerActionType() {
    this.hasNodeAuth(this.model.GroupID, this.organizationType.CallCenter)
      .subscribe(x => {
        if (x === false) {
          this.uiActionType = this.actionType.Read;
        }
      })
  }



  subscription() {

    this.store.select(`case`, `caseCreator`, `${this.key}`)
      .pipe(
        filter(x => !!x),
        takeUntil(this.destroy$)
      ).subscribe(x => {
        this.model = x;
        this.loading = false;
        this.considerActionType();
        this.initializeUI();
      });

    this.store.select(`case`, `caseCreator`, `caseKeyPair`, `${this.key}`)
      .pipe(
        filter(x => !!x),
        takeUntil(this.destroy$)
      ).subscribe(caseKeyPairs => {
        this.subLoading = false;
        this.caseKeyPairs = caseKeyPairs;
      });

    interval(500).pipe(
      takeUntil(this.destroy$)
    ).subscribe(x => {
      this.changeDetectorRef.markForCheck();
      this.pushTemp()
    })

    this.store.select(x => x.case.caseCreator.activeCaseTab)
      .pipe(
        takeUntil(this.destroy$))
      .subscribe((payload) => {
        this.tabActive(payload)
      });

  }

  tabActive(caseID: string) {

    if (!caseID) return;

    setTimeout(() => {

      if (this.tabsRef) {
        const arr = this.tabsRef.tabs.toArray()

        arr.forEach(element => {

          if (element.tabTitle == caseID) {
            element.active = true;
          } else {
            element.active = false;
          }
        });
      }
    }, 2000);
  }

  validConcatAndComplained<T>(datas: T[], validor: (data: T) => boolean) {

    let valid = true;
    datas.forEach(user => {

      // 上一筆驗證通過就繼續, 否則不繼續驗證
      valid == true && (valid = validor(user));
    })

    return valid;
  }

}
