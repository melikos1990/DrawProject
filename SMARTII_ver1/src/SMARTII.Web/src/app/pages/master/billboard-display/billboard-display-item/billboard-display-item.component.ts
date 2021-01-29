import { Component, OnInit, Input, Injector } from '@angular/core';
import { BillboardListViewModel } from 'src/app/model/master.model';
import { BaseComponent } from 'src/app/pages/base/base.component';

const PREFIX = 'BillboardDisplayComponent';

@Component({
  selector: 'app-billboard-display-item',
  templateUrl: './billboard-display-item.component.html',
  styleUrls: ['./billboard-display-item.component.scss']
})
export class BillboardDisplayItemComponent extends BaseComponent implements OnInit {

  @Input() model: BillboardListViewModel;

  options = {};

  imgpatch = "";
  name = "";

  constructor(public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initialize();
  }

  initialize() {

    const previews = this.model.FilePaths.map(path => path.toHostApiUrl())
    const previewConfigs = this.model.FilePaths.map(path => {
      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteBillboardFile?id=${this.model.ID}&key=${path}`.toHostApiUrl()
      }
    });

    this.options = {
      preferIconicPreview: true,
      initialPreview: previews,
      initialPreviewConfig: previewConfigs,
      fileActionSettings: {
        showRemove: false,
        showZoom: false,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
    if (this.model.ImagePath != null) {
      this.imgpatch = this.model.ImagePath ? this.model.ImagePath.toHostApiUrl() : "";
    }
    else{
      this.name = this.model.CreateUserName;
    }
    
  }



}
