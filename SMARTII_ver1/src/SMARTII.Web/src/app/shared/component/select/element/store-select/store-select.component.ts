import { Component, OnInit, forwardRef, Injector, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { SelectDataItem, ChangeInfo, PtcSelect2AjaxOptions } from 'ptc-select2';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Guid } from 'guid-typescript';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => StoreSelectComponent),
  multi: true
};

@Component({
  selector: 'app-store-select',
  templateUrl: './store-select.component.html',
  styleUrls: ['./store-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class StoreSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {

  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() isEnable?: boolean;
  @Input() isSystemUser?: boolean;
  @Input() isSelf: boolean;
  @Input() userID: string;
  @Input() disabled: boolean = false;
  @Input() hasClearOption: boolean = true;

  @Input() selectedItems: SelectDataItem[] = [];

  @Output() itemChange: EventEmitter<ChangeInfo> = new EventEmitter();

  public innerValue;
  public innerSearchTerm: {
    isEnable?: boolean;
    isSystemUser?: boolean;
    isSelf: boolean;
    userID: string;

  };
  public isDisabled: boolean = false;

  public options: PtcSelect2AjaxOptions<any> = {
    pageIndex: 0,
    size: 20,
    url: "Common/Organization/GetStores".toHostApiUrl(),
    method: "POST"
  };

  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v != this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
    }
  }

  @Input()
  get searchTerm(): {
    isEnable?: boolean;
    isSystemUser?: boolean;
    isSelf: boolean;
    userID: string;
  } {
    return this.innerSearchTerm;
  }

  set searchTerm(v: {
    isEnable?: boolean;
    isSystemUser?: boolean;
    isSelf: boolean;
    userID: string;
  }) {
    this.innerSearchTerm = v;
  }

  constructor(
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

    this.options.criteria = this.searchTerm;
  }

  onItemChange = ($event) => this.itemChange.emit($event);

  writeValue(obj: any): void {

    if (obj != this.innerValue) {
      this.innerValue = obj;
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

}
