import { Component, Input, OnInit, Injector, OnDestroy, ViewChild, TemplateRef } from '@angular/core';

import { NbMenuService, NbSidebarService } from '@nebular/theme';

import { BaseComponent } from 'src/app/pages/base/base.component';
import { Store } from '@ngrx/store';
import { State as fromRootReducers, userInfoSelector } from "../../../store/reducers";

import * as fromAuthActions from "../../../store/actions/auth.actions";
import * as fromAlertActions from "../../../store/actions/alert.actions";
import * as fromRouteActions from "../../../store/actions/route.actions";
import * as fromNotificationActions from "../../../store/actions/notification.actions";
import { Subscription } from 'rxjs';
import { User, Role, JobPosition } from 'src/app/model/authorize.model';
import { AuthenticationService } from 'src/app/shared/service/authentication.service';
import { PtcSwalType } from 'ptc-swal';
import { concatMap, filter, takeUntil } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LayoutService } from 'src/app/@core/utils';
import { NotficationCalcViewModel, PersonalNotificationType } from 'src/app/model/notification.model';
import { OrganizationType } from 'src/app/model/organization.model';
import * as fromHomePageActions from '../../../store/actions/home-page.actions';
// import { State as fromRootReducer } from 'src/app/store/reducers/index';
import { AuthBaseComponent } from 'src/app/pages/base/auth-base.component';


