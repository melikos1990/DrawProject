import * as fromCaseCreatorReducer from './case-creator.reducers'
import * as fromCaseAssignmentReducer from './case-assignment.reducers'
import * as fromRootReducer from 'src/app/store/reducers'
import * as fromOfficialEmailAdoptReducer from './official-email-adopt.reducers'
import * as fromPPCLifeEffectiveReducer from './ppclife-effective.reducers'
import * as fromNotificationGroupSenderReducer from "./notification-group-sender.reducers";


export interface IndexState {
    caseCreator: fromCaseCreatorReducer.State;
    caseAssignment: fromCaseAssignmentReducer.State;
    officialEmailAdopt: fromOfficialEmailAdoptReducer.State;
    notificationGroupSender: fromNotificationGroupSenderReducer.State;
    ppclifeEffectiveSender: fromPPCLifeEffectiveReducer.State;
}

export interface State extends fromRootReducer.State {
    case: IndexState;   
}

export const reducer = {
    caseCreator: fromCaseCreatorReducer.reducer,
    caseAssignment: fromCaseAssignmentReducer.reducer,
    officialEmailAdopt: fromOfficialEmailAdoptReducer.reducer,
    ppclifeEffectiveSender: fromPPCLifeEffectiveReducer.reducer,
    notificationGroupSender: fromNotificationGroupSenderReducer.reducer    
};
