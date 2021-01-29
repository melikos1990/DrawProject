import { Component, OnInit, Input, forwardRef, Injector, SimpleChanges } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CaseFinishedReasonSelectComponent),
  multi: true
};

@Component({
  selector: 'app-case-finished-reason-select',
  templateUrl: './case-finished-reason-select.component.html',
  styleUrls: ['./case-finished-reason-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class CaseFinishedReasonSelectComponent extends BaseComponent implements OnInit {


  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;

  @Input() nodeID: number;

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

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["nodeID"] && changes["nodeID"].currentValue) {
      this.getList();
    }
  }

  getList() {
    this.http.get("Common/Master/GetFinishReasonClassification", { nodeID: this.nodeID }).subscribe((resp: any) => {
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
    this.disabled = isDisabled;
  }
}
