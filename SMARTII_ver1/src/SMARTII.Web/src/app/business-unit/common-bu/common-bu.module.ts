import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterModule } from './master/master.module';
import { CASE_FACTORY } from '..';
import { CaseFactory } from './service/case.factory';



const SHARD_SERVICES = [
  CaseFactory,
  { provide: CASE_FACTORY, useExisting: CaseFactory, multi: true },
];


@NgModule({
  declarations: [],
  imports: [
    MasterModule,
    CommonModule
  ],
  providers: [
    ...SHARD_SERVICES
  ],
  exports: [
    MasterModule
  ]
})
export class CommonBuModule { }
