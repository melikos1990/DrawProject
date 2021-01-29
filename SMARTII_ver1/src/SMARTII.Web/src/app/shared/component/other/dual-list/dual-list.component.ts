import { Component, OnInit, ViewChild, Input, AfterViewInit, Output, EventEmitter, Injector, forwardRef } from '@angular/core';
import { DualListComponent as DualList } from 'angular-dual-listbox';

import { HttpService } from 'src/app/shared/service/http.service';
import { Observable } from 'rxjs';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NG_VALUE_ACCESSOR } from '@angular/forms';


export class DualListAjaxOption {
  url: string;
  method: 'Get' | 'Post' = 'Get';
  body?: any;
  resSelector?: (data) => any
}

export class FormatOpt {
  add: string;
  remove: string;
  all: string;
  none: string;
  direction: string; // 擺放方向 
  draggable: boolean;
  locale: any;
}


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DualListComponent),
  multi: true
};

@Component({
  selector: 'app-dual-list',
  templateUrl: './dual-list.component.html',
  styleUrls: ['./dual-list.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class DualListComponent extends BaseComponent implements OnInit, AfterViewInit {

  @ViewChild(DualList) dualList: DualList;

  @Input() url: string;
  @Input() ajaxOpt: DualListAjaxOption = new DualListAjaxOption();
  @Input() height: string = "300px";

  @Input() options: FormatOpt = this.defaultFormat;
  @Input() keyField: string = 'key';
  @Input() textField: string = 'name';
  @Input() enableFilter: boolean = false;

  @Input() disabled :boolean;
  @Output() onDestinationChange: EventEmitter<any> = new EventEmitter();

  source: any[] = [];

   _destination: any = [];

  get destination() { return this._destination; }
  set destination(val) { 
    this._destination = val;
    this._change();
  }


  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };
  private event: () => void;

  constructor(
    public injector: Injector,
    private http: HttpService
  ) { 
    super(injector);
  }

  ngOnInit() {
  }
  

  ngAfterViewInit(){

    if(!this.ajaxOpt.url)
      this.ajaxOpt.url = this.url;


    this.send().subscribe(res => {
      this.source =  this.ajaxOpt.resSelector ?  this.ajaxOpt.resSelector(res) : res;
      
      this.event && this.event();
      
    })
  }
  

  destinationChange = (event) => this.onDestinationChange.emit(event);

  private distinguishDest = (data = []) => this.destination = this.source.filter(x => data.some(g => g == x[this.keyField]));


  private send(): Observable<any>{
    let { url, method, body } = this.ajaxOpt;
    let res$;

    switch (method.toLocaleLowerCase()) {
      case 'get':
        res$ = this.http.get(url, body);
        break;

      case 'post':
        res$ = this.http.post(url, null, body);
        break;
    
      default:
        res$ = this.http.get(url, body);
        break;
    }

    return res$;
  }


  private get defaultFormat(): FormatOpt{
    return {
      add: this.translateService.instant('COMMON.DUALLIST.ADD'),
      remove: this.translateService.instant('COMMON.DUALLIST.REMOVE'),
      all: this.translateService.instant('COMMON.DUALLIST.ALL'),
      none: this.translateService.instant('COMMON.DUALLIST.NONE'),
      direction: DualList.LTR,
      draggable: false,
      locale: null
    }
  }

  private _change = () => {
    let data = [...this.destination];
    let result = !!(data) ? data.map(x => x[this.keyField]) : [];
    
    this.onChangeCallback(result)
  }


  writeValue(obj: any): void {
    if(obj && Array.isArray(obj)){
      if(!this.event)
        this.event = this.distinguishDest.bind(this, obj);
      else{
        this.event = this.distinguishDest.bind(this, obj);
        this.event();
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
    
  }


}
