import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { State as fromCaseReducers } from '../../store/reducers';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import * as fromPPCLifeEffectiveActions from '../../store/actions/ppclife-effective.actions';
import { PtcAjaxOptions } from 'ptc-server-table';
import { skip, takeUntil, tap } from 'rxjs/operators';
import { interval } from 'rxjs';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { PPCLifeEffectiveListViewModel } from 'src/app/model/master.model';

export const REFRESH_TIME = 60000;
export const PREFIX = 'PpclifeEffectiveSummaryComponent';

@Component({
  selector: 'app-ppclife-effective-list',
  templateUrl: './ppclife-effective-list.component.html',
  styleUrls: ['./ppclife-effective-list.component.scss']
})
export class PpclifeEffectiveListComponent extends BaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  columns: any[] = [];
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  currentData: PPCLifeEffectiveListViewModel = new PPCLifeEffectiveListViewModel();

  constructor(
    public store: Store<fromCaseReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  render() {
    setTimeout(() => {
      this.table.render();
    }, 0);
  }

  ajaxSuccess(){
    if (this.currentData == null) return;
    console.log("準備重新Trigger currentData", this.currentData);
        this.checkExist() == true ?    
          this.onRowSelect(this.currentData) :
          this.store.dispatch(new fromPPCLifeEffectiveActions.refreshCaseList());
  }

  onRowSelect($event) {
    this.store.dispatch(new fromPPCLifeEffectiveActions.selectChangeAction($event));
  }

  ngOnInit() {

    console.log("執行查詢大量叫修資訊");
    this.initializeTable();
    this.subscription();
    this.store.dispatch(new fromPPCLifeEffectiveActions.TriggerGetArrivedList());
  }

  subscription() {

    this.store.select(x => x.case.ppclifeEffectiveSender.triggerFetch)
      .pipe(
        skip(1),
        takeUntil(this.destroy$)
      ).subscribe(() =>
        this.render(),
      );
    //頻率刷新Table資訊
    interval(REFRESH_TIME).pipe(
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.render(),
        console.log("刷新"),
        this.checkExist() == true ?
          this.onRowSelect(this.currentData) :
          this.store.dispatch(new fromPPCLifeEffectiveActions.refreshCaseList());
    });


    //監聽無視按鈕確認後 重查Table
    this.store
      .select(x => x.case.ppclifeEffectiveSender.refreshMain)
      .subscribe(x => {
        console.log("重查統藥大量叫修資料", this.currentData);
        this.render();      
      });

    //儲存Row選中的值
    this.store.select(x => x.case.ppclifeEffectiveSender.saveSelected)
      .subscribe(x => {
        this.currentData = new PPCLifeEffectiveListViewModel();
        this.currentData = x;
      })
  }

  checkExist(): boolean {
    if (this.currentData == null) return false;

    let data: PPCLifeEffectiveListViewModel[] = this.table.ptcServerTable.ajaxResp.data;
    console.log("table資料", data);
    console.log("currentData資料", this.currentData);
    return data.some(c => c.EffectiveID == this.currentData.EffectiveID);
  };
  
  /**
  * 初始化Table資訊
  */
  initializeTable() {

    this.ajax.url = 'PPCLIFE/NotificationSender/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.ARRIVE_TYPE'),
        name: 'PPCLifeArriveType',
        disabled: false,
        order: 'ARRIVE_TYPE'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.ITEM_ID'),
        name: 'InternationalBarcode',
        disabled: false,
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.ITEM_NAME'),
        name: 'CommodityName',
        disabled: false,
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.BATCH_NO'),
        name: 'BatchNo',
        disabled: true,
        customer: 'BATCH_NO'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.LIST_ACTUAL_COPUNT'),
        name: 'ArriveCount',
        disabled: false,
      },
    ];


  }
}
