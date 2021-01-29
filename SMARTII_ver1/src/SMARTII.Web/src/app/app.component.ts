import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { State as fromRootReducer } from './store/reducers';
import { Store } from '@ngrx/store';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import * as fromRootAction from './store/actions';
import { AuthenticationService } from './shared/service/authentication.service';
import { CultureService } from './shared/service/culture.service';
import { BreadcrumbsService } from 'ng6-breadcrumbs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationLayoutComponent } from './layout/notification/notification-layout/notification-layout.component';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {

  signalr$: Subscription;
  culture$: Subscription;
  breadcrumbs$: Subscription;
  routeChange$: Subscription;
  routeStartup$: Subscription;
  notifyOrder$: Subscription;


  constructor(
    private modalService: NgbModal,
    private store: Store<fromRootReducer>,
    private auth: AuthenticationService,
    private languageService: CultureService,
    private breadcrumbsService: BreadcrumbsService,
    private router: Router) {
  }


  ngOnInit(): void {

    this.subscription();

  }


  subscription() {

    this.culture$ =
      this.store.select((state: fromRootReducer) => state.app.cluture)
        .subscribe(this.setLang.bind(this));

    this.breadcrumbs$ =
      this.breadcrumbsService.get()
        .subscribe(breadcrumbs =>
          this.store.dispatch(new fromRootAction.RouteActions.changeBreadcrumbAction(breadcrumbs)));

    this.routeChange$ = this.store.select((state: fromRootReducer) => state.route.navigate)
      .pipe(
        filter(payload => payload != null)
      )
      .subscribe(payload => {
        this.router.navigate([payload.url, payload.params]);

      });

    this.routeStartup$ = this.router.events.pipe(
      filter(event => event instanceof NavigationStart))
      .subscribe((event: NavigationStart) => this.onPageChange(event));


    this.notifyOrder$ = 
      this.store.select((state: fromRootReducer) => state.notification.orderPayload)
          .pipe(filter(payload => !!(payload)))
          .subscribe(payload => {
            let modal = this.modalService.open(NotificationLayoutComponent, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
            let instance =  <NotificationLayoutComponent>modal.componentInstance;
            instance.title = payload.title;
            console.log("payload.ajax => ", payload.ajax);
            instance.ajax = payload.ajax;
          })

  }

  onPageChange(event: NavigationStart) {
    if (event.url !== '/') {
      // 重整畫面行為
      if (this.router.navigated === false) {
        this.onPageRefresh();
      }

      this.onPageRedirect();
    }
  }


  setLang(lang: string) {
    if (!lang) {
      this.languageService.setInitState();
    } else {
      this.languageService.setLang(lang);
      this.auth.setCacheLangKey(lang);
    }
  }

  onPageRefresh() {

    // 語系cache 回應
    const cacheLang = this.auth.getCacheLangKey();
    if (cacheLang) {
      this.store.dispatch(new fromRootAction.AppActions.changeCultureAction(cacheLang));
    }

    // 重新初始化Signalr連線 改為進入立案畫面才初始化
    // this.store.dispatch(new fromRootAction.AuthActions.activateExistIdentity());
  }
  onPageRedirect() {
    this.store.dispatch(new fromRootAction.AuthActions.parseAuthAction());
  }

  disableContextMenu() {
    this.store.dispatch(new fromRootAction.AppActions.contextMenuAction({
      display: false,
      position: { x: 0, y: 0 },
      cbDist: []
    }));
  }

  ngOnDestroy(): void {
    this.routeChange$ && this.routeChange$.unsubscribe();
    this.routeStartup$ && this.routeStartup$.unsubscribe();
    this.breadcrumbs$ && this.breadcrumbs$.unsubscribe();
    this.culture$ && this.culture$.unsubscribe();
    this.signalr$ && this.signalr$.unsubscribe();
    this.notifyOrder$ && this.notifyOrder$.unsubscribe();
  }


}
