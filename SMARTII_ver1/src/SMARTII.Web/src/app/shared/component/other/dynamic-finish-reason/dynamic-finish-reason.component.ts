import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { FinishReasonDynamicViewModel, CaseFinishDataDetailViewModel } from 'src/app/model/master.model';
import { HttpService } from 'src/app/shared/service/http.service';

@Component({
  selector: 'app-dynamic-finish-reason',
  templateUrl: './dynamic-finish-reason.component.html',
  styleUrls: ['./dynamic-finish-reason.component.scss']
})
export class DynamicFinishReasonComponent implements OnInit {


  @Input() buID: number;
  @Input() labelCol: string = 'col-sm-2';
  @Input() contentCol: string = 'col-sm-10';

  @Input() useDefault: boolean = true;

  finishReasonClassification: FinishReasonDynamicViewModel[] = [];

  /**
    * 選擇的處置原因 格式: { "類型ID": ["處理方式1", "處理方式2"]}
    */
  get selected(): { [idx: number]: number[] } {
    let result: { [idx: number]: number[] } = {};
    let origin = { ...this._selected };
    for (let item in origin) {
      if (origin[item].length > 0) result[item] = origin[item];
    }

    return result;
  }

  private _selected: { [idx: number]: number[] } = {};

  constructor(
    private http: HttpService
  ) { }



  ngOnInit() {
  }


  ngOnChanges(changes: SimpleChanges): void {
    if (changes["buID"] && changes["buID"].currentValue) {
      this._selected = {};
      this.getFinishReason();
    }
  }


  getFinishReason() {
    this.http.get<FinishReasonDynamicViewModel[]>('Common/Master/GetFinishReasonClassificationChecked', { BuID: this.buID })
      .subscribe(res => {
        this.finishReasonClassification = res;

        // 填入預設資料
        if (this.useDefault) {
          res.forEach(x => {
            let data = x.FinishReasons.filter(g => g.Default == true);
            this.backFill(data);
          })
        }

      })
  }


  _change(data: CaseFinishDataDetailViewModel) {
    if (this.check(data, data.ClassificationID)) this.remove(data, data.ClassificationID);
    else this.add(data, data.ClassificationID);
  }


  private backFill(data: CaseFinishDataDetailViewModel[]) {
    data.forEach(x => this._change(x));
  }


  private remove = (data, classificationID) => this._selected[classificationID].splice(this.findIdx(this._selected[classificationID], data.ID), 1);

  private add = (data, classificationID) => Array.isArray(this._selected[classificationID]) ? this._selected[classificationID].push(data.ID) : this._selected[classificationID] = [data.ID];

  private check = (data, classificationID) => Array.isArray(this._selected[classificationID]) ? this._selected[classificationID].some(ID => ID == data.ID) : false;

  private findIdx = (data: number[], ID: number) => data.findIndex(x => x == ID);


}
