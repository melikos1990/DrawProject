import * as fromNodeDefinitionReducer from './node-definition.reducers';
import * as fromRootReducer from 'src/app/store/reducers';
import * as fromHeaderQuarterNodeReducer from './headerquarter-node.reducers';
import * as fromCallCenterNodeReducer from './callcenter-node.reducers';
import * as fromVendorNodeReducer from './vendor-node.reducers';
import * as fromEnterpriseReducer from './enterprise.reducers';

export interface IndexState {
  nodeDefinition: fromNodeDefinitionReducer.State;
  headerQuarterNode: fromHeaderQuarterNodeReducer.State;
  callcenterNode: fromCallCenterNodeReducer.State;
  vendorNode: fromVendorNodeReducer.State;
  enterprise: fromEnterpriseReducer.State;
}

export interface State extends fromRootReducer.State {
  organization: IndexState;
}

export const reducer = {
  nodeDefinition: fromNodeDefinitionReducer.reducer,
  headerQuarterNode: fromHeaderQuarterNodeReducer.reducer,
  callcenterNode: fromCallCenterNodeReducer.reducer,
  vendorNode: fromVendorNodeReducer.reducer,
  enterprise: fromEnterpriseReducer.reducer,
};
