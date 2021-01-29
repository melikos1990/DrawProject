import { Component, OnInit, Input, OnChanges, SimpleChanges, Injector, forwardRef } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CaseWarningSelectComponent),
  multi: true
};

@Component({
  selector: 'app-case-warning-select',
  templateUrl: './case-warning-select.component.html',
  styleUrls: ['./case-warning-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class CaseWarningSelectComponent extends BaseComponent implements OnInit, OnChanges {


  @Input() placeholder: string = '';
  @Input() disabled: boolean;
  @Input() buID: number;
  @Input() enabled?: boolean;
  @Input() defaultFirst: boolean = false;

  private fetched: boolean = false;
  private url: string = "Common/Master/GetCaseWarning";

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
    private http: HttpService,
    public injector: Injector
  ) { 
    super(injector);
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["buID"] && changes["buID"].currentValue) {
      this.value = null;
      this.getCaseWarning();
    }
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

  defaultSelectFirst() {
    let data = this.items.length > 0 ? this.items[0] : null;
    data && (this.value = data.id);
  }

  private getCaseWarning() {
    this.http.get<any>(this.url, { BuID: this.buID , Enabled: this.enabled })
      .subscribe(data => {
        this.items = data;
        this.fetched = true;
        this.event && this.event();
        this.defaultFirst && this.defaultSelectFirst();
      })
  }

  isNull(obj) {
    return obj == null || obj === undefined;
  }
}
