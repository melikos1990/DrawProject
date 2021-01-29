import { Component, OnInit, Input, Output, EventEmitter, Injector } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup } from '@angular/forms';
import { SearchBase } from 'src/app/model/search.model';

@Component({
  selector: 'app-complained-input',
  templateUrl: './complained-input.component.html',
  styleUrls: ['./complained-input.component.scss']
})
export class ComplainedInputComponent extends FormBaseComponent implements OnInit {

  @Input() form: FormGroup;
  @Input() model: SearchBase;

  @Output() onSelectedChange: EventEmitter<any> = new EventEmitter();

  filter: (data: any) => boolean = (data) => {
    return data.id != this.unitType.Customer
  }

  constructor(
    public injector: Injector
  ) { 
    super(injector)
  }

  ngOnInit() {
  }


  _onSelectedChange($event){
    this.onSelectedChange.emit($event);
  }
}
