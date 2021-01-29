import { Component, OnInit, Injector, Input, OnDestroy, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { CaseApplyListViewModel, CaseApplyCommitViewModel } from 'src/app/model/substitute.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { ChangeInfo } from 'ptc-select2';
import { User } from 'src/app/model/authorize.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { State as fromSubstitutrReducer } from '../../../store/reducers';
import { Store } from '@ngrx/store';
import * as fromRootActions from "src/app/store/actions"
import * as fromCaseApplyActions from '../../../store/actions/case-apply.actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSelectComponent } from 'src/app/shared/component/select/element/user-select/user-select.component';

export const feature = 'CaseApplyComponent';

@Component({
  selector: 'app-apply-user',
  templateUrl: './apply-user.component.html',
  styleUrls: ['./apply-user.component.scss']
})
export class ApplyUserComponent extends FormBaseComponent implements OnInit {

  @ViewChild('userSelect') userSelect: UserSelectComponent;
  @Input() selectItems: Array<CaseApplyListViewModel>;

  tableRow: number;

  public model: CaseApplyListViewModel = new CaseApplyListViewModel();

  public btnRender: () => void

  data: any[] = [];

  table: LocalTableComponent;
  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];


  constructor(
    public http: HttpService,
    public store: Store<fromSubstitutrReducer>,
    public activeModal: NgbActiveModal,
    public modalService: NgbModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
    this.getList();
  }

  @loggerMethod()
  closeModel() {
    this.activeModal.close();
  }

  getList() {
    this.data = this.selectItems;
  }

  public caseIDs: string[] = []
  @loggerMethod()
  addUser($event) {
    if (!this.userSelect.currentItem) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('選擇分派對象')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('確認分派?',
      () => {
        //取要分派的案件編號
        this.caseIDs = this.data.map(function (item) {
          return item.CaseID;
        })

        const payload = new EntrancePayload<CaseApplyCommitViewModel>();
        payload.data = new CaseApplyCommitViewModel();
        payload.data.ApplyUserID = this.userSelect.currentItem.extend.UserID;
        payload.data.CaseIDs = this.caseIDs;
        payload.success = () => {
          this.closeModel();
          this.btnRender();
        }
        this.store.dispatch(new fromCaseApplyActions.applyAction(payload));
      }
    )));

  }

  initializeTable() {
    this.tableRow = this.selectItems.length;

    this.columns = [
      {
        text: this.translateService.instant('CASE_APPLY.NODE_ID'),
        name: 'NodeName',
      },
      {
        text: this.translateService.instant('CASE_APPLY.CASE_ID'),
        name: 'CaseID',
      },
      {
        text: this.translateService.instant('CASE_APPLY.CASE_TYPE'),
        name: 'CaseType',
      },
      {
        text: this.translateService.instant('CASE_APPLY.CASE_WARNING_ID'),
        name: 'CaseWarningName',
      },
      {
        text: this.translateService.instant('CASE_APPLY.APPLY_USER_ID'),
        name: 'ApplyUserName',
      },
      {
        text: this.translateService.instant('CASE_APPLY.CREATE_DATETIME'),
        name: 'CreateDateTime',
      },
    ];
  }
}

