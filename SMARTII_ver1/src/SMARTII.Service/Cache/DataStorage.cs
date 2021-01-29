using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Autofac;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;

namespace SMARTII.Service.Cache
{
    public sealed class DataStorage
    {

        private static IContainer _container;
        private static object _lock = new object();

        public static readonly Lazy<DataStorage> Instance = new Lazy<DataStorage>(() => new DataStorage());

        private static IMasterAggregate _MasterAggregate { get; set; }
        private static ISystemAggregate _SystemAggregate { get; set; }
        private static IOrganizationAggregate _OrganizationAggregate { get; set; }
        private static IAuthenticationAggregate _AuthenticationAggregate { get; set; }


        public static void Initailize(IContainer container)
        {
            _container = container;

            _MasterAggregate = _container.Resolve<IMasterAggregate>();
            _SystemAggregate = _container.Resolve<ISystemAggregate>();
            _OrganizationAggregate = _container.Resolve<IOrganizationAggregate>();
            _AuthenticationAggregate = _container.Resolve<IAuthenticationAggregate>();


            InitStatic();

        }


        public static void Refresh()
        {
            InitStatic();
        }

        private static void InitStatic()
        {
            InitialWorkTime();
            InitialWorkSchedule();
            InitialWorkDay();
            InitialCanFastFinishedCase();
            InitialCaseCanFinishReturn();
            InitialCaseCanGetAssignmentUser();
            InitCaseMultipleTicketType();
            InitCaseInvoiceSheetPassword();
            InitialCaseComplaintInvoiceType();
            InitCaseNearlyDays();
            InitStoreType();
            InitCaseSourceType();
            InitZipPassWord();
            InitEmailDefaultTitle();
            InitNodeKey();
            InitLDAP();
        }

        public static void InitialWorkSchedule()
        {


            var start = new DateTime(DateTime.Now.Year, 1, 1);

            // 取得到明年度的
            // 預防跨年度時 , 靜態資料並未更新
            var end = new DateTime((DateTime.Now.Year + 1), 12, 31);

            WorkScheduleDict =
               _MasterAggregate.WorkSchedule_T1_T2_
                               .GetList(x => x.DATE <= end &&
                                             x.DATE >= start)
                               .ToList()?
                               .GroupBy(x => x.NodeID)
                               .ToDictionary(x => x.Key, e => e.AsEnumerable());

        }

        public static void InitialWorkTime()
        {


            WorkTimeDict =
                _SystemAggregate.SystemParameter_T1_T2_
                                .GetList(x => x.ID == EssentialCache.WorkValue.ServiceTime)?
                                .ToDictionary(
                                    x => x.Key,
                                    e =>
                                    {
                                        string start = e.Value.Split('-')[0];
                                        string end = e.Value.Split('-')[1];

                                        return new TimeSpanRange(start, end);

                                    });

        }

        public static void InitialWorkDay()
        {


            WorkOffDayDict =
                _SystemAggregate.SystemParameter_T1_T2_
                                .GetList(x => x.ID == EssentialCache.WorkValue.WorkOffDay)?
                                .ToDictionary(
                                    x => x.Key,
                                    e =>
                                    {
                                        var days = e.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                        return days.Select(x => x.GetEnumFromString<DayOfWeek>());

                                    });



        }

        public static void InitialCanFastFinishedCase()
        {
            _CanFastFinishedCaseArray =
                _SystemAggregate.SystemParameter_T1_T2_
                                .GetList(x => x.ID == EssentialCache.CaseValue.CASE_ALLOW_FASTCLOSE)?
                                .Select(x => x.Key)
                                .ToArray();
        }
        public static void InitialCaseCanFinishReturn()
        {
            _CaseCanFinishReturnArray =
                _SystemAggregate.SystemParameter_T1_T2_
                                .GetList(x => x.ID == EssentialCache.CaseValue.CASE_FINISHRETURN)?
                                .Select(x => x.Key)
                                .ToArray();
        }
        public static void InitialCaseCanGetAssignmentUser()
        {
            _CaseCanGetAssignmentUserArray =
                _SystemAggregate.SystemParameter_T1_T2_
                                .GetList(x => x.ID == EssentialCache.CaseValue.CASE_GETASSIGNMENTUSER)?
                                .Select(x => x.Key)
                                .ToArray();
        }

        public static void InitialCaseComplaintInvoiceType()
        {
            _CaseAssignmentComplaintInvoiceTypeDict =
                _SystemAggregate.SystemParameter_T1_T2_
                                .GetList(x => x.ID == EssentialCache.CaseValue.CASE_ASSIGNMENTINVOICECOMPLAIN)?
                                .ToDictionary(x => x.Key, v =>
                                {
                                    return JsonConvert.DeserializeObject<List<SelectItem>>(v.Value);
                                });


        }

