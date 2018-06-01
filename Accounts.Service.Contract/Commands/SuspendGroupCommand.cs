
using CORE2;

namespace Accounts.Service.Contract.Commands
{
    public class SuspendGroupCommand : Command
    {
        public SuspendGroupCommand(BizflyIdentity identity,string companyId,string ownerId) 
            : base(identity)
        {
            CompanyId = companyId;
            OwnerId = ownerId;
        }
        public string CompanyId { get; set; }
        public string OwnerId { get; set; }
    }
}
