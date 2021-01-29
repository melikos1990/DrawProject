import * as fromLoadingReducer from "../../store/reducers/loading.reducer";
import * as fromAlertReducer from "../../store/reducers/alert.reducer";
import * as fromRouteReducer from "../../store/reducers/route.reducer";
import * as fromAuthReducer from "../../store/reducers/auth.reducer";
import * as fromAppReducer from "../../store/reducers/app.reducer";
import * as fromNotificationReducer from "../../store/reducers/notification.reducer";
import * as fromHomePageReducer from "../../store/reducers/home-page.reducers";
import { createSelector } from '@ngrx/store';

export interface State {
  loading: fromLoadingReducer.State;
  alert: fromAlertReducer.State;
  route: fromRouteReducer.State;
  auth: fromAuthReducer.State;
  app: fromAppReducer.State;
  notification: fromNotificationReducer.State
  home: fromHomePageReducer.State
}

export const reducers = {
  loading: fromLoadingReducer.reducer,
  alert: fromAlertReducer.reducer,
  route: fromRouteReducer.reducer,
  auth: fromAuthReducer.reducer,
  app: fromAppReducer.reducer,
  notification: fromNotificationReducer.reducer,
  home: fromHomePageReducer.reducer
}



export const userInfoSelector = createSelector(
  (state: State) => state.auth.member,
  (state: State) => state.auth.menu,
  (state: State) => state.auth.jobPosition,
  (user, menu, jobPosition) => {

    if (user) {

      return {
        user,
        menu,
        jobPosition
      };
    }

    return undefined;
  }
);
