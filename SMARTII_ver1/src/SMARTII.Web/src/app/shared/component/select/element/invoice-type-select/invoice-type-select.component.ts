import { Component, OnInit, Injector, forwardRef, Input, OnChanges, SimpleChanges } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseService } from 'src/app/shared/service/case.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { BusinesssUnitParameters } from 'src/app/model/organization.model';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => InvoiceTypeSelectComponent),
  multi: true
};


@Component({
  selector: 'app-invoice-type-select',
  templateUrl: './invoice-type-select.component.html',
  styleUrls: ['./invoice-type-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class InvoiceTypeSelectComponent extends BaseComponent implements OnInit, OnChanges {


  @Input() placeholder: string = '';
  @Input() buID: number;


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
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
    }
  }

  constructor(
    public injector: Injector,
    public caseService: CaseService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getInvoiceType((resp) => {
      this.items = resp.CaseComplaintInvoiceTypes;
      this.fetched = true;
      this.event && this.event();
    })
  }


  ngOnChanges(simples: SimpleChanges) {
    if (simples["buID"] && simples["buID"].currentValue) {
      this.getInvoiceType((resp) => this.items = resp.CaseComplaintInvoiceTypes)
    }
  }


  getInvoiceType(cb: (resp: BusinesssUnitParameters) => void) {
    if (!this.buID) {
      return;
    }
    this.caseService.getBUParameters(this.buID)
      .subscribe(cb)
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
