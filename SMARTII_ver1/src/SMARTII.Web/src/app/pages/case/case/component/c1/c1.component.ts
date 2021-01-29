import { Component, OnInit, Injector, Input, ViewChild, OnDestroy } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { Store } from '@ngrx/store';

import { State as fromCaseReducers } from '../../../store/reducers';
import { skip, takeUntil } from 'rxjs/operators';
import * as fromCaseCreatorAction from '../../../store/actions/case-creator.actions';
import * as fromRootAction from 'src/app/store/actions/index';
import { NbSidebarService } from '@nebular/theme';
import { ActionType } from 'src/app/model/common.model';
import { ActivatedRoute } from '@angular/router';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { SignalRHubService } from 'src/app/shared/service/signalR.hub.service';
import { CaseService } from 'src/app/shared/service/case.service';
import { Guid } from 'guid-typescript';


const PREFIX = 'C1Component';

@Component({
  selector: 'app-c1',
  templateUrl: './c1.component.html',
  styleUrls: ['./c1.component.scss']
})
export class C1Component extends FormBaseComponent implements OnInit, OnDestroy {

  @ViewChild('tabs') tabsRef: any;
  @Input() public menu: any[];
  @Input() public uiActionType: ActionType;

  currentTabKey: string;

  constructor(
    public active: ActivatedRoute,
    public injector: Injector,
    public signalRService: SignalRHubService,
    public sidebarService: NbSidebarService,
    public caseService: CaseService,
    public store: Store<fromCaseReducers>) {
    super(injector, PREFIX);
  }


  isNew = (id: string) => id === 'new';

  ngOnInit() {
    this.subscription();
    this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceEntryAction());
    setTimeout(() => {
      this.sidebarService.toggle(true, 'menu-sidebar');
    }, 0);
  }

  ngOnDestroy() {
    console.log('ngOnDestroy')

    let sources = this.menu.map(x => x.key);

    if (sources && sources.length > 0)
      sources.forEach(key => this.store.dispatch(new fromCaseCreatorAction.removeSorceTabAction(key)))
    // this.store.dispatch(new fromCaseCreatorAction.clearAllAction());
  }

  btnCreateSource() {

    // 驗證 來源頁簽(不能開啟超過10個)
    if (this.menu.length > 10) {
      this.store.dispatch(new fromRootAction.AlertActions.alertOpenAction(this.getFieldInvalidMessage("來源頁簽不能開啟超過10個")));
      return;
    }

    this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceEntryAction());
  }


  newTabActive() {
    setTimeout(() => {

      const arr = this.tabsRef.tabs.toArray();
      let newTabs = arr.filter(x => this.isNew(x.tabTitle));

      if ((!!newTabs) == false) return;

      const reverseTabs = newTabs.reverse();
      reverseTabs[0].active = true;

    }, 100);
  }

  tabActive(sourceID: string) {
    setTimeout(() => {
      const arr = this.tabsRef.tabs.toArray();
      let openFirst = false;

      arr.forEach(element => {

        //如果已經開啟一個Tab就不再繼續開啟其他Tab
        if (openFirst == true) {
          return;
        }
        //找到對應的key開啟其Tab
        if (element.tabTitle == sourceID) {
          element.active = true;
          openFirst = true;
        } else {
          element.active = false;
        }
      });
    }, 100);
  }

  closeCaseSource(key: string) {
    this.store.dispatch(new fromCaseCreatorAction.removeSorceTabAction(key));

    var sourceKey = this.getLastSourceKey();

    if (this.isNew(sourceKey)) {
      this.newTabActive();
      return;
    }
    else if (this.currentTabKey == sourceKey) {
      this.tabActive(sourceKey);
      return;
    }

    this.store.dispatch(new fromCaseCreatorAction.activeSourceTabAction(sourceKey));
  }

  getLastSourceKey() {
    let keys = this.menu.map(x => x.key);
    let key = !!(keys) ? keys[keys.length - 1] : null;

    return Guid.isGuid(key) ? 'new' : key;
  }

  subscription() {

    // 有可能是從 window.open 進入 , 因此需使用 queryParams
    this.active.params.subscribe(x => {
      this.loadPage(x);
    })

    this.store.select(x => x.case.caseCreator.sourceKeyPair)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(x => {
        this.menu = x;
      });

    this.store.select(x => x.case.caseCreator.activeSourceTab)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe((payload) => {
        if (this.menu.length < 1 || this.menu.every(x => x.id != 'new')) {
          this.btnCreateSource();
        }
        this.currentTabKey = payload;
        this.tabActive(payload)

      });


    this.signalRService.listener('CurrentLockUpUsers')
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe((payload: any) => {
        console.log("CurrentLockUpUsers => ", payload);
        this.caseService.lockUsersSubject.next(payload);
      });

    if (this.signalRService.isConnected)
      this.signalRService.start();
    else
      this.store.dispatch(new fromRootAction.AuthActions.activateExistIdentity());

  }


  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);
    const caseID = params['caseID'];

    if (caseID) {
      this.store.dispatch(new fromCaseCreatorAction.loadCaseSourceAction(caseID));
    }

  }

}
