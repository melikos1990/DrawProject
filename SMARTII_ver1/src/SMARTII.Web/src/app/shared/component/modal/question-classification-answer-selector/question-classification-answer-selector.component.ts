import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../../../store/reducers';
import {  QuestionClassificationAnswerSearchViewModel } from 'src/app/model/question-category.model';
import { ServerTableComponent } from '../../table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-question-classification-answer-selector',
  templateUrl: './question-classification-answer-selector.component.html',
  styleUrls: ['./question-classification-answer-selector.component.scss']
})
export class QuestionClassificationAnswerSelectorComponent extends FormBaseComponent implements OnInit {

  btnAdd: any;

  @Input() model : QuestionClassificationAnswerSearchViewModel = new QuestionClassificationAnswerSearchViewModel();
 

  @ViewChild('table')
  table: ServerTableComponent;

  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  constructor(
    public store: Store<fromMasterReducer>,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
  }

  closeModel() {
    this.activeModal.close();
  }
  
  btnRender() {
    this.table.render();
  }

  addQuestionClassificationAnswer() {
    let datas = this.table.getSelectItem();

    if (!datas || datas.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    if (datas && datas.length > 1) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('只能選擇一個項目')));
      return;
    }

    this.btnAdd && this.btnAdd(datas[0]);
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  initializeTable() {

    this.ajax.url = 'Master/QuestionClassificationAnswer/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ANSWER_ID'
      },
      {
        text: '主旨',
        name: 'Title',
        disabled: false,
        order: 'TITLE'
      },
      {
        text: '內容',
        name: 'Content',
        disabled: false,
        order: 'CONTENT'
      },
    ];

  }
  ngAfterViewInit()
  {
    this.btnRender();
  }


}
