import * as $ from 'jquery';
import 'bootstrap-daterangepicker';

import { Directive, ElementRef, OnInit, AfterViewInit, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { fromEvent, Subscription } from 'rxjs';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DaterangepickerDirective),
  multi: true
};

@Directive({
  selector: '[daterangepicker]',
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class DaterangepickerDirective implements OnInit, AfterViewInit, ControlValueAccessor {


  private readonly defaultOpts = {
    timePicker24Hour: true,
    timePickerSeconds: true,
    timePicker: true,
    locale: {
      format: 'YYYY/MM/DD HH:mm:ss',
      cancelLabel: '清除',
      applyLabel: '確定'
    },
    autoUpdateInput: false,
  };

  @Input()
  options: any = {};

  public isDisabled: boolean = false;
  public innerValue = null;

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.backfillData(v);
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
    this.resetValue();

    const options = { ...this.defaultOpts, ...this.options };

    $(this.el.nativeElement).daterangepicker(options);

    $(this.el.nativeElement).on('apply.daterangepicker', function (ev, picker) {
      const value = picker.startDate.format(options.locale.format) + ' - ' + picker.endDate.format(options.locale.format);
      this.value = value;
      console.log(value);
      $(this.el.nativeElement).val(value);
    }.bind(this));

    $(this.el.nativeElement).on('cancel.daterangepicker', function (ev, picker) {
      this.value = null;
      ev.target.value = null;
      $(this.el.nativeElement).val(null);
    }.bind(this));

    $(this.el.nativeElement).on('change', function (ev, picker) {
      if (!this.el.nativeElement.value) {
        this.value = null;
        this.resetValue();
      }

    }.bind(this));

    // $(this.el.nativeElement).on('focus', function (ev, picker) {
    //   let resetOption = {};
    //   $(this.el.nativeElement).daterangepicker(resetOption);
    // }.bind(this));

    this.onFocusoutSubscription = fromEvent(this.el.nativeElement, 'focusout').subscribe(x => this.onTouchedCallback());

  }

  writeValue(obj: any): void {
    this.value = obj;
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

  private backfillData(dateStr: string) {

    // if(!dateStr) return;

    setTimeout(() => {
      $(this.el.nativeElement).val(dateStr);
    }, 0)

  }

  resetValue() {
    $(this.el.nativeElement).val(null)
  }

}
