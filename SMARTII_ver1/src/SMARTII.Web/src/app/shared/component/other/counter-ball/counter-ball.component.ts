import { Component, OnInit, Input, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { tap, takeUntil } from 'rxjs/operators';
import { AspnetJsonResult } from 'src/app/model/common.model';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-counter-ball',
  templateUrl: './counter-ball.component.html',
  styleUrls: ['./counter-ball.component.scss']
})
export class CounterBallComponent extends BaseComponent implements OnInit {

  @Input() params: any = {};
  @Input() url;
  @Input() buID?: number;

  public countString: string;

  public loading$: Subject<boolean> = new Subject<boolean>();

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.getCount();
  }

  getCount() {

    this.http.post<AspnetJsonResult<any>>(this.url, this.params, {})
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
    console.log('onSuccess');
    console.log(data);
    this.countString = data;
  }

  onComplete() {
    console.log('onComplete');
    this.loading$.next(false);
  }

}
