import { Component, OnInit, Input } from '@angular/core';
import { trigger, state, transition, animate, style } from '@angular/animations';

@Component({
  selector: 'app-note-list-group',
  templateUrl: './note-list-group.component.html',
  styleUrls: ['./note-list-group.component.scss'],
  animations: [
    trigger('openGuide', [
      state('show' , style({ height: '400px' })),
      state('hidden', style({ height: '0px' })),
      transition('show => hidden', animate('300ms')),
      transition('hidden => show', animate('300ms')),
    ])
  ]
})
export class NoteListGroupComponent implements OnInit {

  @Input() columnName: string = '';
  @Input() data: any[] = [];

  expandState: string = "show";

  constructor() { }

  ngOnInit() {
  }


  removeAll() {
    this.data = [];
  }

  onItemDismiss(id: number) {
    const index = this.data.findIndex(x => x.ID === id);
    this.data.splice(index, 1);
  }

  changeState(){ 
    if(this.expandState == "show") this.expandState = "hidden";
    else this.expandState = "show";
  }

}
