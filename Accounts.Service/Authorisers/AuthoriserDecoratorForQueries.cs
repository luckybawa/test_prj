using CORE2;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Accounts.Service.Authorisers
{
    public class AuthoriserDecoratorForQueries<TQuery,TResult> : IQueryHandler<TQuery,TResult> 
        where TQuery : Query<TResult> where TResult :DTO
    {
        private readonly IQueryHandler<TQuery,TResult> _handler;
        private readonly IAuthoriser<TQuery> _authoriser;

        public AuthoriserDecoratorForQueries(IQueryHandler<TQuery, TResult> handler, IAuthoriser<TQuery> authoriser)
        {
            _authoriser = authoriser;
            _handler = handler;
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            if (_authoriser is null)
            {
                throw new UnauthorizedAccessException("Unauthorised");
            }
            _authoriser.ValidateAndThrow(query);
           return  await _handler.HandleAsync(query);
        }
    }
}
