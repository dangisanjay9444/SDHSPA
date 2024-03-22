using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;
        public PhotoService(IConfiguration config)
        {           
      
            Account account = new Account(
                config.GetSection("CloudinarySettings:CloudName").Value,                
                config.GetSection("CloudinarySettings:APIKey").Value,
                config.GetSection("CloudinarySettings:APISecret").Value
            ); 

            cloudinary = new Cloudinary(account);

        }

        // method to delete photo from cloudinary (cloud)
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var deleteResult = await cloudinary.DestroyAsync(deleteParams);

            return deleteResult;
        }
        
        // method to upload photos on cloudinary (cloud service)
        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();
            if (photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadparams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                            .Height(500).Width(800)
                };

                uploadResult = await cloudinary.UploadAsync(uploadparams);
            }

            return uploadResult;
        }
    }
}