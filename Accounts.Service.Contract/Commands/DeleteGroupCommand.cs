
using CORE2;

namespace Accounts.Service.Contract.Commands
{
    public class DeleteGroupCommand : Command
    {

        public DeleteGroupCommand(BizflyIdentity identity, string id, string ownerId) : base(identity)
        {
            Id = id;
            OwnerUserId = ownerId;
        }
        public string GroupEtag { get; set; }
        public string OwnerUserId { get; set; }
        public string Id { get; set; }
    }
}
