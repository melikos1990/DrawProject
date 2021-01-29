import { Component, OnInit, Injector, Input, KeyValueDiffer, KeyValueDiffers, ViewChild, Optional, Inject } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseSourceViewModel, CaseViewModel, CaseFocusType } from 'src/app/model/case.model';
import { State as fromCaseReducers } from '../../../store/reducers';
import { skip, takeUntil, filter, exhaustMap } from 'rxjs/operators';
import * as fromCaseCreatorAction from '../../../store/actions/case-creator.actions';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PcModalComponent } from '../../modal/pc-modal/pc-modal.component';

import { Guid } from 'guid-typescript';
import { BusinesssUnitParameters, UnitType } from 'src/app/model/organization.model';
import { empty } from 'rxjs';
import { CffModalComponent } from '../../modal/cff-modal/cff-modal.component';
import { CaseTemplateListViewModel } from 'src/app/model/master.model';
import { AcComponent } from '../../atom/ac/ac.component';
import { GroupSelectComponent } from 'src/app/shared/component/select/element/group-select/group-select.component';
import { UsComponent } from '../../element/us/us.component';
import { Cc1Component } from '../cc1/cc1.component';
import { C1Component } from '../c1/c1.component';
import { CaseSourceSelectComponent } from 'src/app/shared/component/select/element/case-source-select/case-source-select.component';



const PREFIX = 'C1Component';


@Component({
  selector: 'app-cs1',
  templateUrl: './cs1.component.html',
  styleUrls: ['./cs1.component.scss']
})
export class Cs1Component extends FormBaseComponent implements OnInit {

  public businessParameter: BusinesssUnitParameters;
  public checkableCaseID: string;
  public isOnlySourceUpdate: boolean = false;

  @ViewChild('ac') acRef: AcComponent;
  @ViewChild('groupSelect') groupSelect: GroupSelectComponent;
  @ViewChild('caseSource') caseSource: CaseSourceSelectComponent;
  @ViewChild('us') usRef: UsComponent;
  @ViewChild('cc1') cc1Ref: Cc1Component;
  @Input() public uiActionType: ActionType;
  @Input() public model: CaseSourceViewModel = new CaseSourceViewModel();
  @Input() public key: string;
  @Input() sourceTab: any[];;
  IsSearchEnabled?: boolean = true;
  private differ: KeyValueDiffer<CaseSourceViewModel, any>;

  public form: FormGroup;
  public subform: FormGroup;
  public usform: FormGroup;
  constructor(
    private differs: KeyValueDiffers,
    public caseService: CaseService,
    public modalService: NgbModal,
    public store: Store<fromCaseReducers>,
    @Optional() @Inject(C1Component) public parnetComp: C1Component,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializePayload();
    this.initializeForm();
    this.subscription();

  }

  generatorPayload() {
    const casement = this.model.Cases[0];
    if (casement != null) {
      casement.JContent = JSON.stringify(this.model.Cases[0].Particular);
      if (casement.CaseComplainedUsers[0].UnitType == undefined ||
        casement.CaseComplainedUsers[0].UnitType == null) {
        casement.CaseComplainedUsers = [];
      }
    }
  }

  btnPreventionCase() {

    if (!this.model.NodeID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇企業別")));
      return;
    }

    const ref = this.modalService.open(PcModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
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

    if (this.chackOpenSourceTab() == false) return;

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
    if (!this.checkableCaseID || !this.checkableCaseID.trim()) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請輸入勾稽案件編號")));
      return;
    }

    this.caseService.checkCase(this.checkableCaseID, this.model.NodeID, null).subscribe(x => {
      if (x.isSuccess) {
        this.appendRelationCase(x.element);
        this.checkableCaseID = '';
      } else {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
      }
    })
  }


