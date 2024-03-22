using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Interfaces
{
    public interface IPhotoService
    {
        // mehtod for uploading file
        Task<ImageUploadResult> UploadPhotoAsync (IFormFile photo);

        // method for deletion of file from cloudinary (cloud)
        Task<DeletionResult> DeletePhotoAsync (string publicId);

        
    }
}