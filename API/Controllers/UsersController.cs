
using System.Security.Claims;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[Authorize]
public class UsersController(IUserRepository userRepo, IMapper mapper) : BaseApiController
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
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(username ==null) 
        {
            return BadRequest("No username provided");
        }
        var user = await userRepo.GetUserByUsername(username);
        
        if(user == null) 
        {
            return BadRequest("No user with username found");
        }

        mapper.Map(memberUpdateDto, user);

        if(await userRepo.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }
}
