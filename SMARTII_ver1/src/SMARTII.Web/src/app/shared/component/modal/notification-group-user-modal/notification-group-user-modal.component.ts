
import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-notification-group-user-modal',
  templateUrl: './notification-group-user-modal.component.html',
  styleUrls: ['./notification-group-user-modal.component.scss']
})
export class NotificationGroupUserModalComponent extends BaseComponent implements OnInit {

  table: LocalTableComponent;
  data: any[] = [];
  public nodeID: number;
  public groupID: number;

  loading: boolean = false;

  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  public btnAddUser: any;

  constructor(
    public http: HttpService,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
  }

  btnRender() {
    this.getList();
  }

  closeModel() {
    this.activeModal.close();
  }

  addUser() {
    if (this.btnAddUser) {
      this.btnAddUser(this.data);
    }
  }

  getList() {
    this.loading = true;
    this.http.post("Common/Notification/GetNotificationGroupUser", { groupID: this.groupID }, {}).subscribe((resp: any) => {
      this.data = resp;
      this.loading = false;
    });
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('NODE_DEFINITION.ORGANIZATION_TYPE'),
        name: 'OrganizationTypeName',
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
      },
    ];

  }


}




