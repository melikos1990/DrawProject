import { Component, OnInit, Input, Injector } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { SystemNotificationViewModel, PersonalNotificationType } from 'src/app/model/notification.model';
import { of } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { AjaxAppender } from 'log4javascript';
import * as fromNotificationActions from "../../../store/actions/notification.actions";
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-notification-layout',
  templateUrl: './notification-layout.component.html',
  styleUrls: ['./notification-layout.component.scss']
})
export class NotificationLayoutComponent extends FormBaseComponent implements OnInit {

  @Input() title: string;
  @Input() ajax: {
    url: string,
    body: any,
    method: "Get" | "Post",
    showBtn: boolean
  };

  todayList: any[] = [];
  beforList: any[] = [];
  personalNotificationType = PersonalNotificationType;

  constructor(
    public injector: Injector,
    public modalService: NgbModal,
    public store: Store<fromCaseReducer>,
    public http: HttpService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.send()
      .subscribe((data: any) => {
        let { today, befor } = this.resetData(data);
        this.todayList = today;
        this.beforList = befor;
        console.log(befor);
      })
  }


  send() {
    switch (this.ajax.method.toLocaleLowerCase()) {
      case "get":
        return this.http.get<SystemNotificationViewModel>(this.ajax.url, this.ajax.body);

      case "post":
        return this.http.post<SystemNotificationViewModel>(this.ajax.url, null, this.ajax.body);
    }
  }

  btnClearNotice(UserId: string) {
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否確定清空通知?',
      () => {
        this.store.dispatch(new fromNotificationActions.clearNotice({ userID: UserId }));
        this.modalService.dismissAll();
      }
    )));

  }

  private resetData(data: SystemNotificationViewModel) {
    let today = [];
    let befor = [];

    today = [...data.TodayNotification.NotificationDatas];
    befor = [...data.BeforeNotification.NotificationDatas];

    return { today, befor };
  }

}
