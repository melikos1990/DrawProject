import { Component, OnInit, Injector, Input, Output, ViewChildren, ViewChild, AfterViewInit, QueryList } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseService } from 'src/app/shared/service/case.service';
import { CaseViewModel } from 'src/app/model/case.model';
import { takeUntil } from 'rxjs/operators';
import { BusinessLayouts } from 'src/app/model/organization.model';
import { CaseFinishDataListViewModel } from 'src/app/model/master.model';
import { ActionType } from 'src/app/model/common.model';


@Component({
  selector: 'app-ccfh',
  templateUrl: './ccfh.component.html',
  styleUrls: ['./ccfh.component.scss']
})
export class CcfhComponent extends FormBaseComponent implements OnInit {


  @ViewChildren('cbx') cbx: QueryList<any>
  @Input() uiActionType: ActionType;
  
  private _model: CaseViewModel;

  @Input()
  public set model(v: CaseViewModel) {
    this._model = v;
  }
  public get model(): CaseViewModel {
    return this._model
  }

  layouts: BusinessLayouts = new BusinessLayouts();

  constructor(
    public caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {

  }
  ngAfterViewInit(): void {
    this.initialize();

  }

  getValue() {
    const datas: CaseFinishDataListViewModel[] = [];
    if (this.cbx) {
      this.cbx.forEach(x => {
        if (x.checked) {
          const value: string = x.value;
          datas.push(this.getCode(value) as any)
        }
      })
    }
    return datas;
  }


  initialize() {
    this.caseService
      .getBULayouts(this.model.NodeID, true)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => this.layouts = x);
  }

  getCode(value): { ClassificationID, ID } {
    return {
      ClassificationID: value.split('-')[0],
      ID: value.split('-')[1]
    }
  }

  onChange($event) {
    const element = $event.source;
    const pair = this.getCode(element.value);
    const classification = this.layouts.CaseFinishReasonClassifications.find(x => x.ID == pair.ClassificationID);

    if (classification.IsMultiple == false) {
      this.cbx.forEach(x => {
        const memberPair = this.getCode(x.value);

        if (memberPair.ClassificationID == classification.ID &&
          memberPair.ID != pair.ID) {
          x.checked = false;
        }

      })
    }

  }

  isChecked(classificationID: number, dataID: number, isDefault: boolean) {

    if (this._model.CaseFinishReasons.length > 0) {
      if (!this._model || !this._model.CaseFinishReasons) return false;
      return this._model.CaseFinishReasons.some(x => x.ClassificationID == classificationID && x.ID == dataID);
    } else {
      return isDefault
    }

  }


}
