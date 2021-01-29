import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { BillboardSearchViewModel, BillboardListViewModel } from 'src/app/model/master.model';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromBillboardActions from '../../store/actions/billboard.actions';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';
import { FormGroup, FormControl, Validators } from '@angular/forms';

const PREFIX = 'BillboardComponent';

@Component({
  selector: 'app-billboard',
  templateUrl: './billboard.component.html',
  styleUrls: ['./billboard.component.scss']
})
export class BillboardComponent extends FormBaseComponent implements OnInit {



  @ViewChild('table')
  table: ServerTableComponent;

  isEnable: boolean = false;

  form: FormGroup;

  columns: any[] = [];

  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model: BillboardSearchViewModel = new BillboardSearchViewModel();

  constructor(
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeTable();
    this.initFormGroup();
  }

  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  initFormGroup() {
    this.form = new FormGroup({
      FirstActivateDateTimeRange: new FormControl(),
      Content: new FormControl()
    })
  }
  /**
   * 將物件傳出之前 , 加工 payload 送回server
   */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  btnRender($event: any) {
    
    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }


  /**
   * 按鈕按下新增
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  btnAdd($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/billboard-detail',
      params: {
        actionType: ActionType.Add,
      }
    }));
  }
  /**
  * 當ptc server table 按下刪除
  */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  onBtnDelete($event: any) {


    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,
        };
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromBillboardActions.deleteAction(payload));
      }
    )));

  }
  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  @SearchCacheMethod(PREFIX)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/billboard-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
      }
    }));
  }

  /**
   * 按鈕按下刪除
   * @param $event
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Delete)
  btnBatchDelete($event: any) {

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

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量刪除?',
      () => {

        const payload = new EntrancePayload<Array<BillboardListViewModel>>();
        payload.data = selectedItems;
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromBillboardActions.deleteRangeAction(payload));
      }
    )));

  }




  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  @SearchCacheMethod(PREFIX)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/master/billboard-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
      }
    }));
  }


  /**
   * 初始化Table資訊
   */
  initializeTable() {

    this.ajax.url = 'Master/Billboard/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('COMMON.ALL_CHECK'),
        name: 'p-check',
        disabled: true,
        order: 'ID', 
      },
      {
        text: this.translateService.instant('BILLBOARD.TITLE'),
        name: 'Title',
        disabled: false,
        order: 'TITLE',
        customer: true
      },
      {
        text: this.translateService.instant('BILLBOARD.CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT',
        customer: true
      },
      {
        text: this.translateService.instant('BILLBOARD.BILLBOARD_WARNING_TYPE'),
        name: 'BillboardWarningTypeName',
        disabled: false,
        order: 'WARNING_TYPE'
      },
      {
        text: this.translateService.instant('BILLBOARD.NAME'),
        name: 'Name',
        order: 'CREATE_USERNAME',
        disabled: false,
      },
      {
        text: this.translateService.instant('BILLBOARD.ACTIVE_DATETIME_RANGE'),
        name: 'ActiveDateTimeRange',
        disabled: false,
        order: 'ACTIVE_DATE_START'
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: 'ID'
      },
    ];


  }

}
