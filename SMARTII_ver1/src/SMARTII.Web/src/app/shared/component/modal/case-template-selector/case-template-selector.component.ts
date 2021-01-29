import { Component, OnInit, Injector, ViewChild, Input, AfterContentInit } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from '../../table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { CaseTemplateSearchViewModel } from 'src/app/model/master.model';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../../../store/reducers';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-case-template-selector',
  templateUrl: './case-template-selector.component.html',
  styleUrls: ['./case-template-selector.component.scss']
})
export class CaseTemplateSelectorComponent extends FormBaseComponent implements OnInit, AfterContentInit {
  


  btnAdd: any;

  @Input() model: CaseTemplateSearchViewModel = new CaseTemplateSearchViewModel();

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

  ngAfterContentInit(): void {

    setTimeout(() => {
      this.btnRender();
    }, 500)

  }

  closeModel() {
    this.activeModal.close();
  }

  btnRender() {
    this.table.render();
  }

  addCaseTemplate() {
    let datas = this.table.getSelectItem();
    
    if(!datas || datas.length == 0){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    if(datas && datas.length > 1){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('只能選擇一個項目')));
      return;
    }

    this.btnAdd && this.btnAdd(datas[0]);
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  initializeTable() {

    this.ajax.url = 'Master/CaseTemplate/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.CASE_TEMPLATE_TITLE'),
        name: 'Title',
        disabled: false,
        order: 'TITLE'
      },
      {
        text: this.translateService.instant('CASE_TEMPLATE.CASE_TEMPLATE_CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT'
      },
    ];

  }


}
