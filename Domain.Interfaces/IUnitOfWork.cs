using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Sensor> SensorRepo { get; }
        int Save();
    }
}
