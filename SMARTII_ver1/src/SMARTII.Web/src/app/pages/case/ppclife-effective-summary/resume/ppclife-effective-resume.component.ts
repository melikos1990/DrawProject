import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Store } from '@ngrx/store';
import { State as fromCaseReducers } from '../../store/reducers';
import { PPCLifeEffectiveResumeSearchViewModel, PPCLifeEffectiveListViewModel } from 'src/app/model/master.model';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import * as fromRootActions from 'src/app/store/actions';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { takeUntil, skip } from 'rxjs/operators';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';

export const PREFIX = 'PpclifeEffectiveSummaryComponent';

@Component({
  selector: 'app-ppclife-effective-resume',
  templateUrl: './ppclife-effective-resume.component.html',
  styleUrls: ['./ppclife-effective-resume.component.scss']
})
export class PpclifeEffectiveResumeComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  searchTerm = new PPCLifeEffectiveResumeSearchViewModel();

  columns: any[] = [];
  options: any = {};
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  constructor(
    public store: Store<fromCaseReducers>,
    public injector: Injector) {
    super(injector, PREFIX);
  }
  ngOnInit() {
    this.initializeTable();

    this.subscription();
  }

  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @loggerMethod()
  btnRender() {
    setTimeout(() => {
      this.table.render();
    }, 0);
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.searchTerm;
    ($event.direction == undefined || $event.direction == null) && ($event.direction = "desc");
  }

  initializeTable() {

    this.ajax.url = 'PPCLIFE/NotificationSender/GetResumeList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.RESUME_CREATE_DATETIME'),
        name: 'CreateDateTime',
        disabled: false,
        order: 'CREATE_DATETIME'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.ARRIVE_TYPE'),
        name: 'PPCLifeArriveType',
        disabled: false,
        order: 'ARRIVE_TYPE'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.ITEM_ID'),
        name: 'InternationalBarcode',
        disabled: false,
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.ITEM_NAME'),
        name: 'ItemName',
        disabled: false,
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.BATCH_NO'),
        name: 'BatchNo',
        disabled: false,
        order: 'BATCH_NO'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.RESUME_CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.RESUME_RESULT_TYPE'),
        name: 'NotificationGroupResultType',
        disabled: false,
        order: 'TYPE'
      },
      {
        text: this.translateService.instant('PPCLIFE_EFFECTIVE_SENDER.RESUME_CREATE_USER'),
        name: 'CreateUserName',
        disabled: false,
        order: 'CREATE_USERNAME'
      },
      {
        text: this.translateService.instant('COMMON.BTN_DOWNLOAD'),
        name: 'EMLFilePath',
        disabled: true,
        customer: true
      },
    ];
  }

  subscription() {

    this.store
      .select(x => x.case.ppclifeEffectiveSender.saveSelected)
      .pipe(
        takeUntil(this.destroy$),
        skip(1)
      )
      .subscribe(x => {
        this.btnRender();
      });
  }
}
