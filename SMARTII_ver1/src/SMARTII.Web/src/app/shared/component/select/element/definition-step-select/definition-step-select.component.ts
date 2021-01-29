import { Component, Input, Output, Injector, EventEmitter, ViewChild, forwardRef } from '@angular/core';

import { OpenSelectComponent } from 'src/app/shared/component/select/atom/open-select/open-select.component';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { OrganizationNodeViewModel } from 'src/app/model/organization.model';
import { HttpService } from 'src/app/shared/service/http.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DefinitionStepSelectComponent),
  multi: true
};

@Component({
  selector: 'app-definition-step-select',
  templateUrl: './definition-step-select.component.html',
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class DefinitionStepSelectComponent extends BaseComponent {


  @ViewChild(OpenSelectComponent) selectRef: OpenSelectComponent;

  @Input() refs: Array<any> = [];
  @Input() level: any;
  @Input() buID: number;
  @Input() isSelf = true;
  @Input() id: string = '';
  @Input() multiple: boolean;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;

  @Output() onSelectedChange: EventEmitter<number> = new EventEmitter();

  private fetched: boolean = false;
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: any[] = [];

  public currentItem: OrganizationNodeViewModel = new OrganizationNodeViewModel();

  public event: () => void;

  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    console.log("v ==========> ", v);
    if (v != this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
    }
  }

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  public findWithoutSelf = (): Array<DefinitionStepSelectComponent> => {
    return this.refs.filter(x => x !== this);
  }

  public findParnets = (): Array<DefinitionStepSelectComponent> => {
    return this.refs.filter(x => parseInt(x.level) < parseInt(this.level));
  }

  public findParent = (): DefinitionStepSelectComponent => {
    return this.refs.find(x => x.level == (parseInt(this.level) - 1));
  }

  public findNext = (): DefinitionStepSelectComponent => {
    return this.refs.find(x => x.level == (parseInt(this.level) + 1));
  }

  public findChildren = (): Array<DefinitionStepSelectComponent> => {
    return this.refs.filter(x => parseInt(x.level) > parseInt(this.level));
  }

  public findCurrentItem = (id) => {
    const item = this.items.find(x => x.id == id);

    return item ? item.extend : null;
  }

  public resetAndFetchNext = () => {
    const next = this.findNext();
    const children = this.findChildren();
    children && children.forEach(x => x.resetItem());
    next && next.getList();
  }

  public resetItem = () => {
    this.selectRef.select.reset();
    this.currentItem = new OrganizationNodeViewModel();
    this.items = [];
  }

  public requestBody = () => {

    const parent = this.findParent();
    const left = (parent && parent.currentItem) ? parent.currentItem.LeftBoundary : null;
    const right = (parent && parent.currentItem) ? parent.currentItem.RightBoundary : null;

    return {
      BUID: this.buID,
      Level: this.level,
      LeftBoundary: left,
      RightBoundary: right,
      IsSelf: this.isSelf,
    };
  }


  getList() {

    const data = this.requestBody();


    this.http.post("Common/Organization/GetHeaderQurterNodesByLevel", null, data)
      .subscribe((resp: any) => {
        this.items = resp.items;
        this.fetched = true;
        this.event && this.event();
      });
  }

  onChange($event) {
    this.onSelectedChange.emit($event);
    if ($event) {
      this.currentItem = this.findCurrentItem($event);
      this.resetAndFetchNext();
    }
    else {
      //清空底下
      const children = this.refs.filter(x => x.level > this.level);
      this.currentItem = new OrganizationNodeViewModel();
      children.forEach(x => x.resetItem());
    }
  }


  writeValue(obj: any): void {

    if (this.fetched) {
      if (obj != this.innerValue) {
        this.innerValue = obj;
      }
    } else {
      this.event = () => {
        if (obj != this.innerValue) {
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
