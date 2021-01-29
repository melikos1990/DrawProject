import { Component, OnInit, Injector, Input, forwardRef, EventEmitter, Output, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { OrganizationType } from 'src/app/model/organization.model';
import { OpenSelectComponent } from '../../atom/open-select/open-select.component';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => GroupSelectComponent),
  multi: true
};

@Component({
  selector: 'app-group-select',
  templateUrl: './group-select.component.html',
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class GroupSelectComponent extends BaseComponent implements OnInit {

  @Input() id: string = '';
  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;
  @Input() isSelf = true;
  @Input() defaultFirst: boolean = true;
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();

  @ViewChild("select") select: OpenSelectComponent;

  private fetched: boolean = false;

  public nodeKey: string;
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: any[] = [];

  public event: () => void;


  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v != this.innerValue) {
      this.innerValue = !this.isNull(v) ? String(v) : null;
      this.onChangeCallback(this.innerValue);
    }
  }
  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    // this.getList();
  }

  getList(buId: number = null) {
    this.http.post(`Common/Organization/GetCallCenterGroupNodes`, {
      isSelf: this.isSelf,
      buID: buId
    }, {}).subscribe((resp: any) => {
      this.items = resp.items;
      this.fetched = true;
      this.event && this.event();
      this.defaultFirst && this.defaultSelectFirst();
    });
  }



  onChange($event) {
    const value = this.items.find(x => x.id == $event);
    (value == null) ? (this.nodeKey == null) : (this.nodeKey = value.extend.NodeKey);
    this.onSelectedChange.emit(value ? value.extend : null);

  }


  getItem(id: any) { return !!(this.select) ? this.select.getItem(id) : null; }

  defaultSelectFirst(){
    let data = this.items.length > 0 ? this.items[0] : null;
    data && (this.value = data.id);
  }

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
