# **SMART II 次代系統** 

##### 後台開發環境 (API SITE)
- .Net Framwork 4.6.1 (C# 7.0)
- Visual Studio 2015 、 2017
- SSMS 2016
- ResX Manager (Extension)

##### 前台開發環境 (WEB SITE)
- Angular7 + Nebular/Bootstrap4 (ngx-Admin)
- ngrx (ver 7.+)
- node 10.16.0
- npm 6.9.0 
- Angular CLI 8.1.1
- ##### 啟動注意須知
  - npm i -g node-gyp
  - node_module/@nebular/theme/styles/core/_breaking-notice.scss 中 , @warn 中有不合法char 需移除
  - 參考 https://github.com/akveo/ngx-admin/tree/master/src/app/pages


##### 系統參考文件
- https://diagrams.visual-paradigm.com (業務流程)
- https://docs.google.com/spreadsheets/d/19qo8o3CWvIi3_mvbI2aAVPL2CbzfDo-Xq2HjrW6TUlQ/edit#gid=996562146 (系統細項)

#### 專案結構

```
SMARTII
│ 
└─── doc
│   │ 
│   └─── tools   
│       │ 
│       │   docfx.json ( 自動產生系統文件 )
│       │   use.txt    ( 使用手順 )
│       └─── diagram   
│       │ 
│       │   Domain_V1.classdiagram (vs2015)
│       │   Service_V1.classdiagram (vs2015)
│          
└─── build  
│   │   NuGet.Config ( Nuget下載路徑設定 )
│   
└─── src
│   │ 
│   └─── ap (.Net Framwork API)
│       │
│       └─── BusinessUnit   
│           │
│           └─── SMARTII.ASO (依照BU之特殊行為拆分)
│           │    ...
│           │
│           │
│       └─── SMARTII  (起始專案)
│           │
│           │
│           │
│       └─── SMARTII.Assist (輔助用 , 處理雜項)
│           │
│           │  
│           │  
│       └─── SMARTII.Domain ( 公布物件清單 )
│           │
│           │   
│           │   
│       └─── SMARTII.Resource (多國語系 , 其他資源..等)
│           │
│           │   
│           │   
│       └─── SMARTII.Service ( 實作服務邏輯 )
│           │
│           │   
│           │ 
│       └─── SMARTII.Tests 
│           │
│           │   
│           │
│   │ 
│   └─── web (angular)
```

#### 課題
| 平台 | 課題概要 | 詳細敘述 | 對應方式 | 對應者 |
| ------ | ------ |------ | ------  |------| 
| 後台 | AD登入驗證 | 未來系統登入時 , 需區分該人員是否為 AD帳戶 , 經由AD驗證後 ,系統授權登入。 | 尚未對應 |  無 |
| 後台 | 巢狀結構設計 | 當案件需要依照該巢狀關連進行查詢時 , 需考量從父階層查詢 , 且顧慮效能問題 。 | 尚未對應 |  無 |
| 後台 | DLL 結構拆分 | 未來系統可能會因為 BU 不同 , 物件內容、服務實作不同 , 因此需要藉由切分dll , 且整合Automapper , Autofac , 能夠依照不同BU進行呼叫。 | 複寫 MutipleFormConverter 與 JSONConverter 對應傳入項目, 傳出項目則各自實作Autofac.Module 與 Automapper.Profile  | 宏慈 |
| 後台 | 案件、商品、門市物件Layout定義 | 未來系統可能會因為 BU 不同 , 可能會導致物件規格不同 , 物件需先經過一般化定義 , 特殊化後的欄位考慮序列化為JSON , 並持久化。 | 需整合 | 宏慈 |
| 後台 | 動態報表 | 系統期望報表欄位可自行定義輸出 |  未來有可能搭配Power BI 與 WebApi 定義ReportLayout , 再且使用者下載Desktop自行編輯定義。  | 無 |
| 後台 | 通知機能整合 | 未來系統通知行為整合 , 目前有 `SMS` `Email` `Webpush` `MobilePush` `LineMessage` `FacebookMessage`需實作 | Factory 拆分功能 , 並依據通知方式定義載具 | 無 |
| 前台、後台 | SignalR整合 | 未來話機進線、案件異動 ..等須及時通知 | 需要於前台建立SignalRHub SingleInstance , 後台也需準備好Hub , 兩邊測通 | 無 |
| 前台、後台 | 話務整合 | 需參照CallCenterCloud 專案 , 將機能整合到新專案中 | 進線狀態需要測試以及確認 |  無 |
| 前台、後台 | 席位監看、Dashboard | 需參照CallCenterCloud 專案 , 將機能整合到新專案中 |  如詳細敘述 | 無 | 
| 前台、後台 | 多語系 | 後台透過resx管理多語系 , 搭配前台 @ngx-translate 進行語言切換 | 如詳細敘述 | 無 | 
| 前台 | Angular 整合 | 整合 ngx-Admin , 將Ptc Component 整合進去 (Select2 、TreeView 、Table ...) | 如詳細敘述 | 文傑、宏慈 |
| 前台 | Angular 頁面切割作業  |未來系統可能因為BU不同 , 而物件內容不同 , 需依照BU切分Module , 並定義特殊化後的物件、Component...等 |  如詳細敘述 | 文傑、宏慈 |



#### 依賴套件 (PTC)
| 專案名稱 | 路徑 | 敘述 |
| ------ | ------ |------ |
| Ptc.Data.Condition2 | http://tfs2:8080/tfs/sd4-collection/LibrarySD4/_git/Condition2?path=%2FREADME.md&version=GBmaster&_a=preview | 包裝資料存取行為
| Ptc.Logger | http://tfs2:8080/tfs/sd4-collection/_git/LibrarySD4?path=%2FPtc.Logger | 紀錄日誌檔案


