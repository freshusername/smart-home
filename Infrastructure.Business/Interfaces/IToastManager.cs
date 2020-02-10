using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IToastManager
    {
        Task ShowMessage(Guid token, string value);
        Task<IEnumerable<ToastDto>> GetToastsBySensorId(int sensorId);
        Task<ToastDto> GetById(int id);
        Task<OperationDetails> Create(ToastDto toastDto);
    }
}
