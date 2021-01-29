import { Component, OnInit, OnDestroy, Injector, Inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { State as fromRootReducer } from '../store/reducers'
import { Subscription, Observable, interval } from 'rxjs';
import { BaseComponent } from './base/base.component';
import { Menu } from '../model/master.model';
import { NbMenuInternalService } from '@nebular/theme/components/menu/menu.service';
import { SignalRHubService } from '../shared/service/signalR.hub.service';
import { ObjectService } from '../shared/service/object.service';
import { LayoutBaseComponent } from './base/layout-base.component';
import { Location } from '@angular/common';
import { IMenuSelect } from '../shared/service/menu-select.service';
import { MENU_SELECT } from '../shared/injection-token';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { concatMap, filter } from 'rxjs/operators';
import * as fromRootAction from '../store/actions/';
import { NotifyCountSecond } from 'src/app.config';
import { User } from '../model/authorize.model';
// import * as fromHomePageActions from '../pages/home/store/actions/home-page.actions';


@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss'],
})
export class PagesComponent extends LayoutBaseComponent implements OnInit, OnDestroy {


  menu: Menu[];
  menu$: Subscription;
  notifyStream$: Subscription;
  currentUser: User;

  constructor(
    @Inject(MENU_SELECT) public menuSelectService: IMenuSelect,
    public store: Store<fromRootReducer>,
    public route: ActivatedRoute,
    public menuService: NbMenuInternalService,
    public signalRService: SignalRHubService,
    public objectSercice: ObjectService,
    private modalService: NgbModal,
    public location: Location,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

    this.menu$ =
      this.store.select((state: fromRootReducer) => state.auth.menu)
        .subscribe((data: Menu[]) => {
          this.menu = this.getTranslateMenu(data);

          setTimeout(() => {
            const subMenu = this.menuSelectService.matchItem(location.pathname, this.menu);
            this.menuService.prepareItems(this.menu);
            if (subMenu) {
              this.menuService.selectItem(subMenu, this.menu, false, 'menu');
            }

          }, 100);
        });


    this.notifyStream$ = 
      interval(NotifyCountSecond * 1000)
      .pipe(filter(_ => !!this.currentUser && !!this.currentUser.UserID))
      .subscribe(_ => {
        console.log("getNotificationCount.1===>",this.currentUser.UserID);
        this.store.dispatch(new fromRootAction.NotificationActions.getNotificationCount({ userID: this.currentUser.UserID }))
      })

    this.getCurrentUser()
      .subscribe(user => {
        this.currentUser = user;
        console.log("getNotificationCount.2===>",user.UserID);
        this.store.dispatch(new fromRootAction.NotificationActions.getNotificationCount({ userID: user.UserID }))

        const orgnizationTypelist = user.JobPosition
          .map(x => x.OrganizationType)
          .filter((x, i, a) => a.indexOf(x) == i);
        setTimeout(() => {
          let homeID: string;

          if (!orgnizationTypelist || orgnizationTypelist.length == 0) {
            homeID = "";
          }
          else {
            homeID = orgnizationTypelist[0].toString();
          }

          this.store.dispatch(new fromRootAction.HomeActions.changeHomeTypeAction(homeID));

        }, 1000);
      })

  }


  ngOnDestroy() {
    this.menu$ && this.menu$.unsubscribe();
    this.notifyStream$ && this.notifyStream$.unsubscribe();
  }

}
