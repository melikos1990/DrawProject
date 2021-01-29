using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ClosedXML.Excel;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;
using SMARTII.Domain.System;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Report.Factory
{
    public class ImportUserFactory : IImportUserFactory
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IAuthenticationAggregate _AuthenticationAggregate;
        private readonly ISystemAggregate _SystemAggregate;

        public ImportUserFactory(IOrganizationAggregate OrganizationAggregate, IAuthenticationAggregate AuthenticationAggregate, ISystemAggregate SystemAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _AuthenticationAggregate = AuthenticationAggregate;
            _SystemAggregate = SystemAggregate;
        }

        public bool ImportUser(IXLWorksheet worksheet, out string ErrorMsg)
        {
            bool success = true;
            ErrorMsg = "";
            try
            {
                // 定義資料起始、結束 Cell
                var firstCell = worksheet.FirstCellUsed();
                var lastCell = worksheet.LastCellUsed();

                // 使用資料起始、結束 Cell，來定義出一個資料範圍
                var data = worksheet.Range(firstCell.Address, lastCell.Address);

                // 將資料範圍轉型
                var table = data.AsTable();

                var list = new List<ImportUser>();
                int headRow = 0;
                foreach (var row in table.Rows())
                {
                    // 跳過標題列
                    if (headRow == 0)
                    {
                        headRow++;
                        continue;
                    }
                    ImportUser temp = new ImportUser();
                    // 取值

                    var RoleIDs = new List<int>();
                    if (string.IsNullOrEmpty(row.Cell("6").Value.ToString()) == false)
                    {
                        int RoleID = Convert.ToInt32(row.Cell("6").Value.ToString().Trim());
                        RoleIDs.Add(RoleID);
                    }

                    var user = new User()
                    {
                        LastChangePasswordDateTime = null,
                        UserID = Guid.NewGuid().ToString(),
                        Name = row.Cell("3").Value.ToString().Trim(),
                        Email = row.Cell("4").Value.ToString().Trim(),
                        IsEnabled = row.Cell("5").Value.ToString().Trim() == "1" ? true : false,
                        Mobile = row.Cell("7").Value.ToString().Trim(),
                        IsSystemUser = row.Cell("9").Value.ToString() == "1" ? true : false,
                    };

                    if (user.IsSystemUser)
                    {
                        user.Account = row.Cell("2").Value.ToString().Trim();
                        user.IsAD = row.Cell("8").Value.ToString().Trim() == "1" ? true : false;
                        user.ActiveStartDateTime = DateTime.Now;
                        user.ActiveEndDateTime = DateTime.MaxValue;
                    }
                    
                    temp.User = user;
                    temp.RoleIDs = RoleIDs.ToArray();
                    
                    if (list.Any(x => x.User.Account == user.Account && x.User.Name == user.Name))
                    {
                        var importUser = list.Where(x => x.User.Account == user.Account && x.User.Name == user.Name).FirstOrDefault();
                        RoleIDs.AddRange(importUser.RoleIDs);
                        importUser.RoleIDs = RoleIDs.ToArray();
                    }
                    else
                    {

                        list.Add(temp);
                    }


                }

                var test = list.Where(x => x.User.Mobile.Count() > 10);

                //匯入使用者
                using (var transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 2, 0), TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in list)
                    {
                        var user = item.User;
                        var roleIDs = item.RoleIDs;
                        if (user.IsAD)
                        {
                            // 如果是AD 帳戶 , 就必須驗證合法性
                            if (_AuthenticationAggregate.AD.IsADUser(user.Account) == false)
                                throw new Exception(Common_lang.AD_USER_NULL);
                        }
                        else
                        {
                            // 給予預設帳號
                            // 使用者初次登入時需給予該組預設帳號進行設置
                            //user.Password = SecurityCache.Instance.DefaultPassword.Md5Hash();
                            user.Password = _SystemAggregate.SystemParameter_T1_T2_.Get(x => 
                            x.KEY == EssentialCache.UserPasswordKeyValue.USER_DEFAULTCODE && x.ID == EssentialCache.UserPasswordKeyValue.SYSTEM_SETTING).Value.Md5Hash();

                            // 重設註記, 標註鎖定時間
                            user.LastChangePasswordDateTime = null;
                        }

                        if (user.IsSystemUser && string.IsNullOrEmpty(user.Account))
                            throw new Exception(Common_lang.USER_ACCOUNT_NULL);

                        _OrganizationAggregate.User_T1_T2_.Operator(context =>
                        {
                            var db = (SMARTIIEntities)context;
                            db.Configuration.LazyLoadingEnabled = false;

                            if (user.IsSystemUser && db.USER.Any(x => x.ACCOUNT.ToLower() == user.Account.ToLower()))
                                throw new Exception(Common_lang.ACCOUNT_EXIST);

                            var entity = AutoMapper.Mapper.Map<USER>(user);
                            entity.CREATE_DATETIME = DateTime.Now;
                            entity.CREATE_USERNAME = GlobalizationCache.APName;
                            entity.VERSION = DateTime.Now;

                            // 找到使用者操作權限
                            var rolesEntity = db.ROLE.Where(x => roleIDs.Contains(x.ID)).ToList();

                            // 綁定人員
                            entity.ROLE = rolesEntity;

                            db.USER.Add(entity);

                            db.SaveChanges();
                        });
                    }
                    transactionscope.Complete();
                }

            }
            catch (Exception ex)
            {
                ErrorMsg = ex.ToString();
                success = false;
            }

            return success;
        }
    }
}
