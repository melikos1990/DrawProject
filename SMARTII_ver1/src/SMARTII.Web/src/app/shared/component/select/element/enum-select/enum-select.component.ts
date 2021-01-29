import { Component, OnInit, Input, forwardRef, Injector, Output, EventEmitter, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { OpenSelectComponent } from '../../atom/open-select/open-select.component';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => EnumSelectComponent),
  multi: true
};

@Component({
  selector: 'app-enum-select',
  templateUrl: './enum-select.component.html',
  styleUrls: ['./enum-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class EnumSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor, OnChanges {

  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;
  @Input() enumName: string;
  @Input() filter: (data: any) => boolean;
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();

  @ViewChild("select") select: OpenSelectComponent;


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
      this.innerValue = !this.isNull(v) ? v : null;
      console.log(this.innerValue);
      this.onChangeCallback(this.innerValue);
    }
  }

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.send();
  }

  ngOnChanges(simple: SimpleChanges){
    if(simple["enumName"] && simple["enumName"].currentValue){
      // debugger
      this.send((resp: any) => this.items = this.filter != null ? resp.items.filter(this.filter) : resp.items );
    }
  }

  onChange($event) {
    this.onSelectedChange.emit($event ? $event : null);
  }

  writeValue(obj: any): void {
    console.log(obj);
    if (this.fetched) {
      if (obj != this.innerValue) {
        this.innerValue = !this.isNull(obj) ? obj : null;
        console.log(this.innerValue);
      }
    } else {
      this.event = () => {
        if (obj != this.innerValue) {
          this.innerValue = !this.isNull(obj) ? obj : null;
          console.log(this.innerValue);
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
    this.disabled = isDisabled;
  }

  isNull(obj) {
    return obj == null || obj === undefined;
  }

  send(cb?: (resp) => void){

    let _cb = cb ? cb : ((resp) => {
      this.items = this.filter != null ? resp.items.filter(this.filter) : resp.items;
      this.fetched = true;
      this.event && this.event();
    }).bind(this)

    this.http.post("Common/Organization/GetEnumType", { enumName: this.enumName }, {}).subscribe(_cb);

  }

}

