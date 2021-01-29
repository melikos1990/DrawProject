import { Component, OnInit, Injector, Input, forwardRef, SimpleChanges } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => StoreTypeSelectComponent),
  multi: true
};

@Component({
  selector: 'app-store-type-select',
  templateUrl: './store-type-select.component.html',
  styleUrls: ['./store-type-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class StoreTypeSelectComponent extends BaseComponent implements OnInit {
  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;

  @Input() nodeKey: string;

  private fetched: boolean = false;
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: [] = [];

  public event: () => void;

  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
    }
  }
  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["nodeKey"] && changes["nodeKey"].currentValue) {
      this.getList();
    }
  }

  getList() {
    this.http.post("Common/System/GetStoreType", { nodeKey: this.nodeKey }, {}).subscribe((resp: any) => {
      this.items = resp.items;
      this.fetched = true;
      this.event && this.event();
    });
  }

  ngOnInit() {

  }
  writeValue(obj: any): void {

    if (this.fetched) {
      if (obj !== this.innerValue) {
        this.innerValue = obj;
      }
    } else {
      this.event = () => {
        if (obj !== this.innerValue) {
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

