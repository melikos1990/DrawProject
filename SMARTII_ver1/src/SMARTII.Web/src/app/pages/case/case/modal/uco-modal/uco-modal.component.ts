import { Component, OnInit, Injector, Input } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CaseConcatUserViewModel } from 'src/app/model/case.model';

@Component({
  selector: 'app-uco-modal',
  templateUrl: './uco-modal.component.html'
})
export class UcoModalComponent extends BaseComponent implements OnInit {

  onCloseModal: any;
  @Input() sourcekey : string;
  @Input() users: CaseConcatUserViewModel[] = [];
  @Input() displayWithConcatUser: boolean = false;

  constructor(
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

  closeModel() {
    this.activeModal.close();
  }
  
  btnCheck() {
    this.onCloseModal && this.onCloseModal(this.users);
    this.activeModal.close();
  }

}
