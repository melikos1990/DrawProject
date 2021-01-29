import { Component, OnInit, Injector, ViewChild, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { PtcLocalTableComponent } from 'ptc-local-table';
import { CaseComplainedUserViewModel } from 'src/app/model/case.model';
import { Action } from 'rxjs/internal/scheduler/Action';
import { ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-umct',
  templateUrl: './umct.component.html',
})
export class UmctComponent extends FormBaseComponent implements OnInit {

  @ViewChild('unitTable') unitTableRef: PtcLocalTableComponent;

  unitColumns: any[] = [];
  unitData: CaseComplainedUserViewModel[] = [];

  @Input() uiActionType: ActionType;
  @Input() users: CaseComplainedUserViewModel[] = [];
  @Output() usersChange = new EventEmitter();
  @Output() rowSelect: EventEmitter<CaseComplainedUserViewModel> = new EventEmitter();

  constructor(public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initUnitTable();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["users"] && changes["users"].currentValue) {
      this.refillTables();
    }
  }

  refillTables() {
    this.unitData = [...this.users.filter(x => x.UnitType == this.unitType.Organization || x.UnitType == this.unitType.Store)].map((x: any) => {
      x["CaseComplainedUserType"] = x.CaseComplainedUserType.toString();
      return x;
    });
  }

  btnRemoveUnitUser(row: CaseComplainedUserViewModel) {

    const index = row.key ?
                  this.unitData.findIndex(x => x.key == row.key) :
                  this.unitData.findIndex(x => x.NodeID == row.NodeID);

    this.unitData.splice(index, 1);
    this.unitData = [...this.unitData]
    this.usersChange.emit(this.unitData);
  }

  onRowSelect($event) {
    console.log($event);
    this.rowSelect.emit($event);
  }

  initUnitTable() {
    this.unitColumns = [
      {//類型
        text: this.translateService.instant('CASE_COMMON.TABLE.UNIT_TYPE'),
        name: 'UnitType',
        customer: true
      },
      {//名稱
        text: this.translateService.instant('CASE_COMMON.TABLE.NODE_NAME'),
        name: 'NodeName',
      },
      {//型態
        text: this.translateService.instant('CASE_COMMON.TABLE.TYPE'),
        name: 'type',
        customer: true
      },
      {//組織
        text: this.translateService.instant('CASE_COMMON.TABLE.ORGANIZATION'),
        name: 'ParentPathName',
      },
      {//負責人
        text: this.translateService.instant('CASE_APPLY.APPLY_USER_ID'),
        name: 'OwnerUserName',
      },
      {//系統操作
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'del',
        customer: true
      },
    ]
  }
}
