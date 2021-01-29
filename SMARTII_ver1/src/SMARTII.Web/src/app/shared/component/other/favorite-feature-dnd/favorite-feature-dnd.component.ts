import { Component, OnInit, Injector, Input, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { PageAuthFavoritePairCollection, PageAuthFavorite } from 'src/app/model/authorize.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-favorite-feature-dnd',
  template: `
  <div class="example-container d-flex" >
    <div class="row" [ngStyle]="{'margin':'30px auto', 'width':'50%'}">
      <div [ngStyle]="{'width': '50%', 'height': '400px'}" >
      <h2 style="text-align:center;">{{ translations.USER_PARAMETER.NONEJOIN | translate }}</h2>
      <div style="overflow-y:scroll; max-height: 400px;">
        
        <div cdkDropList #todoList="cdkDropList" [cdkDropListData]="_left" [cdkDropListConnectedTo]="[doneList]"
          class="example-list" (cdkDropListDropped)="drop($event)">
        <div class="example-box" *ngFor="let item of _left" cdkDrag>{{translateService.instant(item.Title)}}</div>
      </div>
    </div>
  </div>
  <div  [ngStyle]="{'width': '50%', 'height': '400px'}"> 
    <h2 style="text-align:center;">{{ translations.USER_PARAMETER.JOINED | translate }}</h2>
      <div  style="overflow-y:scroll; max-height: 400px;">
        
        <div cdkDropList #doneList="cdkDropList" [cdkDropListData]="_right" [cdkDropListConnectedTo]="[todoList]"
          class="example-list" (cdkDropListDropped)="drop($event)">
        <div class="example-box" *ngFor="let item of _right" cdkDrag>{{translateService.instant(item.Title)}}</div>
      </div>
    </div>
</div>
 </div>`,
  styleUrls: ['./favorite-feature-dnd.component.scss']
})
export class FavoriteFeatureDndComponent extends FormBaseComponent implements OnInit {


  public _right: PageAuthFavorite[] = [];
  public _left: PageAuthFavorite[] = [];

  private _data = new PageAuthFavoritePairCollection();

  @Input()
  set data(v: PageAuthFavoritePairCollection) {

    this.filterList();
    this._data = v;

  }
  get data(): PageAuthFavoritePairCollection {
    return this._data;
  }

  get right(): PageAuthFavorite[] {
    return this._right
  }
  get left(): PageAuthFavorite[] {
    return this.left
  }

  constructor(
    public translateService: TranslateService,
    public injector: Injector) {
    super(injector);
  }
  ngOnInit() {

  }

  /**
   * 拖曳行為
   */
  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
  }

  /**
   * 比較左右差異，濾掉已經存在右邊的資料
   */
  filterList() {
    if (this.data.Right.length > 0) {
      this._left = this.data.Left.filter(x => this.data.Right.every(c => c.Feature != x.Feature));
      this._right = this.data.Left.filter(x => this.data.Right.some(c => c.Feature == x.Feature));

      //回填順序 並進行排序
      this._right.forEach(x => x.Order = this.data.Right.find(c => c.Feature == x.Feature).Order);
      this._right.sort((a, b) => { return a.Order - b.Order });
    }
    else {
      this._left = this.data.Left;
    }

  }
}
