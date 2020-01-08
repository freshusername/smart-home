using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class IconManager : BaseManager, IIconManager
    {
        private readonly string _path = @"\images\Icons\";
        private readonly IHostingEnvironment _env;

        public string UploadPath
        {
            get { return _env.WebRootPath + _path; }
        }

        public IconManager(IUnitOfWork unitOfWork, IMapper mapper, IHostingEnvironment env) : base(unitOfWork, mapper)
        {
            _env = env;

            try {
                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }   
            } catch (Exception ex) {
                // TODO: add logs
            }
        }

        public async Task<IconDto> GetById(int id)
        {
            Icon icon = await unitOfWork.IconRepo.GetById(id);
            IconDto iconDto = mapper.Map<Icon, IconDto>(icon);
            return iconDto;
        }

        public async Task<OperationDetails> Create(IconDto iconDto)
        {
            try
            {
                Icon icon = mapper.Map<IconDto, Icon>(iconDto);
                await unitOfWork.IconRepo.Insert(icon);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "New icon has been added", "Name");
        }

        public async Task<int> CreateAndGetIconId(IFormFile iformFile)
        {
            var newFileName = Guid.NewGuid() + Path.GetExtension(iformFile.FileName);
            await UploadImage(iformFile, newFileName);

            var iconDto = new IconDto()
            {
                Path = _path + newFileName
            };

            Icon icon = mapper.Map<IconDto, Icon>(iconDto);

            await unitOfWork.IconRepo.Insert(icon);
            unitOfWork.Save();

            return icon.Id;
        }

        public async Task<OperationDetails> Update(IconDto iconDto)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new OperationDetails(true, ex.Message, "Name");
            }
            return new OperationDetails(true, "Not implemented", "Name");
        }

        private async Task UploadImage(IFormFile formFile, string newFileName)
        {
            using (var fileStream = new FileStream(Path.Combine(UploadPath, newFileName), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }
    }
}
