import { Component, OnInit, Injector, Input, ViewChild, TemplateRef } from '@angular/core';
import { OrganizationType, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CallcenterNodeTreeUserSelectorComponent } from '../callcenter-node-tree-user-selector/callcenter-node-tree-user-selector.component';
import { HeaderquarterNodeTreeUserSelectorComponent } from '../headerquarter-node-tree-user-selector/headerquarter-node-tree-user-selector.component';
import { VendorNodeTreeUserSelectorComponent } from '../vendor-node-tree-user-selector/vendor-node-tree-user-selector.component';


@Component({
  selector: 'app-all-node-tree-user-selector',
  templateUrl: './all-node-tree-user-selector.component.html',
  styleUrls: ['./all-node-tree-user-selector.component.scss']
})
export class AllNodeTreeUserSelectorComponent extends BaseComponent implements OnInit {

  @Input() disabled: boolean = false;
  @Input() type: OrganizationType;
  
  @Input() model : OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  @Input() isUserEnabled?: boolean ;//人員清單是否篩選啟用人員
  @ViewChild(HeaderquarterNodeTreeUserSelectorComponent)
  headerquartersRef: HeaderquarterNodeTreeUserSelectorComponent;
  @ViewChild(CallcenterNodeTreeUserSelectorComponent)
  callCenterRef: CallcenterNodeTreeUserSelectorComponent;
  @ViewChild(VendorNodeTreeUserSelectorComponent)
  vendorRef: VendorNodeTreeUserSelectorComponent;

  public mapping = new Map<OrganizationType, any>(
    [
      [OrganizationType.HeaderQuarter, HeaderquarterNodeTreeUserSelectorComponent],
      [OrganizationType.CallCenter, CallcenterNodeTreeUserSelectorComponent],
      [OrganizationType.Vendor, VendorNodeTreeUserSelectorComponent],
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




}
