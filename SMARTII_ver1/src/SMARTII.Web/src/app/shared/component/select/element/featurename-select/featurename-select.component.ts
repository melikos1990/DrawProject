import { Component, OnInit, Injector, forwardRef, Input } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { PtcSelect2AjaxOptions } from 'ptc-select2';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => FeaturenameSelectComponent),
  multi: true
};

@Component({
  selector: 'app-featurename-select',
  templateUrl: './featurename-select.component.html',
  styleUrls: ['./featurename-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class FeaturenameSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {


  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean = false;
  @Input() hasClearOption: boolean = true;

  public innerValue;

  public isDisabled: boolean = false;

  public options: PtcSelect2AjaxOptions<{}> = {
    url: "Common/Misc/GetAppFeatures".toHostApiUrl(),
    method: "POST",
    pageIndex: 0,
    size: 20

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
  constructor(
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }


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
