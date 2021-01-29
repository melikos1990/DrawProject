import { Component, OnInit, Injector, Input, OnChanges, SimpleChanges, ViewChild, Output, EventEmitter, KeyValueDiffers, KeyValueDiffer, ChangeDetectionStrategy } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseConcatUserViewModel } from 'src/app/model/case.model';
import { UcoiComponent } from './ucoi/ucoi.component';
import { Guid } from 'guid-typescript';
import { ActionType } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import * as fromRootActions from "src/app/store/actions";
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'app-uco',
  templateUrl: './uco.component.html',
})
export class UcoComponent extends FormBaseComponent implements OnInit {

  private differ: KeyValueDiffer<CaseConcatUserViewModel[], any>;

  @ViewChild('ucoi') ucoiRef: UcoiComponent;
  @Input() isShowDelete: boolean = true;
  @Input() uiActionType: ActionType = this.actionType.Add;
  @Input() sourcekey: string;
  @Input() visibleInput: boolean = true;
  @Input() visibleTable: boolean = true;
  @Input() displayWithConcatUser: boolean = true;
  @Input() form: FormGroup = new FormGroup({});
  @Input() concatUsers: CaseConcatUserViewModel[] = [];
  @Output() concatUsersChange = new EventEmitter();

  // 目前輸入項中的
  // 此區隔主要是區分在新增行為時 , 表格與輸入項目為分格開的
  @Input() onSetUser: CaseConcatUserViewModel = new CaseConcatUserViewModel();


  constructor(
    public differs: KeyValueDiffers,
    private store: Store<any>,
    public injector: Injector) {
    super(injector);
  }

  finder = (x: CaseConcatUserViewModel) => {

    if (x.key) {
      return x.key == this.onSetUser.key
    }

    if (x.ID) {
      return x.ID == this.onSetUser.ID
    }

  }

  ngOnInit() {
    this.differ = this.differs.find(this.concatUsers).create();
  }


  btnAddUser() {
    console.log("onSetUser => ", this.onSetUser);
    if (!this.valid(this.onSetUser)) return;

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    // 避免 instance 重複
    this.concatUsers = [...this.concatUsers];
    // 避免新增入集合中 , instance 重複
    const isolateUser = { ...this.onSetUser };
    isolateUser.key = Guid.create().toString();
    this.concatUsers.push(isolateUser);
    this.ucoiRef.resetUI();

  }


  resetUI() {
    this.ucoiRef.resetUI();
  }

  btnUpdateUser() {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    const index = this.concatUsers.findIndex(this.finder);
    this.concatUsers[index] = { ...this.onSetUser }
    this.concatUsers = [...this.concatUsers];
    this.ucoiRef.resetUI();
  }


  onCustomerRowSelect($event: CaseConcatUserViewModel) {
    if (this.uiActionType == this.actionType.Read) { return; }

    this.onSetUser = { ...$event };
    console.log(this.onSetUser);
    this.uiActionType = this.actionType.Update;
  }

  onUnitRowSelect($event: CaseConcatUserViewModel) {
    if (this.uiActionType == this.actionType.Read) { return; }
    
    this.onSetUser = { ...$event };
    console.log(this.onSetUser);
    this.uiActionType = this.actionType.Update;
  }


  ngDoCheck(): void {
    const changes = this.differ.diff(this.concatUsers);
    if (changes) {
      this.concatUsersChange.emit(this.concatUsers)
    }
  }

  public valid(concatUser: CaseConcatUserViewModel) {
    
    let result = true, payload;
    switch (concatUser.UnitType) {
      case this.unitType.Customer:

        if (!!(concatUser.UserName) == false) {
          payload = this.getFieldInvalidMessage("[案件反應者]消費者姓名必須輸入");
          result = false;
        }

        if (concatUser.Gender == null || concatUser.Gender == undefined) {
          payload = this.getFieldInvalidMessage("[案件反應者]消費者性別必須輸入");
          result = false;
        }

        break;

      case this.unitType.Store:
        result = !!(concatUser.NodeName);
        payload = result ? null : this.getFieldInvalidMessage("[案件反應者]門市必須選擇");
        break;

      case this.unitType.Organization:
        result = !!(concatUser.NodeName);
        payload = result ? null : this.getFieldInvalidMessage("[案件反應者]單位必須選擇");
        break;

      default:
        result = false;
        payload = result ? null : this.getFieldInvalidMessage("[案件反應者]請選擇類型");
        break;
    }


    !!(payload) && this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(payload));
    return result;
  }

}
