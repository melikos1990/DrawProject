import { Component, OnInit, ViewChild, Input, Injector, OnDestroy } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { OrganizationTreeComponent } from 'src/app/shared/component/tree/organization-tree/organization-tree.component';
import { HeaderQuarterNodeViewModel, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { PreviewNodeTreeUserComponent } from '../preview-node-tree-user/preview-node-tree-user.component';


@Component({
  selector: 'app-vendor-node-tree-user-for-search-selector',
  templateUrl: './vendor-node-tree-user-for-search-selector.component.html',
})
export class VendorNodeTreeUserForSearchSelectorComponent extends BaseComponent implements OnInit, OnDestroy {

  @ViewChild('organizationTree') organizationTree: OrganizationTreeComponent;
  @ViewChild('table') table: PreviewNodeTreeUserComponent;
  @Input() model : OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
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
    this.http.post(`Common/Organization/GetVendorNodeTreeForSearch`,  null, this.model).subscribe((resp: any) => {
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
