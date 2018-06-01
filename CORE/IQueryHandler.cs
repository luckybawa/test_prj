using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> where TResult : DTO
    {
        Task<TResult> HandleAsync(TQuery query);
    }

    public interface ICompanyQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> where TResult : DTO
    {

    }

    public interface IProviderQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> where TResult : DTO
    {

    }

    public interface IManagementQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> where TResult : DTO
    {

    }

}
