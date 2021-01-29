import { Component, OnInit, Input, ViewChild, Injector, EventEmitter, Output, HostListener, TemplateRef, ElementRef } from '@angular/core';
import { CaseViewModel, CaseSourceViewModel, CaseFocusType, CaseConcatUserViewModel } from 'src/app/model/case.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { State as fromCaseReducers } from '../../../store/reducers'
import { CaseService } from 'src/app/shared/service/case.service';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { takeUntil, filter, map, take } from 'rxjs/operators';
import * as fromRootActions from 'src/app/store/actions';
import * as fromCaseCreatorAction from '../../../store/actions/case-creator.actions';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { HashtagComponent } from 'src/app/shared/component/other/hashtag/hashtag.component';
import { UcoComponent } from '../../element/uco/uco.component';
import { UcmComponent } from '../../element/ucm/ucm.component';
import { CcfhComponent } from '../ccfh/ccfh.component';
import { CcoComponent } from '../cco/cco.component';
import { CallCenterNodeDetailViewModel, ConcatableUserViewModel, BusinesssUnitParameters } from 'src/app/model/organization.model';
import { User } from 'src/app/model/authorize.model';
import { RemindModalComponent } from '../../modal/remind-modal/remind-modal.component';
import { environment } from 'src/environments/environment';
import { webHostPrefix } from 'src/app.config';
import { CcfiComponent } from '../ccfi/ccfi.component';
import { Guid } from 'guid-typescript';
import { HttpService } from 'src/app/shared/service/http.service';

export const PREFIX = 'C1Component';

@Component({
  selector: 'app-cc2',
  templateUrl: './cc2.component.html',
  styleUrls: ['./cc2.component.scss']
})
export class Cc2Component extends FormBaseComponent implements OnInit {


  @ViewChild('hashtag') tagRef: HashtagComponent;
  @ViewChild('ccfi') ccfiRef: CcfiComponent;
  @ViewChild('ccfh') ccfhRef: CcfhComponent;
  @ViewChild('uco') ucoRef: UcoComponent;
  @ViewChild('ucm') ucmRef: UcmComponent;
  @ViewChild('cco') ccoRef: CcoComponent;
  @ViewChild('caseOperator') caseOperator: ElementRef<HTMLElement>;


  public model: CaseViewModel = new CaseViewModel();

  checkableCaseID: string = '';
  focusAssignmentId: string;
  focusFinishedId: string;
  hasFinishReturn:boolean;

  @Input() uiActionType: ActionType;
  @Input() caseKey: string;
  @Input() sourcekey: string;

  @Output() closeTab: EventEmitter<any> = new EventEmitter();

  // 為了解決 angular 重複rander畫面 導致 caseOperator.offsetTop 會不定
  // 所以做暫存變數
  affix: boolean = false;
  private _operatorOffsetTop: number = 0;
  get operatorOffsetTop() {

    if (
      this.caseOperator &&
      this.caseOperator.nativeElement.offsetTop >= this._operatorOffsetTop
    ) {
      this._operatorOffsetTop = this.caseOperator.nativeElement.offsetTop
    }

    return this._operatorOffsetTop;
  }

  @HostListener('window:scroll', ['$event'])
  handleScroll($event) {
    if (window.pageYOffset < this.operatorOffsetTop) this.affix = true;
    else this.affix = false;
  }

  caseFileOpts = {};
  caseFinishFileOpts = {};

  @Input() public form: FormGroup = new FormGroup({});
  ccfiform: FormGroup = new FormGroup({});

  user: User;
  groupNode: CallCenterNodeDetailViewModel;
  tempSource: CaseSourceViewModel = new CaseSourceViewModel();
  expectTime?: Date
  loading: boolean = true;
  isAdminFlag: boolean;

  hasInput: boolean = false;

  constructor(
    public store: Store<fromCaseReducers>,
    public modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector,
    public http: HttpService) {
    super(injector, PREFIX)
  }

  ngOnInit() {
    this.initializeForm();
    this.subscription();
    this.loadCase();
    this.isAdmin();
  }

