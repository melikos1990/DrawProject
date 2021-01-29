import { Component, OnInit, Injector, Input, Output, EventEmitter, SimpleChanges, OnChanges, ViewChild, KeyValueDiffer, KeyValueDiffers, ChangeDetectionStrategy } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseComplainedUserViewModel } from 'src/app/model/case.model';
import { UnitType } from 'src/app/model/organization.model';
import { Guid } from 'guid-typescript';
import { UcmiComponent } from './ucmi/ucmi.component';
import { ActionType } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';


@Component({
  selector: 'app-ucm',
  templateUrl: './ucm.component.html',
  //changeDetection: ChangeDetectionStrategy.OnPush
})
export class UcmComponent extends FormBaseComponent implements OnInit {

  private differ: KeyValueDiffer<CaseComplainedUserViewModel[], any>;

  @ViewChild('ucmi') ucmiRef: UcmiComponent;

  @Input() uiActionType: ActionType = this.actionType.Add;
  @Input() sourcekey: string;
  @Input() visibleInput: boolean = true;
  @Input() visibleTable: boolean = true;
  @Input() needResponsibility: boolean = true;
  @Input() allowMultiResponsibility: boolean = false;
  @Input() organizationSearchTerm: any = {NotIncludeDefKey: [this.definitionKey.STORE], IsEnabled: true};

  @Input() complainedUsers: CaseComplainedUserViewModel[] = [];
  @Output() complainedUsersChange = new EventEmitter();

  // 目前輸入項中的
  // 此區隔主要是區分在新增行為時 , 表格與輸入項目為分格開的
  @Input() onSetUser: CaseComplainedUserViewModel = new CaseComplainedUserViewModel();


  constructor(
    public differs: KeyValueDiffers,
    public injector: Injector,
    public store: Store<fromCaseReducer>, ) {
    super(injector);
  }

  finder = (x: CaseComplainedUserViewModel) => {

    if (x.key) {
      return x.key == this.onSetUser.key
    }

    if (x.ID) {
      return x.ID == this.onSetUser.ID
    }

  }

  ngOnInit() {
    this.differ = this.differs.find(this.complainedUsers).create();
  }

  btnAddUser() {
    debugger;
    
    console.log("this.complainedUsers ==> ", this.complainedUsers);

    if(!this.valid(this.onSetUser, [...this.complainedUsers.concat([{...this.onSetUser}])] )) return;

    // 避免 instance 重複
    this.complainedUsers = [...this.complainedUsers];
    console.log("complainedUsers => ", this.complainedUsers);

    // 避免新增入集合中 , instance 重複
    const isolateUser = { ...this.onSetUser };
    console.log("isolateUser => ", isolateUser);


    isolateUser.key = Guid.create().toString();
    this.complainedUsers.push(isolateUser);
    this.complainedUsersChange.emit(this.complainedUsers)
    this.ucmiRef.resetUI();
  }

  btnUpdateUser() {
    const index = this.complainedUsers.findIndex(this.finder);
    this.complainedUsers[index] = { ...this.onSetUser }
    this.complainedUsers = [...this.complainedUsers];
    this.complainedUsersChange.emit(this.complainedUsers)
    this.ucmiRef.resetUI();
  }

  resetUI() {
    this.ucmiRef.resetUI();

  }

  onRowSelect($event) {
    if (this.uiActionType == this.actionType.Read) { return; }
    
    this.onSetUser = { ...$event, CaseComplainedUserType: parseInt($event.CaseComplainedUserType) };
    console.log(this.onSetUser);
    this.uiActionType = this.actionType.Update;
  }
  ngDoCheck(): void {

    const changes = this.differ.diff(this.complainedUsers);
    if (changes) {
      this.complainedUsersChange.emit(this.complainedUsers)
    }
  }

  /**
   * 驗證被反應者
   * @param complainedUser 加入的被反應者
   * @param totallComplainedUser 所有的被反應者(外部Conponent用的 cs1/cs2)
   */
  public valid(complainedUser: CaseComplainedUserViewModel, totallComplainedUser: CaseComplainedUserViewModel[]) {
    
    let result, payload;
    
    switch (complainedUser.UnitType) {

      case this.unitType.Store:
        result = !!(complainedUser.NodeName);
        payload = result ? null : this.getFieldInvalidMessage("[案件被反應者]門市必須選擇");
        break;

      case this.unitType.Organization:
        result = !!(complainedUser.NodeName);
        payload = result ? null : this.getFieldInvalidMessage("[案件被反應者]單位必須選擇");
        break;
    }
    
    if(complainedUser.UnitType != null && complainedUser.UnitType != undefined){

      if(complainedUser.CaseComplainedUserType == null || complainedUser.CaseComplainedUserType == undefined){
        payload = this.getFieldInvalidMessage("[案件被反應者]權責/知會必須選擇");
        result = false;
      }
      else if(!totallComplainedUser.some(x => x.CaseComplainedUserType == this.caseComplainedUserType.Responsibility) && this.needResponsibility){
        payload = this.getFieldInvalidMessage("[案件被反應者]必須要有一個權責單位")
        result = false;
      }
      // 判斷是否重複新增
      else if(totallComplainedUser.filter(x => x.NodeID == complainedUser.NodeID).length > 1 ){
        payload = this.getFieldInvalidMessage(`[案件被反應者]${complainedUser.NodeName} 已重複加入`)
        result = false;
      }
      // 判斷 是否多個權責單位
      else if (
        !this.allowMultiResponsibility &&
        complainedUser.CaseComplainedUserType == this.caseComplainedUserType.Responsibility &&
        totallComplainedUser.some(x => 
            x.CaseComplainedUserType == this.caseComplainedUserType.Responsibility &&
            x.NodeID != complainedUser.NodeID)
      ){
        payload = this.getFieldInvalidMessage("[案件被反應者]不可選擇多個權責單位")
        result = false;
      }

    }

    
    !!(payload) && this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(payload));
    return result;
  }


}
