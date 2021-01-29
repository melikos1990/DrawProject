import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { SelectController } from './model/selection.controller';
import { moveItemInArray, CdkDragDrop, CdkDragMove, CdkDragStart } from '@angular/cdk/drag-drop';
import { HttpService } from 'src/app/shared/service/http.service';
import { Observable,iif, of } from 'rxjs';
import { concatMap } from 'rxjs/operators';


@Component({
  selector: 'app-draggable-list',
  templateUrl: './draggable-list.component.html',
  styleUrls: ['./draggable-list.component.scss']
})
export class DraggableListComponent implements OnInit {

  @Input() contentClass: string;
  @Input() itemClass: string;
  @Input() isSelectReq: boolean = false;
  @Input() ajaxOpt: {
    url: string,
    method: "Get" | "Post",
    body: any
  };

  @Input() sortCompare: (accumulator, currentValue) => any;

  @Output() onDragMove: EventEmitter<CdkDragMove> = new EventEmitter();
  @Output() onDragStart: EventEmitter<CdkDragStart> = new EventEmitter();
  @Output() onDrop: EventEmitter<any> = new EventEmitter();

  get completeData(){ return this.ctrl.getComplete(); }

  ctrl: SelectController = new SelectController();

  constructor(
    private http: HttpService
  ) { }

  ngOnInit() {
    this.send()
      .pipe(
        concatMap(data => {
          let isSelectResponse = () => data.hasOwnProperty("items");
          return iif(isSelectResponse, of(data["items"]), of(data))
        })
      )
      .subscribe(data => {
        this.ctrl.init(data, this.sortCompare);
      }) 
  }


  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.ctrl.source, event.previousIndex, event.currentIndex);
    this.onDrop.emit(event);
  }

  dragStart(event) {
    this.onDragStart.emit(event);
  }

  dragMove(event) {
    this.onDragMove.emit(event);
  }


  private send(): Observable<any> {
    let { method, url, body } = this.ajaxOpt;
    let newBody = this.getCompleteReq(body);
    let res$;

    switch (method.toLocaleLowerCase()) {
      case "get":
        res$ = this.http.get(url, newBody);
        break;
      case "post":
        res$ = this.http.post(url, null, newBody);
        break;
      default:
        throw new Error("ajaxOpt 未給定 Method 參數");
    }

    return  res$;

  }

  private getCompleteReq(data: any){
    
    if(!this.isSelectReq) return data;

    return {
      criteria: data,
      parentID: data.hasOwnProperty('parentID') ? data["parentID"] : null,
      pageIndex: 0,
      size: Number.MAX_SAFE_INTEGER
    }
  }

}
