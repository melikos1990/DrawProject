import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ad-password',
  templateUrl: './ad-password.component.html',
  styleUrls: ['./ad-password.component.scss']
})
export class AdPasswordComponent extends BaseComponent implements OnInit {

  public adPassword: string;
  public btnValidAD: any

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

  validAD() {
    if (this.btnValidAD) {
      this.btnValidAD(this.adPassword, this.activeModal);
    }
  }


}
