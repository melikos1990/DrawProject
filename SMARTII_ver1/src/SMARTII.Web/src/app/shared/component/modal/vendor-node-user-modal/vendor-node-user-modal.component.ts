import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { OrganizationNodeDetailViewModel } from 'src/app/model/organization.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-vendor-node-user-modal',
  templateUrl: './vendor-node-user-modal.component.html',
  styleUrls: ['./vendor-node-user-modal.component.scss']
})
export class VendorNodeUserModalComponent extends BaseComponent implements OnInit {
  
  @Input() main: OrganizationNodeDetailViewModel = new OrganizationNodeDetailViewModel();

  nodeID: number;
  data : any[] = [];
  table: LocalTableComponent;

  loading: boolean = false;

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
  }

  @loggerMethod()
  btnBack($event: any) {
    this.activeModal.close();
  }

  initializeTable() {
    this.loading = true;
    this.http.post("Common/Organization/GetVendorNodeUsersFromID", { nodeID: this.nodeID }, {}).subscribe((resp: any) => {
      this.data = resp;
      this.loading = false;
    });

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
        text: this.translateService.instant('USER.IS_SYSTEM_USER'),
        name: 'IsSystemUser',
      },
      {
        text: this.translateService.instant('USER.ACCOUNT'),
        name: 'Account',
      },
    ];

  }

}
