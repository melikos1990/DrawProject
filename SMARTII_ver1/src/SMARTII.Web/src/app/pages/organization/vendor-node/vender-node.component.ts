import { Component, OnInit, Injector } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { ActionType } from 'src/app/model/common.model';
import { State as fromOrganizationReducers } from '../../../store/reducers';
import * as fromVendorNodeActions from '../store/actions/vendor-node.action';
import { Store } from '@ngrx/store';
import { FormBaseComponent } from '../../base/form-base.component';

const PREFIX = 'VendorNodeComponent';

@Component({
    selector: 'app-vendor-node',
    templateUrl: './vender-node.component.html'
})
export class VendorNodeComponent extends FormBaseComponent implements OnInit {

    public uiActionType = ActionType.Update;

    constructor(
        public store: Store<fromOrganizationReducers>,
        public injector: Injector
    ) {
        super(injector, PREFIX);
    }

    ngOnInit() {
        this.authVerification();
        this.store.dispatch(new fromVendorNodeActions.loadEntryAction());
    }

    authVerification() {
        console.log('this.authType => ', this.authType);

        this.ishasAuth$(this.authType.Update)
            .subscribe(hasAuth => {
                console.log('hasAuth =>', hasAuth);
                this.uiActionType = hasAuth ? this.actionType.Update : this.actionType.Read
            })
    }

}
