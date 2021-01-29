import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseAssignmentCommunicateViewModel, CaseViewModel } from 'src/app/model/case.model';

@Component({
  selector: 'app-ccmi2-i',
  templateUrl: './ccmi2-i.component.html',
})
export class Ccmi2IComponent extends FormBaseComponent implements OnInit {

  @Input() case: CaseViewModel;
  @Input() model : CaseAssignmentCommunicateViewModel;
  
  constructor(public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
  }
}
