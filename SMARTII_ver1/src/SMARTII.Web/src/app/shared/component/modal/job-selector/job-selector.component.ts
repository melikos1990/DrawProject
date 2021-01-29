import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { JobSearchViewModel, HeaderQuarterNodeDetailViewModel, OrganizationNodeDetailViewModel } from 'src/app/model/organization.model';


@Component({
  selector: 'app-job-selector',
  templateUrl: './job-selector.component.html',
  styleUrls: ['./job-selector.component.scss']
})
export class JobSelectorComponent extends BaseComponent implements OnInit {


  @Input() main: OrganizationNodeDetailViewModel = new OrganizationNodeDetailViewModel();

  model: JobSearchViewModel = new JobSearchViewModel();

  @ViewChild('table')
  table: ServerTableComponent;

  isEnable: boolean = false;

  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public btnAddJob: any;

  constructor(
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
  }

  closeModel() {
    this.activeModal.close();
  }

  addJob() {
    if (this.btnAddJob) {
      this.btnAddJob(this.table.getSelectItem());
    }
  }

  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    this.model.DefinitionID = this.main.DefindID;
    this.model.Name = this.model.Name;
    $event.criteria = this.model;
    $event.direction = 'asc';
  }


  @loggerMethod()
  btnRender($event: any) {    
    
    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }


  initializeTable() {

    this.ajax.url = 'Organization/NodeDefinition/GetJobList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'LEVEL'
      },
      {
        text: this.translateService.instant('JOB.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('JOB.IS_ENABLED'),
        name: 'IsEnabledName',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('JOB.LEVEL'),
        name: 'Level',
        disabled: false,
        order: 'LEVEL'
      },
    ];

  }
  ngAfterViewInit()
  {
    this.btnRender(null);
  }

}
