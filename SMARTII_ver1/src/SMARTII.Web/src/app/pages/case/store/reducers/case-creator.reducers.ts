import * as fromCaseCreatorActions from '../actions/case-creator.actions';
import { Guid } from 'guid-typescript';



export interface State {
  sourceKeyPair: any[];
  caseKeyPair: { [key: string]: any[] };
  activeSourceTab: string;
  activeCaseTab: string;
}

export const initialState: State = {
  sourceKeyPair: [] = [],
  caseKeyPair: {} = {},
  activeSourceTab: null,
  activeCaseTab: null
};


const removeCaseTab = (state: State, action: fromCaseCreatorActions.removeCaseTabAction): State => {
  const casekey = action.payload.caseID;
  const sourcekey = action.payload.sourceID;

  const index = state.caseKeyPair[sourcekey].findIndex(x => x.key == casekey);
  const clone = { ...state }

  clone.caseKeyPair[sourcekey].splice(index, 1);
  clone[casekey] = null;
  return clone;
}

const removeSourceTab = (state: State, action: fromCaseCreatorActions.removeSorceTabAction): State => {
  const sourcekey = action.payload;
  const index = state.sourceKeyPair.findIndex(x => x.key == sourcekey);

  const clone = { ...state }
  clone.sourceKeyPair.splice(index, 1);
  clone[sourcekey] = null;
  clone.caseKeyPair[sourcekey] = [];


  return clone;
}

const loadCaseSourceEntry = (state: State, action: fromCaseCreatorActions.loadCaseSourceEntryAction): State => {
  const id = Guid.create().toString();
  action.payload.key = id;

  const tabs = { id: 'new', key: id };
  const result = {
    ...state,
    sourceKeyPair: [...state.sourceKeyPair, tabs],
  };
  result[id] = action.payload
  return result;
}

const loadCaseEntry = (state: State, action: fromCaseCreatorActions.loadCaseEntryAction): State => {
  const id = Guid.create().toString();
  const sourceKey = action.payload.sourceKey;

  const result = { ...state };

  if (result.caseKeyPair[sourceKey]) {
    result.caseKeyPair[sourceKey].push({ id: 'new', key: id })
  } else {
    result.caseKeyPair[sourceKey] = [{ id: 'new', key: id }];
  }

  result[id] = action.payload.model;

  return result;
}

const loadCaseIDs = (state: State, action: fromCaseCreatorActions.loadCaseIDsSuccessAction): State => {
  const caseIDs = action.payload.data.caseIDs || [];
  const sourceID = action.payload.data.sorceID;

  if (state.caseKeyPair[sourceID]) {

    caseIDs.forEach(caseID => {

      const exist = state.caseKeyPair[sourceID].some(x => x.id === caseID);

      // 這邊只針對並未比對到的案件進行推送
      // 更新行為留給 LOAD_CASE_SUCCESS 進行覆蓋
      if (!exist) {
        state.caseKeyPair[sourceID].push({ id: caseID, key: caseID });
      }
    })

  } else {

    // 比對不到就沒有細項
    // 直接填入即可
    state.caseKeyPair[sourceID] = caseIDs.map(caseID => { return { id: caseID, key: caseID }; });
  }


  return { ...state };
}

const loadCase = (state: State, action: fromCaseCreatorActions.loadCaseSuccessAction): State => {
  const caseID = action.payload.CaseID;
  const sourceID = action.payload.SourceID;
  if (state.caseKeyPair[sourceID]) {

    const exist = state.caseKeyPair[sourceID].some(x => x.id === caseID);
    if (!exist) {
      state.caseKeyPair[sourceID].push({ id: caseID, key: caseID });
    }

  } else {
    state.caseKeyPair[sourceID] = [{ id: caseID, key: caseID }];
  }
  state[caseID] = action.payload;

  return { ...state };
}

const loadCaseSource = (state: State, action: fromCaseCreatorActions.loadCaseSourceSuccessAction | fromCaseCreatorActions.loadCaseSourceNativeSuccessAction): State => {


  const id = action.payload.SourceID;
  const focusCaseID = action.payload.FocusCaseID;
  action.payload.key = id;

  const tabs = { id: id, key: id, focusCaseID: focusCaseID };

  const isExistKey = state.sourceKeyPair.some(x => x.id === id);

  const result = {
    ...state,
    sourceKeyPair: isExistKey ? state.sourceKeyPair : [...state.sourceKeyPair, tabs]
  };

  result[action.payload.SourceID] = action.payload;
  return result;

}

export function reducer(state: State = initialState, action: fromCaseCreatorActions.Actions) {
  switch (action.type) {
    case fromCaseCreatorActions.ACTIVE_CASE_TAB:
      return {
        ...state,
        activeCaseTab: action.payload
      }
    case fromCaseCreatorActions.UNACTIVE_CASE_TAB:
      return {
        ...state,
        activeCaseTab: null
      }
    case fromCaseCreatorActions.ACTIVE_SOURCE_TAB:
      return {
        ...state,
        activeSourceTab: action.payload
      }
    case fromCaseCreatorActions.UNACTIVE_SOURCE_TAB:
      return {
        ...state,
        activeSourceTab: null
      }
    case fromCaseCreatorActions.REMOVE_SOURCE_TAB: return removeSourceTab(state, <fromCaseCreatorActions.removeSorceTabAction>action);
    case fromCaseCreatorActions.REMOVE_CASE_TAB: return removeCaseTab(state, <fromCaseCreatorActions.removeCaseTabAction>action);
    case fromCaseCreatorActions.CLEAR_ALL: return initialState;
    case fromCaseCreatorActions.LOAD_CASE_SOURCE_ENTRY: return loadCaseSourceEntry(state, <fromCaseCreatorActions.loadCaseSourceEntryAction>action)
    case fromCaseCreatorActions.LOAD_CASE_ENTRY: return loadCaseEntry(state, <fromCaseCreatorActions.loadCaseEntryAction>action)
    case fromCaseCreatorActions.LOAD_CASE_IDS_SUCCESS: return loadCaseIDs(state, <fromCaseCreatorActions.loadCaseIDsSuccessAction>action)
    case fromCaseCreatorActions.LOAD_CASE_SUCCESS: return loadCase(state, <fromCaseCreatorActions.loadCaseSuccessAction>action)
    case fromCaseCreatorActions.LOAD_CASE_SOURCE_NATIVE_SUCCESS: return loadCaseSource(state, <fromCaseCreatorActions.loadCaseSourceNativeSuccessAction>action)
    case fromCaseCreatorActions.LOAD_CASE_SOURCE_SUCCESS: return loadCaseSource(state, <fromCaseCreatorActions.loadCaseSourceSuccessAction>action)
    default:
      return state;
  }
}

