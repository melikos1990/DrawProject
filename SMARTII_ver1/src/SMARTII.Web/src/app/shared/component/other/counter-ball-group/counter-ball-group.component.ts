import { Component, OnInit, Injector, Input, TemplateRef, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { AspnetJsonResult } from 'src/app/model/common.model';
import { SummaryTargetViewModel } from 'src/app/model/organization.model';
import { Subject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-counter-ball-group',
  templateUrl: './counter-ball-group.component.html',
  styleUrls: ['./counter-ball-group.component.scss']
})
export class CounterBallGroupComponent extends BaseComponent implements OnInit {


  @Input() templateRef: TemplateRef<any>;
  @Input() url: string = '';
  @Input() bgColor: string;
  @Input() model: SummaryTargetViewModel[] = [];
  @Input() cuurentTarget: SummaryTargetViewModel;

  @Output() onDivClickCallBack: EventEmitter<any> = new EventEmitter();

  public loading$: Subject<boolean> = new Subject<boolean>();

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    if (!this.model || this.model.length < 1) {
      this.getList();
    }
    console.log(this.templateRef);
  }

  getList() {

    this.http.post<AspnetJsonResult<SummaryTargetViewModel[]>>(this.url, null, {})
      .pipe
      (
        tap(x => this.loading$.next(true)),
        takeUntil(this.destroy$)
      )
      .subscribe(
        (x) => this.onSuccess(x.element),
        (error) => { },
        () => this.onComplete());

  }

  onSuccess(data) {
    console.log('onSuccessCounterBallGroupComponent');
    console.log(data);
    this.model = data;

  }

  onComplete() {
    console.log('onCompleteCounterBallGroupComponent');
    this.loading$.next(false);
  }

  onDivClick(item: SummaryTargetViewModel) {
    this.onDivClickCallBack.emit(item);
  }

  getClass(item) {
    let classString = "common ";// + this.bgColor;

    if (!!this.cuurentTarget && JSON.stringify(item) === JSON.stringify(this.cuurentTarget)) {
      classString += " specific";
    }

    return classString
  }

}
