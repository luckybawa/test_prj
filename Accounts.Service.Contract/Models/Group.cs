using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using CORE2;

namespace Models.Accounts
{
    public class Group : Node
    {
        // created this default constructor because JSON.NET was throw error when serialize this object
        public Group(){}

        public Group(string groupType, string groupName, string ownerId, GroupStatus status, dynamic profile, Dictionary<string, string[]> customPermissions)
        {
            Id = GroupId;
            GroupType = groupType;
            GroupName = groupName;
            OwnerId = ownerId;
            Status = status;
            Profile = profile;
            CustomPermissions = customPermissions;
        }
        public Group(string newgroupId,string groupType, string groupName, string ownerId, GroupStatus status, dynamic profile, Dictionary<string, string[]> customPermissions, CommandInformation commandInformation)
        {
            Id = newgroupId;
            GroupId = newgroupId;
            GroupType = groupType;
            GroupName = groupName;
            OwnerId = ownerId;
            Status = status;
            Profile = profile;
            CustomPermissions = customPermissions;
            CreatedCommandInformation = commandInformation;
            LastChangedCommandInformation = commandInformation;
        }

        [JsonProperty(PropertyName = "groupType")]
        public virtual string GroupType { get; set; }
        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }
        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }
        [JsonProperty(PropertyName = "status")]
        public GroupStatus Status { get; set; }
        [JsonProperty(PropertyName = "profile")]
        public dynamic Profile { get; set; }
        [JsonProperty(PropertyName = "customPermissions")]
        public Dictionary<string, string[]> CustomPermissions { get; set; }
    }

    public enum GroupStatus { ACTIVE, PAUSED, PENDING_DELETION, SUSPENDED}
}
