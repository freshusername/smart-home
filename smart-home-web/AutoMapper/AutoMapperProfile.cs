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
using Infrastructure.Business.DTOs.Icon;
using smart_home_web.Models.IconViewModel;
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

            CreateMap<Sensor, SensorDto>().ReverseMap();
            CreateMap<SensorDto, SensorViewModel>()
                .ForMember(svm => svm.IconPath, map => map.MapFrom( s => s.Icon.Path));
            CreateMap<CreateSensorViewModel, SensorDto>();
            CreateMap<CreateSensorViewModel, Sensor>();

            CreateMap<Icon, IconDto>().ReverseMap();
            CreateMap<IconDto, GetIconViewModel>().ReverseMap();
            CreateMap<CreateIconViewModel, IconDto>()
                .ForMember(i => i.Name, map => map.MapFrom(civm => civm.ImageFile.FileName)); //TOD0: Cut icon extension (.png)


            CreateMap<HistoryDto, History>().ReverseMap().ForMember(hd => hd.SensorId, map => map.MapFrom(h => h.Sensor.Id));
            CreateMap<HistoryDto, HistoryViewModel>().ReverseMap();
        }

        private string GetIconPath(IconDto icon)
        {
            string sensorIconPath = "wwwroot/images/SensorIcons/" + Convert.ToString(icon.Name) + ".png";
            string sensorTypeIconPath = "wwwroot/images/SensorTypeIcons/" + Convert.ToString(icon.Name) + ".png";

            if (File.Exists(sensorIconPath))
            {
                //  / images / SensorIcons / " + Convert.ToString(icon.Name) + ".png";
                return "wwwroot/images/SensorIcons/" + Convert.ToString(icon.Name) + ".png";
            }

            if (File.Exists(sensorTypeIconPath))
            {
                //  / images / SensorTypeIcons / " + Convert.ToString(icon.Name) + ".png";
                return "wwwroot/images/SensorTypeIcons/" + Convert.ToString(icon.Name) + ".png";
            }

            return null;
        }
    }
}
