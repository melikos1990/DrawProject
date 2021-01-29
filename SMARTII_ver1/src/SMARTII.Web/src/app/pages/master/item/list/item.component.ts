import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormBaseComponent } from '../../../base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { ItemSearchViewModel, ItemListViewModel } from 'src/app/model/master.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from 'src/app/store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromItemActions from '../../store/actions/item.actions';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { commonBu, tryGetProviderKey } from 'src/global';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { HttpService } from 'src/app/shared/service/http.service';
import { NgInputBase } from 'ptc-dynamic-form';
import { State as fromMasterReducer } from '../../store/reducers';
import { takeUntil } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';
import { LocalTableComponent } from 'src/app/shared/component/table/local-table/local-table.component';

export const PREFIX = 'ItemComponent';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})
export class ItemComponent extends FormBaseComponent implements OnInit {


  @ViewChild('table')
  table: LocalTableComponent;

  @ViewChild(BuSelectComponent) select: BuSelectComponent;

  @ViewChild('localTableExportTemp')
  localTableExportTemp: TemplateRef<any>;

  columnsExport: any[] = [];

  json: string;
  inputs: NgInputBase[] = [];
  isEnable: boolean = false;
  columns: any[] = [];
  data: any[] = [];
  loading: boolean = false;

  isEnableResult: boolean = false;
  form: FormGroup;

  public model: ItemSearchViewModel = new ItemSearchViewModel();
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
    this.initFormGroup();
    this.subscritpion();
  }

  @loggerMethod()
  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCacheModel(this.table, this.btnRender.bind(this));
  }

  initFormGroup() {
    this.form = new FormGroup({
      BuID: new FormControl(null, [
        Validators.required
      ]),
      Name: new FormControl(),
      Code: new FormControl(),
      JContent: new FormControl()
    })
  }

  subscritpion() {
    this.storeMaster
      .select((state: fromMasterReducer) => state.master.item.itemListLayout)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(layout => {
        this.json = layout;
        this.isButtonEnable(layout);
      });
  }

  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/item-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
      }
    }));
  }


  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/item-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
      }
    }));
  }

  //共同查詢
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender() {

    if (this.validSearchForm() == false) return;

    //開啟查詢結果
    this.isEnableResult = null;

    const url = `${commonBu}/Item/GetList/`;
    this.getList(url);
  }

  //進階查詢
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnAdvancedRender() {

    if (this.validSearchForm() == false) return;

    const nodekey = this.select.items.find(x => {
      return x.id == this.select.innerValue;
    });

    const providerKey = tryGetProviderKey(!!nodekey ? nodekey.extend.NodeKey : "");

    const url = `${providerKey}/Item/GetList/`;
    this.getList(url);
  }

  //共通/進階 查詢條件
  @loggerMethod()
  getList(url: string) {
    this.loading = true;
    this.model.Particular = this.particular;
    this.http.post(url, null, this.model).subscribe((resp: any) => {

      this.data = resp.element;
      if (!resp.isSuccess) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(resp.message)));
      }

      this.loading = false;
    });
  }

  //開啟進階查詢畫面
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnAdvancedSearch() {
    this.deserialize(this.json);
  }

  //檔案上傳  
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Add)
  btnUpload($event: any) {
    const file: File = $event.target.files[0];
    if (!file) {
      return;
    }

    const formData = new FormData();
    formData.append('file', file, file.name);

    //檔案上傳
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否上傳?',
      () => {

        const payload = new EntrancePayload<FormData>();
        payload.data = formData;
        payload.dataExport = this.localTableExportTemp;
        payload.success = () => this.btnRender();
        this.store.dispatch(new fromItemActions.uploadAction(payload));
      }
    )));
    //檔案上傳

    //上傳後將檔案清除
    $event.target.value = '';
  }

  //BU下拉選單切換
  onBuChange() {
    this.particular = {};
    this.store.dispatch(new fromItemActions.getItemListTemplateAction({ nodeKey: this.select.nodeKey }));
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

  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event: any) {

    if (this.validSearchForm() == false) return;

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/item-detail',
      params: {
        actionType: ActionType.Add,
        BuID: this.model.NodeID,
      }
    }));
  }



  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender();
        this.store.dispatch(new fromItemActions.disableAction(payload));
      }
    )));

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Admin)
  btnBatchDisable($event: any) {

    const selectedItems = this.table.getSelectItem();
    const searchTotalCount = this.table.getSearchTotalCount();
    
    if (searchTotalCount == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('目前查無資料')));
      return;
    }

    if (selectedItems == null || selectedItems.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage('至少選擇一個項目')));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量停用?',
      () => {

        const payload = new EntrancePayload<Array<ItemListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender();
        this.store.dispatch(new fromItemActions.disableRangeAction(payload));
      }
    )));

  }


  initializeTable() {

    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID'
      },
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'BUName',
        disabled: false,
        order: 'NODE_ID'
      },
      {
        text: this.translateService.instant('ITEM.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('ITEM.DESCRIPTION'),
        name: 'Description',
        disabled: false,
        order: 'DESCRIPTION'
      },
      {
        text: this.translateService.instant('ITEM.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: ''
      },
    ];

    this.columnsExport = [
      {
        text: this.translateService.instant('COMMON.BU_NAME'),
        name: 'BUName',
      },
      {
        text: this.translateService.instant('COMMON.ID'),
        name: 'ID',
      },
    ];

  }

  //驗證查詢表單
  private validSearchForm() {
    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }
    else {
      return true;
    }
  }



}
