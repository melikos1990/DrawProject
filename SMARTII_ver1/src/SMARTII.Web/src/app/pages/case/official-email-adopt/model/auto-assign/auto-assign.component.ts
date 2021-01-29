import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { OfficialEmailAutoOrderViewModel, OfficialEmailAdoptResult } from 'src/app/model/case.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CallcenterNodeTreeUserSelectorComponent } from 'src/app/shared/component/modal/tree-user/callcenter-node-tree-user-selector/callcenter-node-tree-user-selector.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromOfficialEmailAdoptActions from 'src/app/pages/case/store/actions/official-email-adopt.actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { successAlertAction } from 'src/app/shared/ngrx/alert.ngrx';
import { PositiveNumberValidator } from 'src/app/shared/data/validator';

@Component({
  selector: 'app-auto-assign',
  templateUrl: './auto-assign.component.html',
  styleUrls: ['./auto-assign.component.scss']
})
export class AutoAssignComponent extends FormBaseComponent implements OnInit {
  
  form: FormGroup;
  model: OfficialEmailAutoOrderViewModel = {} as OfficialEmailAutoOrderViewModel;

  selectedUsers: any[] = [];

  columns: any[] = [];
  @ViewChild('selector') selector: TemplateRef<any>;

  selectUserRef: NgbModalRef;

  onRefrach: any;

  constructor(
    public injector: Injector,
    public modalService: NgbModal,
    public store: Store<fromCaseReducer>
  ) { 
    super(injector);
  }

  ngOnInit() {
    this.initializeForm();
    this.initializeTable();
  }


  initializeForm() {
    this.form = new FormGroup({
      EachPersonMail: new FormControl(null, [
        Validators.required,
        PositiveNumberValidator
      ]),
      UserIDs: new FormControl(null, [
        Validators.required
      ])
    })
  }

  btnDeleteUser(event) {

    let _users = [...this.selectedUsers];
    
    this.selectedUsers = _users.filter(x => x.UserID != event.UserID);

  }


  btnConfirm(){

    if(!this.selectedUsers || this.selectedUsers.length <= 0){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("需選擇一位對象")));
      return false;
    }
    
    this.model.UserIDs = this.selectedUsers.map(x => x.UserID);
    this.form.controls["UserIDs"].patchValue(this.model.UserIDs);


    if(this.validForm(this.form) === false){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }


    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否指派?',
      () => {
        let payload = new EntrancePayload(this.model);

        payload.success = this.ajaxSuccess.bind(this);
        
        this.store.dispatch(new fromOfficialEmailAdoptActions.autoAssignEmail(payload));
      }
    )));

  }

  btnClose(){
    this.modalService.dismissAll();
  }

  ajaxSuccess(){
    this.btnClose();
    this.onRefrach && this.onRefrach();
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.USER_NAME'),
        name: 'UserName'
      },
      {
        text: this.translateService.instant('NOTIFICATION_GROUP.ACCOUNT'),
        name: 'Account',
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
      }
    ];
  }

  btnUserSelected(){
    const ref = this.modalService.open(this.selector, { size: 'lg', container: 'nb-layout', backdrop: 'static', windowClass: 'modal-xl' });
    this.selectUserRef = ref;

  }
  

  btnModalAdd(tempRef: CallcenterNodeTreeUserSelectorComponent){

    let users = tempRef.getValue();

    this.selectedUsers = this.distinct([...this.selectedUsers], [...users], x => x.UserID);
    
    console.log("selectedUsers => ", this.selectedUsers);

    if(this.selectUserRef) this.selectUserRef.dismiss();

  }

  private distinct(src: any[], dest: any[], compare: (data) => any) {
    let keys: any[] = [];
    let result: any[] = [];

    src.forEach(data => {

      if(!keys.includes(compare(data))){
        keys.push(compare(data));
        result.push(data);
      }

    })

    dest.forEach(data => {

      if(!keys.includes(compare(data))){
        keys.push(compare(data));
        result.push(data);
      }

    })

    return result;
  }

}
