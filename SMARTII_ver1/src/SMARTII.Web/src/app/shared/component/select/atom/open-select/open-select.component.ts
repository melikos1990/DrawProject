import { Component, OnInit, Injector, Input, forwardRef, Output, EventEmitter, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';



export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => OpenSelectComponent),
  multi: true
};

@Component({
  selector: 'app-open-select',
  templateUrl: './open-select.component.html',
  styleUrls: ['./open-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class OpenSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {

  @Input() id: string = '';
  @Input() mutiple: boolean = false;

  @Input() placeholder: string = '';

  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter;
  

  private _items: any[];

  @Input() get items(): any[] {
    return this._items;
  }
  set items(v: any[]) {
    this._items = v;
    this.value = this.innerValue;
  }

  @Input() get selectedValue(): any{
    return this.selectValue;
  }

  set selectedValue(v: any){
    this.selectValue = v;
    this.value = v;
    this.writeValue(this.selectValue);
  }

  @ViewChild('select') select: any;


  @Output() onItemChange: EventEmitter<any> = new EventEmitter();

  @Input() disabled: boolean = false;

  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any | any[] {
    return this.innerValue;
  }

  set value(v: any | any[]) {
    if (v !== this.innerValue) {
      this.innerValue = v;

      if (!this.mutiple) {
        this.selectValue = String(v);
      } else {
        this.selectValue = v;
      }
      this.onChangeCallback(this.innerValue);
    }
  }

  constructor(public injector: Injector) {
    super(injector);
  }
  public innerItems: any[] = [];
  public innerValue: any | any[];
  public selectValue: any | any[];


  ngOnInit() {
  }
  
  onChange($event) {
    this.onSelectedChange.emit($event);
  }

  getItem(id: any) { return this.items.find(x => x.id == id); }

  writeValue(obj: any): void {
    if (obj !== this.innerValue) {
      this.innerValue = obj;
      setTimeout(() => {
        if (!this.mutiple) {
          this.selectValue = String(obj);
        } else {
          if (obj) {
            this.selectValue = (<any[]>obj).map(x => x.toString());
          }
        }
      }, 0);
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

 
}
