﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IPhotoManager
    {
        Task<byte[]> GetPhotoFromFile(IFormFile uploadedFile, int width, int height);
    }
}
