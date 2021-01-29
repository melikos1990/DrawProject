import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { OrganizationType, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';


@Component({
  selector: 'app-all-node-tree-selector-for-cc',
  templateUrl: './all-node-tree-selector-for-cc.component.html',
})
export class AllNodeTreeSelectorForCcComponent extends BaseComponent implements OnInit {


  @Input() disabled: boolean = false;
  @Input() type: OrganizationType;
  @Input() model: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Output() onNodeClick: EventEmitter<any> = new EventEmitter();
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

  nodeClick($event) {
    this.onNodeClick.emit($event);
  }

  selectedChange($event) {
    this.onSelectedChange.emit($event);
  }

}
