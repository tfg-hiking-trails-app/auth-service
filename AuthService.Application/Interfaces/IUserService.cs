using AuthService.Application.Common.Pagination;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Common;
using AuthService.Application.DTOs.Update;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    IEnumerable<UserEntityDto> GetAll();

    Task<Page<UserEntityDto>> GetPaged(FilterEntityDto filter, CancellationToken cancellationToken);
    
    Task<IEnumerable<UserEntityDto>> GetAllAsync();
    
    UserEntityDto GetById(int id);
    
    Task<UserEntityDto> GetByIdAsync(int id);
    
    UserEntityDto GetByCode(Guid code);
    
    Task<UserEntityDto> GetByCodeAsync(Guid code);
    
    Guid Create(CreateUserEntityDto entity);
    
    void Update(Guid code, UpdateUserEntityDto entity);
    
    void Delete(Guid code);
}