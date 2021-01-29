
import { createSelector } from '@ngrx/store';
import { ActionType } from 'src/app/model/common.model';
import * as fromRootReducer from 'src/app/store/reducers';

export interface IndexState {

}

export interface State extends fromRootReducer.State {
  master: IndexState;
}

export const reducer = {

}


