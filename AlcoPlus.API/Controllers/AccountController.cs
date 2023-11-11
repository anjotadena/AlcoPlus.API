﻿using AlcoPlus.API.Contracts;
using AlcoPlus.API.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace AlcoPlus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthManager _authManager;

    public AccountController(IAuthManager authManager)
    {
        _authManager = authManager;
    }

    // api/account/register
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
    {
        var errors = await _authManager.Register(apiUserDto);

        if (errors.Any())
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return Ok();
    }

    // api/account/login
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        var isValidUser = await _authManager.Login(loginUserDto);

        if (isValidUser)
        {
            return Ok();
        }

        return Unauthorized();
    }

    //// api/account/logout
    //[HttpPost]
    //[Route("login")]
    //public Task<IActionResult> Logout()
    //{

    //}
}
