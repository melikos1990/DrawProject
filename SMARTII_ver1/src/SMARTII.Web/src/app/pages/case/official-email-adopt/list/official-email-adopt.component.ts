import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AutoAssignComponent } from '../model/auto-assign/auto-assign.component';
import { AdminAssignComponent } from '../model/admin-assign/admin-assign.component';
import { OfficialEmailAdminOrderViewModel, OfficialEmailReplyRengeViewModel, OfficialEmailListViewModel, OfficialEmailAutoOrderViewModel, OfficialEmailBatchAdoptViewModel, OfficialEmailSearchViewModel, OfficialEmailAdoptViewModel } from 'src/app/model/case.model';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { BatchReplyComponent } from '../model/batch-reply/batch-reply.component';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import * as fromOfficialEmailAdoptActions from 'src/app/pages/case/store/actions/official-email-adopt.actions';
import { HttpService } from 'src/app/shared/service/http.service';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GroupSelectComponent } from 'src/app/shared/component/select/element/group-select/group-select.component';
import { ActivatedRoute } from '@angular/router';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';


export const PREFIX = 'OfficialEmailAdoptComponent';

@Component({
  selector: 'app-official-email-adopt',
  templateUrl: './official-email-adopt.component.html',
  styleUrls: ['./official-email-adopt.component.scss']
})
export class OfficialEmailAdoptComponent extends FormBaseComponent implements OnInit {

  @ViewChild('groupList') groupList: GroupSelectComponent;

  @ViewChild('table')
  table: ServerTableComponent;
  form: FormGroup;
  columns: any[] = [];
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  //buID: number; 20201023註解-改使用model BuId(回填查詢條件使用)

  model: OfficialEmailSearchViewModel = new OfficialEmailSearchViewModel();

  groupID: number;

  isEnable: boolean = false;
  IsSearchEnabled?: boolean = true;

