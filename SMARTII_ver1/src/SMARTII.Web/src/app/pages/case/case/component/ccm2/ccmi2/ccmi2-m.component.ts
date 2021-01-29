import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseAssignmentViewModel, CaseAssignmentUserViewModel, CaseViewModel, CaseComplainedUserViewModel } from 'src/app/model/case.model';
import { Guid } from 'guid-typescript';
import { UcmModalComponent } from '../../../modal/ucm-modal/ucm-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CaseService } from 'src/app/shared/service/case.service';


@Component({
  selector: 'app-ccmi2-m',
  templateUrl: './ccmi2-m.component.html',
})
export class Ccmi2MComponent extends FormBaseComponent implements OnInit {


  private _model: CaseAssignmentViewModel = new CaseAssignmentViewModel();

  @Output() SetUsers = new EventEmitter<CaseComplainedUserViewModel[]>();//20201028OP需求-需將轉派單位人員(含父節點以上)帶入Mail清單

  @Input() sourcekey: string;
  @Input() case: CaseViewModel;
  sameOfComplaintedUser: boolean = false;

  @Input() isGetAssignmentUser: boolean;//20201028OP需求-需將轉派單位人員(含父節點以上)帶入Mail清單

  @Input() set model(v: CaseAssignmentViewModel) {
    this._model = v;
    this.initializePayload();
  }
  get model(): CaseAssignmentViewModel {
    return this._model
  }

  public get responsibilityUsers(): CaseAssignmentUserViewModel[] {
    return this.model.CaseAssignmentUsers.filter(x => x.CaseComplainedUserType == this.caseComplainedUserType.Responsibility);
  }

  public get NoticeUsers(): CaseAssignmentUserViewModel[] {
    return this.model.CaseAssignmentUsers.filter(x => x.CaseComplainedUserType == this.caseComplainedUserType.Notice);
  }


  constructor(
    public modalService: NgbModal,
    public injector: Injector,
    private caseService: CaseService) {
    super(injector)
    this.initializePayload();
  }

  btnSameOfComplaintedUser($event) {
    if (this.sameOfComplaintedUser === true) {
      this.model.CaseAssignmentUsers = this.case.CaseComplainedUsers.map(c => this.toCaseAssignmentUsers(c));
    }

  }

  initializePayload() {
    this.model.CaseAssignmentUsers = new Array<CaseAssignmentUserViewModel>();
  }

  btnDeleteUser(user: CaseAssignmentUserViewModel) {
    const index = this.model.CaseAssignmentUsers.findIndex(x => x.key === user.key);
    this.model.CaseAssignmentUsers.splice(index, 1);
  }

  btnAddUnitModal() {
    const ref = this.modalService.open(UcmModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    const instance = <UcmModalComponent>ref.componentInstance;
    instance.sourcekey = this.sourcekey;
    instance.title = "新增轉派對象";
    instance.users = this.model.CaseAssignmentUsers.map(c => this.toCaseComplainedUsers(c));
    instance.onCloseModal = (users: CaseComplainedUserViewModel[]) => {
      let newusers = users.map(c => this.toCaseAssignmentUsers(c));
      //newusers = newusers.filter(x => this.model.CaseAssignmentUsers.every(z => z.NodeID != x.NodeID));
      this.model.CaseAssignmentUsers = newusers;

      
      if(this.isGetAssignmentUser){
        this.GetAssignmentUserList(newusers);
      }
      
    }
  }

  //20201028OP需求-需將轉派單位人員(含父節點以上)帶入Mail清單
  GetAssignmentUserList(data: CaseAssignmentUserViewModel[]) {
    this.caseService
      .getNodeAllUsers(data)
      .subscribe(x => {
        if (x.isSuccess && x.element.length > 0) {
          this.SetUsers.emit(x.element);
        }

      })
  }





  toCaseAssignmentUsers(user: CaseComplainedUserViewModel) {
    const result = new CaseAssignmentUserViewModel();

    result.BUID = user.BUID;
    result.BUName = user.BUName;
    result.CaseComplainedUserType = user.CaseComplainedUserType;
    result.CaseID = user.CaseID;
    result.JobID = user.JobID;
    result.JobName = user.JobName;
    result.NodeID = user.NodeID;
    result.NodeName = user.NodeName;
    result.OrganizationType = user.OrganizationType;
    result.OrganizationTypeName = user.OrganizationTypeName;
    result.ParentPathName = user.ParentPathName;
    result.UnitType = user.UnitType;
    result.UserID = user.UserID;
    result.UserName = user.UserName;
    result.StoreNo = user.StoreNo;
    result.key = Guid.create().toString();

    return result;
  }

  toCaseComplainedUsers(user: CaseAssignmentUserViewModel) {
    const result = new (CaseComplainedUserViewModel);

    result.BUID = user.BUID;
    result.BUName = user.BUName;
    result.CaseComplainedUserType = user.CaseComplainedUserType;
    result.CaseID = user.CaseID;
    result.JobID = user.JobID;
    result.JobName = user.JobName;
    result.NodeID = user.NodeID;
    result.NodeName = user.NodeName;
    result.OrganizationType = user.OrganizationType;
    result.OrganizationTypeName = user.OrganizationTypeName;
    result.ParentPathName = user.ParentPathName;
    result.UnitType = user.UnitType;
    result.UserID = user.UserID;
    result.UserName = user.UserName;
    result.StoreNo = user.StoreNo;
    result.key = Guid.create().toString();

    return result;
  }

  ngOnInit() {
  }

}
