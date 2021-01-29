import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { ServerTableComponent } from 'src/app/shared/component/table/server-table/server-table.component';
import { PtcAjaxOptions, PtcServerTableRequest } from 'ptc-server-table';
import { NodeDefinitionSearchViewModel, NodeDefinitionListViewModel, OrganizationType } from 'src/app/model/organization.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import { Store } from '@ngrx/store';
import { State as fromNodeDefinitionReducer } from '../../store/reducers/node-definition.reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromNodeDefinitionActions from '../../store/actions/node-definition.actions';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { NodeDefinitionJobModalComponent } from 'src/app/shared/component/modal/node-definition-job-modal/node-definition-job-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SearchCacheMethod } from 'src/app/shared/decorator/searchCache.decorator';

const PREFIX = 'NodeDefinitionComponent';

@Component({
  selector: 'app-node-definition',
  templateUrl: './node-definition.component.html',
  styleUrls: ['./node-definition.component.scss']
})
export class NodeDefinitionComponent extends FormBaseComponent implements OnInit {

  /**
   * 這邊使用套件為 ptc-server-table
   * 請參照以下網址 ：
   * http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/ng-ptc-server-table?path=%2FREADME.md&version=GBmaster&_a=preview
   */
  @ViewChild('table')
  table: ServerTableComponent;

  items: any[] = [
    { id: true, text: "啟用" },
    { id: false, text: "停用" },
  ]

  /**
   * 定義顯示之欄位 , 用途請參照以上網址
   */
  columns: any[] = [];

  /**
   * 定義ajax欄位 , 用途請參照以上網址
   */
  ajax: PtcAjaxOptions = new PtcAjaxOptions();

  public model = new NodeDefinitionSearchViewModel();
  isEnable: boolean = false;

  constructor(
    public store: Store<fromNodeDefinitionReducer>,
    public modalService: NgbModal,
    public injector: Injector) {
    super(injector, PREFIX);

  }


  ngOnInit() {
    this.initializeTable();
  }

  @loggerMethod()
  ngAfterViewInit() {
    //撈取查詢紀錄
    this.model = this.fillbackCache(this.table, this.btnRender.bind(this));
  }

  /**
  * 將物件傳出之前 , 加工 payload 送回server
  */
  @loggerMethod()
  critiria($event: PtcServerTableRequest<any>) {
    $event.criteria = this.model;
  }


  /**
   * 按鈕按下新增
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event: any) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/organization/node-definition-detail',
      params: {
        actionType: ActionType.Add,
      }
    }));
  }

  /**
   * 按鈕按下查詢,渲染table
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnRender($event: any) {
    
    //開啟查詢結果
    this.isEnable = null;

    this.table.render();
  }

  /**
   * 按鈕按下查看職稱
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  btnCheckJob($event: any) {
    if (!this.model.Identification) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("請選擇識別值")));
      return;
    }

    const ref = this.modalService.open(NodeDefinitionJobModalComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<NodeDefinitionJobModalComponent>ref.componentInstance);
    instance.nodeID = this.model.Identification;
    instance.type = this.model.OrganizationType;
  }


  /**
   * 當ptc server table 按下停用
   */
  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Read)
  onBtnDelete($event: NodeDefinitionListViewModel) {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否停用?',
      () => {
        const payload = new EntrancePayload<{ ID: number, organizationType: OrganizationType }>
          ({
            ID: $event.ID,
            organizationType: $event.OrganizationType
          });
        payload.success = () => this.btnRender(null);
        this.store.dispatch(new fromNodeDefinitionActions.disableAction(payload));
      }
    )));

  }

  /**
   * 當ptc server table 按下查詢
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Read)
  onBtnSearch($event: NodeDefinitionListViewModel) {

    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/organization/node-definition-detail',
      params: {
        actionType: ActionType.Read,
        id: $event.ID,
        organizationType: $event.OrganizationType
      }
    }));
  }

  /**
   * 當ptc server table 按下編輯
   */
  @loggerMethod()
  @SearchCacheMethod(PREFIX)
  @AuthorizeMethod(PREFIX, AuthenticationType.Update | AuthenticationType.Read)
  onBtnEdit($event: NodeDefinitionListViewModel) {
    this.store.dispatch(new fromRootActions.RouteActions.changeRouteAction({
      url: './pages/organization/node-definition-detail',
      params: {
        actionType: ActionType.Update,
        id: $event.ID,
        organizationType: $event.OrganizationType
      }
    }));
  }

  //切換組織型態，清空節點
  onChange($event) {
    this.model.Identification = null;
  }

  initializeTable() {

    this.ajax.url = 'Organization/NodeDefinition/GetList/'.toHostApiUrl();
    this.ajax.method = 'POST';
    this.columns = [
      {
        text: this.translateService.instant('NODE_DEFINITION.ORGANIZATION_TYPE'),
        name: 'OrganizationTypeName',
        disabled: false,
        order: 'ORGANIZATION_TYPE'
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.IDENTIFICATION_NAME'),
        name: 'IdentificationName',
        disabled: false,
        order: 'IDENTIFICATION_NAME'
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.NAME'),
        name: 'Name',
        disabled: false,
        order: 'NAME'
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.IS_ENABLED'),
        name: 'IsEnabled',
        disabled: false,
        order: 'IS_ENABLED'
      },
      {
        text: this.translateService.instant('NODE_DEFINITION.LEVEL'),
        name: 'Level',
        disabled: false,
        order: 'LEVEL'
      },
      {
        text: this.translateService.instant('COMMON.ACTION'),
        name: 'p-operator',
        disabled: true,
        order: ''
      },
    ];

  }

}
