using Domain;
using Domain.AggregateModels.TaskAggregate;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class LabelRepository : Repository<Label>, ILabelRepository
    {
        public LabelRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
