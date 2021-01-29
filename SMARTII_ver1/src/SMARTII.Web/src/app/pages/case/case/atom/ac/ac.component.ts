import { Component, OnInit, Injector, Output, Input, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CaseService } from 'src/app/shared/service/case.service';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { takeUntil, filter } from 'rxjs/operators';
import { CaseSourceViewModel } from 'src/app/model/case.model';
import { CaseTagDetailViewModel } from 'src/app/model/master.model';

@Component({
  selector: 'app-ac',
  templateUrl: './ac.component.html',
  styleUrls: ['./ac.component.scss']
})
export class AcComponent extends FormBaseComponent implements OnInit {


  @Output() related: EventEmitter<any> = new EventEmitter();
  @Output() lookup: EventEmitter<any> = new EventEmitter();

  @Input() sourcekey: string;
  loading: boolean = false;
  columns: any[] = [];
  data: any[] = [];

  tempSource: CaseSourceViewModel = new CaseSourceViewModel();

  constructor(public injector: Injector,
    public caseService: CaseService) {
    super(injector)
  }
  ngOnInit() {
    this.initializeTable();
    this.subscription();
  }


  getList() {
    this.loading = true;
 
      this.caseService
        .getNearlyCaseList(this.tempSource.NodeID , this.tempSource.User)
        .pipe(takeUntil(this.destroy$))
        .subscribe(x => {
          this.loading = false;
          this.data = x.element;
        })
  
  }
  btnRelated(row) {
    this.related.emit(row);
  }
  btnLookup(row) {
    this.lookup.emit(row);
  }

  isOnNodeIDChanged(newer: CaseSourceViewModel, exist: CaseSourceViewModel) {

    if (!exist) {

      if (!newer || !newer.NodeID) return false;
      else return true;
    }

    if (exist && newer) {
      return newer.NodeID !== exist.NodeID;
    }

  }

  initializeTable() {
    this.columns = [
      {//操作
        text: this.translateService.instant('CASE_COMMON.TABLE.OPERATE'),
        name: 'related',
        customer: true
      },
      {//案件編號
        text: this.translateService.instant('CASE_COMMON.CASE_ID'),
        name: 'CaseID',
      },
      {//立案日期
        text: this.translateService.instant('CASE_COMMON.CREATE_TIME'),
        name: 'CreateDateTime',
      },
      {//案件內容
        text: this.translateService.instant('CASE_COMMON.CASE_CONTENT'),
        name: 'Content',
        customer: true
      },
      {//被反應者
        text: this.translateService.instant('CASE_COMMON.CASE_COMPAINED_USER'),
        name: 'NodeName',
      },
      {//案件狀態
        text: this.translateService.instant('CASE_COMMON.CASE_TYPE'),
        name: 'CaseType',
      },
    ];
  }

  subscription() {

    this.caseService.sorceTempSubject.pipe(
      takeUntil(this.destroy$),
      filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey)),
      //filter(x => this.isSameNodeID(x) === false)
    )
      .subscribe(x => {
        const newer = { ...x[this.sourcekey] };
        this.tempSource = newer;

        // 初始化先不進行撈取
        //this.getList();

      })
  }

  isSameNodeID(data): boolean {
    return data[this.sourcekey].NodeID === this.tempSource.NodeID
  }

}
