import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'src/app/model/authorize.model';
import { ChangeInfo } from 'ptc-select2';


@Component({
  selector: 'app-user-selector',
  templateUrl: './user-selector.component.html',
  styleUrls: ['./user-selector.component.scss']
})
export class UserSelectorComponent extends BaseComponent implements OnInit {


  public btnAddUser: any;

  public selectUser: User;

  constructor(
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

  addUser() {
    if (this.btnAddUser) {
      this.btnAddUser(this.selectUser);
    }
  }
  closeModel() {
    this.activeModal.close();
  }

  onItemChange($event: ChangeInfo) {
    if ($event.item !== undefined)
      this.selectUser = $event.item.extend;
  }

}
