using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    public class SensorController
    {
        private readonly IMapper _mapper;
        private readonly ISensorManager<Sensor> _sensorManager { get; private set; }

        public SensorController(SensorManager sensorManager, IMapper mapper, ISensorManager sensorManager)
        {
            _sensorManager = sensorManager;
            _mapper = mapper;
        }
    }
}
