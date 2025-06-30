namespace AuthService.API.Middlewares;

public interface IAuthServiceMiddleware
{
    Task InvokeAsync(HttpContext context);
}