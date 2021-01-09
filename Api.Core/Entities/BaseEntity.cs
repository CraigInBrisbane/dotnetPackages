using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Entities
{
    public abstract class BaseEntity
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