  btnSaveCaseSource() {

    if (this.isValid() === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    else if (this.validConcatAndComplained([this.usRef.user], this.usRef.valid.bind(this.usRef)) == false) {
      return;
    }

    // console.log("this.model => ", JSON.stringify(this.model));
    const data = new EntrancePayload<CaseSourceViewModel>(this.model);

    data.success = (model: CaseSourceViewModel) => {
      this.removeExistAndCreateNewAction();
      this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceNativeAction({
        sourceID: model.SourceID,
        navigate: null
      }))
    }
    this.store.dispatch(new fromCaseCreatorAction.addCaseSourceAction(data));
  }


  btnSaveCaseSourceComplete(navigation: CaseFocusType = this.caseFocusType.Assignment) {

    if (this.chackOpenSourceTab() == false) return;

    if (this.isCompleteValid() === false) {
      return;
    }

    if (!this.model.Cases[0].QuestionClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_NOTICE.CASE_CATEGORY_NEED'))));
      return;
    }

    this.generatorPayload();

    // console.log("this.mode => ", JSON.stringify(this.model))
    const data = new EntrancePayload<CaseSourceViewModel>(this.model);

    data.success = (source: CaseSourceViewModel) => {
      this.removeExistAndCreateNewAction();

      this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceNativeAction({
        sourceID: source.SourceID,
        navigate: navigation
      }))
    }


