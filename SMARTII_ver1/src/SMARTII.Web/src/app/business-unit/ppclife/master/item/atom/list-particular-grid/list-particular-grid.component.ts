import { Component, OnInit, Input, Injector } from '@angular/core';
import { PPCLifeItem } from '../../../../model/master.model';
import { BaseComponent } from 'src/app/pages/base/base.component';

@Component({
  selector: 'app-list-particular-grid',
  templateUrl: './list-particular-grid.component.html',
  styleUrls: ['./list-particular-grid.component.scss']
})
export class ListParticularGridComponent extends BaseComponent implements OnInit {

  @Input() particular: PPCLifeItem;

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit() { }

}
