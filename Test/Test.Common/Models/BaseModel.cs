using System.Collections.Generic;

namespace Test.Common.Models
{
    public interface BaseModel<TEntity>
    {

        TEntity Entity { get; }

        IList<TEntity> EntityList { get; }

    }
}
