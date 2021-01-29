import { Component, OnInit, Input, forwardRef, Injector } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';



export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => WorkProcessSelectComponent),
  multi: true
};

@Component({
  selector: 'app-work-process-select',
  templateUrl: './work-process-select.component.html',
  styleUrls: ['./work-process-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class WorkProcessSelectComponent extends BaseComponent implements OnInit {

  @Input() disabled : boolean = false;
  @Input() id: string = '';
  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';

  public innerValue: [] ;
  public isDisabled: boolean = false;
  public items: any[] = [
    {id: 0, text: "負責人模式"},
    {id: 1, text: "共通處理模式"}
  ];

  private fetched: boolean = false;
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
    // this.http.post("Common/Organization/GetAppRoles", null, {}).subscribe((resp: any) => {
    //   this.items = resp.items;
    //   this.fetched = true;
    //   this.event && this.event();
    // });
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
