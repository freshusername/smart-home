using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IPhotoManager
    {
        Task<byte[]> GetPhotoFromFile(IFormFile uploadedFile, int width, int height);
    }
}
