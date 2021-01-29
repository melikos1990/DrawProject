import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';

@Component({
  selector: 'app-operator',
  templateUrl: './operator.component.html',
  styleUrls: ['./operator.component.scss']
})
export class OperatorComponent implements OnInit, ViewCell {

  @Input() edit: (data) => void;
  @Input() delete: (data) => void;
  @Input() value: any;
  @Input() rowData: any;

  constructor() { }

  ngOnInit() {

  }

  onBtnDelete($event) {
    this.delete && this.delete(this.rowData);
  }

}

