import { Component, OnInit, OnDestroy, Injector, ViewChildren, QueryList, ViewChild } from '@angular/core';
import { QuestionCategoryDetail, QuestionSelectInfo, QuestionClassificationAnswerViewModel, AnswerActionType } from 'src/app/model/question-category.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ActivatedRoute } from '@angular/router';
import { ActionType, AspnetJsonResult } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../../store/reducers"
import * as fromQuestionCategoryActions from "../../store/actions/question-category.actions";
import * as fromRootActions from "src/app/store/actions";
import { takeUntil, filter, map, tap } from 'rxjs/operators';
import { FormGroup, FormControl, Validators, FormArray, AbstractControl } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { PtcSelect2AjaxOptions, PtcSelect2Service } from 'ptc-select2';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';


export const PREFIX = 'QuestionCategoryComponent';

@Component({
  selector: 'question-category-detail',
  templateUrl: './question-category-detail.component.html',
  styleUrls: ['./question-category-detail.component.scss']
})
export class QuestionCategoryDetailComponent extends FormBaseComponent implements OnInit {

  form: FormGroup = new FormGroup({});
  detail: QuestionCategoryDetail = new QuestionCategoryDetail();

  removeAnswerItems: QuestionClassificationAnswerViewModel[] = [];

  maxLevel: any[] = [];
  titleTypeString: string = "";
  public uiActionType: ActionType;

  get answerForm() {
    return this.form.get("Answers") as FormArray;
  }


  @ViewChild(DynamicQuestionSelectComponent) dynamicSelect: DynamicQuestionSelectComponent;

  constructor(
    public inejctor: Injector,
    public active: ActivatedRoute,
    public http: HttpService,
    public store: Store<fromReducers>,
    public ptcSelect2Service: PtcSelect2Service
  ) {
    super(inejctor, PREFIX);
  }
  ngOnInit() {
    this.listnerStore();
    this.initFormGroup();
  }

  ngOnDestroy() {
    this.store.dispatch(new fromQuestionCategoryActions.Clear());
    super.ngOnDestroy();
  }

  listnerStore() {
    this.active.params.pipe(takeUntil(this.destroy$)).subscribe(this.loadPage.bind(this));

    this.store.select(x => x.master.questionCategory.detail)
      .pipe(
        filter(x => x != null),
        takeUntil(this.destroy$)
      )
      .subscribe(detail => {
        this.detail = Object.assign(new QuestionCategoryDetail(), detail);
        this.dynamicSelect.updateView();

        // 常用語
        // setTimeout(() => {
        //   this.detail.Answers.forEach(answer => this.createAnswer(answer));
        // }, 0)
      });
  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);
    this.detail.BuID = params['node_ID'];

    const payload = {
      id: params['id'],
      nodeID: params['node_ID'],
      organizationType: params['organizationType']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromQuestionCategoryActions.GetDetail(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromQuestionCategoryActions.GetDetail(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
    }

  }

  initFormGroup() {
    this.detail.IsEnable = true;
    this.form = new FormGroup({
      BuID: new FormControl('', [
        Validators.required,
      ]),
      Name: new FormControl('', [
        Validators.required,
        Validators.maxLength(20)
      ]),
      ParentNodeID: new FormControl(''),
      IsEnable: new FormControl(true),

      CreateUserName: new FormControl(),
      CreateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateTime: new FormControl(),
    })
  }


  questionChange(parentQuestion?: QuestionSelectInfo) {
    var question = new QuestionCategoryDetail();

    question.Level = parentQuestion ? parentQuestion.Level : null;
    question.Name = parentQuestion ? parentQuestion.Name : null;
    question.ID = parentQuestion ? parentQuestion.ID : null;

    this.detail.ParentNode = parentQuestion ? question : null;
  }


  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd() {


    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    let req = Object.assign({}, this.form.value);
    req.BuID = this.detail.BuID;
    req.ParentNodeID = <any>this.dynamicSelect.lastHasValue;
    req.Level = this.dynamicSelect.currentLevel();

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.CreateDetail(req))
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  btnEdit($event) {


    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    let req = Object.assign({}, this.form.value);
    req.ID = this.detail.ID;
    req.ParentNodeID = <any>this.dynamicSelect.lastHasValue;
    req.Level = this.dynamicSelect.currentLevel();

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.EditDetail(req));
      }
    )));
  }


  @loggerMethod()
  btnBack($event) {
    history.back();
  }



}
