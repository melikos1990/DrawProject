import { Component, OnInit, Injector, ViewChild, Input } from '@angular/core';
import { CaseAssignmentBaseViewModel, CaseAssignmentProcessType, CaseAssignmentViewModel, CaseAssignmentResumeViewModel, CaseAssignmentUserViewModel } from 'src/app/model/case.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { ActionType, AspnetJsonResult, EntrancePayload } from 'src/app/model/common.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { Observable } from 'rxjs';
import { tap, filter, takeUntil, switchMap } from 'rxjs/operators';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import * as fromRootActions from "src/app/store/actions";
import * as fromCaseAssignmentActions from '../../store/actions/case-assignment.action'
import { Store } from '@ngrx/store';
import { userInfoSelector } from 'src/app/store/reducers';
import { JobPosition } from 'src/app/model/authorize.model';

@Component({
  selector: 'app-case-assignment-detail',
  templateUrl: './case-assignment-detail.component.html',
  styleUrls: ['./case-assignment-detail.component.scss']
})
export class CaseAssignmentDetailComponent extends FormBaseComponent implements OnInit {


  @ViewChild(DynamicQuestionSelectComponent) dynamicSelect: DynamicQuestionSelectComponent;

  complaintedColumn = [];
  resumeColumn = [];
  jobPositions: any[] = [];
  jobPosition: JobPosition;
  resumes = [];
  isShowFinishs: boolean;
  isShowReplys: boolean;
  isShowRefills: boolean;
  @Input() public uiActionType: ActionType;

  public uiCaseAssignmentProcessType: CaseAssignmentProcessType;
  public caseID: string;
  public identityID: number;
  public caseFileOpts: any = {};
  public caseAssignFileOpts: any = {};
  public form: FormGroup;
  public model: CaseAssignmentBaseViewModel = new CaseAssignmentBaseViewModel();

  constructor(
    private store: Store<any>,
    private caseService: CaseService,
    private modalService: NgbModal,
    private active: ActivatedRoute,
    public injector: Injector) {
    super(injector);
  }



  ngOnInit() {

    this.subscription();
    this.initializeFrom();
  }
  updateSelectView() {
    if (this.dynamicSelect) {
      this.dynamicSelect.updateView();
    }
  }

  subscription() {

    // 有可能是從 window.open 進入 , 因此需使用 queryParams
    this.active.params.subscribe(x => {
      this.loadPage(x);
    })

    this.store.select(userInfoSelector).pipe(
      filter(x => x !== undefined),
      takeUntil(this.destroy$),
      tap(userInfo => {
        this.setJobPosition(userInfo.jobPosition);

      })
    ).subscribe();

  }

  get(): Observable<AspnetJsonResult<CaseAssignmentBaseViewModel>> {
    switch (this.uiCaseAssignmentProcessType) {
      case +this.caseAssignmentProcessType.Assignment.toString():
        return this.caseService.getAssignment(this.caseID, this.identityID);
      case +this.caseAssignmentProcessType.Invoice.toString():
        return this.caseService.getAssignmentIvoice(this.identityID);
      case +this.caseAssignmentProcessType.Notice.toString():
        return this.caseService.getAssignmentNotice(this.identityID);
    }

  }
  getResume(): Observable<AspnetJsonResult<CaseAssignmentResumeViewModel[]>> {
    return this.caseService.getResumes(this.caseID, this.identityID);
  }
  setJobPosition(v: JobPosition[]) {
    if (v && v.length > 0) {
      v.forEach(x => this.jobPositions.push({
        id: x.ID,
        text: x.NodeName,
        extend: x,
      }));

      this.jobPositions = Array.from(new Set(this.jobPositions.map(x => JSON.stringify(x)))).map(x => JSON.parse(x));

      setTimeout(() => {
        const id = this.jobPositions[0].id;
        this.model.EditorNodeJobID = id;
        this.setCurrentJonPosition(id);
      }, 1000);

    }
  }
  setCurrentJonPosition(id) {
    this.jobPosition = this.jobPositions.find(x => x.id == id).extend;
  }

