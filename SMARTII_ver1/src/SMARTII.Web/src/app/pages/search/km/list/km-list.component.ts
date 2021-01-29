import { Component, OnInit, Injector, ViewChild, Input } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { KMSearchViewModel, KMClassificationNodeViewModel } from 'src/app/model/master.model';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { EntrancePayload, ActionType } from 'src/app/model/common.model';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromKMActions from '../../store/actions/km.actions';
import { skip, takeUntil, filter } from 'rxjs/operators';

const PREFIX = 'KmComponent'

@Component({
  selector: 'app-km-list',
  templateUrl: './km-list.component.html',
  styleUrls: ['./km-list.component.scss']
})
export class KmListComponent extends FormBaseComponent implements OnInit {

  @ViewChild('table')
  table: ServerTableComponent;

  selectedItem: KMClassificationNodeViewModel;

  ajax: PtcAjaxOptions = new PtcAjaxOptions();
  columns: any[] = [];
  model: KMSearchViewModel = new KMSearchViewModel();

  constructor(
    public store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX)
  }

  ngOnInit() {
    this.subscription();
    this.initializeTable();
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender() {

    setTimeout(() => {

      if (!this.selectedItem) {
        this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
          this.getFieldInvalidMessage("請選擇分類")));
        return;
      }
 

      this.table.render();
    }, 100);

  }

  /**
   * 按鈕按下新增
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event: any) {
    if (!this.selectedItem) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(
        this.getFieldInvalidMessage("請選擇分類")));
      return;
    }
  
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({      
      url: './pages/search/km-detail',
      params: {        
        actionType: ActionType.Add,
        classificationID: this.selectedItem.ClassificationID,
        classificationName: this.selectedItem.ClassificationName,
        pathName: this.selectedItem.PathName
      }
    }));
  }
  /**
    * 當ptc server table 按下刪除
    */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Add)
  onBtnDelete($event: any) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否刪除?',
      () => {
        const payload = new EntrancePayload<{ ID: number }>();
        payload.data = {
          ID: $event.ID,

        };
        payload.success = () => this.btnRender();
        this.store.dispatch(new fromKMActions.deleteAction(payload));
      }
    )));

  }
  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/search/km-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
        classificationID: this.selectedItem.ClassificationID,
        classificationName: this.selectedItem.ClassificationName,
        pathName: this.selectedItem.PathName
      }
    }));
  }
  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Update)
  onBtnEdit($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/search/km-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
        classificationID: this.selectedItem.ClassificationID,
        classificationName: this.selectedItem.ClassificationName,
        pathName: this.selectedItem.PathName
      }
    }));
  }


  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {

    console.log(this.selectedItem);
    if (this.selectedItem) {
      this.model.ClassificationID = this.selectedItem.ClassificationID;
      this.model.OrganizationType = this.selectedItem.OrganizationType;
      this.model.BuID = this.selectedItem.NodeID;
      this.model.OrganizationType = this.selectedItem.OrganizationType;
    }

    $event.criteria = this.model;
  }

  subscription() {
    this.store.select(x => x.mySearch.km.selectedItem)
      .pipe(
        filter(x => !!x),
        takeUntil(this.destroy$))
      .subscribe(selectedItem => {

        this.selectedItem = { ...selectedItem }
        this.btnRender();

      })
  }

  initializeTable() {

    this.ajax.url = `/Master/KMClassification/GetList`.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('KM.CLASSIFICATION_NAME'),
        name: 'ClassificationName',
        disabled: false,
        order: 'KM_CLASSIFICATION.NAME'
      },
      {
        text: this.translateService.instant('KM.TITLE'),
        name: 'Title',
        disabled: false,
        order: 'TITLE'
      },
      {
        text: this.translateService.instant('KM.CONTENT'),
        name: 'Content',
        disabled: false,
        order: 'CONTENT',
        customer: true
      },
      {
        text: this.translateService.instant('KM.UPDATE_USERNAME_LABEL'),
        name: 'UpdateUserName',
        disabled: false,
        order: 'UPDATE_USERNAME'
      },
      {
        text: this.translateService.instant('KM.UPDATE_DATETIME_LABEL'),
        name: 'UpdateDateTime',
        disabled: false,
        order: 'UPDATE_DATETIME'
      }, 
      {
        text: this.translateService.instant('COMMON.TABLE_OPERATOR_TEXT'),
        name: 'p-operator',
        disabled: true,
        order: ''
      },
    ];


  }

}
