import { Component, OnInit, Injector, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { CaseService } from 'src/app/shared/service/case.service';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseAssignmentCommunicateViewModel } from 'src/app/model/case.model';
import { BusinesssUnitParameters } from 'src/app/model/organization.model';

@Component({
  selector: 'app-cam',
  templateUrl: './cam.component.html',
  styleUrls: ['./cam.component.scss']
})
export class CamComponent extends FormBaseComponent implements OnInit {

  opts = {};

  public businessParameter: BusinesssUnitParameters = new BusinesssUnitParameters();


  @Input() model: CaseAssignmentCommunicateViewModel;

  public form: FormGroup;

  constructor(
    public ref: ChangeDetectorRef,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initialize();
    this.initializeForm();
  }



  initialize() {
    this.getParameter();
    // this.setFileOpts();

  }

  getParameter() {

    this.caseService
      .getBUParameters(this.model.NodeID)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.businessParameter = x;
      });
  }

  initializeForm() {
    this.form = new FormGroup({
      Content: new FormControl()

    });
  }

  // setFileOpts() {
  //   const paths = this.model.FilePath || [];
  //   const previews = paths.map(path => path.toHostApiUrl())
  //   const previewConfigs = paths.map(path => {

  //     return {
  //       caption: path.split('fileName=')[1],
  //       key: path,
  //       downloadUrl: path.toHostApiUrl(),
  //       url: `/File/DeleteCaseAssignmentInvocieFile`.toHostApiUrl(),
  //       extra: {
  //         id: this.model.ID,
  //         key: path,
  //       }
  //     }
  //   });

  //   this.opts = {
  //     preferIconicPreview: true,
  //     initialPreview: previews,
  //     initialPreviewConfig: previewConfigs,
  //     fileActionSettings: {
  //       showRemove: true,
  //       showUpload: false,
  //       showClose: false,
  //       uploadAsync: false,
  //     }
  //   };
  // }

}
