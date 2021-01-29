import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { JobListViewModel, CallCenterNodeDetailViewModel } from 'src/app/model/organization.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-store-owner-selector',
  templateUrl: './store-owner-selector.component.html',
  styleUrls: ['./store-owner-selector.component.scss']
})
export class StoreOwnerSelectorComponent extends BaseComponent implements OnInit {
  disabled: boolean = true;
  buID: number;
  nodeID: number;
  jobKey: string;
  isTraversing: boolean;
  data : any[] = [];
  loading: boolean = false;
  @ViewChild('table') table: LocalTableComponent;
  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  public btnAddOneJob: any;

  constructor(
    public http: HttpService,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
    this.getList();
  }

  @loggerMethod()
  btnBack($event: any) {
    this.activeModal.close();
  }
  public model: CallCenterNodeDetailViewModel = new CallCenterNodeDetailViewModel();
  addOneJob() {
    if (this.btnAddOneJob) {
      this.btnAddOneJob(this.table.getSelectItem()[0]);
    }
  }

  getList() {
    this.loading = true;
    this.http.post("Common/Organization/GetAboveHeaderQuarterNodeJobUsers", { nodeID: this.nodeID, jobKey: this.jobKey, isTraversing: this.isTraversing }, {}).subscribe((resp: any) => {
      this.data = resp;
      this.loading = false;
    });
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.NAME'),
        name: 'JobName',
      },
    ];

  }

}
