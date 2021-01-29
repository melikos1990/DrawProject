import { Component, OnInit, Injector, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseTemplateListViewModel } from 'src/app/model/master.model';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';


@Component({
  selector: 'app-cff',
  templateUrl: './cff.component.html',
  styleUrls: ['./cff.component.scss']
})
export class CffComponent extends FormBaseComponent implements OnInit {

  column = [];
  @Input() data: CaseTemplateListViewModel;
  @Output() onRowSelect: EventEmitter<CaseTemplateListViewModel> = new EventEmitter();

  @ViewChild(LocalTableComponent) table: LocalTableComponent;

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
  }

  btnRowSelect(row: CaseTemplateListViewModel) {
    console.log(row);
    this.onRowSelect.emit(row);
  }

  initializeTable() {
    this.column = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.TITLE'),
        name: 'Title',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CONTENT'),
        name: 'Content',
      },
    ]
  }

}
