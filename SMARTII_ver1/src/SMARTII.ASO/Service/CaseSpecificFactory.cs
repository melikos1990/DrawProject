using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.ASO.Domain;
using SMARTII.Resource.Tag;

namespace SMARTII.ASO.Service
{
    public class CaseSpecificFactory : ICaseSpecificFactory
    {
        public readonly ICaseAggregate _CaseAggregate;
        public readonly IMasterAggregate _MasterAggregate;

        public CaseSpecificFactory(ICaseAggregate CaseAggregate,
                                    IMasterAggregate MasterAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
        }

        /// <summary>
        /// 局部更新資料
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        public Case Update(Case @case)
        {

            var particular = @case.GetParticular<ASO_Case>();

            if (particular.CaseItem == null)
                return @case;

            //驗證是否輸入必要欄位
            particular.CaseItem.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.ProductName))
                    throw new Exception(Case_lang.CASE_ITEM_REQUIRED);
            });

            //加入流水號
            int i = 1;
            particular.CaseItem.ForEach(x => x.ItemID = i++);

            // 這邊針對 PPCLIFE_Case 內獨有欄位進行更新
            particular.CaseItem?.ForEach(x => {
                x.CaseID = @case.CaseID;
                x.JContent = x.GetJContentUseExist(x);

            });


            //_CaseAggregate.Case_T1_T2_.Operator(x =>
            //{
            //    var context = (SMARTIIEntities)x;


            //    var query = context.CASE
            //                       .Include("CASE_ITEM")
            //                       .FirstOrDefault(g => g.CASE_ID == @case.CaseID);

            //    var ef = AutoMapper.Mapper.Map<List<CASE_ITEM>>(particular.CaseItem);

            //    query.CASE_ITEM = ef;

            //    context.SaveChanges();


            //});


            return @case;

        }
    }
}
