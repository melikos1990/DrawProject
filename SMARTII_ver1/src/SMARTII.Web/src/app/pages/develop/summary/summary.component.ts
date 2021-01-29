import { Component, OnInit, Injector, ViewChild, TemplateRef } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { HttpClient, HttpResponse } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { HeadquarterNodeUserModalComponent } from 'src/app/shared/component/modal/headquarter-node-user-modal/headquarter-node-user-modal.component';
import { CallcenterNodeUserModalComponent } from 'src/app/shared/component/modal/callcenter-node-user-modal/callcenter-node-user-modal.component';
import { VendorNodeUserModalComponent } from 'src/app/shared/component/modal/vendor-node-user-modal/vendor-node-user-modal.component';
import { NodeDefinitionJobModalComponent } from 'src/app/shared/component/modal/node-definition-job-modal/node-definition-job-modal.component';
import { StoreOwnerSelectorComponent } from 'src/app/shared/component/modal/store-owner-selector/store-owner-selector.component';
import { CaseAssignGroupUserModalComponent } from 'src/app/shared/component/modal/case-assign-group-user-modal/case-assign-group-user-modal.component';
import { CallCenterNodeDetailViewModel, OrganizationNodeDetailViewModel, OrganizationDataRangeSearchViewModel } from 'src/app/model/organization.model';
import { CaseTemplateSelectorComponent } from 'src/app/shared/component/modal/case-template-selector/case-template-selector.component';
import { DualListComponent } from 'src/app/shared/component/other/dual-list/dual-list.component';
import { Store } from '@ngrx/store';
import * as fromRootActions from "src/app/store/actions";
import { PtcSwalType } from 'ptc-swal';
import { HttpService } from 'src/app/shared/service/http.service';
import { CaseService } from 'src/app/shared/service/case.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss']
})
export class SummaryComponent extends BaseComponent implements OnInit {
  public headquartermodel: CallCenterNodeDetailViewModel = new CallCenterNodeDetailViewModel();
  public ccmodel: OrganizationNodeDetailViewModel = new OrganizationNodeDetailViewModel();
  pdfSrc;
  excelSrc;
  kmNode;
  tagData;
  defaultTag;
  @ViewChild('buNodeRelationSelector') pop1: TemplateRef<any>;
  @ViewChild('headerQuarterNodeTreeUserSelector') pop2: TemplateRef<any>;
  @ViewChild('callCenterNodeTreeUserSelector') pop3: TemplateRef<any>;
  @ViewChild('pdfViewer') pop4: TemplateRef<any>;
  @ViewChild('excelViewer') pop5: TemplateRef<any>;
  @ViewChild('vendorNodeTreeUserSelector') pop6: TemplateRef<any>;
  @ViewChild('allNodeTreeUserSelector') pop7: TemplateRef<any>;
  @ViewChild('headerQuarterNodeUserModal') pop8: TemplateRef<any>;
  @ViewChild('callCenterNodeUserModal') pop9: TemplateRef<any>;
  @ViewChild('vendorNodeUserModal') pop10: TemplateRef<any>;
  @ViewChild('nodeDefinitionJobModal') pop11: TemplateRef<any>;
  @ViewChild('kmTreeModal') pop12: TemplateRef<any>;
  @ViewChild('hashTagModal') pop14: TemplateRef<any>;
  @ViewChild('ccball') pop15: TemplateRef<any>;
  @ViewChild('selectByBU') pop16: TemplateRef<any>;
  @ViewChild('finishReason') finishReason: TemplateRef<any>;
  @ViewChild('mailsender') pop17: TemplateRef<any>;
  @ViewChild('dualList') pop18: TemplateRef<any>;
  @ViewChild('dndList') pop19: TemplateRef<any>;
  @ViewChild('textAarea') textAarea: HTMLElement;
  @ViewChild('testUnauthorized') testUnauthorizedRef: TemplateRef<any>;

  fileinputOpt: any = {
    uploadUrl: "/file-upload-batch/2",
    previewFileIcon: '<i class="fas fa-file"></i>',
    allowedPreviewTypes: ['image', 'text'], // allow only preview of image & text files
    previewFileIconSettings: {
        'docx': '<i class="fas fa-file-word text-primary"></i>',
        'xlsx': '<i class="fas fa-file-excel text-success"></i>',
        'pptx': '<i class="fas fa-file-powerpoint text-danger"></i>',
        'pdf': '<i class="fas fa-file-pdf text-danger"></i>',
        'zip': '<i class="fas fa-file-archive text-muted"></i>',
    }
  }


