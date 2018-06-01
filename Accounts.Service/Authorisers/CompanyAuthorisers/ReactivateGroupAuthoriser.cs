using Accounts.Service.Contract.Commands;
using FluentValidation;

namespace Accounts.Service.Authorisers.CompanyAuthorisers
{

    public class ReactivateGroupAuthoriser : AbstractValidator<ReactivateGroupCommand>, ICompanyAuthoriser<ReactivateGroupCommand>
    {

        public ReactivateGroupAuthoriser()
        {
            RuleFor(x => x.Identity.UserId).Must((instance, excecutingUserId) => instance.OwnerId == excecutingUserId);

        }
    }
}
