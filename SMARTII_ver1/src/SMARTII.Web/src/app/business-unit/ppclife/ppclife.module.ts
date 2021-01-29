import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterModule } from './master/master.module';
import { CaseFactory } from './service/case.factory';
import { CASE_FACTORY } from '..';


const SHARD_SERVICES = [
  CaseFactory,
  { provide: CASE_FACTORY, useExisting: CaseFactory, multi: true },
];

@NgModule({
  declarations: [],
  imports: [
    MasterModule,
    CommonModule,

  ],
  providers: [
    ...SHARD_SERVICES
  ],
  exports: [
    MasterModule
  ]
})
export class PpclifeModule { }
