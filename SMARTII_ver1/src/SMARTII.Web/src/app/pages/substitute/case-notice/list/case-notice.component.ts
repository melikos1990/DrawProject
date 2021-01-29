import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { CaseNoticeSearchViewModel } from 'src/app/model/substitute.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from "src/app/store/reducers"
import * as fromRootActions from "src/app/store/actions"
import * as fromCaseNoticeActions from '../../store/actions/case-notice.actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';

const feature = 'CaseNoticeComponent';

@Component({
  selector: 'app-case-notice',
  templateUrl: './case-notice.component.html',
  styleUrls: ['./case-notice.component.scss']
})
export class CaseNoticeComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table') table: LocalTableComponent;
  data: any[] = [];
  loading: boolean = false;
  pageSize: number;
  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];


  /**
   * 查詢條件
   */
  public model: CaseNoticeSearchViewModel = new CaseNoticeSearchViewModel();

  constructor(
    public http: HttpService,
    public authService: AuthenticationService,
    private active: ActivatedRoute,
    private store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector, '');

    this.featrueName = feature;
  }

  @loggerMethod()
  ngOnInit() {
    this.initializeTable();
    this.subscription();
    
  }

  subscription() {
    this.active.params.subscribe(params => {
      this.model.CaseNoticeType = params["caseNoticeType"];
      setTimeout(() => {
        this.selectedChange(null);
      }, 0);
    });
  }


  getList() {
    this.loading = true;
    this.pageSize = 0;
    this.http.post("Substitute/CaseNotice/GetList", {}, this.model).subscribe((resp: any) => {

      this.data = resp.element;
      if (!resp.isSuccess) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(resp.message)));
      }

      this.loading = false;
    });
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Read)
  selectedChange($event: any) {

    this.getList();
  }

  /**
 * 按鈕按下刪除
 * @param $event
 */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Read | AuthenticationType.Delete)
  btnNoticeRnage($event: any) {

    const selectedItems = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (selectedItems == null || selectedItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量確認?',
      () => {
        const payload = new EntrancePayload<number[]>();
        payload.data = selectedItems.map(x => x.ID);
        payload.success = () => this.selectedChange(null);
        this.store.dispatch(new fromCaseNoticeActions.noticeRangeAction(payload));
      }
    )));

  }

  /**
   * 當ptc local table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Delete | AuthenticationType.Add)
  onBtnEdit($event: any) {
    // const payload = new EntrancePayload<{ caseID: string, ID: number }>();
    
    // payload.data = {
    //   caseID: $event.CaseID,
    //   ID: $event.ID,
    // };

    // this.store.dispatch(new fromCaseNoticeActions.noticeAction(payload));


    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Update,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');
    
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.NODE_ID'),
        name: 'NodeName',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.CASE_ID'),
        name: 'CaseID',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.CREATE_DATETIME'),
        name: 'CreateDateTime',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.CASE_TYPE'),
        name: 'CaseTypeName',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.CASE_WARNING_ID'),
        name: 'CaseWarningTypeName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.CASE_CONTENT'),
        name: 'CaseContent',
        customer: true,
      },     
      {
        text: this.translateService.instant('CASE_NOTICE.CONTACT_USER_NAME'),
        name: 'ContactUserNeme',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.CREATE_USERNAME'),
        name: 'CreateUserName',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.APPLY_DATETIME'),
        name: 'ApplyDateTime',
      },
      {
        text: this.translateService.instant('CASE_NOTICE.PROMISE_DATETIME'),
        name: 'PromiseDateTime',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
      },
    ];


  }
}

