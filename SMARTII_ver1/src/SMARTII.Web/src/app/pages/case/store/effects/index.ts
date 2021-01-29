import { CaseCreatorEffects } from './case-creator.effects';
import { CaseAssignmentEffects } from './case-assignment.effects';
import { OfficialEmailAdoptEffects } from './official-email-adopt.effects';
import { PPCLifrEffectiveEffects } from './ppclife-effective.effects';
import { NotificationGroupSenderEffects } from './notification-group-sender.effects';


export const effects = [
    CaseCreatorEffects,
    CaseAssignmentEffects,
    OfficialEmailAdoptEffects,
    PPCLifrEffectiveEffects,
    NotificationGroupSenderEffects
];
