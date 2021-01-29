import * as $ from 'jquery';
import 'bootstrap-daterangepicker';
import { Directive, ElementRef, OnInit, AfterViewInit, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import * as moment from 'moment';
import { Subscription, fromEvent } from 'rxjs';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DateTimepickerDirective),
  multi: true
};

@Directive({
  selector: '[datetimepicker]',
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class DateTimepickerDirective implements OnInit, AfterViewInit, ControlValueAccessor {

  private readonly defaultOpts = {
    singleDatePicker: true,
    autoUpdateInput: false,
    showDropdowns: false,
    timePicker24Hour: true,
    timePicker: true,
    locale: {
      format: 'YYYY/MM/DD HH:mm:ss',
      cancelLabel: '清除',
      applyLabel: '確定'
    },
  };

  currnetOpt: any = {};

  @Input() options: any = {};

  public isDisabled: boolean = false;
  public innerValue = null;

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    console.log("datePic => ", v);
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.backfillData(this.innerValue);
      this.onChangeCallback(this.innerValue);
    }
  }


  private onTouchedCallback: () => void = () => { };
  private onChangeCallback: (_: any) => void = (_: any) => { };
  private onFocusoutSubscription: Subscription;

  ngOnInit() {

  }
  constructor(private el: ElementRef) {

  }

  ngOnDestroy() {
    this.onFocusoutSubscription && this.onFocusoutSubscription.unsubscribe();
  }

  ngAfterViewInit() {

    this.currnetOpt = { ...this.defaultOpts, ...this.options };

    if (this.value) {
      const datetime = moment(this.value).format(this.currnetOpt.locale.format);
      $(this.el.nativeElement).val(datetime);
    }


    $(this.el.nativeElement).daterangepicker(this.currnetOpt, this.datePickerInit.bind(this));

    this.listener();

    this.onFocusoutSubscription = fromEvent(this.el.nativeElement, 'focusout').subscribe(x => this.onTouchedCallback());

  }

  writeValue(obj: any): void {
    this.value = obj;
    
    this.backfillData(this.value);
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

  private backfillData(date: string | Date) {

    const options = { ...this.defaultOpts, ...this.options };

    if (!date) return;

    const datetime = moment(date).format(options.locale.format);

    if (this.isInvalid(datetime)) {
      $(this.el.nativeElement).val(<string>date);
    } else {
      $(this.el.nativeElement).val(datetime);
      $(this.el.nativeElement).daterangepicker({...this.currnetOpt, setDate: datetime}, this.datePickerInit.bind(this));
      this.listener();
    }


  }

  isInvalid = (obj) => obj === 'Invalid date';

  resetValue(){
    $(this.el.nativeElement).val(null)
  }

  datePickerInit(start, end, label){
    const value = start.format(this.currnetOpt.locale.format);
    this.value = value;
    $(this.el.nativeElement).val(value);
  }


  listener(){
    
    $(this.el.nativeElement).on('apply.daterangepicker', function (ev, picker) {
      const value = picker.startDate.format(this.currnetOpt.locale.format);
      this.value = value;
      $(this.el.nativeElement).val(value);
    }.bind(this));


    $(this.el.nativeElement).on('cancel.daterangepicker', function (ev, picker) {
      this.value = null;
      $(this.el.nativeElement).val(null);
    }.bind(this));

    
    $(this.el.nativeElement).on('change', function (ev, picker) {
      if (!this.el.nativeElement.value) {
        this.value = null;
        this.resetValue();
      }
    }.bind(this));

  }

}
