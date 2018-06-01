
using CORE2;

namespace Accounts.Service.Contract.Commands
{
    public class ReactivateGroupCommand : Command
    {
        public ReactivateGroupCommand(BizflyIdentity identity,string companyId,string ownerId) : base(identity)
        {
            CompanyId = companyId;
            OwnerId = ownerId;
        }

        public string OwnerId { get; set; }
        public string Etag { get; set; }
        public string CompanyId { get; set; }
    }
}
