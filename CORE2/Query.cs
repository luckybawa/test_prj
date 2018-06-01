using CORE2;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE2
{
    public class Query<TResult> : IQuery<TResult> where TResult : DTO
    {
        public BizflyIdentity Identity { get; }
        public Query(BizflyIdentity identity)
        {
            Identity = identity;
        }
    }
}
