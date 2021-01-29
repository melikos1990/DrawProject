import { Component, OnInit, Injector, Input, ViewChild, Inject, ComponentFactoryResolver } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { DynamicHostDirective } from 'ptc-dynamic-form';
import { MASTER_ITEM_LIST_COLUMN_GRID } from 'src/app/business-unit';
import { commonBu } from 'src/global';

@Component({
  selector: 'app-column-grid',
  templateUrl: './column-grid.component.html'
})
export class ColumnGridComponent extends BaseComponent implements OnInit {

  @ViewChild(DynamicHostDirective) host: DynamicHostDirective;

  @Input() data: any;
  @Input() buKey?: string;

  constructor(
    public injector: Injector,
    private resolver: ComponentFactoryResolver,
    @Inject(MASTER_ITEM_LIST_COLUMN_GRID) private grids: { key: string, component: any }[]) {
    super(injector);
   
  }

  ngOnInit() {

    this.createComponents();
  }


  createComponents() {

    if (this.buKey !== null && this.buKey !== undefined) {

      const defaultMap = this.grids.find(x => x.key === commonBu);

      let map = this.grids.find(x => x.key === this.buKey);
      map = map || defaultMap;
      this.createInsatance(map);
    }
  }

  createInsatance(pair: { key: string, component: any }) {
    const container = this.host.container;
    container.clear();

    const component = this.resolver.resolveComponentFactory(pair.component);
    const componentRef = container.createComponent(component, null, this.injector);
    const instance = componentRef.instance;

    (instance as any).particular = this.data;


  }

}
