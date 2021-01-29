import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { OrganizationNodeDetailViewModel } from 'src/app/model/organization.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-callcenter-node-user-modal',
  templateUrl: './callcenter-node-user-modal.component.html',
  styleUrls: ['./callcenter-node-user-modal.component.scss']
})
export class CallcenterNodeUserModalComponent extends BaseComponent implements OnInit {

  @Input() main: OrganizationNodeDetailViewModel = new OrganizationNodeDetailViewModel();


  data : any[] = [];
  nodeID: number;

  loading: boolean = false;

  table: LocalTableComponent;
  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

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


  getList() {
    this.loading = true;
    this.http.post("Common/Organization/GetCallCenterNodeUsersFromID", { nodeID: this.nodeID }, {}).subscribe((resp: any) => {
      this.data = resp;
      this.loading = false;
    });
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('JOB.NAME'),
        name: 'JobName',
      },
      {
        text: this.translateService.instant('JOB.LEVEL'),
        name: 'Level',
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('USER.IS_ENABLED'),
        name: 'IsEnabled',
      },
      {
        text: this.translateService.instant('USER.IS_AD'),
        name: 'IsAD',
      },
      {
        text: this.translateService.instant('USER.ACCOUNT'),
        name: 'Account',
      },
    ];

  }

}
