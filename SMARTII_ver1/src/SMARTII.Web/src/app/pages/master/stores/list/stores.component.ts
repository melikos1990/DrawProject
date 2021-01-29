import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from '../../../base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { StoresSearchViewModel } from 'src/app/model/master.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromStoresActions from '../../store/actions/stores.actions';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { commonBu, tryGetStoreKey } from 'src/global';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgInputBase } from 'ptc-dynamic-form';
import { State as fromMasterReducer } from '../../store/reducers';
import { BuNodeDefinitionLevelSelectorComponent } from 'src/app/shared/component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { OrganizationType } from 'src/app/model/organization.model';
import { takeUntil } from 'rxjs/operators';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';
import { FormGroup, FormControl } from '@angular/forms';


export const PREFIX = 'StoresComponent';

@Component({
  selector: 'app-stores',
  templateUrl: './stores.component.html',
  styleUrls: ['./stores.component.scss']
})
export class StoresComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table') table: ServerTableComponent;

  @ViewChild('allNodeSelector') allNodeSelector: BuNodeDefinitionLevelSelectorComponent;

  form: FormGroup = new FormGroup({});
  json: string;
  inputs: NgInputBase[] = [];
  isEnableResult: boolean = false;
  isEnable: boolean = false;
  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: StoresSearchViewModel = new StoresSearchViewModel();
  public particular = {};

  constructor(
    public store: Store<fromRootReducers>,
    public storeMaster: Store<fromMasterReducer>,
    public http: HttpService,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.subscritpion();
    this.initFormGroup();
  }

  initFormGroup() {
    this.form = new FormGroup({
      NodeID: new FormControl(),
      Name: new FormControl(),
      StoreOpenDateTime: new FormControl(),
      StoreCloseDateTime: new FormControl()
    })
  }

  subscritpion() {
    this.storeMaster
      .select((state: fromMasterReducer) => state.master.stores.storesListLayout)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(layout => {
        this.json = layout;
        this.isButtonEnable(layout);
      });
  }

  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
    if (this.model.BuID) {
      this.allNodeSelector.buID = this.model.BuID;
      this.allNodeSelector.getSteps();
      this.getnodeKey(this.model.BuID);
    }

  }

  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/stores-detail',
      params: {
        actionType: ActionType.Read,
        Id: $event.NodeID,
        OrganizationType: OrganizationType.HeaderQuarter,
      }
    }));
  }


  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/stores-detail',
      params: {
        actionType: ActionType.Update,
        Id: $event.NodeID,
        OrganizationType: OrganizationType.HeaderQuarter,
      }
    }));
  }

  //共同查詢
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {
    const selectedItems = this.allNodeSelector;
    if (!(selectedItems.buID) && !this.allNodeSelector.validSearchForm()) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('請選擇企業別')));
      return;
    }

    //開啟查詢結果
    this.isEnableResult = null;

    this.ajax.url = `${commonBu}/Store/GetList/`.toHostApiUrl();
    this.ajax.method = 'POST';

    this.table.render();
  }

  //進階查詢
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnAdvancedRender() {
    const selectedItems = this.allNodeSelector;
    if (!(selectedItems.buID)) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('請選擇企業別')));
      return;
    }

    const providerKey = tryGetStoreKey(this.allNodeSelector.nodeKey);

    this.ajax.url = `${providerKey}/Store/GetList/`.toHostApiUrl();
    this.ajax.method = 'POST';

    this.table.render();
  }

  //共通/進階 查詢條件
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {

    const selectedItemsValue = this.allNodeSelector.getEndValue();

    this.model.BuID = this.allNodeSelector.buID;
    if (!!(selectedItemsValue)) {
      this.model.NodeID = selectedItemsValue.ID;
      this.model.LeftBoundary = selectedItemsValue.LeftBoundary;
      this.model.RightBoundary = selectedItemsValue.RightBoundary;
    }
    else {
      this.model.NodeID = null;
      this.model.LeftBoundary = null;
      this.model.RightBoundary = null;
    }

    this.model.Particular = this.particular;
    $event.criteria = this.model;
  }

  //開啟進階查詢畫面
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnAdvancedSearch($event: any) {
    this.deserialize(this.json);
  }

  //BU下拉選單切換
  onBuChange() {
    this.particular = {};
    this.store.dispatch(new fromStoresActions.getStoresListTemplateAction({ nodeKey: this.allNodeSelector.nodeKey }));
  }

  //檢查是否有進階查詢條件
  isButtonEnable(layout: string) {
    if (!(layout)) {
      this.isEnable = false;
    }
    else {
      this.isEnable = true;
    }

    this.deserialize(""); //清空進階查詢
  }
  //返回時，需取得nodekey
  getnodeKey(buID: number) {
    this.http.get("Common/Organization/GetHeaderQuarterRootNodeKey", {
      buID: buID
    }).subscribe((resp: string) => {
      this.allNodeSelector.nodeKey = resp;
    });
  }
  //將字串轉查詢物件
  deserialize(jsonLayout) {
    this.inputs = [];
    try {

      if (jsonLayout) {
        const objects = <Array<NgInputBase>>JSON.parse(jsonLayout);
        if (objects) {
          this.inputs = [...objects];
        } else {
          this.inputs = [];
        }
      } else {
        this.inputs = [];
      }
    } catch (e) {
      console.log(e);
    }
  }

  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'NodeName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('STORE.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('STORE.CODE'),
        name: 'Code',
        disabled: false,
        order: 'CODE'
      },

      {
        text: this.translateService.instant('STORE.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('STORE.ADDRESS'),
        name: 'Address',
        disabled: false,
        order: 'ADDRESS'
      },
      {
        text: this.translateService.instant('STORE.TELEPHONE'),
        name: 'Telephone',
        disabled: false,
        order: 'TELEPHONE'
      },
      {
        text: this.translateService.instant('STORE.STORE_OPEN_DATETIME'),
        name: 'StoreOpenDateTime',
        disabled: false,
        order: 'STORE_OPEN_DATETIME'
      },
      {
        text: this.translateService.instant('STORE.STORE_CLOSE_DATETIME'),
        name: 'StoreCloseDateTime',
        disabled: false,
        order: 'STORE_CLOSE_DATETIME'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: ''
      },
    ];


  }



}

