import { Component, OnInit, Input, Injector } from '@angular/core';
import { OfficialEmailAdminOrderViewModel, OfficialEmailAdoptResult } from 'src/app/model/case.model';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EntrancePayload } from 'src/app/model/common.model';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../../store/reducers';
import * as fromOfficialEmailAdoptActions from 'src/app/pages/case/store/actions/official-email-adopt.actions';

@Component({
  selector: 'app-admin-assign',
  templateUrl: './admin-assign.component.html',
  styleUrls: ['./admin-assign.component.scss']
})
export class AdminAssignComponent extends FormBaseComponent implements OnInit {

  @Input() selectedEmails: any[] = [];
  @Input() model: OfficialEmailAdminOrderViewModel = {} as OfficialEmailAdminOrderViewModel;

  
  form: FormGroup;
  columns: any[] = [];

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

  btnConfirm(){
    
    this.model.MessageIDs = this.selectedEmails.map(x => x.MessageID);
    this.form.controls["MessageIDs"].patchValue(this.model.MessageIDs);

    if(this.validForm(this.form) === false){
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return false;
    }


    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否指派?',
      () => {
        let payload = new EntrancePayload(this.model);
    
        payload.success = this.ajaxSuccess.bind(this);
        
        this.store.dispatch(new fromOfficialEmailAdoptActions.adminAssignEmail(payload));
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

  btnDeleteUser(event) {

    let _emails = [...this.selectedEmails];
    
    this.selectedEmails = _emails.filter(x => x.MessageID != event.MessageID);

  }


  
  initializeForm() {
    this.form = new FormGroup({
      MessageIDs: new FormControl(null, [
        Validators.required
      ]),
      UserID: new FormControl(this.model.MessageIDs, [
        Validators.required
      ])
    })
  }

  /**
   * 初始化Table資訊
   */
  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_SUBJECT'),
        name: 'Subject',
        disabled: false,
        order: 'NODE_ID',
        customer: true,
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_SENDER'),
        name: 'FromName',
        disabled: true,
        order: 'ACCOUNT'
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_EMAIL'),
        name: 'FromAddress',
        disabled: true,
        order: ''
      },
      {
        text: this.translateService.instant('OFFICIAL_EMAIL_ADOPT.TABLE_CONTENT'),
        name: 'Body',
        disabled: true,
        order: '',
        customer: true
      },
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: '',
      },
    ];
  }

}
