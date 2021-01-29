import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { CaseViewModel, CaseSourceViewModel } from 'src/app/model/case.model';
import { CaseService } from 'src/app/shared/service/case.service';
import { takeUntil, filter, take } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { NgInputBase } from 'ptc-dynamic-form';
import { ActionType } from 'src/app/model/common.model';

@Component({
  selector: 'app-cco',
  templateUrl: './cco.component.html',
})
export class CcoComponent extends FormBaseComponent implements OnInit {

  inputs = [];
  form: FormGroup = new FormGroup({});


  tempSource: CaseSourceViewModel = new CaseSourceViewModel();

  private _model: CaseViewModel;

  @Output() hasInputChange: EventEmitter<boolean> = new EventEmitter();
  @Input() uiActionType: ActionType;
  @Input() sourcekey: string;
  @Input()
  set model(v: CaseViewModel) {
    this._model = v
  }
  get model(): CaseViewModel {
    return this._model;
  }

  constructor(
    private caseService: CaseService,
    public injector: Injector) {
    super(injector)
  }

  ngOnInit() {
    this.subscription();
  }


  initialize() {

    this.caseService
      .getBULayouts(this.model.NodeID, true)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        setTimeout(() => {
          this.deserialize(x.CaseOtherLayout);      
        }, 1000);
      })
  }

  deserialize(jsonLayout) {
    this.inputs = [];
    try {

      if (jsonLayout) {
        const objects = <Array<NgInputBase>>JSON.parse(jsonLayout);
        if (objects) {
          this.inputs = objects.map(data => {
            console.log('data', data);
            data["disabled"] = this.uiActionType == this.actionType.Read ? true : null;
            this.hasInputChange.emit(true);
            return data;
          });
        } else {
          this.inputs = [];       
          this.hasInputChange.emit(false);
        }
      } else {
        this.inputs = [];    
        this.hasInputChange.emit(false); 
      }
    } catch (e) {
      console.log(e);
    }
  }


  subscription() {

    this.caseService
      .sorceTempSubject
      .pipe(
        takeUntil(this.destroy$),
        filter(x => this.caseService.listenOnSourceFlter(x, this.sourcekey)),
        filter(x => this.isSameNodeID(x) === false),
      )
      .subscribe(x => {
        const newer = { ...x[this.sourcekey] }
        this.tempSource = newer;
        this.model.NodeID = newer.NodeID;
        this.model.OrganizationType = newer.OrganizationType;

        if (this.model.NodeID) {
          this.initialize();
        }

      })

  }
  isSameNodeID(data): boolean {
    return data[this.sourcekey].NodeID === this.tempSource.NodeID
  }
}
