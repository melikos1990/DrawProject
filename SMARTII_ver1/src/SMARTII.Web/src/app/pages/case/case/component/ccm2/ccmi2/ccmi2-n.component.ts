import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseAssignmentComplaintNoticeViewModel, CaseViewModel } from 'src/app/model/case.model';


@Component({
  selector: 'app-ccmi2-n',
  templateUrl: './ccmi2-n.component.html',
})
export class Ccmi2NComponent extends FormBaseComponent implements OnInit {

  @Input() case: CaseViewModel;
  @Input() model : CaseAssignmentComplaintNoticeViewModel;
  
  constructor(public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
  }

}
