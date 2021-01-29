using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Newtonsoft.Json;
using SMARTII.Domain.Organization;
using SMARTII.Configuration;
using Ptc.Data.Condition2;
using Autofac;
using System.Web.Http;

namespace StressTest
{
    /// <summary>
    /// CaseTest 的摘要說明
    /// </summary>
    [TestClass]
    public partial class CaseTest : BaseTest
    {

        public List<JobPosition> jobPositions = new List<JobPosition>
        {
            new JobPosition{ID = 17,OrganizationType = OrganizationType.CallCenter},
            new JobPosition{ID = 21,OrganizationType = OrganizationType.Vendor},
            new JobPosition{ID = 23,OrganizationType = OrganizationType.HeaderQuarter},
            new JobPosition{ID = 26,OrganizationType = OrganizationType.HeaderQuarter},
            new JobPosition{ID = 31,OrganizationType = OrganizationType.CallCenter},
            new JobPosition{ID = 37,OrganizationType = OrganizationType.HeaderQuarter},
            new JobPosition{ID = 73,OrganizationType = OrganizationType.HeaderQuarter},
        };

        public string CurrentFolderPath { get { return @"C:\project\SMARTII\src\StressTest"; } }

        public string CreateCaseDataPath { get { return $"{this.CurrentFolderPath}/Datas/立案"; } }

        
        public string CreateCaseSourceDataPath { get { return $"{this.CurrentFolderPath}/Datas/建立來源"; } }

        public string CaseSearchDataPath { get { return $"{this.CurrentFolderPath}/Datas/案件查詢"; } }


        public CaseTest()
        {
        }

        public void init()
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjoie1xyXG4gIFwiJGlkXCI6IFwiMVwiLFxyXG4gIFwiRG93blByb3ZpZGVyQlVEaXN0XCI6IHtcclxuICAgIFwiJGlkXCI6IFwiMlwiLFxyXG4gICAgXCJDYWxsQ2VudGVyXCI6IFtcclxuICAgICAgMTYsXHJcbiAgICAgIDI0LFxyXG4gICAgICAyOSxcclxuICAgICAgMzgsXHJcbiAgICAgIDQ2LFxyXG4gICAgICA0NyxcclxuICAgICAgNDgsXHJcbiAgICAgIDQ5XHJcbiAgICBdLFxyXG4gICAgXCJWZW5kb3JcIjogW1xyXG4gICAgICAxNixcclxuICAgICAgMjQsXHJcbiAgICAgIDI2LFxyXG4gICAgICAyOSxcclxuICAgICAgMzgsXHJcbiAgICAgIDQxLFxyXG4gICAgICA0NSxcclxuICAgICAgNDYsXHJcbiAgICAgIDQ3LFxyXG4gICAgICA0OCxcclxuICAgICAgNDlcclxuICAgIF0sXHJcbiAgICBcIkhlYWRlclF1YXJ0ZXJcIjogW1xyXG4gICAgICAzMixcclxuICAgICAgMjksXHJcbiAgICAgIDI5LFxyXG4gICAgICAyOVxyXG4gICAgXVxyXG4gIH0sXHJcbiAgXCJVc2VySURcIjogXCI5OTg1MGM0ZC1kYjg0LTRkMTEtYWE5Ny01MTkwMzExOWRmNjJcIixcclxuICBcIlBhc3N3b3JkXCI6IG51bGwsXHJcbiAgXCJBY2NvdW50XCI6IFwiZ29vMDEyM1wiLFxyXG4gIFwiQ3JlYXRlVXNlck5hbWVcIjogbnVsbCxcclxuICBcIkNyZWF0ZURhdGVUaW1lXCI6IFwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFxyXG4gIFwiRW1haWxcIjogXCJyc2hvdjAxMjNAcHRjLW5lYy5jb20udHdcIixcclxuICBcIklzQURcIjogZmFsc2UsXHJcbiAgXCJJc0VuYWJsZWRcIjogZmFsc2UsXHJcbiAgXCJMYXN0Q2hhbmdlUGFzc3dvcmREYXRlVGltZVwiOiBudWxsLFxyXG4gIFwiTG9ja291dERhdGVUaW1lXCI6IG51bGwsXHJcbiAgXCJQYXN0UGFzc3dvcmRRdWV1ZVwiOiBudWxsLFxyXG4gIFwiTmFtZVwiOiBcImdvb1wiLFxyXG4gIFwiTW9iaWxlUHVzaElEXCI6IG51bGwsXHJcbiAgXCJNb2JpbGVcIjogbnVsbCxcclxuICBcIkFkZHJlc3NcIjogbnVsbCxcclxuICBcIlRlbGVwaG9uZVwiOiBudWxsLFxyXG4gIFwiVXBkYXRlVXNlck5hbWVcIjogbnVsbCxcclxuICBcIlVwZGF0ZURhdGVUaW1lXCI6IG51bGwsXHJcbiAgXCJQaWN0dXJlXCI6IG51bGwsXHJcbiAgXCJJbWFnZVBhdGhcIjogXCJGaWxlL0dldFVzZXJJbWFnZT9maWxlTmFtZT05OTg1MGM0ZC1kYjg0LTRkMTEtYWE5Ny01MTkwMzExOWRmNjIuanBnXCIsXHJcbiAgXCJWZXJzaW9uXCI6IFwiMjAyMC0wMi0yNlQxMTo1NTo0OS41NzdcIixcclxuICBcIkFjdGl2ZVN0YXJ0RGF0ZVRpbWVcIjogbnVsbCxcclxuICBcIkFjdGl2ZUVuZERhdGVUaW1lXCI6IG51bGwsXHJcbiAgXCJJc1N5c3RlbVVzZXJcIjogZmFsc2UsXHJcbiAgXCJFeHRcIjogbnVsbCxcclxuICBcIlJvbGVzXCI6IFtdLFxyXG4gIFwiRmVhdHVyZVwiOiBbXSxcclxuICBcIkpvYlBvc2l0aW9uc1wiOiBbXSxcclxuICBcIlVzZXJQYXJhbWV0ZXJcIjoge1xyXG4gICAgXCIkaWRcIjogXCIzXCIsXHJcbiAgICBcIlVzZXJJRFwiOiBudWxsLFxyXG4gICAgXCJOYXZpZ2F0ZU9mTmV3YmllXCI6IGZhbHNlLFxyXG4gICAgXCJOb3RpY2VPZldlYnNpdGVcIjogZmFsc2UsXHJcbiAgICBcIkltYWdlUGF0aFwiOiBudWxsLFxyXG4gICAgXCJQaWN0dXJlXCI6IG51bGwsXHJcbiAgICBcIkZhdm9yaXRlRmVhdHVyZVwiOiBbXVxyXG4gIH1cclxufSIsIm5iZiI6MTU4MzIzMzQ4NiwiZXhwIjoxNTgzMjM1Mjg2LCJpYXQiOjE1ODMyMzM0ODYsImlzcyI6InNlbGYiLCJhdWQiOiJodHRwczovL3d3dy5teXdlYnNpdGUuY29tIn0.p6j3XP6pXm5NerW0AnjUXWYr0QQZ_kjKXcBIqSXQ6Bw";
            
        }


        public T RandomData<T>(string dataPath)
        {
            Random Rnd = new Random();
            
            var files = System.IO.Directory.GetFiles(dataPath);

            var number = Rnd.Next(0, (files.Length - 1));

            var json = File.ReadAllText(files[number]);


            return JsonConvert.DeserializeObject<T>(json);

        }

    }
}
