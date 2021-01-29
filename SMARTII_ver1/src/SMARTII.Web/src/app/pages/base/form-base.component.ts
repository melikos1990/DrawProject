import { Component, Injector, Optional, Inject } from '@angular/core';
import { FormGroup, FormArray } from '@angular/forms';
import { AuthBaseComponent } from '../base/auth-base.component';
import { PtcSwalType } from 'ptc-swal';
import { PREFIX_TOKEN } from 'src/app/shared/injection-token';
import * as moment from 'moment'
import { takeUntil } from 'rxjs/operators';
import { defaultCacheKeys } from 'src/app/shared/decorator/searchCache.decorator';


@Component({
  selector: 'app-form-base',
  template: ''
})
export class FormBaseComponent extends AuthBaseComponent {


  constructor(
    public injector: Injector,
    @Optional() @Inject(PREFIX_TOKEN) public prefix?: string) {
    super(injector, prefix);

  }


  protected getLoopQuestionMessage(title: string, confirm?: () => void, cancel?: () => void) {
    return {

      detail: {
        title: title,
        type: PtcSwalType.question,
        showCancelButton: true,
        confirmButtonText: this.translateService.instant('COMMON.BTN_CHECK'),
        cancelButtonText: this.translateService.instant('COMMON.BTN_CANCEL')
      },
      isLoop: false,
      confirm: confirm, //確認後 CALLBACK
      cancel: cancel    //取消後 CALLBACK
    }
  }

  protected getFieldInvalidMessage(remark?: string, showCancel: boolean = true, confirm?: () => void, cancel?: () => void, errorType?: PtcSwalType) {
    return {
      detail: {
        title: this.translateService.instant('ERROR.TITLE'),
        text: `${this.translateService.instant('ERROR.FIELD_VALID_ERROR')} ${(remark ? `: ${remark}` : '')}`,
        type: errorType ? errorType : PtcSwalType.error,
        showCancelButton: showCancel,
        confirmButtonText: this.translateService.instant('COMMON.BTN_CHECK'),
        cancelButtonText: showCancel ? this.translateService.instant('COMMON.BTN_CANCEL') : null
      },
      isLoop: false,
      confirm: confirm, //確認後 CALLBACK
      cancel: cancel    //取消後 CALLBACK
    };

  }

  protected markNestedForm(form) {
    form.updateValueAndValidity();
    for (let control in form.controls) {
      let controlItem = form.controls[control]
      if (
        controlItem instanceof FormGroup ||
        controlItem instanceof FormArray) {
        this.markNestedForm(controlItem);
      }
      form.controls[control].markAsDirty();
      form.controls[control].markAsTouched();
    }

  }

  protected validForm(form: FormGroup) {
    form.updateValueAndValidity();
    const invalid: boolean = form.invalid;
    if (invalid) {
      this.markNestedForm(form);
    }
    return !invalid;

  }

  protected defaultDateTimeRange(format: string = "YYYY/MM/DD HH:mm:ss") {
    return moment().format(format) + ' - ' + '2099/12/31 23:59:59';
  }

  protected defaultDateTime(format: string = "YYYY/MM/DD HH:mm:ss") {
    return moment().format(format);
  }

  protected dateTimeToString(date: Date, format = "YYYY/MM/DD HH:mm:ss") {
    return moment(date).format(format);
  }


  protected appendOperator(columns: any[]) {

    const operator = {
      text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
      name: 'p-operator',
    };

    columns.includes(operator);

    let newColumns = columns.some(function name(params) {
      return params.name === operator.name
    }) ? columns : columns.concat(operator);

    return newColumns;
  }

  protected fillbackCache(table: any, cd: () => {} = null) {

    //撈取查詢紀錄
    let { model, ajax } = this.getCache<{ model, ajax }>(Object.keys(defaultCacheKeys));

    console.log("fillbackCache => ", model, ajax)
    //若有查詢紀錄，帶回並查詢
    if (!!model && !!ajax) {

      console.log(table);
      table.opts.pageIndex = ajax.body.pageIndex;
      table.opts.pageSize = ajax.body.pageSize;

      cd && cd();

      table.ajaxSuccess
        .pipe(takeUntil(this.destroy$))
        .subscribe(data => {
          //回歸預設值
          this.clearCache();
          table.opts.pageIndex = table.defaultPageIndex;
          table.opts.pageSize = table.defaultPageSize;
        })

      return model;
    }
    else {
      this.clearCache();
      return {};
    }
  }

  protected fillbackCacheModel(table: any, cd: () => {} = null) {
    //撈取查詢紀錄
    let { model } = this.getCache<{ model }>(Object.keys(defaultCacheKeys));

    console.log("fillbackCache => ", model)
    //若有查詢紀錄，帶回並查詢
    if (!!model) {

      cd && cd();

      //回歸預設值
      this.clearCache();
      return model;
    }
    else {
      this.clearCache();
      return {};
    }
  }

}
