using API.Controllers;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API;

public class ErrorController(DataContext dataContext) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret";
    }

    [HttpGet("not-found")]
    public ActionResult<User> GetNotFound()
    {
        var user = dataContext.Users.Find(-1);
        if(user == null) return NotFound();
        return user;
    }

    [HttpGet("server-error")]
    public ActionResult<User> GetServerError()
    {
        var user = dataContext.Users.Find(-1) ?? throw new Exception("null reference");
        return user;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("Bad request");
    }
}
