import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl } from '@angular/forms';
import { CaseAssignmentComplaintNoticeViewModel } from 'src/app/model/case.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-can',
  templateUrl: './can.component.html',
  styleUrls: ['./can.component.scss']
})
export class CanComponent extends FormBaseComponent implements OnInit {

  opts = {};
  columns = [];
  @Input() noticeID: number;
  @Input() model: CaseAssignmentComplaintNoticeViewModel = new CaseAssignmentComplaintNoticeViewModel();

  public form: FormGroup;

  constructor(
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.initialize();
    this.initializeForm();
    this.initializeTable();

  }


  initialize() {

    this.setFileOpts()
  }

  initializeTable() {
    this.columns = [
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.CUSTOMER_NAME'),
        name: 'UserName',
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.TYPE'),
        name: 'NotificationRemark',
        customer: true
      },
      {
        text: this.translateService.instant('CASE_COMMON.TABLE.EMAIL'),
        name: 'Email',
      },
    ];
  }

  initializeForm() {
    this.form = new FormGroup({
      Content: new FormControl()
    });
  }

  setFileOpts() {
    const paths = this.model.FilePath || [];
    const previews = paths.map(path => path.toHostApiUrl())
    const previewConfigs = paths.map(path => {

      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteCaseAssignmentNoticeFile`.toHostApiUrl(),
        extra: {
          id: this.model.ID,
          key: path,
        }
      }
    });

    this.opts = {
      preferIconicPreview: true,
      initialPreview: previews,
      initialPreviewConfig: previewConfigs,
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
  }

}
