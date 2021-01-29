import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  selector: 'app-smart-local-table',
  templateUrl: './smart-local-table.component.html',
  styleUrls: ['./smart-local-table.component.scss']
})
export class SmartLocalTableComponent extends BaseComponent implements OnInit {


  @Input() options: {} = {};

  public defaultOpts;
  public innerValue: [] = [];
  public source: LocalDataSource = new LocalDataSource();

  @Output() add: EventEmitter<any> = new EventEmitter();
  @Output() edit: EventEmitter<any> = new EventEmitter();
  @Output() delete: EventEmitter<any> = new EventEmitter();
  @Output() rowSelect: EventEmitter<any> = new EventEmitter();
  @Input()
  get data(): [] {

    return this.innerValue;
  }

  set data(v: []) {
    if (v !== undefined) {
      this.source.load(v);
      this.innerValue = v;
    }

  }

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

    this.defaultOpts = {
      hideSubHeader: true,
      editable: false,
      noDataMessage: this.translateService.instant('COMMON.TABLE_NO_DATA'),
      actions: {
        position: 'right',
        columnTitle: this.translateService.instant('COMMON.ACTION'),
        edit: false,
        add: false
      },
      delete: {
        deleteButtonContent: '<i class="nb-trash"></i>',
        confirmDelete: true,
      },
    };

    this.options = { ...this.defaultOpts, ...this.options };

  }

  userRowSelect($event) {
    this.rowSelect.emit($event);
  }
  btnAdd($event) {
    this.add.emit($event);
  }
  btnDelete($event) {
    this.delete.emit($event);
  }
  btnEdit($event) {
    this.edit.emit($event);
  }


}
