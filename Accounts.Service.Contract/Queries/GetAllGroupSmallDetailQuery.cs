using CORE2;
using Accounts.Service.Contract.DTOs;

namespace Accounts.Service.Contract.Queries
{
    public class GetAllGroupSmallDetailDTOQuery : Query<ListGroupSmallDetailDTO>
    {
        public GetAllGroupSmallDetailDTOQuery(BizflyIdentity identity)
            :base(identity)
        {

        }

    }
}
