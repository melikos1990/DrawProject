
import 'bootstrap-fileinput';
import { Directive, ElementRef, AfterViewInit, OnInit, forwardRef, Input, NgZone, Injector } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';

declare const $: JQueryStatic;

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => FileInputDirective),
  multi: true
};


@Directive({
  selector: '[fileInput]',
  providers: [INPUT_CONTROL_VALUE_ACCESSOR],

})
export class FileInputDirective extends FormBaseComponent implements AfterViewInit, OnInit, ControlValueAccessor {


  defaultOpts = {
    preferIconicPreview: true,
    initialPreviewAsData: true,
    overwriteInitial: false,
    showRemove: true,
    showUpload: false,
    fileActionSettings: {
      showRemove: true,
      showUpload: false,
      showClose: false,
      uploadAsync: false,
    },
    layoutTemplates: {
      actionDownload:
        '<a class="{downloadClass}" title="{downloadTitle}" data-caption="{caption}" href=\'{downloadUrl}\' download>{downloadIcon}</a> '
    }
  };



  // public isDisabled: boolean = false;
  public innerValue: any = null;
  public innerOpts: any = null;


  @Input()
  get options(): any {
    return this.innerOpts;
  }
  set options(v: any) {
    this.innerOpts = v;
    this.loadPage();
  }

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    this.innerValue = v;
    this.onChangeCallback(this.innerValue);

  }

  private onTouchedCallback: () => void = () => { };
  private onChangeCallback: (_: any) => void = (_: any) => { };

  ngOnInit() {

  }

  constructor(private el: ElementRef, private store: Store<fromCaseReducer>, public injector: Injector) { super(injector); }


  ngAfterViewInit() {


  }


  loadPage() {




    $(this.el.nativeElement).fileinput('destroy');
    $(this.el.nativeElement).val(null);
    let setting = { ...this.defaultOpts, ...this.options };

    if (this.el.nativeElement && this.el.nativeElement.multiple) {
      setting = {
        ...setting,
        uploadUrl: !!(this.options["uploadUrl"]) == false ? "multiple" : this.options["uploadUrl"]
      }
    }


    this.calcOpts(setting);
    $(this.el.nativeElement).fileinput(setting);
    $(this.el.nativeElement).on('change', function ($event) {
      this.value = this.getImages();
    }.bind(this));

    // 拖曳檔案事件
    $(this.el.nativeElement).on('filebatchselected', function ($event, files) {
      this.value = this.el.nativeElement.multiple ? this.getMultipleFiles(files) : this.getImages();
    }.bind(this));

    //確認刪除
    $(this.el.nativeElement).fileinput().on(
      'filebeforedelete', function () {
        return new Promise(function (resolve, reject) {
          return this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
            () => {

              return resolve();
            }
          )))
        }.bind(this))
      }.bind(this)
    );

    $(this.el.nativeElement).on('fileremoved', function (event, id, index) {
      this.value = $(this.el.nativeElement).fileinput('getFileList');
    }.bind(this));

    $(this.el.nativeElement).on('filecleared', function (event) {
      this.value = this.getImages();
    }.bind(this));

    $('button[type="button"].kv-file-zoom').prop('disabled', false);


  }
  getImages() {
    const result = [];
    const element = $(this.el.nativeElement).get(0);

    if (!element) {
      return result;
    }
    for (let i = 0; i < element.files.length; i++) {
      result.push(element.files[i]);
    }
    return result;
  }


  getMultipleFiles(files: Object) {
    const result = [];

    let keys = Object.keys(files);

    keys.forEach(key => {
      let file = files[key].file;
      result.push(file);
    })

    return result;
  }

  writeValue(obj: any): void {
    this.innerValue = obj;
  }
  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    // this.isDisabled = isDisabled;
  }

  calcOpts(setting) {

    if (setting.preferIconicPreview && setting.preferIconicPreview === true) {
      setting.previewFileIconSettings = { // configure your icon file extensions
        'doc': '<i class="fas fa-file-word text-primary"></i>',
        'xls': '<i class="fas fa-file-excel text-success"></i>',
        'ppt': '<i class="fas fa-file-powerpoint text-danger"></i>',
        'pdf': '<i class="fas fa-file-pdf text-danger"></i>',
        'zip': '<i class="fas fa-file-archive text-muted"></i>',
        'htm': '<i class="fas fa-file-code text-info"></i>',
        'txt': '<i class="fas fa-file-alt text-info"></i>',
        'mov': '<i class="fas fa-file-video text-warning"></i>',
        'mp3': '<i class="fas fa-file-audio text-warning"></i>',
        // note for these file types below no extension determination logic
        // has been configured (the keys itself will be used as extensions)
        'jpg': '<i class="fas fa-file-image text-danger"></i>',
        'gif': '<i class="fas fa-file-image text-muted"></i>',
        'png': '<i class="fas fa-file-image text-primary"></i>'
      },
        setting.previewFileExtSettings = { // configure the logic for determining icon file extensions
          'doc': function (ext) {
            return ext.match(/(doc|docx)$/i);
          },
          'xls': function (ext) {
            return ext.match(/(xls|xlsx)$/i);
          },
          'ppt': function (ext) {
            return ext.match(/(ppt|pptx)$/i);
          },
          'zip': function (ext) {
            return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
          },
          'htm': function (ext) {
            return ext.match(/(htm|html)$/i);
          },
          'txt': function (ext) {
            return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
          },
          'mov': function (ext) {
            return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
          },
          'mp3': function (ext) {
            return ext.match(/(mp3|wav)$/i);
          }
        }
    }

  }

}