  selectorUserModel: OrganizationDataRangeSearchViewModel = new OrganizationDataRangeSearchViewModel();
  caseID: string = '00320011700001';//測試資料
  datsa: any[];
  data: any[] = [
    {
      position: '動態組織選單(BU)',
      type: 1
    },
    {
      position: '從組織選人(總部)',
      type: 2
    },
    {
      position: '從組織選人(CC)',
      type: 3
    },
    {
      position: '從組織選人(廠商)',
      type: 4
    },
    {
      position: 'pdf 瀏覽',
      type: 5
    },
    {
      position: 'excel 瀏覽',
      type: 6
    },
    {
      position: '組織節點選人(全部)',
      type: 7
    },
    {
      position: '給組織節點顯示人員(總部)',
      type: 8
    },
    {
      position: '給組織節點顯示人員(CC)',
      type: 9
    },
    {
      position: '給組織節點顯示人員(廠商)',
      type: 10
    },
    {
      position: '組織節點顯示職稱(全部)',
      type: 11
    },
    {
      position: '選擇負責人職稱清單',
      type: 12
    },
    {
      position: 'KM組織樹',
      type: 13
    },
    {
      position: '標籤',
      type: 14
    },
    {
      position: '案件球(CC為例)',
      type: 15
    },
    {
      position: '案件標籤/案件時校/系統參數(BU)',
      type: 16
    },
    {
      position: '結案處置原因',
      type: 17
    },

    {
      position: '派工群組',
      type: 18
    },
    {
      position: '共通寄信畫面',
      type: 19
    },
    {
      position: '範本主檔選取',
      type: 20
    },
    {
      position: '左右清單',
      type: 21
    },
    {
      position: '拖曳清單',
      type: 22
    },
    {
      position: '客制化彈出視窗',
      type: 23
    },
    {
      position: '案件歷程',
      type: 24
    },
    {
      position: '測試回傳401',
      type: 25
    },
  ];

  columns = [
    {
      text: '組件名稱',
      name: 'position'
    },
    {
      text: '執行',
      name: 'column1',
      customer: true
    }
  ];

  test = [{ id: 1, text: 'test123' }];
  modelSelectByBU = {
    buID: null,
    caseWarning: null,
    hashTags: [],
    systemParameter: null
  }

  daulListInputs = {
    url: "Test/TestDualList",
    destIDs: [1, 3, 6],
    height: '300px'
  }

  dndAjaxOpt = {
    url: "Test/Test_Dnd",
    method: "Get",
    body: {
      buID: 24,
      parentID: 12
    }
  }

  sortCompare = (accumulator, currentValue) => accumulator.extend.Order - currentValue.extend.Order;

