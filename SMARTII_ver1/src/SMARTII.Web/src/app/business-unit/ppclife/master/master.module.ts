import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListParticularGridComponent as ppclifeParticularGridComponent  } from './item/atom/list-particular-grid/list-particular-grid.component';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MASTER_ITEM_LIST_COLUMN_GRID } from '../..';

const ENTRY_COMPONENTS = [ppclifeParticularGridComponent];


const SHARD_SERVICES = [
  { provide: MASTER_ITEM_LIST_COLUMN_GRID, useValue: { key: 'PPCLIFE', component: ppclifeParticularGridComponent }, multi: true },
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
    ...ENTRY_COMPONENTS,
  ]
})
export class MasterModule { }
