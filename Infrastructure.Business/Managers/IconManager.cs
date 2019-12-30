using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<OperationDetails> Create(IconDto iconDto)
        {
            try
            {
                Icon icon = mapper.Map<IconDto, Icon>(iconDto);
                unitOfWork.IconRepo.Insert(icon);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "New icon has been added", "Name");
        }

        public async Task<int> CreateAndGetIconId(IconDto iconDto)
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

        public async Task<OperationDetails> Update(IconDto iconDto)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return new OperationDetails(true, "Not implemented", "Name");
        }
    }
}
