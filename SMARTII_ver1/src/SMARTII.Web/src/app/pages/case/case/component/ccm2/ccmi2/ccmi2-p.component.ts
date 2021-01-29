import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseAssignmentComplaintInvoiceViewModel, CaseViewModel } from 'src/app/model/case.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil } from 'rxjs/operators';
import { BusinesssUnitParameters } from 'src/app/model/organization.model';

@Component({
  selector: 'app-ccmi2-p',
  templateUrl: './ccmi2-p.component.html',
})
export class Ccmi2PComponent extends FormBaseComponent implements OnInit {

  @Input() case: CaseViewModel;
  @Input() model: CaseAssignmentComplaintInvoiceViewModel;

  businessParameter: BusinesssUnitParameters;

  constructor(
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initialize();
  }

  initialize() {

    this.caseService
      .getBUParameters(this.case.NodeID)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => this.businessParameter = x)
  }

}
