import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { QuestionClassificationGuideDetail, QuestionClassificationGuideViewModel } from 'src/app/model/question-category.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActivatedRoute } from '@angular/router';
import { ActionType } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../../store/reducers"
import * as fromQuestionCategoryActions from "../../store/actions/question-classification-guide.actions";
import * as fromRootActions from "src/app/store/actions";
import { takeUntil, skip, filter } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { PtcSelect2Service } from 'ptc-select2';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';


export const PREFIX = 'QuestionClassificationGuideComponent';

@Component({
  selector: 'question-classification-guide-detail',
  templateUrl: './question-classification-guide-detail.component.html',
  styleUrls: ['./question-classification-guide-detail.component.scss']
})
export class QuestionClassificationGuideDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild(DynamicQuestionSelectComponent) dynamicSelect: DynamicQuestionSelectComponent;

  public form: FormGroup;

  public detail: QuestionClassificationGuideDetail = new QuestionClassificationGuideDetail();
  columns = [];
  answerItems: QuestionClassificationGuideDetail[] = [];
  titleTypeString: string = "";
  public uiActionType: ActionType;

  constructor(
    public inejctor: Injector,
    public active: ActivatedRoute,
    public http: HttpService,
    public store: Store<fromReducers>,
    public ptcSelect2Service: PtcSelect2Service) {
    super(inejctor, PREFIX);
  }

  ngOnInit() {
    this.subscription();
    this.initializeForm();
  }

  subscription() {
    this.active.params.pipe(takeUntil(this.destroy$)).subscribe(this.loadPage.bind(this));

    this.store.select(x => x.master.questionClassificationGuide.detail)
      .pipe(
        filter(x => !!x),
        takeUntil(this.destroy$)
      )
      .subscribe(x => {
        this.detail = x;
        this.dynamicSelect.updateView();
      });
  }

  ngOnDestroy() {
    this.store.dispatch(new fromQuestionCategoryActions.ClearAction());
    super.ngOnDestroy();
  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);
    this.detail.NodeID = params['nodeID'];

    const payload = {
      id: params['id'],
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.titleTypeString=this.translateService.instant('QUESTION_CLASSIFICATION_GUIDE.ADD');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromQuestionCategoryActions.GetDetailAction(payload));
        this.titleTypeString=this.translateService.instant('QUESTION_CLASSIFICATION_GUIDE.READ');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromQuestionCategoryActions.GetDetailAction(payload));
        this.titleTypeString=this.translateService.instant('QUESTION_CLASSIFICATION_GUIDE.EDIT');
        break;
    }
  }

  initializeForm() {

    this.form = new FormGroup({
      BuID: new FormControl(),
      Name: new FormControl(),
      ParentNodeID: new FormControl(),
      IsEnable: new FormControl(true),
      Content: new FormControl('', [
        Validators.required
      ]),
      CreateUserName: new FormControl(),
      CreateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateTime: new FormControl(),
    })
  }

  /**
   * 新增引導
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd() {

    if (this.validSearchForm() == false) return;

    const data = this.refillPayload();

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.CreateDetailAction(data))
      }
    )));
  }

  /**
   * 修改引導
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnEdit($event) {

    if (this.validSearchForm() == false) return;

    const data = this.refillPayload();

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.EditDetailAction(data));
      }
    )));
  }

  refillPayload() {
    let data = new QuestionClassificationGuideViewModel();
    data.ID = this.detail.ID;
    data.ClassificationID = <any>this.dynamicSelect.lastHasValue;
    data.NodeID = this.detail.NodeID
    data.Content = this.detail.Content;

    return data;
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else {
      if (this.dynamicSelect.lastHasValue == null) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('只少選擇一項問題分類')));
        return false;
      }
      return true;
    }
  }
}


