import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CASE_FACTORY } from '..';
import { CaseFactory } from './service/case.factory';


const SHARD_SERVICES = [
  CaseFactory,
  { provide: CASE_FACTORY, useExisting: CaseFactory, multi: true },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    ...SHARD_SERVICES
  ],
})
export class AsoModule { }
