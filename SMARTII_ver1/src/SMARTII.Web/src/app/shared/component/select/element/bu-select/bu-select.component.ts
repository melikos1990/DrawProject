import { Component, OnInit, Injector, Input, forwardRef, EventEmitter, Output, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { HeaderQuarterNodeDetailViewModel, OrganizationType } from 'src/app/model/organization.model';
import { OpenSelectComponent } from '../../atom/open-select/open-select.component';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => BuSelectComponent),
  multi: true
};

@Component({
  selector: 'app-bu-select',
  templateUrl: './bu-select.component.html',
  providers: [INPUT_CONTROL_VALUE_ACCESSOR],
  styles:[
    'option:hover { background-color:gren; }'
  ]
})
export class BuSelectComponent extends BaseComponent implements OnInit {

  @Input() id: string = '';
  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;
  @Input() isSelf = true;
  @Input() buID?: number;
  @Input() IsSearchEnabled?: boolean = null;
  @Input() typeStyle: OrganizationType = OrganizationType.CallCenter;

  @Input() changeSelectItems: any;

  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();
  @Output() sendNodeKey = new EventEmitter();

  @ViewChild("select") select: OpenSelectComponent;

  private fetched: boolean = false;

  public nodeKey: string;
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: any[] = [];

  public selectedValue: any;

  public event: () => void;


  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v != this.innerValue) {
      this.innerValue = !this.isNull(v) ? v : null;
      this.onChangeCallback(this.innerValue);
    }
  }
  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.http.post(`Common/Organization/GetHeaderQuarterRootNodes`, {
      typeStyle: this.typeStyle,
      isSelf: this.isSelf,
      buID: this.buID,
      IsSearchEnabled: this.IsSearchEnabled
    }, {}).subscribe((resp: any) => {
      let res = resp.items;
      if(this.changeSelectItems != null){
        res = this.changeSelectItems(resp.items);
      }
      this.items = res;
      this.fetched = true;

      if (this.items.length === 1) {
        this.selectedValue = this.items[0].id;        
        this.sendNodeKey.emit(this.items[0].extend.NodeKey);
      }

      this.event && this.event();
    });
  }

  onChange($event) {
    const value = this.items.find(x => x.id == $event);
    (value == null) ? (this.nodeKey == null) : (this.nodeKey = value.extend.NodeKey);
    this.onSelectedChange.emit(value ? value.extend : null);
  }

  getItem(id: any){ return !!(this.select) ? this.select.getItem(id) : null ; }

  writeValue(obj: any): void {
    if (this.fetched) {
      if (obj != this.innerValue) {
        this.innerValue = !this.isNull(obj) ? String(obj) : null;
      }
    } else {
      this.event = () => {
        if (obj != this.innerValue) {
          this.innerValue = !this.isNull(obj) ? String(obj) : null;
        }
      }
    }
  }
  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  isNull(obj) {
    return obj == null || obj === undefined;
  }
}
