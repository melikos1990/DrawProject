import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { DailyReportViewModel } from 'src/app/model/download.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { State as fromReducers } from "../store/reducers"
import { HttpService } from 'src/app/shared/service/http.service';
import { tryGetReportKey, commonBu } from 'src/global';
import { HttpHeaders } from '@angular/common/http';
import { _loadingWork$ } from 'src/app/shared/ngrx/loading.ngrx';
import * as DownloadActions from '../store/actions/download.actions';
import { pipe, observable, Observable, interval, of } from 'rxjs';
import { ObjectService } from 'src/app/shared/service/object.service';
import { tap, take } from 'rxjs/operators';

export const PREFIX = 'AsoDailyReportComponent';

@Component({
  selector: 'app-aso-daily-report',
  templateUrl: './aso-daily-report.component.html',
  styleUrls: ['./aso-daily-report.component.scss']
})
export class AsoDailyReportComponent extends FormBaseComponent implements OnInit {

  model = new DailyReportViewModel();

  form: FormGroup;
  isEnable: boolean = false;
  nodeKey: string;
  /**
 * 這邊使用套件為 ptc-server-table
 * 請參照以下網址 ：
 * http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/ng-ptc-server-table?path=%2FREADME.md&version=GBmaster&_a=preview
 */
  @ViewChild('table')
  table: ServerTableComponent;

  constructor(
    public injector: Injector,
    public objectService: ObjectService,
    private store: Store<fromReducers>,
    public http: HttpService) {
    super(injector, PREFIX);
  }
  
  @loggerMethod()
  ngOnInit() {
    this.initFormGroup();
    this.setDefault();
  }

  initFormGroup() {
    this.form = new FormGroup({
      NodeID: new FormControl(null, [
        Validators.required
      ]),
      ReportType: new FormControl(null, [
        Validators.required
      ]),
    })
  }

  onselectChange(extend: any) {
    this.nodeKey = !extend ? "" : extend.NodeKey;
  }

  changeItems(items: any) {
    console.log("items", items);
    items = items.filter(c => c.extend.NodeKey == '007');
    
    if(items.length > 0){
      this.model.NodeID = items[0].id;
      this.nodeKey = items[0].extend.NodeKey;
    }
    
    return items;
  }

  /**
 * 按鈕按下查詢,渲染table
 */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnDownload() {

    if (this.validSearchForm() == false)
      return;

    
    const providerKey = tryGetReportKey(this.nodeKey);
    
    let params = { ReportType: this.model.ReportType, DateRange: this.model.DateTimeRange };

    let payload = {
        providerKey : providerKey,
        params : params
    };

    this.store.dispatch(new DownloadActions.downloadAsoReport(payload));
  }

  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else if (this.model.DateTimeRange == null) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(this.translateService.instant("DOWNLOAD.NEED_DATERANGE"))));
      return false;
    }
    else {
      return true;
    }
  }

  private setDefault(){
    
    // 如果為當月第一天 就不減天數
    var day = new Date().getDate();
    var startTime = day <= 1 ? new Date(new Date().setDate(1)) : new Date(new Date().setDate(day-1));

    // 計算扣除天數
    var decrementDay = startTime.getDate() <= 1 ? 0 : -(startTime.getDate() - 1);

    this.model.DateTimeRange = this.objectService.setDateTimeRange(startTime, decrementDay);


  }

}
