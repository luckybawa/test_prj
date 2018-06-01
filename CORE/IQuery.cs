using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IQuery<TResult> where TResult :DTO
    {
        BizflyIdentity Identity { get; }
    }
}
