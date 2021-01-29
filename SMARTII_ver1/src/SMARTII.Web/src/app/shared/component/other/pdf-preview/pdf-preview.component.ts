import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-pdf-preview',
  templateUrl: './pdf-preview.component.html',
  styleUrls: ['./pdf-preview.component.scss']
})
export class PdfPreviewComponent implements OnInit, OnChanges {
  

  @Input() source;

  loading : boolean = false;

  constructor() { }

  ngOnInit() {
    this.loading = true;

  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes["source"] && changes["source"].currentValue){
      debugger;
      console.log(this.source);
      
    }
  }

  afterLoadComplete($event) {
    this.loading = false;
    console.log('afterLoadComplete')
  }

}
