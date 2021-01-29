import { Component, OnInit, Injector, Input, OnChanges, SimpleChanges, ViewChild, forwardRef, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { SelectDataItem } from 'ptc-select2';
import { OpenSelectComponent } from '../../atom/open-select/open-select.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { CaseSourceType } from 'src/app/model/case.model';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CaseSourceSelectComponent),
  multi: true
};

@Component({
  selector: 'app-case-source-select',
  templateUrl: './case-source-select.component.html',
  styleUrls: ['./case-source-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class CaseSourceSelectComponent extends BaseComponent implements OnInit, OnChanges {


  @Input() id: string = '';
  @Input() buID: number;
  @Input() disabled: boolean;
  @Input() placeholder: string = '';

  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();

  @ViewChild("select") select: OpenSelectComponent;

  items: SelectDataItem[] = [];
  private fetched: boolean = false;
  public innerValue: any;

  onChangeCallback: (_: any) => void = (_: any) => { };

  event: () => void;

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
    public injector: Injector,
    public http: HttpService
  ) {
    super(injector);
  }

  ngOnInit() {

    if (!this.buID) { return; }

    this.getSourceType().subscribe((res: any) => {
      this.items = res;
      this.fetched = true;
      this.event && this.event();
    });

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["buID"] && changes["buID"].currentValue) {
      this.value = null;
      this.getSourceType().subscribe((res: any) => this.items = res);
    }
  }

  onChange($event) { this.onSelectedChange.emit($event); }




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

  }
  setDisabledState?(isDisabled: boolean): void {

  }

  private getSourceType() {
    return this.http.get("Case/Case/GetCaseSourceType", { buID: this.buID });
  }

  getList(buId: number = null) {
    this.buID = buId;
    this.getSourceType().subscribe((res: any) => {
      this.items = res;
      this.fetched = true;
      this.event && this.event();
      this.defaultSelectFirst();
    });
  }

  /**
 * 預設預選
 */
  defaultSelectFirst() {
    let data = this.items.length > 0 ? this.items[0] : null;
    data && (this.value = CaseSourceType.Phone);
  }

  isNull(obj) {
    return obj == null || obj === undefined;
  }

}
