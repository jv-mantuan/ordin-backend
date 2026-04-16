using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordin.Application.Interfaces
{
    public interface ICacheableQueryHandler<in TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
    }
}