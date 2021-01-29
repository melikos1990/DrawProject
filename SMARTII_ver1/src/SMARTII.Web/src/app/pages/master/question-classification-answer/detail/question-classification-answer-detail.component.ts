import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { QuestionClassificationAnswerDetail, QuestionClassificationAnswerViewModel } from 'src/app/model/question-category.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActivatedRoute } from '@angular/router';
import { ActionType, AspnetJsonResult } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../../store/reducers"
import * as fromQuestionCategoryActions from "../../store/actions/question-classification-answer.actions";
import * as fromRootActions from "src/app/store/actions";
import { takeUntil, filter, map, tap, skip } from 'rxjs/operators';
import { FormGroup, FormControl, Validators, MaxLengthValidator } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { PtcSelect2Service } from 'ptc-select2';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';


export const PREFIX = 'QuestionClassificationAnswerComponent';


@Component({
  selector: 'question-classification-answer-detail',
  templateUrl: './question-classification-answer-detail.component.html',
  styleUrls: ['./question-classification-answer-detail.component.scss']
})


export class QuestionClassificationAnswerDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild(DynamicQuestionSelectComponent) dynamicSelect: DynamicQuestionSelectComponent;

  public form: FormGroup;
  public formT: FormGroup;

  public detail: QuestionClassificationAnswerDetail = new QuestionClassificationAnswerDetail();
  columns = [];
  answerItems: QuestionClassificationAnswerDetail[] = [];
  maxLevel: any[] = [];
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
    this.initializeTable();
  }


  clearItem = () => {
    this.detail.Title = "";
    this.detail.Content = "";
  }

  addItem = () => this.answerItems = [{ ...this.detail }, ...this.answerItems];

  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_ANSWER.SUBJECT'),
        name: 'Title'
      },
      {
        text: this.translateService.instant('QUESTION_CLASSIFICATION_ANSWER.CONTENT'),
        name: 'Content',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
      }
    ];
  }

  /**
   * 寫入常用語表格
   */
  btnAddTable() {
    if (this.validForm(this.formT) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('必須填寫內容才能加入')));
      return;
    }

    const exist = this.answerItems.some(v => (v.Title === this.detail.Title));

    if (exist) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('已經有相同主旨')));
      return;
    }

    this.addItem();
    this.clearItem();
  }

  findIndex = (title) => this.answerItems.findIndex(x => x.Title === title);

  btnDelete($event: QuestionClassificationAnswerViewModel) {
    const index = this.findIndex($event.Title);
    this.answerItems.splice(index, 1);
    this.answerItems = [...this.answerItems]; // clone ref to refresh view....
  }

  subscription() {
    this.active.params.pipe(takeUntil(this.destroy$)).subscribe(this.loadPage.bind(this));

    this.store.select(x => x.master.questionClassificationAnswer.detail)
      .pipe(
        skip(1),
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
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromQuestionCategoryActions.GetDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromQuestionCategoryActions.GetDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
    }
  }

  initializeForm() {

    this.form = new FormGroup({
      BuID: new FormControl(),
      Name: new FormControl(),
      ParentNodeID: new FormControl(),
      IsEnable: new FormControl(true),

      CreateUserName: new FormControl(),
      CreateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateTime: new FormControl(),
    })

    this.formT = new FormGroup({
      Title: new FormControl('', [
        Validators.maxLength(30),
        Validators.required
      ]),
      Content: new FormControl('', [
        Validators.required
      ]),
    })
  }

  /**
   * 新增
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd() {

    if (this.validSearchForm() == false) return;

    if (this.answerItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少填寫一項常用語')));
      return;
    }

    let data: QuestionClassificationAnswerViewModel[] = [];

    this.answerItems.forEach(x =>
      data.push({
        NodeID: this.detail.NodeID,
        ClassificationID: <any>this.dynamicSelect.lastHasValue,
        Title: x.Title,
        Content: x.Content,
      } as any)
    );

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.CreateDetailAction(data))
      }
    )));
  }

  /**
   * 修改
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnEdit($event) {

    if (this.validSearchForm() == false) return;

    if (this.validForm(this.formT) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const data = this.refillPayload();

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.EditDetailAction(data));
      }
    )));
  }

  refillPayload() {
    let data = new QuestionClassificationAnswerViewModel();
    data.NodeID = this.detail.NodeID
    data.ID = this.detail.ID;
    data.ClassificationID = <any>this.dynamicSelect.lastHasValue;
    data.Title = this.detail.Title;
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


