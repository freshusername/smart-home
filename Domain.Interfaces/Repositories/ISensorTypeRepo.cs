using Domain.Core.Model;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ISensorTypeRepo : IGenericRepository<SensorType>
    {
        Task<SensorType> GetLastSensorType();
    }
}
