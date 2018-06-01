using Accounts.Service.Authorisers;
using CORE2;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Accounts.Service.Validators
{
    public class AuthoriserDecorator<T> : ICommandHandler<T> where T : Command
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IAuthoriser<T> _authoriser;

        public AuthoriserDecorator(ICommandHandler<T> handler, IAuthoriser<T> authoriser)
        {
            _authoriser = authoriser;
            _handler = handler;
        }

        public async Task Handle(T command)
        {
            if (_authoriser is null)
            {
                throw new UnauthorizedAccessException("Unauthorised");
            }
            _authoriser.ValidateAndThrow(command);
            await _handler.Handle(command);
        }
    }

}
