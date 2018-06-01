using CORE2;
using FluentValidation;
using System;
using System.Threading.Tasks;
namespace Accounts.Service.Validators
{
    public class ValidationDecoratorForQueries<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : Query<TResult> where TResult : DTO
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly IRatifier<TQuery> _validators;

        public ValidationDecoratorForQueries(IQueryHandler<TQuery, TResult> handler, IRatifier<TQuery> validators)
        {
            _validators = validators;
            _handler = handler;
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            if (_validators is null)
            {
                throw new NotImplementedException("No validator specified");
            }
            _validators.ValidateAndThrow(query);
            return await _handler.HandleAsync(query);
        }
    }
}
