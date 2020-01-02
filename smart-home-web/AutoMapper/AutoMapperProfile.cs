using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.History;
using smart_home_web.Models;
using smart_home_web.Models.SensorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smart_home_web.Models.History;
using Infrastructure.Business.DTOs.Notification;
using smart_home_web.Models.Notification;

namespace smart_home_web.AutoMapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
	        CreateMap<RegisterViewModel, UserDTO>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
            CreateMap<UserDTO, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<LoginViewModel, UserDTO>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
            CreateMap<UserDTO, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<CreateSensorViewModel, SensorDto>().ReverseMap();
            CreateMap<SensorDto, Sensor>().ReverseMap();
            CreateMap<SensorDto, SensorViewModel>();

            CreateMap<CreateSensorViewModel, Sensor>();


            CreateMap<HistoryDto, History>().ReverseMap()
	            .ForMember(hd => hd.SensorName, map => map.MapFrom(h => h.Sensor.Name))
	            .ForMember(hd => hd.MeasurementName, map => map.MapFrom(vm => vm.Sensor.SensorType.MeasurementName))
	            .ForMember(hd => hd.MeasurementType, map => map.MapFrom(vm => vm.Sensor.SensorType.MeasurementType))
	            .ForMember(hd => hd.SensorId, map => map.MapFrom(vm => vm.Sensor.Id));
	            
            CreateMap<HistoryDto, HistoryViewModel>()
	            .ForMember(hd => hd.Value, map => map.MapFrom(vm => vm.GetStringValue()));

            CreateMap<NotificationDto, Message>().ReverseMap()
                .ForMember(hd => hd.Date, map => map.MapFrom(h => h.History.Date));
            CreateMap<NotificationDto, NotificationViewModel>().ReverseMap();

            CreateMap<GraphDTO, GraphViewModel>();
        }
    }
}
