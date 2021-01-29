import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CallCenterNodeDetailViewModel, CallCenterNodeViewModel } from 'src/app/model/organization.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { Node } from 'src/app/shared/component/tree/ptc-tree/model/node';
import { PtcTreeComponent } from 'src/app/shared/component/tree/ptc-tree/ptc-tree.component';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';

const PREFIX = 'CallCenterNodeComponent';

@Component({
  selector: 'app-callcenter-node-create',
  templateUrl: './callcenter-node-create.component.html',
  styleUrls: ['./callcenter-node-create.component.scss']
})
export class CallCenterNodeCreateComponent extends FormBaseComponent implements OnInit {


  public btnAddOrgniazationNode: (model, prefixNode) => void;
  @Input() prefixNode: Node<CallCenterNodeViewModel>;
  @Input() uiActionType: ActionType;

  public form: FormGroup;
  model = new CallCenterNodeDetailViewModel();

  constructor(
    public store: Store<any>,
    public activeModal: NgbActiveModal,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeForm();
  }

  closeModel() {
    this.activeModal.close();
  }

  addOrgniazationNode(model, prefixNode) {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.btnAddOrgniazationNode && this.btnAddOrgniazationNode(model, prefixNode)
      }
    )));
  }


  initializeForm() {
    this.model.IsEnabled = true; // 預設資料
    this.form = new FormGroup({
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(20),
      ]),
      IsEnabled: new FormControl(this.model.IsEnabled),
    });
  }

}
