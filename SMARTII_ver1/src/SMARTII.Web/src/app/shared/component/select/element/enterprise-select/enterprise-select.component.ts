import { Component, OnInit, Input, Injector, forwardRef } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => EnterpriseSelectComponent),
  multi: true
};

@Component({
  selector: 'app-enterprise-select',
  templateUrl: './enterprise-select.component.html',
  styleUrls: ['./enterprise-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class EnterpriseSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {

  @Input() id: string = '';
  @Input() multiple: boolean = true;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean = null;

  public event: () => void;
  private fetched: boolean = false;
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: [] = [];


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

  ngOnInit() {
    this.http.post("Common/Organization/GetEnterprises", null, {}).subscribe((resp: any) => {
      this.items = resp.items;
      this.fetched = true;
      this.event && this.event();
    });
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
