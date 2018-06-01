using Accounts.Service.Contract.Commands;
using FluentValidation;

namespace Accounts.Service.Authorisers.CompanyAuthorisers
{
    public class CompanyDeleteGroupAuthoriser : AbstractValidator<DeleteGroupCommand>, ICompanyAuthoriser<DeleteGroupCommand>
    {

        public CompanyDeleteGroupAuthoriser()
        {
            RuleFor(x => x.Identity.UserId).Must((instance, excecutingUserId) => instance.OwnerUserId == excecutingUserId);

        }
    }
}
