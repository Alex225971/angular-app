
using System.Security.Claims;
using API.Controllers;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API;

[Authorize]
public class UsersController(IUserRepository userRepo, IMapper mapper, IPhotoService photoService) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepo.GetMembersAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepo.GetMemberAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) 
    {

        var user = await userRepo.GetUserByUsername(User.GetUsername());
        
        if(user == null) 
        {
            return BadRequest("No user with username found");
        }

        mapper.Map(memberUpdateDto, user);

        if(await userRepo.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) 
    {
        var user = await userRepo.GetUserByUsername(User.GetUsername());

        if(user == null) return BadRequest("Cannot update user, no username found");

        var result = await photoService.AddPhotoAsync(file);

        if(result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);

        if(await userRepo.SaveAllAsync()) return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId) {
        var user = await userRepo.GetUserByUsername(User.GetUsername());
        if(user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if(photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if(currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if(await userRepo.SaveAllAsync()) return NoContent();

        return BadRequest("Error trying to set main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId) {
        var user = await userRepo.GetUserByUsername(User.GetUsername());
        if(user == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if(photo == null || photo.IsMain) return BadRequest("Cannot delete main photo");
        if(photo.PublicId != null) {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error!=null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if(await userRepo.SaveAllAsync()) return Ok();

        return BadRequest("Error deleting photo");
    }
}
