using CORE2;
using Models.Accounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Service.Contract.DTOs
{
    public class GroupSmallDetailDTO : DTO
    {
        [JsonProperty(PropertyName = "groupId")]
        public string GroupId { get; set; }
        [JsonProperty(PropertyName = "groupType")]
        public virtual string GroupType { get; set; }
        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }
        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        public GroupSmallDetailDTO(Group group)
        {
            GroupId = group.Id;
            GroupType = group.GroupType;
            GroupName = group.GroupName;
            OwnerId = group.OwnerId;
            Status = group.Status.ToString();
            ETag = group.ETag;
        }
    }
}
