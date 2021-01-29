import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { HttpService } from 'src/app/shared/service/http.service';
import { HeaderQuarterNodeViewModel, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { OrganizationTreeComponent } from 'src/app/shared/component/tree/organization-tree/organization-tree.component';
import { skip, debounceTime, takeUntil } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { PreviewNodeTreeUserComponent } from '../preview-node-tree-user/preview-node-tree-user.component';

@Component({
  selector: 'app-headerquarter-node-tree-user-selector',
  templateUrl: './headerquarter-node-tree-user-selector.component.html',
})
export class HeaderquarterNodeTreeUserSelectorComponent extends BaseComponent implements OnInit {

  @ViewChild('organizationTree') organizationTree: OrganizationTreeComponent;
  @ViewChild('table') table: PreviewNodeTreeUserComponent;
  @Input() model: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Input() isUserEnabled?: boolean;
  loading: boolean = false;

  treeModel: HeaderQuarterNodeViewModel = new HeaderQuarterNodeViewModel();

  currentItem: HeaderQuarterNodeViewModel;

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
    
    this.http.post(`Common/Organization/GetHeaderQuarterNodeTree`, null, this.model).subscribe((resp: any) => {
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
