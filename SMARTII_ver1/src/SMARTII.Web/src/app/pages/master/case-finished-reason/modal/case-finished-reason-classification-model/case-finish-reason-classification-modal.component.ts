import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CaseFinishClassificationDetailViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionType } from 'src/app/model/common.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import * as fromRootActions from 'src/app/store/actions';
import { State as fromMasterReducers } from '../../../store/reducers';
import { takeUntil } from 'rxjs/operators';


const PREFIX = 'CaseFinishedReasonComponent';


@Component({
  selector: 'app-case-finish-reason-classification-modal',
  templateUrl: './case-finish-reason-classification-modal.component.html',
  styleUrls: ['./case-finish-reason-classification-modal.component.scss']
})
export class CaseFinishReasonClassificationModalComponent extends FormBaseComponent implements OnInit {


  public uiActionType: ActionType;

  public btnAddClassification: (model) => void;
  public btnUpdateClassification: (model) => void;
  public form: FormGroup;

  public model: CaseFinishClassificationDetailViewModel = new CaseFinishClassificationDetailViewModel();

  constructor(
    public store: Store<fromMasterReducers>,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector, PREFIX);

  }

  addClassification() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.btnAddClassification && this.btnAddClassification(this.model);
      }
    )));
  }

  updateClassification() {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.btnUpdateClassification && this.btnUpdateClassification(this.model);
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    this.activeModal.dismiss();
  }

  ngOnInit() {
    this.initializeForm();
    this.subscription();
  }

  initializeForm() {

    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      Title: new FormControl(this.model.Title, [
        Validators.required,
      ]),
      IsEnabled: new FormControl(this.model.IsEnabled, null),
      IsMultiple: new FormControl(this.model.IsMultiple, null),
      IsRequired: new FormControl(this.model.IsRequired, null)

    });
  }
  closeModel() {
    this.activeModal.close();
  }

  subscription() {
    this.store.select(x => x.master.caseFinishedReason.classificationDetail)
      .pipe(takeUntil(this.destroy$))
      .subscribe(classification => this.model = { ...classification });
  }

}