  initializeFile() {

    const paths = this.model.FilePath || [];
    const previews = paths.map(path => path.toHostApiUrl())
    const PreviewConfigs = paths.map(path => {

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
      initialPreviewConfig: PreviewConfigs,
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };

    const assignPaths = this.model['FinishFilePath'] || [];
    const assignPreviews = assignPaths.map(path => path.toHostApiUrl())
    const assignPreviewConfigs = assignPaths.map(path => {

      return {
        caption: path.split('fileName=')[1],
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteCaseAssignmentFile`.toHostApiUrl(),
        extra: {
          id: (<CaseAssignmentViewModel>this.model).AssignmentID,
          key: path,
          extend: this.model.CaseID
        }
      }
    });

    this.caseAssignFileOpts = {
      preferIconicPreview: true,
      initialPreview: assignPreviews,
      initialPreviewConfig: assignPreviewConfigs,
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };

  }
  initializeTable() {

    this.complaintedColumn = [
      {
        //類型
        text: this.translateService.instant('CASE_COMMON.TABLE.UNIT_TYPE'),
        name: 'UnitTypeName'
      },
      {
        //單位名稱
        text: this.translateService.instant('CASE_COMMON.TABLE.ORGANIZAITON_NAME'),
        name: 'NodeName'
      },
      {
        //電話
        text: this.translateService.instant('CASE_COMMON.TABLE.TELEPHONE', { number: '' }),
        name: 'Telephone'
      },
      {
        //型態
        text: this.translateService.instant('CASE_COMMON.TABLE.TYPE'),
        name: 'CaseComplainedUserTypeName'
      },
      {
        //組織
        text: this.translateService.instant('CASE_COMMON.TABLE.ORGANIZATION'),
        name: 'ParentPathName'
      },
      {
        //負責人
        text: this.translateService.instant('CASE_COMMON.APPLY_USER'),
        name: 'OwnerUserName'
      },
      {
        //負責人電話
        text: this.translateService.instant('CASE_COMMON.TABLE.APPLY_USER_PHONE'),
        name: 'OwnerUserPhone'
      },
      {
        //負責人Email
        text: this.translateService.instant('CASE_COMMON.TABLE.APPLY_USER_EMAIL'),
        name: 'OwnerUserEmail'
      },
    ]

    this.resumeColumn = [
      {
        //序號
        text: this.translateService.instant('CASE_COMMON.CASE_ASSIGNMENT_ID'),
        name: 'Index'
      },
      {
        //回覆內容
        text: this.translateService.instant('CASE_COMMON.REPLY_CONTENT'),
        name: 'Content',
        customer: true,
      },
      {
        //回覆單位
        text: this.translateService.instant('CASE_COMMON.REPLY_UNIT'),
        name: 'CreateNodeName'
      },
      {
        //回覆人員
        text: this.translateService.instant('CASE_COMMON.REPLY_USER'),
        name: 'CreateUserName'
      },
      {
        //回覆時間
        text: this.translateService.instant('CASE_COMMON.REPLY_TIME'),
        name: 'CreateDateTime'
      },
    ]


  }
  initializeFrom() {
    this.form = new FormGroup({
      CaseContent: new FormControl(),
      AssignmentContent: new FormControl(),
      ReplyContent: new FormControl(this.model.ReplyContent, [
        Validators.maxLength(1024),
      ]),
      FinishContent: new FormControl(this.model['FinishContent'], [
        Validators.maxLength(1024),
      ])
    });
  }
  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);
    this.identityID = params['ID'];     // noticeID / invocieIdentityID / assignemntID
    this.uiCaseAssignmentProcessType = parseInt(params['type']); // caseAssignmentProcessType
    this.caseID = params['caseID'];

    console.log(this.uiActionType);

    this.get()
      .pipe(
        tap(x => this.model = x.element),
        switchMap(() => this.getResume()),
        tap(c => this.resumes = c.element)
      )
      .subscribe(() => {
        this.initializeFile();
        this.initializeTable();
        this.updateSelectView();
        setTimeout(() => {
          this.isShowFinishs = this.isShowFinish();
          this.isShowReplys = this.isShowReply();
          this.isShowRefills = this.isShowRefill();
        }, 1000);
      });

  }
  btnProcessed() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    if (!this.model.EditorNodeJobID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇處置單位")))
      return;
    }

    if (!this.model['FinishContent']) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請輸入銷案內容")))
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否銷案?',
      () => {
        const data = new EntrancePayload<CaseAssignmentViewModel>(<CaseAssignmentViewModel>this.model);
        data.success = () => window.location.reload();
        this.store.dispatch(new fromCaseAssignmentActions.processedCaseAssignmentAction(data))
      }
    )));
  }

  btnSaveRefill() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    if (!this.model.EditorNodeJobID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇處置單位")))
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否資料重填?',
      () => {
        const data = new EntrancePayload<CaseAssignmentViewModel>(<CaseAssignmentViewModel>this.model);
        data.success = () => window.location.reload();
        this.store.dispatch(new fromCaseAssignmentActions.refillCaseAssignmentAction(data))
      }
    )));
  }

  btnSaveAssignment() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }

    if (!this.model.EditorNodeJobID) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇處置單位")))
      return;
    }
    
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否確定?',
      () => {
        const data = new EntrancePayload<CaseAssignmentViewModel>(<CaseAssignmentViewModel>this.model);
        data.success = () => window.location.reload();
        this.store.dispatch(new fromCaseAssignmentActions.editCaseAssignmentAction(data))
      }
    )));
  }

  btnBack() {
    if (window.history.length > 1) {
      window.history.back();
    } else {
      window.close();
    }

  }

  onJobChange($event) {
    if (!!$event) {
      this.setCurrentJonPosition($event);
    }

    // 更新畫面狀態
    this.isShowFinishs = this.isShowFinish();
    this.isShowReplys = this.isShowReply();
    this.isShowRefills = this.isShowRefill();
  }

  // 是否為轉派對象
  isShowFinish() {
    if (this.uiActionType == this.actionType.Read)
      return false;

    if (this.model.CaseAssignmentProcessType !== this.caseAssignmentProcessType.Assignment)
      return false;

    if ((<CaseAssignmentViewModel>this.model).CaseAssignmentType != this.caseAssignmentType.Assigned) {
      return false;
    }

    if (!this.jobPosition)
      return false;

    if ((<CaseAssignmentViewModel>this.model).CaseAssignmentUsers.some(this.isResponsibility) == false)
      return false;

    return true;
  }

  isResponsibility = (model: CaseAssignmentUserViewModel) => model.NodeID == this.jobPosition.NodeID &&
    model.OrganizationType == this.jobPosition.OrganizationType &&
    model.CaseComplainedUserType == this.caseComplainedUserType.Responsibility;


  // 是否為知會對象
  isShowReply() {
    if (this.uiActionType == this.actionType.Read)
      return false;

    //已處理完成或銷案 則不顯示
    if ((<CaseAssignmentViewModel>this.model).CaseAssignmentType != this.caseAssignmentType.Assigned) {
      return false;
    }

    if ((<CaseAssignmentViewModel>this.model).CaseAssignmentUsers.some(this.isNotice) == false)
      return false;

    return true;
  }

  isNotice = (model: CaseAssignmentUserViewModel) => model.NodeID == this.jobPosition.NodeID &&
    model.OrganizationType == this.jobPosition.OrganizationType;

  // 是否為被駁回單位
  isShowRefill() {
    //檢視不顯示
    if (this.uiActionType == this.actionType.Read)
      return false;

    //非派工不顯示
    if (this.model.CaseAssignmentProcessType !== this.caseAssignmentProcessType.Assignment)
      return false;

    //非處理完成 或 非資料重填 則不顯示
    if ((<CaseAssignmentViewModel>this.model).CaseAssignmentType != this.caseAssignmentType.Processed ||
      (<CaseAssignmentViewModel>this.model).RejectType != this.rejectType.FillContent) {
      return false;
    }

    //無權限
    if (!this.jobPosition)
      return false;

    //是否為銷案單位
    if ((<CaseAssignmentViewModel>this.model).CaseAssignmentUsers.some(this.isResponsibility) == false ||
      (<CaseAssignmentViewModel>this.model).FinishNodeID != this.jobPosition.NodeID)
      return false;

    return true;
  }

}
