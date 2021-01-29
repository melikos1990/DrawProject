import { Component, OnInit, Input, Injector, ViewChild, TemplateRef } from '@angular/core';
import { CaseService } from 'src/app/shared/service/case.service';
import { CaseResumeListViewModel } from 'src/app/model/case.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { takeUntil } from 'rxjs/operators';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-case-resume',
  templateUrl: './case-resume.component.html',
  styleUrls: ['./case-resume.component.scss']
})
export class CaseResumeComponent extends FormBaseComponent implements OnInit {

  @ViewChild('detial') detial: TemplateRef<any>;
  constructor(
    private modalService: NgbModal,
    public caseService: CaseService,
    public injector: Injector,
  ) { super(injector); }
  @Input() caseID: string;

  concatColumn = [];
  data = [];
  popupUserRef: NgbModalRef;
  public model: CaseResumeListViewModel = new CaseResumeListViewModel();

  ngOnInit() {
     

  }

  initializeTable() {
    this.caseService.getResumeList(this.caseID).pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.data = x.element;
        console.log()
      });
    this.concatColumn = [
      {//案件狀態
        text: this.translateService.instant('CASE_COMMON.CASE_TYPE'),
        name: 'CaseType'
      },
      {//建立時間
        text: this.translateService.instant('CASE_TEMPLATE.CREATE_DATETIME_LABEL'),
        name: 'CreateDateTime',
      },
      {//異動內容
        text: this.translateService.instant('CASE_COMMON.TABLE.CHANGE_CONTENT'),
        name: 'Content',
      },
      {//處理人員
        text: this.translateService.instant('CASE_COMMON.TABLE.PROCESS_USER'),
        name: 'CreateUserName',
      },
    ];


  }

  btnViewDetial() {
    this.initializeTable();
    this.popupUserRef = this.modalService.open(this.detial, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
  }

  closeModel(){ 
    this.popupUserRef.dismiss();
  }

}
