using AuthService.API.DTOs;
using AuthService.API.DTOs.Update;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AutoMapper;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private const string RefreshToken = "refresh_token";
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenManager _tokenManager;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public AuthenticationController(
        IAuthenticationService authenticationService,
        ITokenManager tokenManager,
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _authenticationService = authenticationService;
        _tokenManager = tokenManager;
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
            
            Response.Cookies.Append(RefreshToken, tokenResponseEntityDto.RefreshToken!, GetCookieOptions());
            
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
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            // The account is created but no session is started: the user must log in afterwards.
            await _authenticationService.Register(_mapper.Map<RegisterEntityDto>(registerDto));

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
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
            
            string? oldRefreshToken = Request.Cookies[RefreshToken];
            
            if (string.IsNullOrEmpty(oldRefreshToken))
                throw new UnauthorizedAccessException("Refresh token is null or empty");
            
            TokenResponseEntityDto tokenResponseEntityDto = 
                await _authenticationService.Refresh(accessToken, oldRefreshToken);

            Response.Cookies.Append(RefreshToken, tokenResponseEntityDto.RefreshToken!, GetCookieOptions());
            
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
    public async Task<IActionResult> Logout()
    {
        try
        {
            string? refreshToken = Request.Cookies[RefreshToken];
        
            if (!string.IsNullOrEmpty(refreshToken))
                await _authenticationService.InvalidateRefreshToken(refreshToken);

            Response.Cookies.Append(RefreshToken, "", GetCookieOptions());

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
    
    [HttpPut("edit/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> EditPassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        try
        {
            if (!updatePasswordDto.NewPassword.Equals(updatePasswordDto.ConfirmNewPassword))
                throw new ArgumentException("The passwords do not match");
            
            string accessToken = GetAccessTokenFromHeaders();
            
            string? userCode = _tokenManager.GetUserCodeFromJwt(accessToken);
            
            if (string.IsNullOrEmpty(userCode) || !_authenticationService.AccessTokenBelongsToUser(accessToken, new Guid(userCode)))
                throw new UnauthorizedAccessException("Access Denied");
            
            await _authenticationService.EditPassword(new Guid(userCode), 
                _mapper.Map<UpdatePasswordEntityDto>(updatePasswordDto));
            
            return NoContent();
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

    [HttpPut("edit/username")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> EditUsername([FromBody] UpdateUsernameDto updateUsernameDto)
    {
        try
        {
            string accessToken = GetAccessTokenFromHeaders();
            
            string? userCode = _tokenManager.GetUserCodeFromJwt(accessToken);
            
            if (string.IsNullOrEmpty(userCode) || !_authenticationService.AccessTokenBelongsToUser(accessToken, new Guid(userCode)))
                throw new UnauthorizedAccessException("Access Denied");
            
            await _authenticationService.EditUsername(new Guid(userCode), updateUsernameDto.Username.Trim());
            
            return NoContent();
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
    
    private string GetAccessTokenFromHeaders()
    {
        StringValues authorizations = Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authorizations))
            throw new UnauthorizedAccessException("Access token is required");

        string? value = authorizations.FirstOrDefault();

        if (string.IsNullOrEmpty(value))
            throw new UnauthorizedAccessException("Access token is required");
        
        string prefix = "Bearer ";

        return value.StartsWith(prefix)
            ? value.Substring(prefix.Length)
            : throw new UnauthorizedAccessException("Access token is required");
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
            Path = "/api/auth", // The HTTP cookie is only received by the auth endpoints
            Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(refreshTokenExpiration))
        };
    }
}