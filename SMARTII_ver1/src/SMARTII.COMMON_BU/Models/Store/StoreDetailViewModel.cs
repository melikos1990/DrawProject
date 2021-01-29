using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using SMARTII.COMMON_BU.Models.User;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Service.Cache;

namespace SMARTII.COMMON_BU.Models.Store
{
    public class StoreDetailViewModel
    {
        public StoreDetailViewModel()
        {
        }

        public StoreDetailViewModel(Store<ExpandoObject> data)
        {
            this.NodeID = data.NodeID;
            this.BuID = data.BuID;
            this.BuName = data.BuName;
            this.NodeKey = data.BuKey;
            
            this.Address = data.Address;
            this.IsEnabled = data.IsEnabled;
            this.StoreCloseDateTime = data.StoreCloseDateTime.DisplayWhenNull(text: "無");
            this.Code = data.Code;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.Email = data.Email;
            this.Name = data.Name;
            this.StoreOpenDateTime = data.StoreOpenDateTime.DisplayWhenNull(text: "無");
            this.ServiceTime = data.ServiceTime;
            this.StoreType = data.StoreType;
            this.Telephone = data.Telephone;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = data.UpdateUserName;
            this.Memo = data.Memo;
            this.OwnerNodeJobID = data.OwnerNodeJobID;
            this.SupervisorNodeJobID = data.SupervisorNodeJobID;
            this.Particular = data.Particular;

            this.NodeParentIDPath = data.HeaderQuarterNode?
                                        .ParentPath
                                        .GetRootNodeParentPathArray()
                                        .Concat(new int[] { this.NodeID })
                                        .ToArray();


            this.OFCUsers = data.OfcJobPosition?
                                .Users?
                                .Select(x => new UserListViewModel(x, data.OfcJobPosition.Job.Name, data.OfcJobPosition.NodeName))
                                .ToList() ?? new List<UserListViewModel>();

            this.OwnerUsers = data.OwnerJobPosition?
                                  .Users?
                                  .Select(x => new UserListViewModel(x, data.OwnerJobPosition.Job.Name, data.Name))
                                  .ToList() ?? new List<UserListViewModel>();

            if (DataStorage.StoreTypeDict.TryGetValue(data.BuKey, out var collection))
            {
                this.StoreTypeName = collection.FirstOrDefault(x => x.Key == this.StoreType.ToString())?.Value;
            }
        }



        public Domain.Organization.Store ToDomain()
        {
            var result = new Domain.Organization.Store()
            {
                NodeID = this.NodeID,
                Code = this.Code,
                IsEnabled = this.IsEnabled,
                Name = this.Name,
                Address = this.Address,
                ServiceTime = this.ServiceTime,
                Email = this.Email,
                Telephone = this.Telephone,
                Memo = this.Memo,
                StoreType = this.StoreType,
                StoreCloseDateTime = string.IsNullOrEmpty(this.StoreCloseDateTime) ? default(DateTime?) : Convert.ToDateTime(this.StoreCloseDateTime),
                StoreOpenDateTime = string.IsNullOrEmpty(this.StoreOpenDateTime) ? default(DateTime?) : Convert.ToDateTime(this.StoreOpenDateTime),
                OrganizationType = OrganizationType.HeaderQuarter,
                OwnerNodeJobID = this.OwnerNodeJobID,
                SupervisorNodeJobID = this.SupervisorNodeJobID,
            };

            return result;
        }

        public int NodeID { get; set; }

        public int BuID { get; set; }

        public string BuName { get; set; }


        public string Address { get; set; }

        public bool IsEnabled { get; set; }

        public int BehaviorType { get; set; }

        public string StoreCloseDateTime { get; set; }

        public string Code { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string StoreOpenDateTime { get; set; }

        public string ServiceTime { get; set; }

        public int StoreType { get; set; }

        public string StoreTypeName { get; set; }

        public string Telephone { get; set; }

        public string UpdateDateTime { get; set; }

        public string UpdateUserName { get; set; }

        public string Memo { get; set; }

        public int? OwnerNodeJobID { get; set; }

        public int? SupervisorNodeJobID { get; set; }



        public ExpandoObject Particular { get; set; }

        public string NodeKey { get; set; }

        public int[] NodeParentIDPath { get; set; }

        public string DynamicForm { get; set; }

        public List<UserListViewModel> OwnerUsers { get; set; } = new List<UserListViewModel>();

        public List<UserListViewModel> OFCUsers { get; set; } = new List<UserListViewModel>();
    }
}
