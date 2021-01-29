import { SystemParameterEffects } from './system-parameter.effects';
import { UserEffects } from './user.effects';
import { UserParameterEffects } from './user-parameter.effects';
import { RoleEffects } from './role.effects';


export const effects = [
  SystemParameterEffects,
  UserEffects,
  UserParameterEffects,
  RoleEffects,
];
