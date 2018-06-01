using Accounts.Service.Contract.Queries;
using FluentValidation;

namespace Accounts.Service.Authorisers.CompanyAuthorisers
{
    public class GetAllGroupAuthoriser :
        AbstractValidator<GetAllGroupSmallDetailDTOQuery>,
        ICompanyAuthoriser<GetAllGroupSmallDetailDTOQuery>
    {
        public GetAllGroupAuthoriser()
        {

        }
    }
}
