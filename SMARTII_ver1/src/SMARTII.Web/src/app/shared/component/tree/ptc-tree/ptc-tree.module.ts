import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// import { MaterialModule } from './shared/material.module';
import { TreeModule } from 'angular-tree-component';
import { FormsModule } from '@angular/forms';
import { PtcTreeComponent } from './ptc-tree.component';

@NgModule({
  declarations: [PtcTreeComponent],
  imports: [
    CommonModule,
    FormsModule,
    TreeModule
  ],
  exports: [
    PtcTreeComponent,
    TreeModule
  ]
})
export class PtcTreeModule { }
