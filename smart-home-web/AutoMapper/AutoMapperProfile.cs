using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.History;
using smart_home_web.Models;
using smart_home_web.Models.SensorViewModel;
using smart_home_web.Models.History;
using Infrastructure.Business.DTOs.SensorType;
using smart_home_web.Models.SensorType;
using Infrastructure.Business.DTOs.Icon;
using System.IO;
using Infrastructure.Business.DTOs.Notification;
using smart_home_web.Models.Notification;
using Infrastructure.Business.DTOs.ReportElements;
using smart_home_web.Models.ReportElements;
using Infrastructure.Business.DTOs.Dashboard;
using smart_home_web.Models.Dashboard;
using Domain.Core.JoinModel;
using Infrastructure.Business.DTOs.DashboardOptions;
using smart_home_web.Models.DashboardOptions;
using Infrastructure.Business.DTOs.Options;
using smart_home_web.Models.Options;
using System;

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

            CreateMap<Sensor, GraphDto>()
               .ForMember(gd => gd.SensorId, map => map.MapFrom(s => s.Id))
               .ForMember(gd => gd.SensorName, map => map.MapFrom(s => s.Name))
               .ForMember(gd => gd.SensorType, map => map.MapFrom(s => s.SensorType.Name))
               .ForMember(gd => gd.MeasurementName, map => map.MapFrom(s => s.SensorType.MeasurementName))
               .ForMember(gd => gd.MeasurementType, map => map.MapFrom(s => s.SensorType.MeasurementType));
            CreateMap<GraphDto, GraphViewModel>();

            CreateMap<Sensor, ReportElementDto>()
               .ForMember(gd => gd.SensorId, map => map.MapFrom(s => s.Id))
               .ForMember(gd => gd.SensorName, map => map.MapFrom(s => s.Name))
               .ForMember(gd => gd.MeasurementName, map => map.MapFrom(s => s.SensorType.MeasurementName))
               .ForMember(gd => gd.MeasurementType, map => map.MapFrom(s => s.SensorType.MeasurementType));
            CreateMap<ReportElementDto, ReportElementViewModel>();

            CreateMap<ReportElement, EditReportElementViewModel>()
                .ForMember(ewc => ewc.DashboardName, map => map.MapFrom(re => re.Dashboard.Name))
                .ForMember(ewc => ewc.SensorName, map => map.MapFrom(re => re.Sensor.Name));

            CreateMap<EditReportElementViewModel, ReportElementDto>();
            CreateMap<ReportElementDto, ReportElement>();


            CreateMap<SensorTypeDto, SensorType>();
            CreateMap<SensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<CreateSensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<EditSensorTypeViewModel, SensorTypeDto>().ReverseMap();
            CreateMap<SensorType, SensorTypeDto>()
                .ForMember(dto => dto.IconPath, map => map.MapFrom(st => st.Icon.Path));

			
            CreateMap<ReportElement, GaugeDto>().ReverseMap();
            CreateMap<GaugeDto, GaugeViewModel>().ReverseMap();
            CreateMap<GaugeDto, GaugeUpdateViewModel>()
                .ForMember(gu => gu.Min, map => map.MapFrom(gd => (int)Math.Floor(gd.Min.Value)))
                .ForMember(gu => gu.Max, map => map.MapFrom(gd => (int)Math.Ceiling(gd.Max.Value)));

            CreateMap<ReportElement, ClockDto>().ReverseMap();
        
			CreateMap<Dashboard, DashboardDto>().ReverseMap();
			CreateMap<DashboardDto, DashboardViewModel>();

			CreateMap<DashboardOptions, DashboardOptionsDto>().ReverseMap();
			CreateMap<DashboardOptionsDto, DashboardOptionsViewModel>();

			CreateMap<Options, OptionsDto>().ReverseMap();
			CreateMap<OptionsDto, OptionsViewModel>();
		}
    }
}
