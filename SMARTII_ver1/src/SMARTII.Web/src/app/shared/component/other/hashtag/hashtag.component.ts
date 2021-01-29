import { Component, OnInit, Input, ElementRef, Injector, forwardRef } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Guid } from 'guid-typescript';
import { HttpService } from 'src/app/shared/service/http.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';

export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => HashtagComponent),
  multi: true
};

@Component({
  selector: 'app-hashtag',
  templateUrl: './hashtag.component.html',
  styleUrls: ['./hashtag.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]
})
export class HashtagComponent extends FormBaseComponent implements OnInit {


  @Input() noDataText: string = this.translateService.instant('COMMON.NODATA');
  @Input() options: any[] = [];
  @Input() placeholder: string = this.translateService.instant('COMMON.SELECT_OR_INPUT_PLACEHOLDER');
  @Input() tags: any[] = [];
  @Input() buID: number = null;
  @Input() defaultHashTags: any[] = [];
  @Input() disabled: boolean = false;
  @Input() isEnabled?: boolean;
  @Input() enterDisabled: boolean = false;

  get _tags() { return [...this.defaultHashTags, ...this.tags]; }
  tag = '';
  isAddMode: boolean = false;
  isSelectMode: boolean = false;

  private url: string = "Common/Master/GetHashTags";

  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  constructor(
    public injector: Injector,
    private store: Store<fromMasterReducer>,
    private http: HttpService
  ) {
    super(injector);
  }
  ngOnInit() {

  }

  addTag($event) {
    if(this.disabled){
      return;
    }
    if(this.enterDisabled)
    {
      return;      
    }
    
    //檢查名稱是否重複
    if(this.isNameRepeat()){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("此案件標籤已存在")));
      return;
    };

    if (this.tag) {
      this.tags.push({ id: Guid.create().toString(), text: this.tag });
      this.tag = '';
      this._change();
    }
  }
  removeTag($event) {
    if(this.disabled){
      return;
    }
    const index = this.tags.findIndex(x => x.id === $event);
    const defaultIndex = this.defaultHashTags.findIndex(x => x.id === $event);
    if (index >= 0) this.tags.splice(index, 1);
    if (defaultIndex >= 0) this.defaultHashTags.splice(defaultIndex, 1);
    this._change();
  }
  btnAddMode() {
    if(this.disabled){
      return;
    }
    this.isAddMode = true;
    this.focusing();
    this.getHashTags();

  }
  onFocusout() {
    // 防止 onSelect 併發
    setTimeout(() => {
      this.isAddMode = false;
    }, 350);

  }
  onfocus() {
    if(this.disabled){
      return;
    }
    setTimeout(() => {
      this.isSelectMode = true;
    }, 100);

  }
  onSelect($event) {
    if(this.disabled){
      return;
    }
    this.tags.push($event);
    this.isAddMode = false;
    this._change();
  }
  getValue() {
    if(this.disabled){
      return;
    }
    return this.tags;
  }

  private _change() {
    let data = this._tags.map(x => x.id);
    this.onChangeCallback(data);
  }

  private getHashTags() {
    this.http.get<any[]>(this.url, { 
      isEnabled: this.isEnabled,
      BuID: this.buID 
    })
      .subscribe(data => {
        this.options = data.filter(x => this.tags.every(g => g.id != x.id));
      })
  }


  writeValue(obj: any): void {
    if (obj && Array.isArray(obj)) this.tags = obj;
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }
  setDisabledState?(isDisabled: boolean): void {

  }

  isNameRepeat(){
    let arraylist = this.tags.map(function(item){
      return item.text; 
    });

    let result = arraylist.includes(this.tag);

    if(!result){
      let existArraylist = this.options.map(function(item){
        return item.text; 
      });

      result = existArraylist.includes(this.tag);
    }

    return result;
  }

  focusing(){
    setTimeout(() => {
      document.getElementById("hashtagInput").focus();
    }, 100)
  }

}
