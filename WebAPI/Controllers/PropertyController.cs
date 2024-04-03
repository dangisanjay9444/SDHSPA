using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Controllers
{    
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public PropertyController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            this.mapper = mapper;
            this.photoService = photoService;
            this.uow = uow;
        }

        //api/Property/list/1        
        [HttpGet("list/{sellRent}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyList(int sellRent)
        {
            var properties = await uow.propertyRepository.GetPropertiesAsync(sellRent);
            var propertiesListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties);
            return Ok(propertiesListDto);
        }

        //api/Property/detail/1        
        [HttpGet("detail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            var property = await uow.propertyRepository.GetPropertyDetailAsync(id);
            var propertyDto = mapper.Map<PropertyDetailDto>(property);
            return Ok(propertyDto);
        }

        //api/Property/add       
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            var property = mapper.Map<Property>(propertyDto);   
            var UserId = GetUserId();
            property.PostedBy = UserId;  
            //property.User.UserModified = UserId.ToString();               
            uow.propertyRepository.AddProperty(property);
            await uow.SaveAsync();
            return StatusCode(201);
        }

        //api/Property/add/photo/1      
        [HttpPost("add/photo/{propId}")]
        [Authorize]
        public async Task<ActionResult<PhotoDto>> AddPropertyPhoto(IFormFile file, int propId)
        {
            var result = await photoService.UploadPhotoAsync(file);
            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var property = await uow.propertyRepository.GetPropertyByIdAsync(propId);

            var photo = new Photo
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(property.Photos.Count == 0)
            {
                photo.IsPrimary = true;
            }

            property.Photos.Add(photo);
            if(await uow.SaveAsync()) return mapper.Map<PhotoDto>(photo);

            return BadRequest("Some problem occured while uploading the Photo, please retry");
            
        }

        //api/Property/set-primary-photo/1/publicid      
        [HttpPost("set-primary-photo/{propId}/{publicId}")]
        [Authorize]
        public async Task<IActionResult> SetPrimaryPhoto(int propId, string publicId)
        {
            var userId =  GetUserId();

            var property = await uow.propertyRepository.GetPropertyByIdAsync(propId);

            if(property == null)
            {
                return BadRequest("No such property or photo exists !");
            }

            if(property.PostedBy != userId)
            {
                return BadRequest("You are not authorized to change the photo");
            }

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == publicId);

            if(photo == null)
            {
                return BadRequest("No such property or photo exists !");
            }

            if(photo.IsPrimary)
            {
                return BadRequest("The photo is already a primary photo !");
            }    

            var currentPrimary = property.Photos.FirstOrDefault(p => p.IsPrimary);
            if(currentPrimary != null)
            {
                currentPrimary.IsPrimary = false;
                photo.IsPrimary = true;
            }

            if(await uow.SaveAsync()) 
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Some error has occured, failed to set primary photo");
            }
        }

        //api/Property/delete-photo/1/publicid      
        [HttpDelete("delete-photo/{propId}/{publicId}")]
        [Authorize]
        public async Task<IActionResult> DeletePhoto(int propId, string publicId)
        {
            var userId =  GetUserId();

            var property = await uow.propertyRepository.GetPropertyByIdAsync(propId);

            if(property == null)
            {
                return BadRequest("No such property or photo exists !");
            }

            if(property.PostedBy != userId)
            {
                return BadRequest("You are not authorized to delete the photo");
            }

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == publicId);

            if(photo == null)
            {
                return BadRequest("No such property or photo exists !");
            }

            if(photo.IsPrimary)
            {
                return BadRequest("You can't delete the primary photo !");
            }    

            // first delete the photo from cloud
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            // after deletion from cloud then delete the photo from database.
            property.Photos.Remove(photo);

            if(await uow.SaveAsync()) 
            {
                return Ok();
            }
            else
            {
                return BadRequest("Some error has occured, failed to delete the photo");
            }
        }
    }
}