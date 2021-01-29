import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { AuthenticationType, PageAuth } from 'src/app/model/authorize.model';
import { CaseApplySearchViewModel } from 'src/app/model/substitute.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from "src/app/store/reducers"
import * as fromRootActions from "src/app/store/actions"
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ChangeInfo } from 'ptc-select2';
import { User } from 'src/app/model/authorize.model';
import { ApplyUserComponent } from '../modal/apply-user/apply-user.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSelectComponent } from 'src/app/shared/component/select/element/user-select/user-select.component';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { filter, takeUntil } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { webHostPrefix } from 'src/app.config';


export const feature = 'CaseApplyComponent';

@Component({
  selector: 'app-case-apply',
  templateUrl: './case-apply.component.html',
  styleUrls: ['./case-apply.component.scss']
})
export class CaseApplyComponent extends FormBaseComponent implements OnInit {

  /**
   * 這邊使用套件為 ptc-server-table
   * 請參照以下網址 ：
   * http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/ng-ptc-server-table?path=%2FREADME.md&version=GBmaster&_a=preview
   */

  @ViewChild(UserSelectComponent) select: UserSelectComponent;

  isEnable: boolean = false;
  form: FormGroup;

  @ViewChild('table')
  table: ServerTableComponent;

  disabled: boolean;
  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  /**
   * 查詢條件
   */
  public model: CaseApplySearchViewModel = new CaseApplySearchViewModel();

  public selectUser: User;

  constructor(
    public authService: AuthenticationService,
    private modalService: NgbModal,
    private store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector, '');

    this.featrueName = feature;
  }

  @loggerMethod()
  ngOnInit() {
    this.initRole();
    this.initializeTable();
    this.initFormGroup();
  }

  initFormGroup() {
    this.form = new FormGroup({
      NodeID: new FormControl(null, [
        Validators.required
      ]),
      CaseID: new FormControl(),
      CaseType: new FormControl(),
      CaseWarningID: new FormControl(),
      ApplyUserID: new FormControl(),
      CreateDateTimeRange: new FormControl(),
    })
  }

  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  @loggerMethod()
  onItemChange($event: ChangeInfo) {
    if ($event.item !== undefined)
      this.selectUser = $event.item.extend;
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Read)
  btnRender($event: any) {

    if (this.validSearchForm() == false) return;

    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  /**
   * 按鈕按下分派
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Update)
  btnAssign($event: any) {
    const selectedItems = this.table.getSelectItem();

    if (selectedItems == null || selectedItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    const ref = this.modalService.open(ApplyUserComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<ApplyUserComponent>ref.componentInstance);
    instance.selectItems = selectedItems;

    //派完後再次查詢
    instance.btnRender = this.btnRender.bind(this);
  }


  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Add)
  onBtnSearch($event: any) {

    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Read,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');

    // console.log($event);
    // this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
    //   url: './pages/case/case-create',
    //   params: {
    //     actionType: ActionType.Read,
    //     caseID: $event.CaseID
    //   }
    // }));
  }

  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(feature, AuthenticationType.Update)
  @AuthorizeMethod('C1Component', AuthenticationType.Update)
  onBtnEdit($event: any) {

    const url = `${environment.webHostPrefix}/pages/case/case-create`.toCustomerUrl({
      actionType: this.actionType.Update,
      caseID: $event.CaseID
    })
    window.open(url, '_blank');

    // console.log($event);
    // this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
    //   url: './pages/case/case-create',
    //   params: {
    //     actionType: this.actionType.Update,
    //     caseID: $event.CaseID
    //   }
    // }));
  }


  @loggerMethod()
  initRole() {
    let userAuth: PageAuth = { Feature: "", AuthenticationType: 0 };
    //取得當前角色
    let cacheRole = this.authService.getCacheRoleID();

    if (!!cacheRole) {
      this.authService.getRoles()
        .pipe(
          filter(x => !!x),
          takeUntil(this.destroy$))
        .subscribe(item => {
          let roleAuth = item.find(x => x.ID == cacheRole).Feature.find(x => x.Feature === "CaseApply");
          userAuth = roleAuth;
        });
    }
    else {
      this.authService.getUserMenu()
        .pipe(
          filter(x => !!x),
          takeUntil(this.destroy$))
        .subscribe(item => {
          let roleAuth = item.find(x => x.Feature === "CaseApply");
          userAuth = roleAuth;
        });
    }

    setTimeout(() => {
      //取得當前使用者資訊
      const cacheUser = this.authService.getCompleteUser();

      //是否為系統使用者
      if (userAuth.AuthenticationType > 31) {
        this.disabled = false;
      }
      else {
        cacheUser.subscribe(item => {
          this.select.selectedItems = [{ id: item.UserID, text: item.Name }];
        });

        this.disabled = true;
      }
    }, 1000)

  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Substitute/CaseApply/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_APPLY.NODE_ID'),
        name: 'NodeName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('CASE_APPLY.CASE_ID'),
        name: 'CaseID',
        disabled: false,
        order: 'CASE_ID'
      },
      {
        text: this.translateService.instant('CASE_APPLY.CASE_TYPE'),
        name: 'CaseType',
        disabled: false,
        order: 'CASE_TYPE'
      },
      {
        text: this.translateService.instant('CASE_APPLY.CASE_WARNING_ID'),
        name: 'CaseWarningName',
        disabled: false,
        order: 'CASE_WARNING_ID'
      },
      {
        text: this.translateService.instant('CASE_APPLY.APPLY_USER_ID'),
        name: 'ApplyUserName',
        disabled: false,
        order: 'APPLY_USERNAME'
      },
      {
        text: this.translateService.instant('CASE_APPLY.CREATE_DATETIME'),
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

  //驗證查詢表單
  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else {
      return true;
    }
  }
}
