import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { OrganizationType, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';

@Component({
  selector: 'app-all-node-tree-selector',
  templateUrl: './all-node-tree-selector.component.html',
})
export class AllNodeTreeSelectorComponent extends BaseComponent implements OnInit {


  @Input() disabled: boolean = false;
  @Input() type: OrganizationType;
  @Input() model: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Output() onNodeClick: EventEmitter<any> = new EventEmitter();

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

  nodeClick($event) {
    this.onNodeClick.emit($event);
  }

  onSelectedChange($event) {
    this.model.Goal = $event;
  }
}
