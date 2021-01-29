import { Component, OnInit, Input, Injector } from '@angular/core';
import { CaseAssignmentViewModel } from 'src/app/model/case.model';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BaseComponent } from 'src/app/pages/base/base.component';

@Component({
  selector: 'app-rej',
  templateUrl: './rej.component.html',
  styleUrls: ['./rej.component.scss']
})
export class RejComponent extends BaseComponent implements OnInit {

  @Input() form: FormGroup;
  @Input() model: CaseAssignmentViewModel

  filter = (data: any) => data.id != this.rejectType.None;

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() { }



}
