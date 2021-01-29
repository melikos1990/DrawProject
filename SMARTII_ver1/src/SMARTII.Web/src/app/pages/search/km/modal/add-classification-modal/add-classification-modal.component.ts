import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { KMClassificationNodeViewModel } from 'src/app/model/master.model';
import * as fromRootActions from 'src/app/store/actions';
import { Store } from '@ngrx/store';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, Validators, FormControl } from '@angular/forms';

const PREFIX = 'KmComponent';
@Component({
  selector: 'app-add-classification-modal',
  templateUrl: './add-classification-modal.component.html',
})
export class AddClassificationModalComponent extends FormBaseComponent implements OnInit {


  public btnAddNode: (name, node) => void;
  public form: FormGroup;

  name: string = '';
  @Input() node: KMClassificationNodeViewModel = new KMClassificationNodeViewModel();

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

  addNode(name, node) {
    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.btnAddNode && this.btnAddNode(name, node)
  }


  initializeForm() {

    this.form = new FormGroup({
      name: new FormControl(name, [
        Validators.required,
        Validators.maxLength(20),
      ]),

    });
  }

}