  constructor(
    public injector: Injector,
    public modalService: NgbModal,
    private active: ActivatedRoute,
    public store: Store<fromCaseReducer>,
    public http: HttpService
  ) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.initFormGroup();
    this.subscription();
  }

  ngAfterViewInit() {
   
    this.model = this.fillbackCacheModel(this.table, null);
    setTimeout(() => {
      if(!!this.model.BuID){
        this.btnRender();
        this.groupList.getList(this.model.BuID);
      }
    }, 3000);
  }

  initFormGroup() {
    this.form = new FormGroup({
      BuID: new FormControl(null, [
        Validators.required
      ]),
      DateRange: new FormControl(),
      FromName: new FormControl(),
      FromAddress: new FormControl(),
      Subject: new FormControl(),
    })
  }

  subscription() {
    //20201023註解 - 用途不明
    // this.active.params.subscribe(params => {
    //   this.model.BuID = params["buID"];
    //   if(!!this.model.BuID){
    //     setTimeout(() => {
    //       this.btnRender();
    //     }, 0);
    //   }
    // });
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = {
      BuID: this.model.BuID,
      DateRange: this.model.DateRange,
      FromName: this.model.FromName,
      FromAddress: this.model.FromAddress,
      Subject: this.model.Subject,
      Body: this.model.Body
    }
  }

  btnRender() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  @SearchCacheMethod(PREFIX)
  btnAdopt(data: OfficialEmailListViewModel) {

    if (this.hasSelectedGroupID == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先選擇歸屬群組")));
      return;
    }

    const adoptModel = {
      MessageID: data.MessageID,
      GroupID: this.groupID,
      BuID: this.model.BuID
    } as OfficialEmailAdoptViewModel;

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否認養?',
      () => {
        let payload = new EntrancePayload(adoptModel);

        this.store.dispatch(new fromOfficialEmailAdoptActions.adoptEmail(payload));
      }
    )));

  }

  btnDeleteRange() {
    let datas = this.table.getSelectItem() as OfficialEmailListViewModel[];
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (!datas || datas.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("必須選擇一筆資料")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量刪除?',
      () => {
        let payload = new EntrancePayload(datas);

        payload.success = this.btnRender.bind(this);

        this.store.dispatch(new fromOfficialEmailAdoptActions.deleteRangeEmail(payload));
      }
    )));

  }


  btnAutoAssign(event) {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    if (this.hasSelectedGroupID == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先選擇歸屬群組")));
      return;
    }
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }
    
    let modal = this.modalService.open(AutoAssignComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    let instance = <AutoAssignComponent>modal.componentInstance;
    instance.onRefrach = this.btnRender.bind(this);
    instance.model = {
      BuID: this.model.BuID,
      GroupID: this.groupID
    } as OfficialEmailAutoOrderViewModel;
  }

  /**
   * 管理者指派
   * @param event 
   */
  btnAdminAssign(event) {


    if (this.hasSelectedGroupID == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先選擇歸屬群組")));
      return;
    }

    let datas = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }


    if (!datas || datas.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("必須選擇一封Email")));
      return;
    }


    if (this.hasCaseID(datas)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("選取的資料含有'案件編號'")));
      return;
    }

    let modal = this.modalService.open(AdminAssignComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    let instance = <AdminAssignComponent>modal.componentInstance;


    instance.selectedEmails = [...datas];

    instance.model = {
      MessageIDs: datas.map(x => x.MessageID),
      GroupID: this.groupID,
      BuID: this.model.BuID
    } as OfficialEmailAdminOrderViewModel;

    instance.onRefrach = this.btnRender.bind(this);


  }

  btnBatchReply(event) {

    if (this.hasSelectedGroupID == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先選擇歸屬群組")));
      return;
    }

    let datas = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (!datas || datas.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("必須選擇一封Email")));
      return;
    }

    if (this.hasCaseID(datas)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("選取的資料含有'案件編號'")));
      return;
    }

    let modal = this.modalService.open(BatchReplyComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    let instance = <BatchReplyComponent>modal.componentInstance;
    instance.buID = this.model.BuID;
    instance.model = {
      MessageIDs: datas.map(x => x.MessageID),
      GroupID: this.groupID,
      BuID: this.model.BuID
    } as OfficialEmailReplyRengeViewModel;

    instance.onRefrach = this.btnRender.bind(this);
  }

  btnBatchAdopt(event) {

    if (this.hasSelectedGroupID == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請先選擇歸屬群組")));
      return;
    }

    let datas: OfficialEmailListViewModel[] = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (!datas || datas.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("必須選擇一封Email")));
      return;
    }

    let model: OfficialEmailReplyRengeViewModel = {} as OfficialEmailReplyRengeViewModel;
    const msgList: string[] = [];

    datas.forEach(x => {
      msgList.push(x.MessageID);
    });
    model.MessageIDs = msgList;
    model.GroupID = this.groupID;
    model.BuID = this.model.BuID;

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量認養?',
      () => {
        let payload = new EntrancePayload(model);

        payload.success = this.btnRender.bind(this);

        this.store.dispatch(new fromOfficialEmailAdoptActions.batchAdoptEmail(payload));
      }
    )));
  }

  onBuChange($event) {

    console.log($event);
    this.groupList.getList($event);
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Case/OfficialEmail/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'RECEIVED_DATETIME'
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_RECEIVEDATETIME'),
        name: 'ReceivedDateTime',
        disabled: false,
        order: 'RECEIVED_DATETIME'
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_SOURCEADDRESS'),
        name: 'SourceAddress',
        disabled: false,
        order: 'FROM_ADDRESS'
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_SUBJECT'),
        name: 'Subject',
        disabled: false,
        order: 'SUBJECT',
        customer: true
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_SENDER'),
        name: 'FromName',
        disabled: false,
        order: 'FROM_NAME'
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_EMAIL'),
        name: 'FromAddress',
        disabled: false,
        order: 'FROM_ADDRESS'
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_CONTENT'),
        name: 'Body',
        disabled: true,
        order: 'BODY',
        customer: true
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_CASEID'),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'Operator',
        disabled: true,
        order: '',
        customer: true
      },
    ];


  }

  private hasCaseID = (datas: OfficialEmailListViewModel[]) => datas.some(x => !!(x.CaseID));

  private get hasSelectedGroupID() { return !!(this.groupID); }

}
