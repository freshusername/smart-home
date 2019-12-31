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
                .ForMember(dto => dto.IconPath, map => map.MapFrom(s =>  s.Icon.Path + s.Icon.Name ))
                //.ForMember(dto => dto.IconPath, map => map.MapFrom(s => (s.Icon != null) ? s.Icon.Path + s.Icon.Name : ))
                .ReverseMap();
            CreateMap<SensorDto, SensorViewModel>();
            CreateMap<CreateSensorViewModel, SensorDto>();
            CreateMap<CreateSensorViewModel, Sensor>();

            CreateMap<Icon, IconDto>().ReverseMap();

            CreateMap<HistoryDto, History>().ReverseMap()
	            .ForMember(hd => hd.SensorName, map => map.MapFrom(h => h.Sensor.Name))
	            .ForMember(hd => hd.MeasurmentName, map => map.MapFrom(vm => vm.Sensor.SensorType.MeasurmentName))
	            .ForMember(hd => hd.MeasurmentType, map => map.MapFrom(vm => vm.Sensor.SensorType.MeasurmentType));
            CreateMap<HistoryDto, HistoryViewModel>()
	            .ForMember(hd => hd.Value, map => map.MapFrom(vm => vm.GetStringValue()));

            CreateMap<SensorTypeDto, SensorType>().ReverseMap();
            CreateMap<SensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<CreateSensorTypeViewModel, SensorTypeDto>().ForMember(s => s.Icon, map => map.Ignore());
        }
    }
}