  constructor(
    public store: Store<any>,
    public http: HttpClient,
    public httpService: HttpService,
    public modalService: NgbModal,
    public sanitizer: DomSanitizer,
    public caseService: CaseService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {

    this.selectorUserModel.Goal = this.organizationType.Vendor;
    this.selectorUserModel.IsSelf = false;
    this.selectorUserModel.NodeID = null;
    // this.selectorUserModel.DefKey = this.definitionKey.STORE;
  }


  execute(type) {
    switch (type) {
      case 1: this.onBuNodeRelationSelector(); break;
      case 2: this.onHeaderQuarterNodeTreeUserSelector(); break;
      case 3: this.onCallCenterNodeTreeUserSelector(); break;
      case 4: this.onVendorNodeTreeUserSelector(); break;
      case 5: this.onPDFViewer(); break;
      case 6: this.onEXCELViewer(); break;
      case 7: this.onAllNodeTreeUserSelector(); break;
      case 8: this.onHeaderQuarterNodeUserModal(); break;
      case 9: this.onCallCenterNodeUserModal(); break;
      case 10: this.onVendorNodeUserModal(); break;
      case 11: this.onNodeDefinitionJobModal(); break;
      case 12: this.onStoreOwnerSelector(); break;
      case 13: this.onKMTreeModal(); break;
      case 14: this.onHashTagModal(); break;
      case 15: this.onCCballModal(); break;
      case 16: this.onSelectByBU(); break;
      case 17: this.onFinishReason(); break;
      case 18: this.onCaseAssignGroupUserModal(); break;
      case 19: this.onMailSender(); break;
      case 20: this.onCaseTemplateSelector(); break;
      case 21: this.onDualListModal(); break;
      case 22: this.onDndListModal(); break;
      case 23: this.onCustomerModal(); break;
      case 25: this.onUnauthorizedModal(); break;

    }
  }



  onBuNodeRelationSelector() {
    this.modalService.open(this.pop1, { size: 'lg', container: 'nb-layout' });
  }

  onHeaderQuarterNodeTreeUserSelector() {
    this.modalService.open(this.pop2, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onCallCenterNodeTreeUserSelector() {
    this.modalService.open(this.pop3, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onVendorNodeTreeUserSelector() {
    this.modalService.open(this.pop6, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }
  onAllNodeTreeUserSelector() {
    this.modalService.open(this.pop7, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onPDFViewer() {
    this.getDemo();
    this.modalService.open(this.pop4, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onEXCELViewer() {
    this.getDemo1();
    this.modalService.open(this.pop5, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onHeaderQuarterNodeUserModal() {
    const ref = this.modalService.open(HeadquarterNodeUserModalComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<HeadquarterNodeUserModalComponent>ref.componentInstance);
    instance.main = this.headquartermodel;
    instance.nodeID = 21;
  }

  onCallCenterNodeUserModal() {
    const ref = this.modalService.open(CallcenterNodeUserModalComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<CallcenterNodeUserModalComponent>ref.componentInstance);
    instance.main = this.ccmodel;
    instance.nodeID = 21;
  }



  onVendorNodeUserModal() {

    const ref = this.modalService.open(VendorNodeUserModalComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<VendorNodeUserModalComponent>ref.componentInstance);
    // instance.main = this.vendormodel; //目前無vendorModel
    instance.nodeID = 21;
  }

  onNodeDefinitionJobModal() {

    const ref = this.modalService.open(NodeDefinitionJobModalComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<NodeDefinitionJobModalComponent>ref.componentInstance);
    instance.nodeID = 1;
    instance.type = this.organizationType.Vendor;
  }

  onCaseTemplateSelector() {
    const ref = this.modalService.open(CaseTemplateSelectorComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<CaseTemplateSelectorComponent>ref.componentInstance);
    instance.model = { BuID: 47, ClassificKey: null, Content: null };
    instance.btnAdd = this.btnAddCaseTemplate.bind(this);

  }

  onStoreOwnerSelector() {
    const ref = this.modalService.open(StoreOwnerSelectorComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<StoreOwnerSelectorComponent>ref.componentInstance);
    instance.buID = 16;
    instance.nodeID = 21;
    instance.jobKey = "OWNER";
    instance.isTraversing = true;
  }

  onKMTreeModal() {
    this.getDemo3();
    this.modalService.open(this.pop12, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onHashTagModal() {
    this.getDemo4();
    this.modalService.open(this.pop14, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }
  onCCballModal() {
    this.modalService.open(this.pop15, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }
  onSelectByBU() {
    this.modalService.open(this.pop16, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }
  onFinishReason() {
    this.modalService.open(this.finishReason, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }
  onMailSender() {
    this.modalService.open(this.pop17, { size: 'lg', container: 'nb-layout', windowClass: 'modal-xl' });
  }

  onCaseAssignGroupUserModal() {
    const ref = this.modalService.open(CaseAssignGroupUserModalComponent, { size: 'lg', container: 'nb-layout' });
    const instance = (<CaseAssignGroupUserModalComponent>ref.componentInstance);
    instance.nodeID = 16;
    instance.btnAddUser = this.btnAddUser.bind(this);
  }

  onDualListModal() {
    const ref = this.modalService.open(this.pop18, { size: 'lg', container: 'nb-layout' });
  }

  onDndListModal() {
    const ref = this.modalService.open(this.pop19, { size: 'lg', container: 'nb-layout' });
  }

  btnAddUser(sss) {
    console.log(sss);
  }

  getDemo1() {

    //取得反應單PDF
    const work$ = this.caseService
      .getPreviewComplaintInvoice(24, "00120032400008", "A0012003011");

    //處理回應
    work$
      .subscribe(x => {
        this.excelSrc = x
      }, (error : HttpResponse<any>) => {
        
      },
        () => { });

    // this.http.post("http://localhost:60312/api/Test/TestXls", null, { responseType: 'blob' }).subscribe((x: any) => {

    //   const blob = new Blob([x], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    //   console.log(blob);

      
    //   (<any>blob).arrayBuffer().then(buffer => {

    //     this.excelSrc = new Uint8Array(buffer);
    //   });
    // });
  }

  getDemo() {

    //取得反應單PDF
    const work$ = this.caseService
      .getPreviewComplaintInvoice(24, "00120032400008", "A0012003011");

    //處理回應
    work$
      .subscribe(x => {
        this.pdfSrc = x
      }, (error : HttpResponse<any>) => {
        
      },
        () => { });

    // this.http.post("Case/Case/GetPreviewComplaintInvoice", null, { responseType: 'blob' }).subscribe((x: any) => {


    //   const blob = new Blob([x], { type: 'application/pdf' });
    //   console.log(blob);

    //   (<any>blob).arrayBuffer().then(buffer => {

    //     this.pdfSrc = new Uint8Array(buffer);
    //   });
    // });

  }

  getDemo3() {
    this.http.post("http://localhost:60312/api/Master/KMClassification/GetKMTree", {}, {
      params: {
        isSelf: 'false'
      }
    }).subscribe((x: any) => {
      console.log(x);
      this.kmNode = x;
    });
  }

  getDemo4() {
    const data = [
      { id: 1, text: '123' },
      { id: 2, text: '456' },
      { id: 3, text: '789' },
      { id: 4, text: '1234' }
    ];
    const default1 = [
      { id: 19, text: '87' },
    ];

    setTimeout(() => {
      this.tagData = data;
      this.defaultTag = default1;
    }, 0);
  }

  kmNodeClick($event) {
    console.log($event);
  }
  test1(ref) {
    console.log('-----------------------動態組織選單(BU)------------------------------');
    console.log('-----------------------取得所有階層------------------------------');
    console.table(ref.getAllValue());
    console.log('-----------------------取得有值的最末階層------------------------------');
    console.log(ref.getEndValue());
    console.log('-----------------------取得最高層級------------------------------');
    console.log(ref.getFirstValue());
    console.log('-----------------------取得最末層級------------------------------');
    console.log(ref.getLatestValue());
  }
  test2(ref) {
    console.log('-----------------------從組織選人(總部)------------------------------');
    console.log('-----------------------取得所選擇的人員清單------------------------------');
    console.table(ref.getValue());

  }
  test3(ref) {
    console.log('-----------------------從組織選人(CC)------------------------------');
    console.log('-----------------------取得所選擇的人員清單------------------------------');
    console.table(ref.getValue());

  }

  test4(ref) {
    console.log('-----------------------從組織選人(廠商)------------------------------');
    console.log('-----------------------取得所選擇的人員清單------------------------------');
    console.table(ref.getValue());
  }

  test5(ref) {
    console.log('-----------------------從組織選人(全部)------------------------------');
    console.log('-----------------------取得所選擇的人員清單------------------------------');
    console.table(ref.getValue());
  }

  test6(ref) {
    console.log('-----------------------從組織選人(全部)------------------------------');
    console.log('-----------------------取得所選擇的人員清單------------------------------');
    console.table(ref.getValue());
  }

  btnEnumSelect(ref) {
    console.log(ref.innerValue);
  }

  test16(hashTemRef) {
    console.log(hashTemRef.getValue());
  }

  onSend($event) {
    console.log($event);
  }

  onBack() {
    console.log('onBack');
  }

  onCustomerModal() {
    this.store.dispatch(new fromRootActions.AlertActions.CustomerAlertOpenAction({
      detail: {
        type: PtcSwalType.info,
        title: '測試標題',
        confirm: () => {
          console.log('onCOnfirm');
        }
      },
      templateRef: this.pop6
    }));
  }

  
  onUnauthorizedModal(){
    this.modalService.open(this.testUnauthorizedRef, { size: 'lg', container: 'nb-layout' });
  }

  btnAddCaseTemplate(data) {
    console.log("btnAddCaseTemplate => ", data);


    var payload = { CaseTemplateID: data.ID, CaseID: '00320032700001', InvoicID: "A0032003001" }

    this.http.post('Common/Master/ParseCaseTemplate/'.toHostApiUrl(), payload)
      .subscribe((res: any) => {
        console.log(res);
        // this.caseTemplateRes = res.element;

      })

  }

  testNotification(){
    this.httpService.get('Test/TestNotification')
      .subscribe(_ => {})
  }


  fileInputBlob: Blob;

  GetUnauthorized(){
    this.httpService.get('Test/Test_Resp_Get_Unauthorized')
      .subscribe(res => console.log("GetUnauthorized => ", res))
  }

  
  PostUnauthorized(){
    
    this.httpService.get('Test/Test_Resp_Post_Unauthorized')
      .subscribe(res => console.log("GetUnauthorized => ", res))
  }
  
  GetNotImplemented(){
    
    this.httpService.get('Test/Test_Resp_Get_NotImplemented')
      .subscribe(res => console.log("GetUnauthorized => ", res))
  }

  
  PostNotImplemented(){
    this.httpService.get('Test/Test_Resp_Post_NotImplemented')
      .subscribe(res => console.log("GetUnauthorized => ", res))
    }

}