@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent extends AuthBaseComponent implements OnInit, OnDestroy {


  @ViewChild('jobPosition') jobPositionRef: TemplateRef<any>;
  @Input() position = 'normal';


  role: any = '';
  homeSelect: any;

  homeSelectEnable: boolean = false;

  userRoles: Array<Role> = [];
  homeList = [
    { id: OrganizationType.CallCenter, text: '客服' },
    { id: OrganizationType.HeaderQuarter, text: '總部' },
    { id: OrganizationType.Vendor, text: '廠商' }
  ];
  userMenu = [];
  jonPositionMenu = [];
  userName = '';
  hasCallCenterOrg: boolean = false;
  notificationCalcCount: NotficationCalcViewModel = new NotficationCalcViewModel();


  user = new User();
  user$: Subscription;
  menu$: Subscription;
  notificationCount$: Subscription;
  homeID$: Subscription;

  constructor(
    // private storeHome: Store<fromRootReducer>,
    private modalService: NgbModal,
    private sidebarService: NbSidebarService,
    private menuService: NbMenuService,
    private layoutService: LayoutService,
    private store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initialize();

  }

  jobPositionClick() {
    this.modalService.open(this.jobPositionRef, { size: 'sm', container: 'nb-layout' });
  }


  initialize() {
    this.userMenu = [{ title: this.translateService.instant('COMMON.LOG_OFF'), id: 'logout' }];

    this.user$ = this.store.select(userInfoSelector).pipe(
      filter(x => x !== undefined),
      concatMap(userInfo => {
        this.user = { ...userInfo.user };
        this.user.ImagePath = this.user.ImagePath ? this.user.ImagePath.toHostApiUrl() + "&timespane=" + new Date().valueOf() : "";
        this.setUserName(userInfo.jobPosition);
        this.setJobPosition(userInfo.jobPosition);
        return this.authService.getRoles();
      })
    ).subscribe(roles => {
      this.userRoles = roles;
      this.setSelectRoleValue();
    });

    this.menu$ =
      this.menuService
        .onItemClick()
        .subscribe(this.profileClick.bind(this));

    this.notificationCount$ =
      this.store.select(x => x.notification.notificationCalc)
        .pipe(
          filter(x => !!(x))
        )
        .subscribe(count => {
          this.notificationCalcCount = count;
          console.log("count => ", count);
        })

    this.homeID$ =
      this.store.select(x => x.home.homeID)
        .pipe(
          filter(x => !!(x))
        )
        .subscribe(homeId => {
          this.homeSelect = homeId;
          console.log("homeId => ", homeId);
        })

    this.getCurrentUser().subscribe(user => {
      this.hasCallCenterOrg = user.JobPosition.some(x => x.OrganizationType == this.organizationType.CallCenter);
    })
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  profileClick(value) {
    switch (value.item.id) {
      case 'logout':
        this.store.dispatch(new fromAuthActions.logOffAction());
        break;
    }
  }

  goToHome() {
    this.menuService.navigateHome();
  }

  setSelectRoleValue() {
    const cacheRole = this.authService.getCacheRoleID();
    this.role = !!cacheRole ? cacheRole.toString() : cacheRole;
  }
  setJobPosition(jobPositions: JobPosition[]) {
    if (jobPositions && jobPositions.length > 0) {
      this.jonPositionMenu = [];
      jobPositions.forEach(x => this.jonPositionMenu.push({
        title: `${x.BUName}/${x.NodeName}/${x.JobName}`,
      }));
    }

    this.setHomeList(jobPositions);
  }

  setHomeList(jobPositions: JobPosition[]) {
    if (!jobPositions || jobPositions.length == 0) {
      return;
    }

    const orgnizationTypelist = jobPositions
      .map(x => x.OrganizationType)
      .filter((x, i, a) => a.indexOf(x) == i);

    let homecopyList: any[] = [];
    this.homeList.forEach(x => {
      if (orgnizationTypelist.indexOf(x.id) > -1) {
        homecopyList.push(x);
      }
    });

    this.homeList = homecopyList;

    orgnizationTypelist.length > 1 ? this.homeSelectEnable = true : this.homeSelectEnable = false;
  }

  setUserName(jobPositions: JobPosition[]) {

    this.userName = '';
    if (jobPositions && jobPositions.length > 1) {
      this.userName = this.user.Name;
    }

    if (jobPositions && jobPositions.length === 1) {
      const jobPosition = jobPositions[0];
      this.userName = `${this.user.Name} (${jobPosition.JobName})`;
    }


  }

  roleChange($event) {

    this.store.dispatch(new fromAlertActions.alertOpenAction(
      {
        detail: {
          title: this.translateService.instant('COMMON.ON_ROLE_CHANGE_HINT'),
          type: PtcSwalType.question,
          showCancelButton: true
        },
        isLoop: false,
        confirm: () => {
          this.authService.setCacheRoleID($event);
          this.store.dispatch(new fromRouteActions.changeRouteAction({
            url: './pages/home/home-page',
            params: {}
          }));
          this.store.dispatch(new fromAuthActions.parseAuthAction());
        },
        cancel: () => {
          this.setSelectRoleValue();
        }
      }
    ));

  }

  homeChange($event: OrganizationType) {
    this.store.dispatch(new fromAlertActions.alertOpenAction(
      {
        detail: {
          title: "確定切換首頁?",
          type: PtcSwalType.question,
          showCancelButton: true
        },
        isLoop: false,
        confirm: () => {
          this.store.dispatch(new fromHomePageActions.changeHomeTypeAction($event.toString()));

          this.store.dispatch(new fromRouteActions.changeRouteAction({
            url: './pages/home/home-page',
            params: {}
          }));

        },
      }
    ));

  }

  ngOnDestroy() {
    this.user$ && this.user$.unsubscribe();
    this.menu$ && this.menu$.unsubscribe();

  }

  systemNotify() {
    this.store.dispatch(new fromNotificationActions.orderOpen({
      ajax: {
        url: "Common/Notification/GetPersonalList",
        body: { UserId: this.user.UserID },
        method: "Get",
        showBtn: true
      },
      title: "系統通知"
    }))
  }

  caseRemind() {
    this.store.dispatch(new fromNotificationActions.orderOpen({
      ajax: {
        url: "Common/Notification/GetCaseRemindList",
        body: { UserId: this.user.UserID },
        method: "Get",
        showBtn: false
      },
      title: "代辦事項"
    }))
  }

  officialEmail() {
    this.store.dispatch(new fromNotificationActions.orderOpen({
      ajax: {
        url: "Common/Notification/GetOfficialEmailList",
        body: { UserId: this.user.UserID },
        method: "Get",
        showBtn: false
      },
      title: "官網來信"
    }))
  }

}
