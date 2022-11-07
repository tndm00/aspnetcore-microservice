using Contracts.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domains
{
    // abstract class - NOt allow create new instance
    public abstract class EntityBase<Tkey> : IEntityBase<Tkey>
    {
        public Tkey Id { get; set; }
    }
}
