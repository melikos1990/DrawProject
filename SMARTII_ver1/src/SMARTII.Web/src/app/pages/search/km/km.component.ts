import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { KMClassificationNodeViewModel } from 'src/app/model/master.model';

const PREFIX = 'KmComponent';

@Component({
  selector: 'app-km',
  templateUrl: './km.component.html',
})
export class KmComponent extends BaseComponent implements OnInit {


  selected : KMClassificationNodeViewModel ;

  constructor(public injector: Injector) {
    super(injector, PREFIX)
  }

  ngOnInit() {

  }

  nodeClick($event) {
    console.log($event);
    this.selected = $event;
  }

}
