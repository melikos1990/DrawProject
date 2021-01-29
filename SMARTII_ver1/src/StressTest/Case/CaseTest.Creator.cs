using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SMARTII.Areas.Case.Models;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Transaction;
using SMARTII.Models.Account;
using SMARTII.Tests;

namespace StressTest
{
    public partial class CaseTest: BaseTest
    {

        [TestMethod]
        public async Task Test_CaseCreate()
        {
            try
            {

                var data = this.RandomData<CaseSourceDetailViewModel>(this.CreateCaseDataPath);

                URL = "Case/Case/SaveCaseSourceComplete";

                var result = await PostAsync<CaseSourceDetailViewModel, JsonResult<CaseSourceDetailViewModel>>(data, this.jobPositions);

                var wait = 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod]
        public async Task Test_CaseSourceCreate()
        {
            try
            {


                var data = this.RandomData<CaseSourceDetailViewModel>(this.CreateCaseSourceDataPath);

                URL = "Case/Case/SaveCaseSource";

                var result = await PostAsync<CaseSourceDetailViewModel, JsonResult<CaseSourceDetailViewModel>>(data, this.jobPositions);

                var wait = 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
