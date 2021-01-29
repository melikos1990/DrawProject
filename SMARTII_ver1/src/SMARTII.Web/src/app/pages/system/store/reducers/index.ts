import * as fromSystemParameterReducer from  "./system-parameter.reducers";
import * as fromRootReducer from "../../../../store/reducers";
import * as fromUserReducer from './user.reducers';
import * as fromUserParameterReducer from "./user-parameter.reducers";
import * as fromRoleReducer from './role.reducers';


export interface IndexState {
  role: fromRoleReducer.State;
    systemParameter : fromSystemParameterReducer.State;
    user: fromUserReducer.State;
    userparameter: fromUserParameterReducer.State;
}

export interface State extends fromRootReducer.State {
    system : IndexState;
}

export const reducer = {
  role: fromRoleReducer.reducer,
  systemParameter : fromSystemParameterReducer.reducer,
  user: fromUserReducer.reducer,
  userparameter: fromUserParameterReducer.reducer,
}
