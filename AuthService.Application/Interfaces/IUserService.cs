using AuthService.Application.Common.Pagination;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Common;
using AuthService.Application.DTOs.Update;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserEntityDto>> GetAllAsync();

    Task<Page<UserEntityDto>> GetPagedAsync(FilterEntityDto filter, CancellationToken cancellationToken);
    
    Task<UserEntityDto> GetByIdAsync(int id);
    
    Task<UserEntityDto> GetByCodeAsync(Guid code);
    
    Guid Create(CreateUserEntityDto entity);
    
    void Update(Guid code, UpdateUserEntityDto entity);
    
    void Delete(Guid code);
}