import { Component, OnInit, Input, Injector } from '@angular/core';
import { NgInputBaseComponent } from 'ptc-dynamic-form';
import { TranslateService } from '@ngx-translate/core';
import { IccCaseItemTableInput } from '../icc/icc-case-item-table-input';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { State as fromRootReducers } from '../../../store/reducers';
import { CaseService } from '../../service/case.service';
import { BusinesssUnitParameters } from 'src/app/model/organization.model';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-icc-case-item-input',
  templateUrl: './icc-case-item-input.component.html',
  styleUrls: ['./icc-case-item-input.component.scss']
})
export class IccCaseItemInputComponent extends NgInputBaseComponent implements OnInit {
  column = [];
  translateService: TranslateService;

  @Input() input: IccCaseItemTableInput;

  items = [];

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
  }

  addItemModal() {
    const collection = [{
      ID: Guid.create().toString(),
      CardNumber: null,
    }];

    this.items = [...collection, ...this.items];
    this.bindingData[this.input.id] = this.items;
  }

  deleteItem(row: any) {
    const index = this.items.findIndex(x => x.ID == row.ID);
    this.items.splice(index, 1)
    this.items = [...this.items]
    this.bindingData[this.input.id] = this.items;
  }

  initializeTable() {

    this.column = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CARD_ID'),
        name: 'CardNumber',
        customer: true
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.OPERATE'),
        name: 'Operator',
        customer: true
      },
    ];
  }
}

