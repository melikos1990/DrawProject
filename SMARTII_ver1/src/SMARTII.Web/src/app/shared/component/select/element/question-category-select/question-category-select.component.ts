import { Component, OnInit, Input, forwardRef, Output, EventEmitter, ViewChild, Injector, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { PtcSelect2AjaxOptions, ChangeInfo, ChangeType, SelectDataItem, PtcSelect2Component, StateInfo, StateType, PtcSelect2Service } from 'ptc-select2';
import { QuestionClassificationSearchViewModel, QuestionSelectInfo } from 'src/app/model/question-category.model';
import { BaseComponent } from 'src/app/pages/base/base.component';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => QuestionCategorySelectComponent),
  multi: true
};
@Component({
  selector: 'app-question-category-select',
  templateUrl: './question-category-select.component.html',
  styleUrls: ['./question-category-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class QuestionCategorySelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {

  @Input() level: number;
  @Input() buID: any;
  @Input() parentID: number;
  @Input() filterID: number;
  @Input() groupName: string;
  @Input() selectedItems: SelectDataItem[] = [];
  @Input() hasClearOption: boolean = true;
  @Input() disabled: boolean = false;
  @Input() isEnabled?: boolean = null;
  @Input() items: any[] = [];
  @Input() id: string = 'id';
  @Input() placeholder: string;

  @Output() onItemChange: EventEmitter<QuestionSelectInfo> = new EventEmitter();
  @Output() ajaxError: EventEmitter<Error> = new EventEmitter();

  options: PtcSelect2AjaxOptions<any> = {
    url: "Common/Master/GetQuestionClassificationList".toHostApiUrl(),
    method: "POST",
    pageIndex: 0,
    size: 10
  }

  @ViewChild(PtcSelect2Component) ptcSelect: PtcSelect2Component;

  private _model: any;
  private _change: (model: any) => void;

  set model(val: any) { if (val) this._model = val; }
  get model() { return this._model; }

  onChange = (model: any) => this._change(model);

  constructor(public injector: Injector, public changeDetectorRef: ChangeDetectorRef) {
    super(injector)
  }


  ngOnChanges(changes: SimpleChanges): void {
    if (changes["buID"] && changes["buID"].currentValue) {
      this.getList();
    }
  }

  getList() {

    this.options.criteria = new QuestionClassificationSearchViewModel(
      this.level,
      this.buID,
      this.filterID,
      this.isEnabled
    );

    this.ptcSelect.stateChange$.subscribe((state: StateInfo) => {
      // debugger;
      console.log("stateChange$ =>", state);
      this.changeDetectorRef.markForCheck();
      if (state.state == StateType.Error) {
        this.ajaxError.emit(state.error);
      }

    })

  }

  ngOnInit() {
    this.getList();
  }

  /**
   * item 異動事件
   */
  itemChange(event: ChangeInfo) {

    let { type, item } = event;
    if (type != ChangeType.Selected) return;

    let data: QuestionSelectInfo = {
      Name: item.text,
      ID: item.id,
      Level: this.level
    }
    this.onItemChange.emit(data);
   
  }

  writeValue(obj: any): void {

    if (obj) {
      this.model = obj;
    }

  }
  registerOnChange(fn: any): void { 
    this._change = fn;
  }
  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {
  }

  /**
   * 改變外部 ngModel
   */
  triggerModelChange() {
    setTimeout(() => {
      this.onChange(this.model);
    }, 0)
  }


}
