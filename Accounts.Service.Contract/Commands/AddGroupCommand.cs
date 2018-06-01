
using CORE2;

namespace Accounts.Service.Contract.Commands
{

    public class AddGroupCommand : Command
    {
        public AddGroupCommand(BizflyIdentity identity, string newGroupId, string ownerUserId, string groupName, string groupType) : base(identity)
        {
            NewGroupId = newGroupId;
            OwnerUserId = ownerUserId;
            GroupName = groupName;
            GroupType = groupType;
        }

        public string NewGroupId { get;  }
        public string OwnerUserId { get; }
        public string GroupName { get; }
        public string GroupType { get; }
    }
}
