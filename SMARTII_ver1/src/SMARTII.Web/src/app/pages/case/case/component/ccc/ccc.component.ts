import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseViewModel } from 'src/app/model/case.model';
import { FormGroup, FormControl } from '@angular/forms';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, filter, take } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { QuestionClassificationAnswerSelectorComponent } from 'src/app/shared/component/modal/question-classification-answer-selector/question-classification-answer-selector.component';
import { QuestionClassificationAnswerSearchViewModel, QuestionClassificationAnswerListViewModel } from 'src/app/model/question-category.model';
import { ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-ccc',
  templateUrl: './ccc.component.html',
  styleUrls: ['./ccc.component.scss']
})
export class CccComponent extends FormBaseComponent implements OnInit {

  @ViewChild('dynamicSeletor') dynamicSelect: DynamicQuestionSelectComponent;

  @Input() uiActionType: ActionType;
  @Input() sourcekey: string;
  @Input() form: FormGroup = new FormGroup({});

  private _model: CaseViewModel;
  @Input() fileOptions = {};

  @Input()
  public set model(v: CaseViewModel) {
    this._model = v;
    this.updateView();
  }
  public get model(): CaseViewModel {
    this.refillPayload();
    return this._model
  }

  guides: any[] = [];
  dynamicGuid = this.guid.create().toString();

  constructor(
    public modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

  }


  updateView() {
    if (this._model && this.dynamicSelect) {
      this.dynamicSelect.updateView();
    }
  }
  refillPayload() {
    if (this._model && this.dynamicSelect) {
      this._model.QuestionClassificationID = this.dynamicSelect.lastHasValue;
    }
  }
  btnQuestionClassificationAnswerModal() {
    const ref = this.modalService.open(QuestionClassificationAnswerSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = (<QuestionClassificationAnswerSelectorComponent>ref.componentInstance);
    const model = new QuestionClassificationAnswerSearchViewModel();
    model.NodeID = this.model.NodeID;
    model.ClassificationID = this.model.QuestionClassificationID;
    instance.model = model;
    instance.btnAdd = (data: QuestionClassificationAnswerListViewModel) => {
      this.model.Content += (this.model.Content) ? `${String.prototype.newLine}${data.Content}` : data.Content;
      ref.dismiss();
    }
  }

  questionChange($event) {

    setTimeout(() => {
      const id = parseInt(this.dynamicSelect.lastHasValue);
      this.caseService.getQuestionClassificationGuides(true, id).subscribe(x => {      
        this.guides = x.element;
      })
    }, 100);

  }

  subscription() {
    this.caseService
      .sorceTempSubject.pipe(
        takeUntil(this.destroy$),
        filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey)),
        take(1)
      )
      .subscribe(x => {
        const newer = { ...x[this.sourcekey] };
        this.model.NodeID = newer.NodeID;
        this.model.OrganizationType = newer.OrganizationType;

      })
  }


}
