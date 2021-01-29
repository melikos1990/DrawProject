import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, filter } from 'rxjs/operators';
import { CaseSourceViewModel } from 'src/app/model/case.model';

@Component({
  selector: 'app-pc',
  templateUrl: './pc.component.html',
})
export class PcComponent extends BaseComponent implements OnInit {

  @Output() related: EventEmitter<any> = new EventEmitter();
  @Input() sourcekey: string;

  loading: boolean = false;
  columns: any[] = [];
  data: any[] = [];

  tempSource: CaseSourceViewModel;

  constructor(
    public injector: Injector,
    public caseService: CaseService) {
    super(injector)
  }

  ngOnInit() {
    this.initializeTable();
    this.subscription();
    this.getList();
  }


  getList() {

    if(!this.tempSource) return;

    this.loading = true;
    this.caseService
      .getPreventCaseList(this.tempSource.NodeID)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.loading = false;
        this.data = x.element;

      })
  }
  btnRelated(row) {
    this.related.emit(row);
  }

  initializeTable() {
    this.columns = [
      {//勾稽
        text: this.translateService.instant('CASE_COMMON.RELATED'),
        name: 'related',
        customer: true
      },
      {//來源編號
        text: this.translateService.instant('CASE_COMMON.SOURCE_ID'),
        name: 'SourceID',
      },
      {//單位名稱
        text: this.translateService.instant('CASE_COMMON.TABLE.ORGANIZAITON_NAME'),
        name: 'NodeName',
      },
      {//立案時間
        text: this.translateService.instant('CASE_COMMON.CREATE_TIME'),
        name: 'CreateDateTime',
      },
      {//反應內容
        text: this.translateService.instant('CASE_COMMON.CONCAT_CONTENT'),
        name: 'Remark',
      },
    ];
  }

  subscription() {

    debugger;
    this.caseService.sorceTempSubject.pipe(
      takeUntil(this.destroy$),
      filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey))
    )
      .subscribe(x => {
        const newer = { ...x[this.sourcekey] }
        this.tempSource = newer;
      })
  }
}
