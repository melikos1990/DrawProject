import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { UserSearchViewModel, AuthenticationType } from 'src/app/model/authorize.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { BaseComponent } from 'src/app/pages/base/base.component';
const PREFIX = 'HeaderquarterNodeComponent';
@Component({
  selector: 'app-user-selector',
  templateUrl: './user-selector.component.html',
  styleUrls: ['./user-selector.component.scss']
})
export class UserSelectorComponent extends BaseComponent implements OnInit {

  model: UserSearchViewModel = new UserSearchViewModel();

  @ViewChild('table')
  table: ServerTableComponent;

  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public btnAddUser: any;

  public enabledItems = [
    { id: true, text: '啟用' },
    { id: false, text: '停用' }
  ];

  public systemUserItems = [
    { id: true, text: '是' },
    { id: false, text: '否' }
  ];

  public adItems = [
    { id: true, text: '是' },
    { id: false, text: '否' }
  ];

  constructor(
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector, PREFIX);
  }


  ngOnInit() {
    this.initializeTable();

    setTimeout(() => {
      this.btnRender(null);
    }, 500);
  }

  closeModel() {
    this.activeModal.close();
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  addJob() {
    if (this.btnAddUser) {
      this.btnAddUser(this.table.getSelectItem());
    }
  }

  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }


  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {
    this.table.render();
  }


  initializeTable() {

    this.ajax.url = 'Organization/User/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'USER_ID'
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('USER.ACCOUNT'),
        name: 'Account',
        disabled: false,
        order: 'ACCOUNT',
      },
      {
        text: this.translateService.instant('USER.IS_AD'),
        name: 'IsAD',
        disabled: false,
        order: 'IS_AD'
      },
      {
        text: this.translateService.instant('USER.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('USER.IS_SYSTEM_USER'),
        name: 'IsSystemUser',
        disabled: false,
        order: 'IS_SYSTEM_USER'
      },
    ];
  }

}
