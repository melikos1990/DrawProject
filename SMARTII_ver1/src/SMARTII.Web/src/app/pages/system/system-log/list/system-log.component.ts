import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthBaseComponent } from 'src/app/pages/base/auth-base.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { SystemLogSearchModel } from 'src/app/model/system.model';


export const PREFIX = 'SystemLogComponent';

@Component({
  selector: 'app-system-log',
  templateUrl: './system-log.component.html',
  styleUrls: ['./system-log.component.scss']
})
export class SystemLogComponent extends AuthBaseComponent implements OnInit {

  JSON = JSON;

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

  isEnableResult: boolean = false; 

  /**F
   * 查詢條件
   */
  public model: SystemLogSearchModel = new SystemLogSearchModel();
  public operatorTypes: any;

  constructor(public injector: Injector) {
    super(injector, PREFIX);
  }


  @loggerMethod()
  ngOnInit() {
    this.initializeTable();
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {
    this.isEnableResult = null;
    this.table.render();
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'System/SystemLog/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('SYSTEM_LOG.FEATURE_NAME'),
        name: 'FeatureName',
        disabled: false,
        order: 'FEATURE_NAME'
      },
      {
        text: this.translateService.instant('SYSTEM_LOG.CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT',
        customer: true,
      },
      {
        text: this.translateService.instant('SYSTEM_LOG.CREATE_DATETIME'),
        name: 'CreateDateTime',
        disabled: false,
        order: 'CREATE_DATETIME'
      },
      {
        text: this.translateService.instant('SYSTEM_LOG.CREATE_USERNAME'),
        name: 'CreateUserName',
        disabled: false,
        order: 'CREATE_USERNAME'
      },
      {
        text: this.translateService.instant('SYSTEM_LOG.OPERATOR'),
        name: 'Operator',
        disabled: false,
        order: 'OPERATOR'
      },
    ];


  }


}
