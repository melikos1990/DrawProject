import { Component, OnInit, Injector, ViewChild, Input, AfterViewInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { skip, debounceTime, takeUntil } from 'rxjs/operators';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ServerTableComponent } from '../../../table/server-table/server-table.component';
import { PtcServerTableRequest, PtcAjaxOptions } from 'ptc-server-table';
import { OrganizationType } from 'src/app/model/organization.model';

@Component({
  selector: 'app-preview-node-tree-user',
  templateUrl: './preview-node-tree-user.component.html',
  styleUrls: ['./preview-node-tree-user.component.scss']
})
export class PreviewNodeTreeUserComponent extends BaseComponent implements OnInit, AfterViewInit {


  @ViewChild('table') table: ServerTableComponent;
  

  @Input()
  set currentNodeItem(val) {
    if (!val) return;
    this._currentNodeItem = val;
    setTimeout(() => {
      this.table.render();
    }, 0);
  }
  get currentNodeItem() { return this._currentNodeItem; }

  @Input() isUserEnabled? : boolean;

  private _currentNodeItem: any;

  nameControl: FormControl = new FormControl();
  userName: string;

  loading: boolean = false;



  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();
  columns: any[] = [];

  constructor(
    public injector: Injector
  ) {
    super(injector);
  }

  ngOnInit() {
    this.initializeTable();
  }
  
  ngAfterViewInit(){
    this.subscription();
    this.table.render();
  }

  subscription() {

    this.nameControl.valueChanges
      .pipe(
        skip(1),
        debounceTime(1000),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
          this.table.render();
      });
  }

  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = {
      UserName: this.userName,
      NodeID: this.currentNodeItem.ID,
      OrganizationType: this.currentNodeItem.OrganizationType,
      IsUserEnabled: this.isUserEnabled
    };
  }

  getValue = () => {
    return this.table != null ? this.table.getSelectItem() : null;
  }


  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = this.initUrl().toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'USER_ID'
      },
      {
        text: this.translateService.instant('USER.JOB_NAME'),
        name: 'JobName',
        disabled: true,
        order: ''
      },
      {
        text: this.translateService.instant('USER.ACCOUNT'),
        name: 'Account',
        disabled: true,
        order: ''
      },
      {
        text: this.translateService.instant('USER.USER_NAME'),
        name: 'UserName',
        disabled: false,
        order: 'NAME'
      },
    ];
  }

  private initUrl() {

    let url = '';

    switch (this.currentNodeItem.OrganizationType) {
      case OrganizationType.CallCenter:
        url = 'Common/Organization/GetCallCenterNodeUsers';
        break;
      case OrganizationType.Vendor:
        url = 'Common/Organization/GetVendorNodeUsers';
        break;
      case OrganizationType.HeaderQuarter:
        url = 'Common/Organization/GetHeaderQuarterNodeUsers';
        break;
      default:
        throw new Error("未取得 OrganizationType");
    }

    return url;

  }

}
