import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-note-list-item',
  templateUrl: './note-list-item.component.html',
  styleUrls: ['./note-list-item.component.scss']
})
export class NoteListItemComponent implements OnInit {


  @Input() columnName: string;
  @Input() data: any

  @Output() onDismiss: EventEmitter<string> = new EventEmitter();

  titleName: string;

  constructor() { }

  ngOnInit() {
    debugger
    const max = this.data.Names.length;
    this.titleName = this.data.Names[max - 1]
  }

  dismiss() {
    this.onDismiss.emit(this.data);
  }


}
