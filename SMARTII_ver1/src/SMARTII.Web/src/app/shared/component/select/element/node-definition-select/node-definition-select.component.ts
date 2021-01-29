import { Component, OnInit, Input, Injector, forwardRef, Output, EventEmitter, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { OrganizationType } from 'src/app/model/organization.model';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => NodeDefinitionSelectComponent),
  multi: true
};

@Component({
  selector: 'app-node-definition-select',
  templateUrl: './node-definition-select.component.html',
  styleUrls: ['./node-definition-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class NodeDefinitionSelectComponent extends BaseComponent implements OnInit, ControlValueAccessor {


  @Input() id: string = '';
  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() 
  get compare() { return this._compare; };
  set compare(val) { 
    this._compare = val; 
    this.ignoreItems();
  }
  @Input() disabled: boolean = false;
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();
  @ViewChild('select') openSelect: any;

  public event: () => void;
  private fetched: boolean = false;
  private _compare: (item) => boolean;


  public innerSearchTerm: {
    OrgnizationType?: OrganizationType,
    BUID?: number;
  }
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: any[] = [];
  public renderItems: any[] = [];


  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v != this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
      if(v != null) this._onSelectedChange(v);
    }
  }


  @Input() 
  get searchTerm(): {
    OrgnizationType?: OrganizationType,
    BUID?: number;
  } {
    return this.innerSearchTerm;
  }

  set searchTerm(v: {
    OrgnizationType?: OrganizationType,
    BUID?: number;
  }) {
    this.innerSearchTerm = v;
    this.getList();
  }

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    setTimeout(() => {
      this.getList();
    }, 100);

  }

  getList() {
    this.fetched = false;
    this.http.post("Common/Organization/GetNodeDefinitions", null ,this.searchTerm).subscribe((resp: any) => {
      this.items = this.renderItems = resp.items;
      this.fetched = true;
      this.event && this.event();
      this.ignoreItems();
    });
  }

  _onSelectedChange(event){
    let value = this.items.find((x: any) => x.id == event);

    this.onSelectedChange.emit(value ? value.extend : null);
  }

  ignoreItems(){
    if(!this._compare) return;
    
    this.renderItems = this.items.filter(x => this._compare(x));
  }

  writeValue(obj: any): void {

    if (this.fetched) {

      this.value = obj;

    } else {
      this.event = () => {
        this.value = !obj ? this.innerValue : obj;    
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
