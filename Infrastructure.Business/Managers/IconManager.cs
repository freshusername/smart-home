using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Icon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Managers
{
    public class IconManager : BaseManager, IIconManager
    {
        public IconManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public IconDto GetById(int id)
        {
            Icon icon = unitOfWork.IconRepo.GetById(id);
            IconDto iconDto = mapper.Map<Icon, IconDto>(icon);
            return iconDto;
        }

        public void Insert(IconDto iconDto)
        {
            Icon iconCheck = unitOfWork.IconRepo.GetById(iconDto.Id);

            if (iconCheck == null)
            {
                Icon icon = mapper.Map<IconDto, Icon>(iconDto);

                unitOfWork.IconRepo.Insert(icon);
                unitOfWork.Save();

            }

        }

        public int InsertGetIconId(IconDto iconDto)
        {
            Icon iconCheck = unitOfWork.IconRepo.GetById(iconDto.Id);
            int iconId = 0;

            if (iconCheck == null)
            {
                Icon icon = mapper.Map<IconDto, Icon>(iconDto);

                unitOfWork.IconRepo.Insert(icon);
                unitOfWork.Save();
                iconId = icon.Id;

            }
            return iconId;
        }

        public void Update(IconDto iconDto)
        {

        }
    }
}
