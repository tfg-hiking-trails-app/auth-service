using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    IEnumerable<UserEntityDto> GetAll();
    
    Task<IEnumerable<UserEntityDto>> GetAllAsync();
    
    UserEntityDto? GetById(int id);
    
    Task<UserEntityDto?> GetByIdAsync(int id);
    
    UserEntityDto? GetByCode(Guid code);
    
    Task<UserEntityDto?> GetByCodeAsync(Guid code);
    
    void Create(UserEntityDto entity);
    
    void Update(Guid code, UserEntityDto entity);
    
    void Delete(Guid code);
}