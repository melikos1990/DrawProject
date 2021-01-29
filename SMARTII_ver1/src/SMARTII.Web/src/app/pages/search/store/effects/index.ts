import { CallCenterCaseSearchEffects } from './call-center-case-search.actions.effects';
import { HeaderqurterStoreCaseSearchEffects } from './headerqurter-store-case-search.effects';
import { CallCenterCaseAssignmentSearchEffects } from './call-center-case-assignment-search.actions.effects';
import { HeaderqurterStoreCaseAssignmentSearchEffects } from './headerqurter-store-case-assignment-search.effects';
import { VendorCaseAssignmentSearchEffects } from './vendor-case-assignment-search.effects';
import { KMEffects } from './km.effects';

export const effects = [
    CallCenterCaseSearchEffects,
    HeaderqurterStoreCaseSearchEffects,
    CallCenterCaseAssignmentSearchEffects,
    HeaderqurterStoreCaseAssignmentSearchEffects,
    VendorCaseAssignmentSearchEffects,
    KMEffects
];
