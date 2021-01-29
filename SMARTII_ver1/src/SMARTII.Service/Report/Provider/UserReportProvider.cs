using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Service.Report.Provider
{
    public class UserReportProvider : IUserReportProvider
    {
        public async Task<byte[]> GetAuthorityReport(UserSearchCondition condition, List<User> user)
        {

            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("使用者管理");

            ws.Cell(1, 1).Value = "使用者帳號：";
            ws.Cell(1, 2).Value = condition.Account;
            ws.Cell(1, 3).Value = "姓名：";
            ws.Cell(1, 4).Value = condition.Name;
            ws.Cell(1, 5).Value = "權限名稱：";
            ws.Cell(1, 6).Value = condition.RoleNames;
            ws.Cell(2, 1).Value = "AD：";
            ws.Cell(2, 2).Value = condition.IsAD;
            ws.Cell(2, 3).Value = "啟用狀態：";
            ws.Cell(2, 4).Value = condition.IsEnable;
            ws.Cell(2, 5).Value = "系統使用者：";
            ws.Cell(2, 6).Value = condition.IsSystemUser;



            var column = 3;

            foreach (var col in user)
            {
                ws.Range(4, column, 4, column + 1).Merge();
                ws.Cell(4, column).Value = col.Name;
                ws.Range(5, column, 5, column + 1).Merge();
                ws.Cell(5, column).Value = col.Account;
                ws.Range(6, column, 6, column + 1).Merge();
                ws.Cell(6, column).Value = col.IsEnabled ? "啟用" : "停用";
                ws.Cell(7, column).Value = "角色";
                ws.Cell(7, column + 1).Value = "個人";
                column += 2;
            }
            var componentList = WebComponentsCache.Instance.Components;

            var row = 8;

            foreach (var item in componentList)
            {
                ws.Range(row, 1, row + 5, 1).Merge();
                ws.Cell(row, 1).Value = item.Value;

                ws.Cell(row, 2).Value = "檢視";
                ws.Cell(row + 1, 2).Value = "新增";
                ws.Cell(row + 2, 2).Value = "刪除";
                ws.Cell(row + 3, 2).Value = "編輯";
                ws.Cell(row + 4, 2).Value = "匯出";
                ws.Cell(row + 5, 2).Value = "特殊權限";

                var colUser = 3;
                foreach (var uItem in user)
                {


                    var featureRoles = uItem.Roles.SelectMany(y => y.Feature).Where(x => x.Feature == item.Key && x.AuthenticationType != AuthenticationType.None).Any() ?
                        uItem.Roles.SelectMany(y => y.Feature).Where(x => x.Feature == item.Key && x.AuthenticationType != AuthenticationType.None)
                        : null;
                    if (featureRoles != null)
                    {
                        ws.Cell(row, colUser).Value = featureRoles.Select(x => x.AuthenticationType.HasFlag(AuthenticationType.Read)).Any(c => c == true) ? "V" : "";
                        ws.Cell(row + 1, colUser).Value = featureRoles.Select(x => x.AuthenticationType.HasFlag(AuthenticationType.Add)).Any(c => c == true) ? "V" : "";
                        ws.Cell(row + 2, colUser).Value = featureRoles.Select(x => x.AuthenticationType.HasFlag(AuthenticationType.Delete)).Any(c => c == true) ? "V" : "";
                        ws.Cell(row + 3, colUser).Value = featureRoles.Select(x => x.AuthenticationType.HasFlag(AuthenticationType.Update)).Any(c => c == true) ? "V" : "";
                        ws.Cell(row + 4, colUser).Value = featureRoles.Select(x => x.AuthenticationType.HasFlag(AuthenticationType.Report)).Any(c => c == true) ? "V" : "";
                        ws.Cell(row + 5, colUser).Value = featureRoles.Select(x => x.AuthenticationType.HasFlag(AuthenticationType.Admin)).Any(c => c == true) ? "V" : "";
                    }
                    var feature = uItem.Feature.Where(x => x.Feature == item.Key).Any() ?
                        uItem.Feature.Where(x => x.Feature == item.Key).FirstOrDefault()
                        : null;
                    if (feature != null)
                    {
                        if (feature.AuthenticationType != AuthenticationType.None)
                        {
                            ws.Cell(row, colUser + 1).Value = feature.AuthenticationType.HasFlag(AuthenticationType.Read) ? "V" : "";
                            ws.Cell(row + 1, colUser + 1).Value = feature.AuthenticationType.HasFlag(AuthenticationType.Add) ? "V" : "";
                            ws.Cell(row + 2, colUser + 1).Value = feature.AuthenticationType.HasFlag(AuthenticationType.Delete) ? "V" : "";
                            ws.Cell(row + 3, colUser + 1).Value = feature.AuthenticationType.HasFlag(AuthenticationType.Update) ? "V" : "";
                            ws.Cell(row + 4, colUser + 1).Value = feature.AuthenticationType.HasFlag(AuthenticationType.Report) ? "V" : "";
                            ws.Cell(row + 5, colUser + 1).Value = feature.AuthenticationType.HasFlag(AuthenticationType.Admin) ? "V" : "";
                        }
                        else if (feature.AuthenticationType == AuthenticationType.None)
                        {
                            ws.Cell(row, colUser + 1).Value = "X";
                            ws.Cell(row + 1, colUser + 1).Value = "X";
                            ws.Cell(row + 2, colUser + 1).Value = "X";
                            ws.Cell(row + 3, colUser + 1).Value = "X";
                            ws.Cell(row + 4, colUser + 1).Value = "X";
                            ws.Cell(row + 5, colUser + 1).Value = "X";
                        }
                    }

                    colUser += 2;
                }

                row += 6;
            }



            //左右至中
            ws.Range(1, 1, row - 1, 2 + (user.Count() * 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(1, 1, row - 1, 2 + (user.Count() * 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            //格線
            var rngTable1 = ws.Range(4, 1, row - 1, 2 + (user.Count() * 2));
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //自動對應欄位寬度
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();


            return ReportUtility.ConvertBookToByte(book, "");
        }
    }
}
