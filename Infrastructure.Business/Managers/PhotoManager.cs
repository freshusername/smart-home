using AutoMapper;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
	public class PhotoManager : BaseManager, IPhotoManager
    {
        public PhotoManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<byte[]> GetPhotoFromFile(IFormFile uploadedFile, int width, int height)
        {
            if (!IsValidImage(uploadedFile))
            {
                throw new ArgumentException();
            }

            Image img = await ResizeImage(uploadedFile, width, height);

            byte[] imgData = ImageToByteArray(img);

            return imgData;
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static bool IsValidImage(IFormFile file) => (file != null);

        public async Task<Image> ResizeImage(IFormFile file, int width, int height)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                using (var img = Image.FromStream(memoryStream))
                {
                    return Resize(img, width, height);
                }
            }
        }

        public Image Resize(Image image, int width, int height)
        {
            var res = new Bitmap(width, height);
            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, width, height);
            }
            return res;
        }
    }
}