  deleteRelationCase(caseID: string) {
    this.model.RelationCaseIDs = this.model.RelationCaseIDs.filter(x => x !== caseID);
  }
  btnRemovePreviewCase(caseID: string) {
    this.deleteRelationCase(caseID);
  }
  btnPreviewCase(caseID: string) {
    this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceAction(caseID))
  }
  btnCheckCase() {
    if (!this.model.NodeID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇企業別")));
      return;
    }

    if (this.model.CaseID == this.checkableCaseID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("此案編為原案件")));
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
  appendRelationCase(caseID: string) {
    const set = new Set([caseID, ...this.model.RelationCaseIDs]);
    this.model.RelationCaseIDs = Array.from(set);
  }

  loadCase() {
    this.store.dispatch(new fromCaseCreatorAction.loadCaseAction(this.caseKey));
  }

  getGroupNode() {

    if (this.model.GroupID == null || this.model.GroupID == undefined)
      return;

    this.caseService.getCallCenterNode(this.model.GroupID)
      .subscribe(x => this.groupNode = x.element)
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
  }
  subscription() {

    this.getCurrentUser().subscribe(x => this.user = x);

    this.caseService.lockUsersSubject
      .pipe(
        filter(x => !!x && x.Key == this.model.CaseID),
        takeUntil(this.destroy$),
      ).subscribe(payload => {
        this.model.LookupUsers = payload.Value.map(x => ({ text: x.Name }));
      })

    this.store.select(`case`, `caseCreator`, `${this.caseKey}`)
      .pipe(
        takeUntil(this.destroy$),
        filter(x => !!x),
      )
      .subscribe(x => {
        this.model = x;
        this.getGroupNode();
        this.initialPayload();
        if (!!x.navigate) {
          this.scrollAndFocus(x.navigate);
        }
        this.caseService
        .getBUParameters(this.model.NodeID)
        .pipe(takeUntil(this.destroy$))
        .subscribe(x => this.hasFinishReturn = x.CanFinishReturn);
      })

    this.store.select(`case`, `caseCreator`, `${this.sourcekey}`)
      .pipe(
        filter(x => !!x),
        takeUntil(this.destroy$),
      ).subscribe(x => {
        if (!!x.navigate) {
          this.scrollAndFocus(x.navigate);
        }
      });


  }

  generatorPayload() {
    this.model.CaseConcatUsers = this.ucoRef.concatUsers;
    this.model.CaseComplainedUsers = this.ucmRef.complainedUsers;
    this.model.CaseTagsMark = this.tagRef.tags;
    this.model.CaseFinishReasons = this.ccfhRef.getValue();
    this.model.JContent = JSON.stringify(this.model.Particular);
  }

  btnSaveCase() {

    this.generatorPayload();

    console.log(this.model);

    if (this.isCompleteValid() == false) return;

    const cacheRole = this.authService.getCacheRoleID();
    const data = new EntrancePayload<{ CaseViewModel: CaseViewModel, roleID: number }>({ CaseViewModel: this.model, roleID: cacheRole });

    data.success = (model: CaseViewModel) => {
      this.store.dispatch(new fromCaseCreatorAction.removeCaseTabAction({
        sourceID: model.SourceID,
        caseID: model.CaseID
      }))
      this.store.dispatch(new fromCaseCreatorAction.loadCaseIDsAction(new EntrancePayload(model.SourceID)));
    }

    this.store.dispatch(new fromCaseCreatorAction.editCaseAction(data));
  }

  btnUnlockCase() {
    this.generatorPayload();

    if (this.isCompleteValid() == false) return;

    const data = new EntrancePayload<CaseViewModel>(this.model);

    data.success = (model: CaseViewModel) => {
      this.store.dispatch(new fromCaseCreatorAction.removeCaseTabAction({
        sourceID: model.SourceID,
        caseID: model.CaseID
      }))
      this.store.dispatch(new fromCaseCreatorAction.loadCaseIDsAction(new EntrancePayload(model.SourceID)));
    }
    this.store.dispatch(new fromCaseCreatorAction.unLockAction(data));

  }

  btnFinishCase(IsReturn :boolean) {

    this.generatorPayload();

    if (this.isCompleteValid() == false) return;

    if (this.validForm(this.ccfiform) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    const data = new EntrancePayload<CaseViewModel>(this.model);

    //callback
    data.success = (model: CaseViewModel) => {
      this.store.dispatch(new fromCaseCreatorAction.removeCaseTabAction({
        sourceID: model.SourceID,
        caseID: model.CaseID
      }))
      this.store.dispatch(new fromCaseCreatorAction.loadCaseIDsAction(new EntrancePayload(model.SourceID)));

      //結案後關閉本頁籤
      this.store.dispatch(new fromCaseCreatorAction.removeSorceTabAction(this.sourcekey));

      //跳往新畫面
      this.store.dispatch(new fromCaseCreatorAction.activeSourceTabAction('new'));
      if(IsReturn)
      {
        history.back();
      }
      
    }
    this.store.dispatch(new fromCaseCreatorAction.finishCaseAction(data));


  }

  isShowFinish() {
    // if (!this.isAdminFlag) return false;

    if (!this.groupNode) return false;

    if (this.groupNode.WorkProcessType == this.workProcessType.Individual) {
      // 一般結案需比對是否為案件負責人
      if (this.model.ApplyUserID != this.user.UserID)
        return false;
    }
    return this.model.CaseType != this.caseType.Finished &&
      this.uiActionType !== this.actionType.Read

  }

  // 是否為admin權限
  isAdmin() {
    //取得當前角色
    let cacheRole = this.authService.getCacheRoleID();

    if (!!cacheRole) {
      this.authService.getRoles()
        .pipe(
          filter(x => !!x),
          takeUntil(this.destroy$))
        .subscribe(item => {
          let roleAuth = item.find(x => x.ID == cacheRole).Feature.find(x => x.Feature === "C1");
          this.isAdminFlag = roleAuth.AuthenticationType > 31 ? true : false;
        });
    }
    else {
      this.authService.getUserMenu()
        .pipe(
          filter(x => !!x),
          takeUntil(this.destroy$))
        .subscribe(item => {
          let roleAuth = item.find(x => x.Feature === "C1");
          this.isAdminFlag = roleAuth.AuthenticationType > 31 ? true : false;
        });
    }
  }

  isShowEdit() {

    // 是否為 自己 Group 的案件(由 cs2 傳進來的) 當不是自己 Group 案件時 會強制轉成 Read Mode;
    let isGroupCase = this.uiActionType !== this.actionType.Read;

    // 案件已結案並且是自己Group的案件 在檢查是否有 特殊權限
    if (this.model.CaseType == this.caseType.Finished && isGroupCase) {
      return this.isAdminFlag;
    }

    return isGroupCase;
  }

  scrollAndFocus(navigate: CaseFocusType) {
    switch (navigate) {
      case this.caseFocusType.Assignment.toString():
        this.focusAssignmentId = navigate + Guid.create().toString();
        setTimeout(() => {
          document.getElementById(this.focusAssignmentId).focus();
          this.tempSource.navigate = null;
        }, 1000);
        break;
      case this.caseFocusType.Finished.toString():
        this.focusFinishedId = navigate + Guid.create().toString();
        setTimeout(() => {
          document.getElementById(this.focusFinishedId).focus();
          this.tempSource.navigate = null;
        }, 1000);
        break;
    }
  }

  initialPayload() {
    this.loading = false;
    this.setDefaultParticular();
    this.setDefaultTags();
    this.setCaseFileOpts();
    this.setCaseFinishFileOpts();
    this.setDefaultInputs();
    this.setConcatUser();
  }

  setDefaultTags() {
    this.model.CaseTagsMark = this.model.CaseTags.map(x => {
      return {
        id: x.ID,
        text: x.Name,
      };
    })
  }

  setDefaultInputs() {
    this.expectTime = this.model.ExpectDateTime;
  }

  setDefaultParticular() {
    this.model.Particular = this.model.Particular || {};
  }

  setCaseFileOpts() {
    const paths = this.model.FilePath || [];
    const previews = paths.map(path => path.toHostApiUrl())
    const previewConfigs = paths.map(path => {

      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteCaseFile`.toHostApiUrl(),
        extra: {
          id: this.model.CaseID,
          key: path,
          extends: +this.caseType.Filling
        }
      }
    });

    this.caseFileOpts = {
      preferIconicPreview: true,
      initialPreview: previews,
      initialPreviewConfig: previewConfigs,
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
  }
  setCaseFinishFileOpts() {
    const paths = this.model.FinishFilePath || [];
    const previews = paths.map(path => path.toHostApiUrl())
    const previewConfigs = paths.map(path => {

      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteCaseFinishFile`.toHostApiUrl(),
        extra: {
          id: this.model.CaseID,
          key: path,
          extend: +this.caseType.Finished
        }
      }
    });

    this.caseFinishFileOpts = {
      preferIconicPreview: true,
      initialPreview: previews,
      initialPreviewConfig: previewConfigs,
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
    // this.caseFinishFileOpts = {
    //   preferIconicPreview: true,
    //   initialPreviewAsData: false,
    //   overwriteInitial: true,
    //   initialPreview: previews,
    //   initialPreviewConfig: previewConfigs,
    //   fileActionSettings: {
    //     showRemove: true,
    //     showUpload: false,
    //     showClose: false,
    //     uploadAsync: false,
    //   }
    // };
  }

  setConcatUser() {
    this.model.CaseConcatUsers.forEach(x => {
      x.key = Guid.create().toString();
    })
  }

  btnReadCaseRemind() {
    let modal = this.modalService.open(RemindModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    let instance = <RemindModalComponent>modal.componentInstance;

    instance.caseRemindIDs = this.model.CaseRemindIDs;
  }


  btnAddCaseRemind() {
    let url = `${environment.webHostPrefix}/pages/master/case-remind-detail`.toCustomerUrl({
      actionType: this.actionType.Add,
      caseID: this.model.CaseID,
    })

    window.open(url, '_blank');
  }

  isCompleteValid() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    if (!this.model.QuestionClassificationID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant('CASE_NOTICE.CASE_CATEGORY_NEED'))));
      return false;
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

    if (!!this.model.FinishContent && this.model.FinishContent.length > 4000) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("結案內容超過上限字數(4000)")));
      return false;
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

  setFinistConcat(data: CaseConcatUserViewModel[]) {
    const newUsers = this.finistConcatPayload(data.filter(x => !!x.Email));
    this.ccfiRef.users = newUsers;
  }

  finistConcatPayload(users: any[]): CaseConcatUserViewModel[] {
    return users.map((user: any | CaseConcatUserViewModel) => {
      const data = new CaseConcatUserViewModel();
      data.UserName = user.UserName;
      data.Email = user.Email;
      data.NotificationBehavior = this.notificationType.Email.toString();
      data.NotificationRemark = !(user.NotificationRemark) ? user.NotificationRemark = this.emailReceiveType.Recipient.toString() : user.NotificationRemark;
      data.key = Guid.create().toString();
      return data;
    });
  }

  hasInputChange($event: boolean) {
    this.hasInput = $event ? null : false;
  }

}
