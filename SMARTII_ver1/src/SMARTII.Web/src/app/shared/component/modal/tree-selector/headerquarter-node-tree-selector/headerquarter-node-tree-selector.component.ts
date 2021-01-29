import { Component, OnInit, ViewChild, Input, Injector, Output, EventEmitter } from '@angular/core';
import { OrganizationTreeComponent } from '../../../tree/organization-tree/organization-tree.component';
import { OrganizationDataRangeSearchViewModel, CallCenterNodeViewModel, HeaderQuarterNodeViewModel } from 'src/app/model/organization.model';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';


@Component({
  selector: 'app-headerquarter-node-tree-selector',
  templateUrl: './headerquarter-node-tree-selector.component.html',
})
export class HeaderquarterNodeTreeSelectorComponent extends BaseComponent implements OnInit {
  
  @ViewChild('organizationTree') organizationTree: OrganizationTreeComponent;
  @Input() model: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Output() onNodeClick : EventEmitter<any> = new EventEmitter();

  loading: boolean = false;
  treeModel: HeaderQuarterNodeViewModel = new HeaderQuarterNodeViewModel();


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

  clickNode = ($event) => {
    const currentItem = $event.data.extend;
    this.onNodeClick.emit(currentItem);
  }

}