import * as fromOfficialEmailAdoptActions from '../actions/official-email-adopt.actions';
import { Guid } from 'guid-typescript';



export interface State {
  adoptEmails: any;
}

export const initialState: State = {
  adoptEmails: [] = []
};


export function reducer(state: State = initialState, action: fromOfficialEmailAdoptActions.Actions){

  switch (action.type) {
    default:
      return state;
  }

}