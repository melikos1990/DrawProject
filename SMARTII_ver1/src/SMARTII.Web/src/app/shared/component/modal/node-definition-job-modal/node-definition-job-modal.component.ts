import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { OrganizationType } from 'src/app/model/organization.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-node-definition-job-modal',
  templateUrl: './node-definition-job-modal.component.html',
  styleUrls: ['./node-definition-job-modal.component.scss']
})
export class NodeDefinitionJobModalComponent extends BaseComponent implements OnInit {

  data: any[] = [];
  nodeID: number;
  type: OrganizationType;

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
    this.http.post("Common/Organization/GetNodeDefJobsFromID", { nodeID: this.nodeID, type: this.type }, {}).subscribe((resp: any) => {
      this.data = resp;
      this.loading = false;
    });
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('NODE_DEFINITION.IDENTIFICATION_NAME'),
        name: 'DefinitionName',
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.NAME'),
        name: 'Name',
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.IS_ENABLED'),
        name: 'IsEnabledName',
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.LEVEL'),
        name: 'Level',
      },
    ];

  }

}

