import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListParticularGridComponent as commonBuParticularGridComponent } from './item/atom/list-particular-grid/list-particular-grid.component';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MASTER_ITEM_LIST_COLUMN_GRID } from '../..';
import { CommonBUKeyPair } from 'src/global';


const ENTRY_COMPONENTS = [commonBuParticularGridComponent];

const SHARD_SERVICES = [
  { provide: MASTER_ITEM_LIST_COLUMN_GRID, useValue: { key: CommonBUKeyPair.NodeKey, component: commonBuParticularGridComponent }, multi: true },
];

@NgModule({
  declarations: [...ENTRY_COMPONENTS],
  entryComponents: [...ENTRY_COMPONENTS],
  providers: [...SHARD_SERVICES],
  imports: [
    CommonModule,
    ThemeModule,
    SharedModule
  ],
  exports: [
    ...ENTRY_COMPONENTS
  ]
})
export class MasterModule { }
