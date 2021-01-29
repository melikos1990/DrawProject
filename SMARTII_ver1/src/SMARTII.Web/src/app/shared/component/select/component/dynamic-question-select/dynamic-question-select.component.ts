import { Component, OnInit, Input, SimpleChanges, Injector, TemplateRef, Output, EventEmitter } from '@angular/core';
import { filter, map } from 'rxjs/operators';

import { PtcSelect2Service, SelectDataItem } from 'ptc-select2';
import { HttpService } from 'src/app/shared/service/http.service';

import { AspnetJsonResult } from 'src/app/model/common.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { QuestionSelectInfo } from 'src/app/model/question-category.model';

@Component({
  selector: 'app-dynamic-question-select',
  templateUrl: './dynamic-question-select.component.html',
  styleUrls: ['./dynamic-question-select.component.scss']
})
export class DynamicQuestionSelectComponent extends FormBaseComponent implements OnInit {

  @Input() buID: any;
  @Input() contentClass: string;
  @Input() groupName: string = null;
  @Input() disabled: boolean = false;
  @Input() colClass: string = "col-sm-4";
  @Input() selectedItems: SelectDataItem[] = [];
  @Input() needValidSelect: number[] = [];
  @Input() validTempRef: TemplateRef<any>;
  @Input() hasClearOption: boolean = true;
  @Input() isStarsign: boolean = false;
  @Input() isEnabled?: boolean = null;
  @Output() onItemChange: EventEmitter<QuestionSelectInfo> = new EventEmitter();

  get selectAll() { return this.ptcSelectService.ptcSelectAll; }
  get selectAllByGroupName() { return this.ptcSelectService.ptcSelectAll.filter(x => x.groupName == this.groupName); }
  get lastHasValue() { return this.ptcSelectService.getLastHasValue(this.groupName); }
  get firstValue() { return this.ptcSelectService.getFirstValue(this.groupName); }
  get lastValue() { return this.ptcSelectService.getLastValue(this.groupName); }

  fetched: boolean = false;

  _maxLevel: any[] = [];

  private event: () => void;

  constructor(
    public injector: Injector,
    private ptcSelectService: PtcSelect2Service,
    private http: HttpService
  ) {
    super(injector);
  }

  ngOnInit() {
  }


  ngOnChanges(changes: SimpleChanges): void {
    
    if (changes["buID"] && changes["buID"].currentValue) {
      this.getMaxLevel();
      this.clearAllSelect();
      this.clearChilrenItems(0); // 清空這個 group Name 所有items
    }
    else if(changes["buID"] && !changes["buID"].currentValue){
      this.clearAllSelect();
    }

  }


  ngOnDestroy(){
    clearTimeout(this.currentTime);
    super.ngOnDestroy();
  }

  getMaxLevel() {
    let payload = { BuID: this.buID };
    this.http.get<AspnetJsonResult<number>>('Master/QuestionClassification/GetQuestionMaxLevel', payload)
      .pipe(
        filter(x => x.isSuccess),
        map(x => x.element)
      )
      .subscribe(maxLevel => {
        this.fetched = true;
        this._maxLevel = maxLevel ? Array(maxLevel).map(x => 1) : [];
        
        this.delay(() => { this.event && this.event(); });
      });
  }


  updateView() {


    this.delay(() => {
      
      if(!this.fetched){
        if (!this.event)
          this.event = this.toCurry(this.selectedItems);
        else {
          this.toCurry(this.selectedItems)();
        }
      }
      else {
        this.toCurry(this.selectedItems)();
      }
    }, 2000)

  }

  /**
   * 回填資料
   * @param datas
   */
  private backfillData(datas: SelectDataItem[]) {
    
    if (!datas || (datas && datas.length <= 0)) return;
    
    let groupSelectAll = this.selectAllByGroupName;

    // 填入資料
    datas.forEach((data, idx, arrar) => {

      if (idx > (groupSelectAll.length - 1)) return;

      groupSelectAll[idx].selectedItems = data;

    });

  }


  clearAllSelect(){

    let allSelect = this.selectAllByGroupName;
    
    if(!allSelect || (allSelect && allSelect.length <= 0)) return;

    allSelect.forEach(select => {

      select._selectModel.clear();
      select["_selected"] = null;

    })

  }

  /**
   * 取得現在的層級 (用值反找component)
   */
  currentLevel() {

    let value = this.lastHasValue;

    if (!value) return 1;

    let groupSelectAll = this.selectAllByGroupName;
    let component = groupSelectAll.filter(x => x.selected == value)[0];

    return component ? component.level + 1 : 1;

  }


  private toCurry = (data?) => this.backfillData.bind(this, data);

  currentTime: any;

  private delay = (cb: Function, time = 0) => {
    this.currentTime = setTimeout(cb.bind(this), time);
  }

  getLastHasValueBySelectedItem() {
    let allSelect = [...this.ptcSelectService.ptcSelectAll];

    let select = allSelect.reverse().find(x => x.selected != null);
    return !!(select) ?
      select.multiple ? select.selectedItems : select.selectedItems[0] :
      null;
  }

  isValidSelect(idx) { return this.needValidSelect.some(x => x == (idx + 1)); }

  change($event) {
    this.onItemChange.emit($event);
    this.clearChilrenItems($event.Level);
  }

  clearChilrenItems(level: number){
    let selects = this.ptcSelectService.getChildren(level, this.groupName);
    
    if(!selects) return;

    selects.forEach(select => {
      select._items = [];
    })
  }

}