        public static void InitCaseMultipleTicketType()
        {
            _CaseMutipleTicketTypeDict =
                _SystemAggregate.SystemParameter_T1_T2_
                .GetList(x => x.ID == EssentialCache.CaseValue.CASE_MULTIASSIGNMENTINVOICECOMPLAIN)?
                .ToDictionary(x => x.Key,
                              e =>
                              {
                                  var id = int.Parse(e.Value);
                                  return (CaseMutipleTicketType)Enum.ToObject(typeof(CaseMutipleTicketType), id);

                              });


        }

        public static void InitCaseInvoiceSheetPassword()
        {
            _CaseInvoiceSheetPasswordDict =
                _SystemAggregate.SystemParameter_T1_T2_
                .GetList(x => x.ID == EssentialCache.CaseValue.CASE_ASSIGNMENTINVOICECOMPLAIN_ZIPCODE)?
                .ToDictionary(x => x.Key,
                              e => e.Value);


        }

        public static void InitCaseNearlyDays()
        {
            _CaseNearlyDaysDict =
             _SystemAggregate.SystemParameter_T1_T2_
             .GetList(x => x.ID == EssentialCache.CaseValue.SOURCE_RELATEDCASE)?
             .ToDictionary(x => x.Key,
                           e => int.Parse(e.Value) as int?);
        }

        public static void InitStoreType()
        {
            _StoreTypeDict =
               _SystemAggregate.SystemParameter_T1_T2_
                               .GetList(x => x.ID == EssentialCache.MasterValue.STORE_TYPE)?
                               .ToDictionary(x => x.Key, v =>
                               {
                                   return JsonConvert.DeserializeObject<List<KeyValuePair>>(v.Value);
                               });
        }

        public static void InitLDAP()
        {
            var value = _SystemAggregate.SystemParameter_T1_T2_
                               .GetFirstOrDefault(
                                    new MSSQLCondition<Database.SMARTII.SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LDAPValue.LDAP_AUTHENTICATION && x.KEY == EssentialCache.LDAPValue.USER)
                                )?
                               .Value;

            if (string.IsNullOrEmpty(value)) return;

            var keyValPair = JsonConvert.DeserializeObject<KeyValuePair>(value);

            _AuthenticationAggregate.AD.SetUp(keyValPair.Key, keyValPair.Value);

        }

        public static void InitCaseSourceType()
        {
            _CaseSourceDict =
               _SystemAggregate.SystemParameter_T1_T2_
                               .GetList(x => x.ID == EssentialCache.CaseValue.CASE_SOURCE)?
                               .ToDictionary(x => x.Key, v =>
                               {
                                   return JsonConvert.DeserializeObject<List<SelectItem>>(v.Value);
                               });
        }
        public static void InitZipPassWord()
        {
            _ZipPassWord =
                _SystemAggregate.SystemParameter_T1_T2_
                .GetList(x => x.ID == EssentialCache.CaseValue.DAILYREPORT_ZIPCODE)?
                .ToDictionary(x => x.Key,
                              e => e.Value);
        }

        public static void InitEmailDefaultTitle()
        {
            _EmailDefaultTitleDict =
                _SystemAggregate.SystemParameter_T1_T2_
                .GetList(x => x.ID == EssentialCache.EmailValue.EMAIL_DEFAULT_TITLE)?
                .ToDictionary(x => x.Key,
                              e => e.Value);
        }

        public static void InitNodeKey()
        {
            _NodeKeyDict =
                _OrganizationAggregate.HeaderQuarterNode_T1_T2_
                .GetList(x => x.NODE_TYPE_KEY == EssentialCache.NodeDefinitionValue.BusinessUnit)
                .ToDictionary(x => x.ID, e => e.NodeKey);
        }

        private static string[] _CanFastFinishedCaseArray;
        private static string[] _CaseCanFinishReturnArray;
        private static string[] _CaseCanGetAssignmentUserArray;
        private static Dictionary<string, TimeSpanRange> _WorkTimeDict;
        private static Dictionary<string, IEnumerable<DayOfWeek>> _WorkDayDict;
        private static Dictionary<int, IEnumerable<WorkSchedule>> _WorkScheduleDict;
        private static Dictionary<string, CaseMutipleTicketType> _CaseMutipleTicketTypeDict;
        private static Dictionary<string, string> _CaseInvoiceSheetPasswordDict;
        private static Dictionary<string, List<SelectItem>> _CaseAssignmentComplaintInvoiceTypeDict;
        private static Dictionary<string, int?> _CaseNearlyDaysDict;
        private static Dictionary<string, List<KeyValuePair>> _StoreTypeDict;
        private static Dictionary<string, List<SelectItem>> _CaseSourceDict;
        private static Dictionary<string, string> _ZipPassWord;
        private static Dictionary<string, string> _EmailDefaultTitleDict;
        private static Dictionary<int, string> _NodeKeyDict;

