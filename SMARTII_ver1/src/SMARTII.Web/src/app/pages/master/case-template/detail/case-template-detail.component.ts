import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { loggerClass, loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { CaseTemplateDetailViewModel } from 'src/app/model/master.model';
import * as fromCaseTemplateActions from '../../store/actions/case-template.actions';
import { State as fromMasterReducer } from '../../store/reducers';
import { Store } from '@ngrx/store';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';
import { skip, takeUntil, filter } from 'rxjs/operators';

export const PREFIX = 'CaseTemplateComponent';

@Component({
  selector: 'app-case-template-detail',
  templateUrl: './case-template-detail.component.html',
  styleUrls: ['./case-template-detail.component.scss']
})
@loggerClass()
export class CaseTemplateDetailComponent extends FormBaseComponent implements OnInit, OnDestroy {

  public form: FormGroup;

  public uiActionType: ActionType;
  public model: CaseTemplateDetailViewModel = new CaseTemplateDetailViewModel();
  public canFastFinish = false;
  titleTypeString: string = "";
  model$: Subscription;

  operatorInfo: {
    input: any,
    modelPropName: string;
    startPos: number,
    endPos: number
  };

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);

  }

  @loggerMethod()
  ngOnInit() {
    this.initializeForm();
    this.subscription();
  }



  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
    () => {
      if(this.model.ClassificKey != this.caseTemplate.CaseFinish){
        this.model.IsFastFinished = null;
      }
      this.store.dispatch(new fromCaseTemplateActions.addAction(this.model));
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

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromCaseTemplateActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));

    this.model$ =
      this.store
        .select((state: fromMasterReducer) => state.master.caseTemplate.detail)
        .pipe(
          skip(1),
          takeUntil(this.destroy$))
        .subscribe(caseTemplate => {
          this.model = { ...caseTemplate };
        });

    this.store.select((state: fromMasterReducer) => state.master.caseTemplate.canFastFinish)
              .pipe(
                filter(x => x != null),
                takeUntil(this.destroy$)
              ).subscribe(_canFastFinish => this.canFastFinish = _canFastFinish)
  }

  initializeForm() {
    this.form = new FormGroup({
      BuID: new FormControl(this.model.BuID, [
        Validators.required,
      ]),
      ClassificKey: new FormControl(this.model.ClassificKey, [
        Validators.required,
      ]),
      Title: new FormControl(this.model.Title, [
        Validators.required,
        Validators.maxLength(255),
      ]),
      Content: new FormControl(this.model.Content, [
        Validators.required,
        Validators.maxLength(4000),
      ]),
      IsDefault: new FormControl(this.model.IsDefault),
      IsFastFinished: new FormControl(this.model.IsFastFinished),
      EmailTitle: new FormControl(this.model.Title, [
        Validators.maxLength(255),
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.model.BuID = parseInt(params['BuID']);
        this.store.dispatch(new fromCaseTemplateActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromCaseTemplateActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromCaseTemplateActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }


  clickInput = (el, propName) => {
    this.operatorInfo = this.resetInputInfo(el, propName);
  }

  changeInputPos = (el, propName) =>{
    this.operatorInfo = this.resetInputInfo(el, propName);
  }


  insertTag(tag: string){
    
    if(!this.operatorInfo) return;

    let { input, startPos, modelPropName} = this.operatorInfo;

    let oldValue: string = input.value;

    let newValue =  oldValue.slice(0, startPos) + tag + oldValue.slice(startPos);
    
    this.model[modelPropName] = newValue;
    
  }

  modelChange = () => this.checkFastFinish();

  private resetInputInfo = (el, propName) => ({
    input: el,
    modelPropName: propName,
    startPos: el.selectionStart,
    endPos: el.selectionEnd
  })

  private checkFastFinish(){

    if(!this.model.BuID) return;

    const finishPayload = {
      buID: this.model.BuID
    };

    this.store.dispatch(new fromCaseTemplateActions.chackFastFinish(finishPayload));
  }

}

