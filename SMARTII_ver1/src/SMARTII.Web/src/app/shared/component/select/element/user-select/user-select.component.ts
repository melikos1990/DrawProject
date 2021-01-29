import { Component, OnInit, forwardRef, Injector, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { PtcSelect2AjaxOptions, ChangeInfo, SelectDataItem, ChangeType, PtcSelect2Component, } from 'ptc-select2';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => UserSelectComponent),
  multi: true
};


@Component({
  selector: 'app-user-select',
  templateUrl: './user-select.component.html',
  styleUrls: ['./user-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class UserSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {

  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() isEnable?: boolean;
  @Input() isSystemUser?: boolean;
  @Input() isSelf: boolean;
  @Input() userID : string;
  @Input() disabled: boolean = false;
  @Input() hasClearOption: boolean = true;
  @Input() groupName: string;

  @Input() selectedItems: SelectDataItem[] = [];

  @Output() itemChange: EventEmitter<ChangeInfo> = new EventEmitter();

  @ViewChild(PtcSelect2Component) ptcSelect: PtcSelect2Component;

  public innerValue;
  public innerSearchTerm:{
    isEnable?: boolean;
    isSystemUser?: boolean;
    isSelf: boolean;
    userID : string;

  };
  public isDisabled: boolean = false;

  public options: PtcSelect2AjaxOptions<any> = {
    pageIndex : 0,
    size : 50,
    url: "Common/Organization/GetOwnCallCenterNodeUsers".toHostApiUrl(),
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

  private _currentItem: SelectDataItem;
  get currentItem() {
    return this._currentItem ? this._currentItem : null;
  }

  @Input()
  get searchTerm(): {
    isEnable?: boolean;
    isSystemUser?: boolean;
    isSelf: boolean;
    userID : string;
  } {
    return this.innerSearchTerm;
  }

  set searchTerm(v: {
    isEnable?: boolean;
    isSystemUser?: boolean;
    isSelf: boolean;
    userID : string;
  }) {
    this.innerSearchTerm = v;
  }

  constructor(
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

    this.options.criteria = this.searchTerm;}

  onItemChange = ($event: ChangeInfo) => {
    if($event.type == ChangeType.Selected){
      this._currentItem = $event.item;
    }
    else{
      this._currentItem = null;
    }
    this.itemChange.emit($event);
  };

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