        /// <summary>
        /// 各企業別年度之工作行事曆
        /// 企業別代號
        /// </summary>
        public static Dictionary<int, IEnumerable<WorkSchedule>> WorkScheduleDict
        {
            get
            {
                lock (_lock)
                {
                    return _WorkScheduleDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _WorkScheduleDict = value;
                }
            }
        }

        /// <summary>
        /// 各企業別之工作時間
        /// key : 企業別三碼代號
        /// </summary>
        public static Dictionary<string, TimeSpanRange> WorkTimeDict
        {
            get
            {
                lock (_lock)
                {
                    return _WorkTimeDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _WorkTimeDict = value;
                }
            }
        }

        /// <summary>
        /// 各企業別之工作天
        /// key : 企業別三碼代號
        /// </summary>
        public static Dictionary<string, IEnumerable<DayOfWeek>> WorkOffDayDict
        {
            get
            {
                lock (_lock)
                {
                    return _WorkDayDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _WorkDayDict = value;
                }
            }
        }

        /// <summary>
        /// 各企業別能否開立多張反應單
        /// </summary>
        public static Dictionary<string, CaseMutipleTicketType> CaseMutipleTicketTypeDict
        {
            get
            {
                lock (_lock)
                {
                    return _CaseMutipleTicketTypeDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseMutipleTicketTypeDict = value;
                }
            }
        }

        /// <summary>
        /// 反應單保護密碼
        /// </summary>
        public static Dictionary<string, string> CaseInvoiceSheetPasswordDict
        {
            get
            {
                lock (_lock)
                {
                    return _CaseInvoiceSheetPasswordDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseInvoiceSheetPasswordDict = value;
                }
            }
        }

        /// <summary>
        /// 各企業別之反應單型態
        /// </summary>
        public static Dictionary<string, List<SelectItem>> CaseAssignmentComplaintInvoiceTypeDict
        {
            get
            {
                lock (_lock)
                {
                    return _CaseAssignmentComplaintInvoiceTypeDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseAssignmentComplaintInvoiceTypeDict = value;
                }
            }
        }

        /// <summary>
        /// 近期案件範圍天數
        /// </summary>
        public static Dictionary<string, int?> CaseNearlyDaysDict
        {
            get
            {
                lock (_lock)
                {
                    return _CaseNearlyDaysDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseNearlyDaysDict = value;
                }
            }
        }

        /// <summary>
        /// 門市型態
        /// </summary>
        public static Dictionary<string, List<KeyValuePair>> StoreTypeDict
        {
            get
            {
                lock (_lock)
                {
                    return _StoreTypeDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _StoreTypeDict = value;
                }
            }
        }

        /// <summary>
        /// 能否快速結案之註記
        /// Key : 企業別三碼代號
        /// </summary>
        public static string[] CanFastFinishedCaseArray
        {
            get
            {
                lock (_lock)
                {
                    return _CanFastFinishedCaseArray;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CanFastFinishedCaseArray = value;
                }
            }
        }

        /// <summary>
        /// 能否使用結案回上頁 註記
        /// Key : 企業別三碼代號
        /// </summary>
        public static string[] CaseCanFinishReturnArray
        {
            get
            {
                lock (_lock)
                {
                    return _CaseCanFinishReturnArray;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseCanFinishReturnArray = value;
                }
            }
        }

        /// <summary>
        /// 能否新增轉派時，將轉派對象節點人員帶入Mail清單 註記
        /// Key : 企業別三碼代號
        /// </summary>
        public static string[] CaseCanGetAssignmentUser
        {
            get
            {
                lock (_lock)
                {
                    return _CaseCanGetAssignmentUserArray;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseCanGetAssignmentUserArray = value;
                }
            }
        }

        /// <summary>
        /// 案件來源
        /// </summary>
        public static Dictionary<string, List<SelectItem>> CaseSourceDict
        {
            get
            {
                lock (_lock)
                {
                    return _CaseSourceDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _CaseSourceDict = value;
                }
            }
        }
        /// <summary>
        /// Zip密碼
        /// </summary>
        public static Dictionary<string,string > ZipPassWord
        {
            get
            {
                lock (_lock)
                {
                    return _ZipPassWord;
                }
            }
            set
            {
                lock (_lock)
                {
                    _ZipPassWord = value;
                }
            }
        }

        /// <summary>
        /// 各BU的罐頭訊息
        /// </summary>
        public static Dictionary<string, string> EmailDefaultTitleDict
        {
            get
            {
                lock (_lock)
                {
                    return _EmailDefaultTitleDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _EmailDefaultTitleDict = value;
                }
            }
        }

        /// <summary>
        /// NodeID 轉換 NodeKey
        /// </summary>
        public static Dictionary<int, string> NodeKeyDict
        {
            get
            {
                lock (_lock)
                {
                    return _NodeKeyDict;
                }
            }
            set
            {
                lock (_lock)
                {
                    _NodeKeyDict = value;
                }
            }
        }
        
    }
}
