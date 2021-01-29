import { Component, OnInit, Injector } from '@angular/core';
import { ActionType } from 'src/app/model/common.model';
import { BaseComponent } from 'src/app/pages/base/base.component';


const PREFIX = 'NodeDefinitionComponent';

@Component({
  selector: 'app-node-definition-detail',
  templateUrl: './node-definition-detail.component.html',
  styleUrls: ['./node-definition-detail.component.scss']
})
export class NodeDefinitionDetailComponent extends BaseComponent implements OnInit {

  public uiActionType: ActionType;
  public jobUIActionType?: ActionType;
  titleTypeString: string = "";

  constructor(public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    
  }



}
