import { Component, OnInit, Input, Injector } from '@angular/core';
import { NgInputBaseComponent } from 'ptc-dynamic-form';
import { TranslateService } from '@ngx-translate/core';
import { CaseItemTableInput } from '../ppcLife/case-item-table-input';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ItemSelectorComponent } from '../../component/modal/item-selector/item-selector.component';
import { ItemSearchViewModel, ItemListViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from '../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import { PtcSwalType } from 'ptc-swal';
import { CaseService } from '../../service/case.service';
import { BusinesssUnitParameters } from 'src/app/model/organization.model';



@Component({
  selector: 'app-case-item-table-input',
  templateUrl: './case-item-table-input.component.html',
  styleUrls: ['./case-item-table-input.component.scss']
})
export class CaseItemTableInputComponent extends NgInputBaseComponent implements OnInit {
  column = [];
  translateService: TranslateService;

  @Input() input: CaseItemTableInput;

  ref: NgbModalRef;
  items = [];

  private sysParam: BusinesssUnitParameters;

  constructor(
    public modalService: NgbModal,
    public store: Store<fromRootReducers>,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector);

    this.translateService = injector.get(TranslateService);
  }


  ngOnInit() {
    this.initializeTable();
  }

  ngAfterViewInit(): void {

    if ((this.bindingData as object).hasOwnProperty(this.input.id)) {
      this.items = this.bindingData[this.input.id];
    }

    this.caseService.getBUParametersByNodeKey(this.input.nodeKey)
      .subscribe(sysParam => {
        this.sysParam = sysParam;
      })

  }

  addItemModal() {
    this.ref = this.modalService.open(ItemSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = <ItemSelectorComponent>this.ref.componentInstance;
    const model = new ItemSearchViewModel();
    model.NodeID = this.sysParam.BuID;
    instance.model = model;
    instance.multiple = true;
    instance.btnAddItem = (items: ItemListViewModel[]) => {

      if (items.some(x => this.items.some(g => g.ItemID == x.ID))) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction({
          detail: {
            title: this.translateService.instant('ERROR.TITLE'),
            text: `${this.translateService.instant('ERROR.FIELD_VALID_ERROR')} 有商品已加入過`,
            type: PtcSwalType.error,
            showCancelButton: false,
          },
          isLoop: false
        }));
        return;
      }

      const collection = items.map(x => {

        return {
          ItemID: x.ID,
          ItemName: x.Name,
          BatchNo: null,
          ErrorStatus: null,
          PurchaseDay: x.Particular.BatchNo,
          InternationalBarcode: x.Particular.InternationalBarcode,
        }
      })

      this.items = [...collection, ...this.items];
      this.bindingData[this.input.id] = this.items;
      this.ref.dismiss();
    }
  }

  deleteItem(row: any) {
    const index = this.items.findIndex(x => x.ItemID == row.ItemID);
    this.items.splice(index, 1)
    this.items = [...this.items]
    this.bindingData[this.input.id] = this.items;
  }

  initializeTable() {

    this.column = [
      {//商品名稱
        text: this.translateService.instant('ITEM.NAME'),
        name: 'ItemName'
      },
      {//國際條碼
        text: this.translateService.instant('ITEM.PPCLIFE.INTERNATIONAL_BARCODE'),
        name: 'InternationalBarcode'
      },
      {//批號
        text: this.translateService.instant('CASE_COMMON.TABLE.BATCH_NO'),
        name: 'BatchNo',
        customer: true
      },
      {//購買日
        text: this.translateService.instant('CASE_COMMON.TABLE.PURCHASE_DATE'),
        name: 'PurchaseDay',
        customer: true
      },
      {//操作
        text: this.translateService.instant('CASE_COMMON.TABLE.OPERATE'),
        name: 'Operator',
        customer: true
      },
    ];


  }
}
