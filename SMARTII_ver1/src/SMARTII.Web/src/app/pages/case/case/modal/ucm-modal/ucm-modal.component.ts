import { Component, OnInit, Input, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { CaseComplainedUserViewModel } from 'src/app/model/case.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ucm-modal',
  templateUrl: './ucm-modal.component.html',
})
export class UcmModalComponent extends BaseComponent implements OnInit {

  onCloseModal: any;
  @Input() sourcekey : string;
  @Input() users: CaseComplainedUserViewModel[] = [];
  @Input() title: string = this.translateService.instant('CASE_COMMON.ADD_COMPLAINEDUSER');
  @Input() organizationSearchTerm: {} = {
    IsEnabled: true,
    IsSelf: true
  }

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
