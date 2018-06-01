using System;
using System.Collections.Generic;
using System.Text;

namespace CORE2
{
    public interface IQuery<TResult> where TResult :DTO
    {
        BizflyIdentity Identity { get; }
    }
}
