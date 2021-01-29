
import { Component, OnInit, Input, forwardRef, Output, EventEmitter, ViewChild, Injector } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { PtcSelect2AjaxOptions, ChangeInfo, ChangeType, SelectDataItem, PtcSelect2Component, StateInfo, StateType, PtcSelect2Service } from 'ptc-select2';
import { QuestionClassificationSearchViewModel, QuestionSelectInfo } from 'src/app/model/question-category.model';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ItemSearchViewModel } from 'src/app/model/master.model';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => ItemSelectComponent),
  multi: true
};

@Component({
  selector: 'app-item-select',
  templateUrl: './item-select.component.html',
  styleUrls: ['./item-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class ItemSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {


  @Input() buID: any;
  @Input() selectedItems: SelectDataItem[] = [];

  @Input() disabled: boolean = false;

  @Input() items: any[] = [];
  @Input() id: string = 'id';
  @Input() placeholder: string;

  @Output() onItemChange: EventEmitter<QuestionSelectInfo> = new EventEmitter();
  @Output() ajaxError: EventEmitter<Error> = new EventEmitter();

  options: PtcSelect2AjaxOptions<any> = {
    url: "Common/Master/GetItemList".toHostApiUrl(),
    method: "POST",
    pageIndex: 0,
    size: 10
  }

  @ViewChild(PtcSelect2Component) ptcSelect: PtcSelect2Component;

  private _innerValue: any;
  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  set value(val: any) {
    if (val) {
      this._innerValue = val;
      this.onChangeCallback(this._innerValue);

    }
  }
  get value() { return this._innerValue; }


  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.getList();
  }

  getList(nodeID: number = null){
    let model = new ItemSearchViewModel();
    model.NodeID = nodeID !==null ?nodeID :this.buID;
    this.options.criteria = model;
    this.ptcSelect.stateChange$.subscribe((state: StateInfo) => {
      if (state.state == StateType.Error) {
        this.ajaxError.emit(state.error);
      }
    })
  }

  writeValue(obj: any): void {
    if (obj) {
      this._innerValue = obj;
    }
  }
  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
  }


}



