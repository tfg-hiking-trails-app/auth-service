using AuthService.API.DTOs;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AutoMapper;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic.CompilerServices;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private readonly string REFRESH_TOKEN = "refresh_token";
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public AuthenticationController(
        IAuthenticationService authenticationService, 
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
        _env = env;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] AuthenticationDto authenticationDto)
    {
        try
        {
            TokenResponseEntityDto tokenResponseEntityDto = await _authenticationService.Login(
                _mapper.Map<AuthenticationEntityDto>(authenticationDto));
            
            Response.Cookies.Append("refresh_token", tokenResponseEntityDto.RefreshToken!, GetCookieOptions());
            
            return Ok(_mapper.Map<TokenResponseDto>(tokenResponseEntityDto));
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponseDto>> Refresh()
    {
        try
        {
            string? accessToken = GetAccessTokenFromHeaders();
            
            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is required.");
            
            string? oldRefreshToken = Request.Cookies[REFRESH_TOKEN];
            
            if (string.IsNullOrEmpty(oldRefreshToken))
                throw new UnauthorizedAccessException("Refresh token is null or empty");
            
            TokenResponseEntityDto tokenResponseEntityDto = 
                await _authenticationService.Refresh(accessToken, oldRefreshToken);

            Response.Cookies.Append(REFRESH_TOKEN, tokenResponseEntityDto.RefreshToken!, GetCookieOptions());
            
            return Ok(_mapper.Map<TokenResponseDto>(tokenResponseEntityDto));
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Logout()
    {
        try
        {
            string? refreshToken = Request.Cookies[REFRESH_TOKEN];
        
            if (!string.IsNullOrEmpty(refreshToken))
                _authenticationService.InvalidateRefreshToken(refreshToken);

            Response.Cookies.Append(REFRESH_TOKEN, "", GetCookieOptions());

            return Ok();
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    private string? GetAccessTokenFromHeaders()
    {
        StringValues authorizations = Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authorizations))
            return null;

        string? value = authorizations.FirstOrDefault();
        
        if (string.IsNullOrEmpty(value))
            return null;
        
        string prefix = "Bearer ";

        return value.StartsWith(prefix)
            ? value.Substring(prefix.Length)
            : null;
    }

    private CookieOptions GetCookieOptions()
    {
        string? refreshTokenExpiration = Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRE");
        
        if (string.IsNullOrWhiteSpace(refreshTokenExpiration))
            throw new UnauthorizedAccessException("Refresh token expiration is empty");
        
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = _env.IsProduction(),
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth/refresh",
            Expires = DateTimeOffset.Now.AddMinutes(IntegerType.FromString(refreshTokenExpiration))
        };
    }
}