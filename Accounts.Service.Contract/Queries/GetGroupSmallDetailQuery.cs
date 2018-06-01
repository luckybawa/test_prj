using CORE2;
using Accounts.Service.Contract.DTOs;

namespace Accounts.Service.Contract.Queries
{
    public class GetGroupSmallDetailDTOQuery : Query<GroupSmallDetailDTO>
    {
        public string Id { get; set; }
        public GetGroupSmallDetailDTOQuery(BizflyIdentity identity,string id)
            :base(identity)
        {
            Id = id;
        }

    }
}
