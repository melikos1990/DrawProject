import { Component, OnInit, Injector, Input, ViewChild, TemplateRef, Output, EventEmitter } from '@angular/core';
import { OrganizationType, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CallcenterNodeTreeUserSelectorComponent } from '../callcenter-node-tree-user-selector/callcenter-node-tree-user-selector.component';
import { HeaderquarterNodeTreeUserSelectorComponent } from '../headerquarter-node-tree-user-selector/headerquarter-node-tree-user-selector.component';
import { VendorNodeTreeUserForSearchSelectorComponent } from '../vendor-node-tree-user-for-search-selector/vendor-node-tree-user-for-search-selector.component';



@Component({
  selector: 'app-all-node-tree-user-selector-for-cc',
  templateUrl: './all-node-tree-user-selector-for-cc.component.html',
  styleUrls: ['./all-node-tree-user-selector-for-cc.component.scss']
})
export class AllNodeTreeUserSelectorForCcComponent extends BaseComponent implements OnInit {

  @Input() disabled: boolean = false;
  @Input() type: OrganizationType;
  
  @Input() model : OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Input() isUserEnabled?: boolean ;//人員清單是否篩選啟用人員
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();
  
  @ViewChild(HeaderquarterNodeTreeUserSelectorComponent)
  headerquartersRef: HeaderquarterNodeTreeUserSelectorComponent;
  @ViewChild(CallcenterNodeTreeUserSelectorComponent)
  callCenterRef: CallcenterNodeTreeUserSelectorComponent;
  @ViewChild(VendorNodeTreeUserForSearchSelectorComponent)
  vendorRef: VendorNodeTreeUserForSearchSelectorComponent;

  public mapping = new Map<OrganizationType, any>(
    [
      [OrganizationType.HeaderQuarter, HeaderquarterNodeTreeUserSelectorComponent],
      [OrganizationType.CallCenter, CallcenterNodeTreeUserSelectorComponent],
      [OrganizationType.Vendor, VendorNodeTreeUserForSearchSelectorComponent],
    ]
  );

  constructor(public injector: Injector) {
    super(injector);

  }

  ngOnInit() { }


  getValue = () => {

    switch (+this.type) {
      case OrganizationType.CallCenter:
        return this.callCenterRef.getValue();
      case OrganizationType.HeaderQuarter:
        return this.headerquartersRef.getValue();
      case OrganizationType.Vendor:
        return this.vendorRef.getValue();
      default:
        return null;
    }

  }

  selectedChange($event) {
    this.onSelectedChange.emit($event);
  }




}
