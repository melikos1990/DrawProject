
import { NgModule } from '@angular/core';
import { SearchRoutingModule } from './search-routing.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ThemeModule } from 'src/app/@theme/theme.module';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import * as fromSearchEffect from './store/effects';
import * as fromSearchReducer from './store/reducers';
import { CallCenterCaseSearchComponent } from './call-center-case-search/list/call-center-case-search.component';
import { SearchComponent } from './search.component';
import { HeaderQurterStoreCaseSearchComponent } from './headerqurter-store-case-search/list/headerqurter-store-case-search.component';
import { HeaderQurterBUCaseSearchComponent } from './headerqurter-bu-case-search/list/headerqurter-bu-case-search.component';
import { CallCenterAssignmentSearchComponent } from './call-center-assignment-search/list/call-center-assignment-search.component';
import { ConcatInputComponent } from './component/concat-input/concat-input.component';
import { ComplainedInputComponent } from './component/complained-input/complained-input.component';
import { HeaderqurterStoreAssignmentSearchComponent } from './headerqurter-store-assignment-search/list/headerqurter-store-assignment-search.component';
import { HeaderqurterBUAssignmentSearchComponent } from './headerqurter-bu-assignment-search/list/headerqurter-bu-assignment-search.component';
import { VendorAssignmentSearchComponent } from './vendor-assignment-search/list/vendor-assignment-search.component';
import { KmComponent } from './km/km.component';
import { KmListComponent } from './km/list/km-list.component';
import { KmDetailComponent } from './km/detail/km-detail.component';
import { BusinessKmTreeComponent } from './km/tree/business-km-tree.component';
import { AddClassificationModalComponent } from './km/modal/add-classification-modal/add-classification-modal.component';
import { EditClassificationModalComponent } from './km/modal/edit-classification-modal/edit-classification-modal.component';
import { SearchBaseComponent } from './base/search-base.component';


const COMPONENT = [
    SearchComponent,
    CallCenterCaseSearchComponent,
    HeaderQurterStoreCaseSearchComponent,
    HeaderQurterBUCaseSearchComponent,
    CallCenterAssignmentSearchComponent,
    SearchBaseComponent,
    ConcatInputComponent,
    ComplainedInputComponent,
    HeaderqurterStoreAssignmentSearchComponent,
    HeaderqurterBUAssignmentSearchComponent,
    VendorAssignmentSearchComponent,
    KmComponent,
    KmListComponent,
    KmDetailComponent,
    BusinessKmTreeComponent,
    AddClassificationModalComponent,
    EditClassificationModalComponent,
]
const ENTRY_COMPONENT = [
    AddClassificationModalComponent,
    EditClassificationModalComponent
]

@NgModule({
    imports: [
        SearchRoutingModule,
        ThemeModule,
        SharedModule,
        CommonModule,
        EffectsModule.forFeature(fromSearchEffect.effects),
        StoreModule.forFeature('mySearch', fromSearchReducer.reducer), // 取名 mySearch 原因是 search 會導致Reducer無法觸發(可能是Nebular內部已佔用search keyword)
    ],
    declarations: [
        ...COMPONENT
    ],
    entryComponents: [...ENTRY_COMPONENT]
})
export class SearchModule {
}
