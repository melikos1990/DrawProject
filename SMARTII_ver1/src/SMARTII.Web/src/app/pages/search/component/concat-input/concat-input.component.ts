import { Component, OnInit, Input, Injector, inject, Output, EventEmitter } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup } from '@angular/forms';
import { SearchBase } from 'src/app/model/search.model';
import { State as fromRootReducers } from 'src/app/store/reducers';
//import * as fromStoresActions from '../../store/actions/stores.actions';
import { Store } from '@ngrx/store';


@Component({
  selector: 'app-concat-input',
  templateUrl: './concat-input.component.html',
  styleUrls: ['./concat-input.component.scss']
})
export class ConcatInputComponent extends FormBaseComponent implements OnInit {

  @Input() form: FormGroup;
  @Input() model: SearchBase;
  @Input() NodeKey: string;
  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();

  constructor(
    public store: Store<fromRootReducers>,
    public injector: Injector
  ) {
    super(injector)
  }

  ngOnInit() {
  }


  _onSelectedChange($event) {
    this.onSelectedChange.emit($event);
  }


  onBuChange() {
    //this.store.dispatch(new fromStoresActions.getStoresListTemplateAction({ nodeKey: this.NodeKey }));
  }
}
