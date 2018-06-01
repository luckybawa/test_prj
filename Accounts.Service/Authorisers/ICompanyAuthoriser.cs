using FluentValidation;

namespace Accounts.Service.Authorisers
{
    public interface IAuthoriser<T> : IValidator<T>
    {
    }

    public interface ICompanyAuthoriser<T> : IAuthoriser<T>
    {
    }

    public interface IProviderAuthoriser<T> : IAuthoriser<T>
    {
    }

    public interface IManagementAuthoriser<T> : IAuthoriser<T>
    {
    }
}
