import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-setting-modal',
  templateUrl: './setting-modal.component.html',
  styleUrls: ['./setting-modal.component.scss']
})
export class SettingModalComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal) { }
  payload: any;
  saveHandler: any;
  closeHandler: any;
  keys = [];

  ngOnInit() {
    this.keys = Object.keys(this.payload);
  }
  save() {
    this.activeModal.close();
    this.saveHandler && this.saveHandler('ok');
  }
  close() {
    this.activeModal.close();
    this.closeHandler && this.closeHandler('ng');
  }

}
