import * as fromAuthActions from '../actions/auth.actions';
import { User, JobPosition, resultBox } from 'src/app/model/authorize.model';
import { Menu } from 'src/app/model/master.model';




export interface State {
  token: string;
  refreshToken: string;
  member: User;
  menu: Menu[];
  jobPosition: JobPosition[];
  changePasswordDisplay: resultBox;
}

export const initialState: State = {
  token: null,
  refreshToken: null,
  member: null,
  menu: [],
  jobPosition: [],
  changePasswordDisplay: null,
};


export function reducer(state = initialState, action: fromAuthActions.Actions): State {

  switch (action.type) {
    case fromAuthActions.LOGIN_SUCCESS:
      return {
        ...state,
        token: action.payload.accessToken,
        refreshToken: action.payload.refreshToken,
      };
    case fromAuthActions.TOKEN_MEMBER:
      return {
        ...state,
        member: action.payload
      };
    case fromAuthActions.CACHE_MENU:
      return {
        ...state,
        menu: action.payload
      };
    case fromAuthActions.CACHE_JOB_POSITION:
      return {
        ...state,
        jobPosition: action.payload
      };
    case fromAuthActions.CHANGE_PASSWORD_DISPLAY:
      return {
        ...state,
        changePasswordDisplay: action.payload
      };
    case fromAuthActions.CLEAR_AUTH:
      return initialState;
    default: {
      return state;
    }

  }

}
