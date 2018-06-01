using CORE2;
using Microsoft.AspNetCore.Http;

namespace Accounts.Company.API.Utilities
{
    public static class UtilityExtensions
    {

        public static BizflyIdentity GetIdentityFromHeaders(this IHeaderDictionary headers)
        {
            var bizflyIdentity = new BizflyIdentity();

            if (headers.TryGetValue("userId", out var userId))
            {
                bizflyIdentity.UserId = userId.ToString();
            }
            if (headers.TryGetValue("groupId", out var groupId))
            {
                bizflyIdentity.GroupId = userId.ToString();
            }
            if (headers.TryGetValue("groupType", out var groupType))
            {
                bizflyIdentity.GroupType = userId.ToString();
            }
            if (headers.TryGetValue("permissions", out var permissions))
            {
                bizflyIdentity.Permissions = userId.ToString().Split(',');
            }

            return bizflyIdentity;
        }
    }
}
