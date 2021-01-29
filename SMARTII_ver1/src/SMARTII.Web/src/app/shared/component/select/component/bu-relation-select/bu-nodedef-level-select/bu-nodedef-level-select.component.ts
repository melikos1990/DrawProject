import { Component, Input, Injector, ViewChildren, QueryList, ViewChild, Output, EventEmitter, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { OrganizationNodeViewModel, HeaderQuarterNodeDetailViewModel } from 'src/app/model/organization.model';
import { DefinitionStepSelectComponent } from '../../../element/definition-step-select/definition-step-select.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FormBaseComponent } from '../../../../../../pages/base/form-base.component';

@Component({
  selector: 'app-bu-nodedef-level-select',
  templateUrl: './bu-nodedef-level-select.component.html',
  styleUrls: ['./bu-nodedef-level-select.component.scss']
})
export class BuNodeDefinitionLevelSelectorComponent extends FormBaseComponent implements OnInit {

  @ViewChildren(DefinitionStepSelectComponent) definitions: QueryList<DefinitionStepSelectComponent>;

  @Input() buID?: number;

  @Input()
  set setbuID(buID: number) {
    this.buID = buID;
    this.getSteps();
  }
  get setbuID(): number {
    return this.buID;
  }

  @Input() disabled: boolean = false;
  @Input() isSelf: boolean = true;
  @Output() btnBuSelect: EventEmitter<any> = new EventEmitter<any>();

  @Input() buVisiable: boolean = true;

  public items: [] = [];
  public arrayId: number[] = [];
  public nodeKey: string;
  form: FormGroup;

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initFormGroup();
  }

  onChange($event: HeaderQuarterNodeDetailViewModel) {

    this.buID = $event ? $event.BUID : null;
    this.nodeKey = $event ? $event.NodeKey : null;
    this.getSteps();
   // this.triggerChildren();

    this.btnBuSelect.emit();
    this.arrayId = []; // 更換 BU 時 清空內部ngModel
  }

  triggerChildren() {

    if(!this.definitions) return;

    // 如果有給 arrayId(回填節點ID) 就呼叫所有階層, 否則只呼叫第一層
    if(this.arrayId && this.arrayId.length > 0){
      this.definitions.forEach(select => select.getList())
    }
    else if(this.definitions.first){
      this.definitions.first.getList();
    }
  }

  getSteps() {
    this.http.get("Common/Organization/GetAggregateNodeDefinitionLevels", {
      buID: this.buID
    }).subscribe((resp: any) => {
      this.items = resp.items;
      setTimeout(() => {
        this.triggerChildren();
      }, 0);
    });
  }

  initFormGroup() {
    this.form = new FormGroup({
      buID: new FormControl(null, [
        Validators.required
      ])
    })
  }

  public hasValue(node: OrganizationNodeViewModel) {
    return node.ID != null && node.ID !== undefined;
  }

  /**
   * 取得最末節點資訊
   */
  public getEndValue() {

    if (!this.definitions) {
      return null;
    }

    const valuable =
      this.definitions
        .map(x => x.currentItem)
        .filter(x => this.hasValue(x));

    return valuable[valuable.length - 1];
  }

  /**
   * 取得最後階層節點資訊
   */
  public getLatestValue() {

    if (!this.definitions) {
      return null;
    }
    if (!this.hasValue(this.definitions.last.currentItem)) {
      return null;
    }

    return this.definitions.last.currentItem;
  }

  /**
   * 取得第一層節點資訊
   */
  public getFirstValue() {

    if (!this.definitions) {
      return null;
    }

    if (!this.hasValue(this.definitions.first.currentItem)) {
      return null;
    }

    return this.definitions.first.currentItem;
  }

  /**
   * 取得所有結資訊
   */
  public getAllValue() {
    if (!this.definitions) {
      return null;
    }
    return this.definitions
      .map(x => x.currentItem)
      .filter(x => this.hasValue(x));
  }

  //驗證查詢表單
  validSearchForm() {
    if (this.validForm(this.form) == false) {
      return false;
    }
    else {
      return true;
    }
  }

}
