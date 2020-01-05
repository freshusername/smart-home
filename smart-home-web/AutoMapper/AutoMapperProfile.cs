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
using Infrastructure.Business.DTOs.SensorType;
using smart_home_web.Models.SensorType;
using Infrastructure.Business.DTOs.Icon;
using System.IO;
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

            CreateMap<Sensor, SensorDto>()
                .ForMember(dto => dto.IconPath, map => map.MapFrom(s => (s.IconId.HasValue) ? s.Icon.Path : s.SensorType.Icon.Path))
                .ForMember(dto => dto.SensorTypeId, map => map.MapFrom(s => s.SensorType.Id))
                .ForMember(dto => dto.SensorTypeName, map => map.MapFrom(s => s.SensorType.Name))
                .ReverseMap();
            CreateMap<SensorDto, Sensor>();
            CreateMap<SensorDto, SensorViewModel>();
            CreateMap<CreateSensorViewModel, SensorDto>();

            CreateMap<Icon, IconDto>().ReverseMap();

            CreateMap<HistoryDto, History>().ReverseMap()
	            .ForMember(hd => hd.SensorName, map => map.MapFrom(h => h.Sensor.Name))
	            .ForMember(hd => hd.MeasurementName, map => map.MapFrom(vm => vm.Sensor.SensorType.MeasurementName))
	            .ForMember(hd => hd.MeasurementType, map => map.MapFrom(vm => vm.Sensor.SensorType.MeasurementName))
	            .ForMember(hd => hd.SensorId, map => map.MapFrom(vm => vm.Sensor.Id));
	            
            CreateMap<HistoryDto, HistoryViewModel>()
	            .ForMember(hd => hd.Value, map => map.MapFrom(vm => vm.GetStringValue()));

            CreateMap<NotificationDto, Message>().ReverseMap()
                .ForMember(nd => nd.UserName, map => map.MapFrom(ap => ap.AppUser.UserName))
                .ForMember(nd => nd.Date, map => map.MapFrom(h => h.History.Date));
            CreateMap<NotificationDto, NotificationViewModel>().ReverseMap();

            CreateMap<GraphDTO, GraphViewModel>();

            CreateMap<SensorTypeDto, SensorType>();
            CreateMap<SensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<CreateSensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<EditSensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<SensorType, SensorTypeDto>()
                .ForMember(dto => dto.IconPath, map => map.MapFrom(st => st.Icon.Path));
        }
    }
}
