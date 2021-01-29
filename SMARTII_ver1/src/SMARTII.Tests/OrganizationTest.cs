using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ptc.Data.Condition2;
using Ptc.Data.Condition2.Mssql.Class;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Tests
{
    [TestClass]
    public class OrganizationTest
    {
        public OrganizationTest()
        {
            // 初始化Autofac
            var container = DIConfig.Init();

            // 初始化AutoMapper
            MapperConfig.Init(container);

            // 初始化MSSQL CONDITION
            DataAccessConfiguration.Configure(Condition2Config.Setups);

            // 設定DI 容器
            DIBuilder.SetContainer(container);
        }

        [TestMethod]
        public void Test_Calc_PageAuth()
        {
            //var c = DIBuilder.Resolve<IOrganizationAggregate>();
            //var b = DIBuilder.Resolve<IUserAuthenticationManager>();

            //var con = new MSSQLCondition<USER>(x => x.USER_ID == "e855a36b-7168-416a-9e97-1ce85e676285");
            //con.IncludeBy(x => x.ROLE);
            //var user = c.User_T1_T2_.Get(con);
            //var role = user.Roles.FirstOrDefault();
            //var result = AuthenticationUtility.GetCompleteMergedPageAuth(role.Feature, user.Feature);
        }

        [TestMethod]
        public void Get_CCNode()
        {
            var c = DIBuilder.Resolve<IMSSQLRepository<HEADQUARTERS_NODE, HeaderQuarterNode>>();

            var list_1 = c.GetList();

            var b = DIBuilder.Resolve<IMSSQLRepository<HEADQUARTERS_NODE, IOrganizationNode>>();

            var list_2 = b.GetList();
        }

        [TestMethod]
        public async Task TryCast()
        {
            var c = DIBuilder.Resolve<IUserService>();
            // var a = await c.GetCompleteUserAsync("ptcsd4");

            // var s = new AccountUserViewModel(a, null);

            //var s = new NODE_JOB()
            //{
            //    ORGANIZATION_TYPE = 1
            //};
            //var c = new NODE_JOB()
            //{
            //    ORGANIZATION_TYPE = 2
            //};
            //var b = new NODE_JOB()
            //{
            //    ORGANIZATION_TYPE = 0
            //};

            //var x = new List<NODE_JOB>()
            //{
            //    s,c,b
            //};

            //var y = AutoMapper.Mapper.Map<IEnumerable<JobPosition>>(x);

            //var k = y.FirstOrDefault().ID;
            //var g = y.Last().LeftBoundary;

            //var cc = y.OfType<VendorJobPosition>().ToList();

            ////var u = x.Cast<CallCenterJobPosition>().ToList();
        }

        [TestMethod]
        public void Flatten_Nested_node()
        {
            var tree = new CallCenterNode()
            {
                NodeID = 0,
                Name = "root"
            };

            tree.Children.Add(new CallCenterNode()
            {
                NodeID = 1,
                Name = "root-1",
                Children = new List<Domain.Data.IRecursivelyModel>()
                {
                    new CallCenterNode(){
                         NodeID = 2,
                         Name = "root-1-1",
                    },
                    new CallCenterNode(){
                         NodeID = 3,
                         Name = "root-1-2",
                    }
                }
            });

            tree.Children.Add(new CallCenterNode()
            {
                Name = "root-2",
                NodeID = 4,
                Children = new List<Domain.Data.IRecursivelyModel>()
                {
                    new CallCenterNode(){
                         NodeID = 5,
                         Name = "root-2-1",
                            Children = new List<Domain.Data.IRecursivelyModel>(){
                                new CallCenterNode(){
                                    NodeID = 6,
                                    Name = "root-2-1-1"
                                }
                            }
                    },
                    new CallCenterNode(){
                        NodeID = 7,
                            Name = "root-2-2",
                    },
                    new CallCenterNode(){
                        NodeID = 8,
                            Name = "root-2-3",
                    }
                }
            });

            var flatten = tree.FlattenNSM();

            var ccnodes = new List<CallCenterNode>()
            {
                new CallCenterNode()
                {
                    NodeID = 0,
                    LeftBoundary = 1,
                    RightBoundary = 14,
                    Name = "root"
                },
                new CallCenterNode()
                {
                    NodeID = 1,
                    LeftBoundary = 2,
                    RightBoundary =5,
                    Name = "root-1"
                },
                  new CallCenterNode()
                {
                    NodeID = 2,
                    LeftBoundary = 6,
                    RightBoundary =13,
                    Name = "root-2"
                },
                new CallCenterNode()
                {
                    NodeID = 3,
                    LeftBoundary = 3,
                    RightBoundary =4,
                    Name = "root-1-1"
                },
                 new CallCenterNode()
                {
                    NodeID = 4,
                    LeftBoundary = 7,
                    RightBoundary =12,
                    Name = "root-2-1"
                },
                   new CallCenterNode()
                {
                    NodeID = 5,
                    LeftBoundary = 8,
                    RightBoundary =9,
                    Name = "root-2-1-1"
                },
                    new CallCenterNode()
                {
                    NodeID = 6,
                    LeftBoundary = 10,
                    RightBoundary =11,
                    Name = "root-2-1-2"
                },
            };

            var nested = ccnodes.AsNestedNSM();
        }

        [TestMethod]
        public void OrderBy_Node()
        {
            var c = DIBuilder.Resolve<IMSSQLRepository<HEADQUARTERS_NODE, HeaderQuarterNode>>();

            var test4 = 1.CompareTo(1);
            var test2 = 1.CompareTo(2);
            var test3 = 2.CompareTo(1);

            var @base = c.Get(
                    x => x.NODE_ID == 65 &&
                    x.ORGANIZATION_TYPE == (byte)0);

            var targets = @base.ParentPath.GetRootNodeParentPathArray();

            var parents = c.GetList(
                                x => targets.Contains(x.NODE_ID) &&
                                x.ORGANIZATION_TYPE == (byte)0);
            
            var order = parents.SortNode(x => x.NodeID, targets).ToList();
            var sort = order.Select(x => x.NodeID).ToList();

            var await = 0;

        }



        [TestMethod]
        public void TEST_Auth()
        {
            var c = DIBuilder.Resolve<IMSSQLRepository<ROLE, Role>>();


            var name = WebComponentsCache.Instance.TryGet("CaseTemplate");

            var targets = c.Get(x => x.ID == 2);

            var feature = targets.Feature.FirstOrDefault();

            var test = feature.AuthenticationType.HasFlag(AuthenticationType.Delete);

            var await = 0;

        }
    }
}