    // console.log("model => ", this.model);
    this.store.dispatch(new fromCaseCreatorAction.addCaseSourceCompleteAction(data));

  }

  doFillCaseTemplate(template: string) {
    this.caseService
      .parseTemplateUseExist(template, null)
      .subscribe(x => {

        if (x.isSuccess) {
          this.model.Cases[0].FinishContent = x.element;
          this.doCaseSourceCompleteFastFinish();
        } else {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage(x.message)));
        }

      })

  }

  btnSaveCaseSourceCompleteFastFinish() {

    if (this.chackOpenSourceTab() == false) return;

    if (this.isCompleteValid() === false) {
      return;
    }

    if (!this.model.Cases[0].QuestionClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_NOTICE.CASE_CATEGORY_NEED'))));
      return;
    }

    this.caseService.getFastFinishedReasons(this.model.NodeID)
      .pipe(
        takeUntil(this.destroy$),
        exhaustMap(x => {
          const reasons = x.element;
          if (reasons.length > 1) {
            // 彈出視窗 , 回傳empty()

            const ref = this.modalService.open(CffModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
            const instance = (<CffModalComponent>ref.componentInstance);
            instance.data = reasons;
            instance.btnConfirm = (model: CaseTemplateListViewModel) => {
              this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage(`是否快速結案 : ${model.Title}`,
                () => {
                  this.model.Cases[0].FinishContent = model.Content;
                  this.doCaseSourceCompleteFastFinish();
                })));
            }
          }
          else {
            if (!reasons[0]) {
              this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('請設定快速結案範本!')));

            }
            else {
              this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否快速結案?',
                () => {
                  const reason = reasons[0];
                  this.model.Cases[0].FinishContent = reason.Content;
                  this.doCaseSourceCompleteFastFinish();
                }
              )));

            }
          }
          return empty();
        })

      ).subscribe()
  }

  doCaseSourceCompleteFastFinish() {

    this.generatorPayload();

    const data = new EntrancePayload<CaseSourceViewModel>(this.model);

    data.success = (source: CaseSourceViewModel) => {
      this.removeExistAndCreateNewAction();
      this.modalService.dismissAll();
    }
    this.store.dispatch(new fromCaseCreatorAction.addCaseSourceCompleteAndFastFinishAction(data));
  }

  btnRefresh() {

    if (this.model.NodeID == undefined || this.model.NodeID == null) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇企業別")))
      return;
    }

    this.acRef.getList();
  }

  onGroupChange($event) {

    if (!$event) return;
    this.pushTemp();
  }

  onBUChange($event) {

    if (!$event) {
      this.businessParameter = null;
      return;
    }

    this.caseService
      .getBUParameters($event)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => this.businessParameter = x)

    this.pushTemp();

    this.groupSelect.getList($event);
    this.caseSource.getList($event);
  }

  onBUSelectChange() {
    this.model.RelationCaseIDs = [];
    this.acRef.data = [];
  }

  onCheckChange() {
    this.model.IsPrevention || this.model.IsTwiceCall ?
      this.isOnlySourceUpdate = true : this.isOnlySourceUpdate = false;
  }

  deleteRelationCase(caseID: string) {
    this.model.RelationCaseIDs = this.model.RelationCaseIDs.filter(x => x !== caseID);
  }

  appendRelationCase(caseID: string) {
    const set = new Set([caseID, ...this.model.RelationCaseIDs]);
    this.model.RelationCaseIDs = Array.from(set);
  }


  removeExistAndCreateNewAction() {
    this.store.dispatch(new fromCaseCreatorAction.removeSorceTabAction(this.model.key));
    this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceEntryAction());
    this.store.dispatch(new fromCaseCreatorAction.activeSourceTabAction(null));
  }

  initializePayload() {
    this.model.Cases = [new CaseViewModel()]

    // 來源被反應者 預設為 消費者
    this.model.User.UnitType = UnitType.Customer;
  }

  ngDoCheck(): void {

    const changes = this.differ.diff(this.model);
    if (changes) {
      this.pushTemp()
    }
  }

  subscription() {

    this.differ = this.differs.find(this.model).create();

    this.store.select(`case`, `caseCreator`, `${this.key}`)
      .pipe(
        filter(x => !!x),
        takeUntil(this.destroy$)
      ).subscribe(x => {
        this.model = x;
        if (Guid.isGuid(this.model.key)) {
          this.initializePayload();
        }
      })
  }


  pushTemp() {
    const temp = {};
    temp[this.model.key] = this.model;
    // console.log("CS1 pushTemp => ", temp);
    this.caseService.sorceTempSubject.next({ ...temp });
  }

  initializeForm() {
    this.subform = new FormGroup({});
    this.usform = new FormGroup({});
    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      GroupID: new FormControl(this.model.GroupID, [
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


  isValid() {

    let result: boolean = true;

    if (this.validForm(this.form) === false) {
      result = false
    }
    if (this.validForm(this.usform) === false) {
      result = false;
    }

    return result;
  }

  isCompleteValid() {

    if (
      this.validForm(this.form) === false ||
      this.validForm(this.subform) === false ||
      this.validForm(this.usform) === false) {

      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    if (!this.cc1Ref.model.CaseConcatUsers || this.cc1Ref.model.CaseConcatUsers.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }


    if (!this.model.Cases[0].QuestionClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_NOTICE.CASE_CATEGORY_NEED'))));
      return false;
    }

    // 判斷 來源反應者
    if (this.validConcatAndComplained([this.usRef.user], this.usRef.valid.bind(this.usRef)) == false) {
      return false;
    }

    // 判斷 反應者
    if (this.validConcatAndComplained([...this.cc1Ref.model.CaseConcatUsers], this.cc1Ref.ucoRef.valid.bind(this.cc1Ref.ucoRef)) == false) {
      return false;
    }


    if (this.cc1Ref.model.CaseComplainedUsers && this.cc1Ref.model.CaseComplainedUsers.length > 0) {
      // 判斷 被反應者項目
      let validor = function (user: any) {
        return this.cc1Ref.ucmRef.valid(user, this.cc1Ref.model.CaseComplainedUsers);
      }.bind(this);

      if (this.validConcatAndComplained(this.cc1Ref.model.CaseComplainedUsers, validor) == false) {
        return false;
      }
    }


    return true;
  }

  validConcatAndComplained<T>(datas: T[], validor: (data: T) => boolean) {

    let valid = true;
    datas.forEach(user => {

      // 上一筆驗證通過就繼續, 否則不繼續驗證
      valid == true && (valid = validor(user));
    })

    return valid;
  }


  chackOpenSourceTab() {

    // 驗證 來源頁簽(不能開啟超過10個)
    if (this.parnetComp && this.parnetComp.menu.length > 10) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("來源頁簽不能開啟超過10個")));
      return false;
    }

    return true;
  }

}
