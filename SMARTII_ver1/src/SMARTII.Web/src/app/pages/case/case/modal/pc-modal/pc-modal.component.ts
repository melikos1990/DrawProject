import { Component, OnInit, Injector, Input } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-pc-modal',
  templateUrl: './pc-modal.component.html',
})
export class PcModalComponent extends BaseComponent implements OnInit {

  @Input() sourcekey: string;

  onBtnRelated: any;

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
  btnRelated($event) {
    this.onBtnRelated && this.onBtnRelated($event);
  }
}
