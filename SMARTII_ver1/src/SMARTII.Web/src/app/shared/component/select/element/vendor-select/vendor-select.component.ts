import { Component, OnInit, Input, forwardRef, Injector, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => VendorSelectComponent),
  multi: true
};

@Component({
  selector: 'app-vendor-select',
  templateUrl: './vendor-select.component.html',
  styleUrls: ['./vendor-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class VendorSelectComponent extends BaseComponent implements OnInit {

  @Input() id: string = '';
  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;
  @Input() isSelf = true;
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();
  
  private fetched: boolean = false;
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
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
    }
  }
  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.http.post("Common/Organization/GetVendorRootNodes", {
      isSelf: this.isSelf
    }, {}).subscribe((resp: any) => {
      this.items = resp.items;
      this.fetched = true;
      this.event && this.event();
    });
  }

  onChange($event) {
    const value = this.items.find(x => x.id == $event);
    this.onSelectedChange.emit(value ? value.extend : null);

  }

  writeValue(obj: any): void {

    if (this.fetched) {
      if (obj != this.innerValue) {
        this.innerValue = obj;
      }
    } else {
      this.event = () => {
        if (obj != this.innerValue) {
          this.innerValue = obj;
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

}
