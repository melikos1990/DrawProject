import { Component, OnInit, Injector, Input, ViewChild, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseConcatUserViewModel } from 'src/app/model/case.model';
import { PtcLocalTableComponent } from 'ptc-local-table';
import { ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-ucot',
  templateUrl: './ucot.component.html',
})
export class UcotComponent extends FormBaseComponent implements OnInit, OnChanges {


  @ViewChild('unitTable') unitTableRef: PtcLocalTableComponent;
  @ViewChild('customerTable') customerTableRef: PtcLocalTableComponent;

  customerData: CaseConcatUserViewModel[] = [];
  unitData: CaseConcatUserViewModel[] = [];

  customerColumns: any[] = [];
  unitColumns: any[] = [];

  @Input() isShowDelete: boolean = true;
  @Input() uiActionType: ActionType;
  @Input() users: CaseConcatUserViewModel[] = [];
  @Output() usersChange = new EventEmitter();
  @Output() customerRowSelect: EventEmitter<CaseConcatUserViewModel> = new EventEmitter();
  @Output() unitRowSelect: EventEmitter<CaseConcatUserViewModel> = new EventEmitter();

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.initializeCustomerTable();
    this.initUnitTable();
    this.initTable();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["users"] && changes["users"].currentValue) {
      this.refillTables();
    }
  }

  btnRemoveUnitUser(row: CaseConcatUserViewModel) {
    const index = this.unitData.findIndex(x => x.key == row.key);
    this.unitData.splice(index, 1);
    this.unitData = [...this.unitData]
    this.usersChange.emit(this.concat());
  }
  btnRemoveCustomerUser(row: CaseConcatUserViewModel) {
    const index = this.customerData.findIndex(x => x.key == row.key);
    this.customerData.splice(index, 1);
    this.customerData = [...this.customerData]
    this.usersChange.emit(this.concat());
  }
  concat() {
    return [...this.customerData, ...this.unitData];
  }

  refillTables() {
    this.customerData = [...this.users.filter(x => x.UnitType == this.unitType.Customer)];
    this.unitData = [...this.users.filter(x => x.UnitType == this.unitType.Organization || x.UnitType == this.unitType.Store)];
  }


  onCustomerRowSelect($event: CaseConcatUserViewModel) {
    this.customerRowSelect.emit($event);
  }

  onUnitRowSelect($event: CaseConcatUserViewModel) {
    this.unitRowSelect.emit($event)
  }

  initializeCustomerTable() {
    this.customerColumns = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.UNIT_TYPE'),
        name: 'UnitType',
        customer: true
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CUSTOMER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.GENDER'),
        name: 'Gender',
        customer: true
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.PHONE'),
        name: 'Mobile'
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.TELEPHONE', { number: 1 }),
        name: 'Telephone',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.TELEPHONE', { number: 2 }),
        name: 'TelephoneBak',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.EMAIL'),
        name: 'Email',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.ADDRESS'),
        name: 'Address',
      },
    ]
  }

  initUnitTable() {
    this.unitColumns = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.UNIT_TYPE'),
        name: 'UnitType',
        customer: true
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.NODE_NAME'),
        name: 'NodeName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.ORGANIZATION'),
        name: 'ParentPathName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CONCATUSER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CONCATUSER_PHONE'),
        name: 'Telephone',
      },
    ]
  }

  initTable(){

    if (this.isShowDelete) {
      let operator = [
        {
          text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
          name: 'del',
          customer: true
        }
      ];
      this.customerColumns = this.customerColumns.concat(operator);
      this.unitColumns = this.unitColumns.concat(operator);
    }

  }

}
