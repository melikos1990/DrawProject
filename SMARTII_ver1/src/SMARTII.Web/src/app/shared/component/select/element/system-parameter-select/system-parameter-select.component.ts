import { Component, OnInit, Input, Injector, OnChanges, SimpleChanges, forwardRef } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => SystemParameterSelectComponent),
  multi: true
};

@Component({
  selector: 'app-system-parameter-select',
  templateUrl: './system-parameter-select.component.html',
  styleUrls: ['./system-parameter-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class SystemParameterSelectComponent extends BaseComponent implements OnInit, OnChanges {

  @Input() id: string;
  @Input() placeholder: string = '';
  @Input() disabled: boolean;
  @Input() buID: number;
  @Input() parameterType: string;

  private fetched: boolean = false;
  private url: string = "Common/System/GetSystemParameter";

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
    private http: HttpService,
    public injector: Injector
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["buID"] && changes["buID"].currentValue && this.parameterType)
      this.getSystemParameter();
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

  private getSystemParameter() {
    this.http.get<any>(this.url, { BuID: this.buID, ParameterType: this.parameterType })
      .subscribe(data => {
        this.items = data;
      })
  }

}
