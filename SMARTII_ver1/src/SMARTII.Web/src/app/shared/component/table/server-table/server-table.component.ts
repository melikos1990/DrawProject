import { Component, OnInit, ViewChild, Input, EventEmitter, Output, Injector } from '@angular/core';
import { PtcServerTableComponent, PtcServerTableOptions, PtcAjaxOptions, PtcServerTableRequest, TableBtnOrderType } from 'ptc-server-table';
import { State as fromRootReducers } from "../../../../store/reducers"
import * as fromRootActions from "../../../../store/actions"
import { Store } from '@ngrx/store';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { PtcSwalType } from 'ptc-swal';

@Component({
  selector: 'app-server-table',
  templateUrl: './server-table.component.html',
  styleUrls: ['./server-table.component.scss']
})
export class ServerTableComponent extends BaseComponent implements OnInit {

  @ViewChild('ptcServerTable')
  ptcServerTable: PtcServerTableComponent;

  opts = new PtcServerTableOptions();

  public loading: boolean = false;

  @Output() critiria: EventEmitter<any> = new EventEmitter();
  @Output() ajaxSuccess: EventEmitter<any> = new EventEmitter();
  @Output() ajaxError: EventEmitter<any> = new EventEmitter();
  @Output() btnDelete: EventEmitter<any> = new EventEmitter();
  @Output() btnSearch: EventEmitter<any> = new EventEmitter();
  @Output() btnEdit: EventEmitter<any> = new EventEmitter();
  @Output() pageChange: EventEmitter<any> = new EventEmitter();
  @Output() rowSelect: EventEmitter<any> = new EventEmitter();
  @Input() btnSearchText: string;
  @Input() btnEditText: string;
  @Input() btnDeleteText: string;
  @Input() templateRefs: any = {};
  @Input() ajax: PtcAjaxOptions = new PtcAjaxOptions();
  @Input() pageSizeOptions: number[] = [50, 100, 150];
  @Input() defaultPageSize: number = 50;
  @Input() defaultOrderIndex?: number;
  @Input() defaultPageIndex: number = 0;
  @Input() columns: any = [];
  @Input() isVisibleBtnDelete: boolean = true;
  @Input() isVisibleBtnEdit: boolean = true;
  @Input() isVisibleBtnSearch: boolean = true;
  @Input() isVisibleFooter: boolean = true;
  @Input() btnOrderSetting: TableBtnOrderType[] = [TableBtnOrderType.Search, TableBtnOrderType.Edit, TableBtnOrderType.Delete];

  constructor(public store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.opts.ajax = this.ajax;
    this.opts.columns = this.columns;
    this.opts.pageSizeOptions = this.pageSizeOptions;
    this.opts.pageSize = this.defaultPageSize;
    this.opts.pageIndex = this.defaultPageIndex;
  }



  beforeAjax($event) {
    this.loading = true;
    //this.store.dispatch(new fromRootActions.LoadingActions.visibleLoadingAction());
  }


  onAjax($event: PtcServerTableRequest<any>) {
    this.critiria.emit($event);
  }

  onAjaxSuccess($event) {
    //this.store.dispatch(new fromRootActions.LoadingActions.invisibleLoadingAction());

    // 由於 SERVER 回傳200 , 但有內部錯誤的情況
    if ($event.isSuccess == false) {
      this.onAjaxError($event);
    } else {
      this.ajaxSuccess.emit($event);
    }

    this.loading = false;

  }

  onAjaxError($event) {

    // 遮罩關閉
    //this.store.dispatch(new fromRootActions.LoadingActions.invisibleLoadingAction());

    // 錯誤訊息提醒
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
      {
        detail: {
          title: this.translateService.instant('ERROR.TITLE'),
          text: this.translateService.instant('ERROR.FAILED', { message: $event.message }),
          type: PtcSwalType.error,
          showCancelButton: true
        },
        isLoop: false
      }));

    this.ajaxError.emit($event);

    this.loading = false;
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
    return this.ptcServerTable.getSelectRows();
  }

  getSearchTotalCount(): any {
    return this.ptcServerTable.ajaxResp.totalCount;
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

  render() {
    this.ptcServerTable.reset();
    this.ptcServerTable.render();
  }

  reset() {
    this.ptcServerTable.reset();
  }
}
