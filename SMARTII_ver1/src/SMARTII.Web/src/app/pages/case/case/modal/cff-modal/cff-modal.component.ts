import { Component, OnInit, Input, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseTemplateListViewModel } from 'src/app/model/master.model';
import { Store } from '@ngrx/store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as fromRootActions from 'src/app/store/actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';

@Component({
  selector: 'app-cff-modal',
  templateUrl: './cff-modal.component.html',
  styleUrls: ['./cff-modal.component.scss']
})
export class CffModalComponent extends FormBaseComponent implements OnInit {


  btnConfirm: any

  @Input() data: CaseTemplateListViewModel[];

  constructor(
    public store: Store<any>,
    public modalService: NgbModal,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {

  }

  btnRowSelect($event: CaseTemplateListViewModel) {
    this.btnConfirm && this.btnConfirm($event);
  }


}
