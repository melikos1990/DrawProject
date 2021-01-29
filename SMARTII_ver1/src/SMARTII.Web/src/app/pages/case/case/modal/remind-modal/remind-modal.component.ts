import { Component, OnInit, Input, ChangeDetectionStrategy, Injector, ChangeDetectorRef } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-remind-modal',
  templateUrl: './remind-modal.component.html',
  styleUrls: ['./remind-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RemindModalComponent extends BaseComponent implements OnInit {

  @Input() caseRemindIDs: number[];
  

  datas: any = [];
  columns: any[] = [];
  loading: boolean = false;

  constructor(
    public injector: Injector,
    public modalService: NgbModal,
    public http: HttpService,
    public changeDetectorRef: ChangeDetectorRef
  ) { 
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
    this.getList();
  }


  getList(){

    this.loading = true;

    this.http.post("Case/Case/GetCaseRemindFromCasePage", { caseRemindIDs: this.caseRemindIDs }, null)
      .subscribe(res => {
        this.loading = false;
        this.datas = res;
        this.changeDetectorRef.detectChanges();
      })
  }

  
  btnSearch($event){    

    let url = `${environment.webHostPrefix}/pages/master/case-remind-detail`.toCustomerUrl({
      actionType: this.actionType.Read,
      id: $event.ID,
    })

    window.open(url, '_blank');
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'BuName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CASE_ID'),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CASE_ASSIGNMENT_ID'),
        name: 'AssignmentID',
        disabled: false,
        order: 'ASSIGNMENT_ID'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT'
      },
      {
        text: this.translateService.instant('CASE_REMIND.LEVEL'),
        name: 'LevelName',
        disabled: false,
        order: 'TYPE'
      },
      {
        text: this.translateService.instant('CASE_REMIND.ISCONFIRM'),
        name: 'IsConfirm',
        disabled: false,
        order: 'IS_CONFIRM'
      },
      {
        text: this.translateService.instant('CASE_REMIND.ACTIVE_DATETIME_RANGE'),
        name: 'ActiveStartDateTime',
        disabled: false,
        order: 'ACTIVE_START_DAETTIME'
      },
      {
        text: this.translateService.instant('CASE_REMIND.CREATE_DATETIME_RANGE'),
        name: 'CreateDateTime',
        disabled: false,
        order: 'CREATE_DATETIME'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];


  }


}
