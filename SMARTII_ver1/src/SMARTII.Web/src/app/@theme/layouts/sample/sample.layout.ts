import { Component, OnDestroy, Injector } from '@angular/core';
import { delay, withLatestFrom, takeWhile } from 'rxjs/operators';
import {
  NbMediaBreakpoint,
  NbMediaBreakpointsService,
  NbMenuItem,
  NbMenuService,
  NbSidebarService,
  NbThemeService,
} from '@nebular/theme';

import { StateService } from '../../../@core/utils';
import { Store } from '@ngrx/store';

import * as fromRootAction from "src/app/store/actions";

import { State as fromRootReducer } from "src/app/store/reducers";
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Subscription } from 'rxjs';
import * as fromRootActions from 'src/app/store/actions';
import { ActionType } from 'src/app/model/common.model';
import { AuthBaseComponent } from 'src/app/pages/base/auth-base.component';



@Component({
  selector: 'ngx-sample-layout',
  styleUrls: ['./sample.layout.scss'],
  templateUrl : './sample.layout.html',
})
export class SampleLayoutComponent extends AuthBaseComponent implements OnDestroy {


  breadcrumbs$: Subscription;

  layout: any = {};
  sidebar: any = {};
  breadcrumbs : any[] = [];

  hasCallCenterOrg: boolean = false;

  private alive = true;

  currentTheme: string;

  constructor(
    private store: Store<fromRootReducer>,
    protected stateService: StateService,
    protected menuService: NbMenuService,
    protected themeService: NbThemeService,
    protected bpService: NbMediaBreakpointsService,
    protected sidebarService: NbSidebarService,
    public injector: Injector) {
    super(injector , '');

    this.stateService.onLayoutState()
      .pipe(takeWhile(() => this.alive))
      .subscribe((layout: string) => this.layout = layout);

    this.stateService.onSidebarState()
      .pipe(takeWhile(() => this.alive))
      .subscribe((sidebar: string) => {
        this.sidebar = sidebar;
      });

    const isBp = this.bpService.getByName('is');
    this.menuService.onItemSelect()
      .pipe(
        takeWhile(() => this.alive),
        withLatestFrom(this.themeService.onMediaQueryChange()),
        delay(20),
      )
      .subscribe(([item, [bpFrom, bpTo]]: [any, [NbMediaBreakpoint, NbMediaBreakpoint]]) => {

        if (bpTo.width <= isBp.width) {
          this.sidebarService.collapse('menu-sidebar');
        }
      });

    this.themeService.getJsTheme()
      .pipe(takeWhile(() => this.alive))
      .subscribe(theme => {
        this.currentTheme = theme.name;
      });

    this.breadcrumbs$ =
      this.store.select((state: fromRootReducer) => state.route.breadcrumbs)
        .subscribe((breadcrumbs : any[]) => {
          this.breadcrumbs = breadcrumbs;
        });

    this.getCurrentUser().subscribe(user => {
      this.hasCallCenterOrg = user.JobPosition.some(x => x.OrganizationType == this.organizationType.CallCenter);
    })
  }

  btnCreateCase($event) {

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/case/case-create',
      params: {
        actionType: ActionType.Add
      }
    }));
    
  }

  ngOnDestroy() {
    this.alive = false;

    this.breadcrumbs$ && this.breadcrumbs$.unsubscribe();
  }
}
