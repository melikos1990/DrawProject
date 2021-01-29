import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';

@Component({
  selector: 'app-list-particular-grid',
  templateUrl: './list-particular-grid.component.html'
})
export class ListParticularGridComponent extends BaseComponent implements OnInit {


  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() { }

}
