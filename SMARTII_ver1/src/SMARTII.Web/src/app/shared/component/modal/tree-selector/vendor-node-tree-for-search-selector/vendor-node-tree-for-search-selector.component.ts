import { Component, OnInit, ViewChild, Input, Injector, EventEmitter, Output } from '@angular/core';
import { PreviewNodeTreeUserComponent } from '../../tree-user/preview-node-tree-user/preview-node-tree-user.component';
import { OrganizationTreeComponent } from '../../../tree/organization-tree/organization-tree.component';
import { OrganizationDataRangeSearchViewModel, CallCenterNodeViewModel, VendorNodeViewModel } from 'src/app/model/organization.model';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';

@Component({
  selector: 'app-vendor-node-tree-for-search-selector',
  templateUrl: './vendor-node-tree-for-search-selector.component.html',
})
export class VendorNodeTreeForSearchSelectorComponent extends BaseComponent implements OnInit {
  @ViewChild('organizationTree') organizationTree: OrganizationTreeComponent;
  @Input() model: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Output() onNodeClick : EventEmitter<any> = new EventEmitter();
  
  loading: boolean = false;
  treeModel: VendorNodeViewModel = new VendorNodeViewModel();


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
    this.http.post(`Common/Organization/GetVendorNodeTreeForSearch`, null, this.model).subscribe((resp: any) => {
      this.treeModel = resp.element;
      setTimeout(() => {
        this.organizationTree.tree.treeUpdate();
        this.organizationTree.tree.collapseAll();
        this.loading = false;
      }, 0);
    });

  }

  clickNode = ($event) => {
    const currentItem = $event.data.extend;
    this.onNodeClick.emit(currentItem);
  }

}
