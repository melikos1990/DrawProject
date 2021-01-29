import { Component, OnInit, Input, Output, EventEmitter, Injector, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { PtcLocalTableComponent, TableBtnOrderType } from 'ptc-local-table';

@Component({
  selector: 'app-local-table',
  templateUrl: './local-table.component.html',
  styleUrls: ['./local-table.component.scss']
})
export class LocalTableComponent extends BaseComponent implements OnInit {

  @ViewChild('ptcLocalTable')
  ptcLocalTable: PtcLocalTableComponent;

  @Input() loading: boolean = false;
  @Input() items: any[] = [];
  @Input() pageIndex: number = 0;
  @Input() pageSize: number = 50;
  @Input() pageSizeOptions: number[] = [50, 100, 150];
  @Input() columns: any = [];
  @Input() templateRefs = {};
  @Output() btnDelete: EventEmitter<any> = new EventEmitter();
  @Output() btnSearch: EventEmitter<any> = new EventEmitter();
  @Output() btnEdit: EventEmitter<any> = new EventEmitter();
  @Output() rowSelect: EventEmitter<any> = new EventEmitter();
  @Output() pageChange: EventEmitter<any> = new EventEmitter();
  @Input() btnSearchText: string;
  @Input() btnEditText: string;
  @Input() btnDeleteText: string;
  @Input() isVisibleBtnDelete: boolean = true;
  @Input() isVisibleBtnEdit: boolean = true;
  @Input() isVisibleBtnSearch: boolean = true;
  @Input() isVisibleFooter: boolean = true;
  @Input() btnOrderSetting: TableBtnOrderType[] = [TableBtnOrderType.Search, TableBtnOrderType.Edit, TableBtnOrderType.Delete];

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

  }


  onBtnDelete($event) {
    this.btnDelete.emit($event);
  }

  onBtnSearch($event) {
    this.btnSearch.emit($event);
  }

  onBtnEdit($event) {
    this.btnEdit.emit($event);
  }

  onPageChange($event) {
    this.pageChange.emit($event);
  }

  onRowSelect($event) {
    this.rowSelect.emit($event);
  }

  getSelectItem(): any {
    return this.ptcLocalTable.getSelectRows();
  }

  getSearchTotalCount(): any {
    return this.ptcLocalTable.data.length;
  }


  setRangeLabel(page: number, pageSize: number, length: number): string {
    if (length === 0 || pageSize === 0) {
      return `0 of ${length}`;
    }
    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
    const text = this.translateService.instant('COMMON.PAGE_DIRECTION', {
      startIndex: startIndex + 1,
      endIndex: endIndex,
      length: length
    });
    return text;
  }


}
