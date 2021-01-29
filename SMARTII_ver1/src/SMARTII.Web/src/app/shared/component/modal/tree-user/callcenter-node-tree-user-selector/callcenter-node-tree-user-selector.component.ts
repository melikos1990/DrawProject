import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { HttpService } from 'src/app/shared/service/http.service';
import { CallCenterNodeViewModel, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { OrganizationTreeComponent } from 'src/app/shared/component/tree/organization-tree/organization-tree.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { FormControl } from '@angular/forms';
import { skip, debounceTime, takeUntil } from 'rxjs/operators';
import { PreviewNodeTreeUserComponent } from '../preview-node-tree-user/preview-node-tree-user.component';

@Component({
  selector: 'app-callcenter-node-tree-user-selector',
  templateUrl: './callcenter-node-tree-user-selector.component.html',
})
export class CallcenterNodeTreeUserSelectorComponent extends BaseComponent implements OnInit {

  @ViewChild('organizationTree') organizationTree: OrganizationTreeComponent;
  @ViewChild('table') table: PreviewNodeTreeUserComponent;

  @Input() model: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Input() isUserEnabled?: boolean;

  loading: boolean = false;

  treeModel: CallCenterNodeViewModel = new CallCenterNodeViewModel();

  currentItem: CallCenterNodeViewModel;

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.getTree();
  }



  getTree() {
    this.loading = true;
    this.http.post(`Common/Organization/GetCallCenterNodeTree`, null, this.model).subscribe((resp: any) => {
      this.treeModel = resp.element;
      setTimeout(() => {
        this.organizationTree.tree.treeUpdate();
        this.organizationTree.tree.collapseAll();
        this.loading = false;
      }, 0);
    });

  }

  getValue = () => {
    return this.table.getValue();
  }

  clickNode = ($event) => {
    this.currentItem = $event.data.extend;
  }


}
