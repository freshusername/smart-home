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
        private readonly string _path = @"\images\SensorIcons\";
        private readonly IHostingEnvironment _env;

        public string UploadPath 
        { 
            get { return _env.WebRootPath + _path; } 
        }
        
        public IconManager(IUnitOfWork unitOfWork, IMapper mapper, IHostingEnvironment env) : base(unitOfWork, mapper)
        {
            _env = env;

            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }
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
                return new OperationDetails(false, "Not implemented", "Error");
            }
            return new OperationDetails(true, "New icon has been added", "Name");
        }

        public async Task<int> CreateAndGetIconId(IFormFile iformFile)
        {  
            await UploadImage(iformFile);

            var iconDto = new IconDto()
            {
                Name = iformFile.FileName,
                Path = _path + iformFile.FileName
            };

            Icon icon = mapper.Map<IconDto, Icon>(iconDto);

            unitOfWork.IconRepo.Insert(icon);
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

            }
            return new OperationDetails(true, "Not implemented", "Name");
        }

        //TODO: private metod upload
        private async Task UploadImage(IFormFile formFile)
        {
            using (var fileStream = new FileStream(Path.Combine(UploadPath, formFile.FileName), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }
    }
}
    