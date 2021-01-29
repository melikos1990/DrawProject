import { Component, OnInit, Injector, ViewChild, Input } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { ItemSearchViewModel } from 'src/app/model/master.model';
import { ServerTableComponent } from '../../table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { commonBu } from 'src/global';
import { FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../../../store/reducers';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import * as fromRootActions from 'src/app/store/actions';
import { LocalTableComponent } from '../../table/local-table/local-table.component';
import { HttpService } from 'src/app/shared/service/http.service';

@Component({
  selector: 'app-item-selector',
  templateUrl: './item-selector.component.html',
  styleUrls: ['./item-selector.component.scss']
})
export class ItemSelectorComponent extends FormBaseComponent implements OnInit {

  @Input() multiple: boolean = false;

  form = new FormGroup({});
  model: ItemSearchViewModel = new ItemSearchViewModel();
  public nodeID: number;

  @ViewChild('table')
  table: LocalTableComponent;


  isEnableResult: boolean = false;
  columns: any[] = [];
  data: any[] = [];
  loading: boolean = false;

  items: any[] = [
    { id: true, text: "啟用" },
    { id: false, text: "停用" },
  ]

  public btnAddItem: any;

  constructor(
    public activeModal: NgbActiveModal,
    private store: Store<fromMasterReducer>,
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }


  ngOnInit() {
    this.initializeTable();
    this.model.IsEnabled = true;
  }

  closeModel() {
    this.activeModal.close();
  }

  addItem() {

    let datas: any[] = this.table.getSelectItem();

    if (!datas || datas.length <= 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("至少選擇一項商品")));
      return;
    }

    if (this.multiple) {
      this.btnAddItem && this.btnAddItem(datas);
    }
    else {
      if (datas.length > 1) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("只能選擇一項商品")));
      }
      else {
        this.btnAddItem && this.btnAddItem(datas[0]);
      }
    }
  }


  @loggerMethod()
  btnRender() {
    //開啟查詢結果
    this.isEnableResult = null;
    
    this.getList();
  }

  getList() {
    this.loading = true;
    this.http.post(`COMMON_BU/Item/GetList/`, null, this.model).subscribe((resp: any) => {

      this.data = resp.element;
      if (!resp.isSuccess) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(resp.message)));
      }

      this.loading = false;
    });
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
        text: this.translateService.instant('ITEM.ITEM_CODE'),
        name: 'Code',
        disabled: false,
        order: 'CODE'
      },
      {
        text: this.translateService.instant('ITEM.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('ITEM.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
    ];



  }

  @loggerMethod()
  btnBack($event: any) {
    this.activeModal.close();
  }
  ngAfterViewInit()
  {
    this.btnRender();
  }
}
