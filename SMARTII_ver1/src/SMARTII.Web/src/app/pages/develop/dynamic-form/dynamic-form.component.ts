import { Component, OnInit, Injector, Inject } from '@angular/core';
import { FormBaseComponent } from '../../base/form-base.component';

import { NgInputBase } from 'ptc-dynamic-form';
import { FormGroup, Form } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SettingModalComponent } from './modal/setting-modal.component';
import { moveItemInArray, CdkDragDrop, transferArrayItem } from '@angular/cdk/drag-drop';
import { DYNAMIC_PAYLOADS } from 'src/app/shared/injection-token';


export const PREFIX = 'DynamicFormComponent';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent extends FormBaseComponent implements OnInit {

  content: string = '';
  inputs: NgInputBase[] = [];
  form: FormGroup;
  demoForm: FormGroup = new FormGroup({});
  demo = {};
  json = '';
  componentBasePayloads = [];
  displayPayloads = [];
  implementPayloads = [];

  constructor(
    private modalService: NgbModal,
    public injector: Injector,
    @Inject(DYNAMIC_PAYLOADS) private dynamicInputs: { key: string, payload: NgInputBase }[]) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.setBasePayloads();
    this.setDisplayPayloads();
  }

  setBasePayloads() {
    this.componentBasePayloads = [];
    let di = [...this.dynamicInputs];
    di.forEach(pair => {
      this.componentBasePayloads.push({ ...pair.payload });
    })
  }
  setDisplayPayloads() {
    this.displayPayloads = [];
    let base = [...this.componentBasePayloads];
    base.forEach(x => {
      this.displayPayloads.push({ ...x });
    })
  }
  rowDelete($event) {
    let index = this.implementPayloads.indexOf($event);
    this.implementPayloads.splice(index, 1);
  }
  rowEdit($event) {
    const activeModal = this.modalService.open(SettingModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    activeModal.componentInstance.payload = $event;
  }

  btnSave() {
    this.content = JSON.stringify(this.implementPayloads);

  }
  btnPreview() {
    this.json = JSON.stringify(this.implementPayloads);
    let objects = <Array<NgInputBase>>JSON.parse(this.json)
    this.inputs = [...objects];
  }
  deprecated(event: CdkDragDrop<string[]>) {
    this.implementPayloads.splice(event.previousIndex, 1);
  }

  btnJsonToObject(){
    let objects = <Array<NgInputBase>>JSON.parse(this.json)
    this.inputs = [...objects];
    this.implementPayloads = [...objects];
  }
  drop(event: CdkDragDrop<string[]>) {
    console.log("event => ", event);
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      if (event.previousContainer.id === 'right') {
        this.deprecated(event);
        return;
      }
      const activeModal = this.modalService.open(SettingModalComponent, { size: 'lg', container: 'nb-layout' });
      activeModal.componentInstance.payload = event.previousContainer.data[event.previousIndex];
      activeModal.componentInstance.saveHandler = (payload) => {
        transferArrayItem(event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex);
        this.setDisplayPayloads();
      }
    }
  }

}
