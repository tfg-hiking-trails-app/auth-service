using AuthService.Application.Common.Pagination;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Common;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserEntityDto>> GetAllAsync();

    Task<Page<UserEntityDto>> GetPagedAsync(FilterEntityDto filter, CancellationToken cancellationToken);
    
    Task<UserEntityDto> GetByIdAsync(int id);
    
    Task<UserEntityDto> GetByCodeAsync(Guid code);
    
    Task<Guid> Create(CreateUserEntityDto entity);
    
    Task<Guid> Update(UpdateUserEntityDto entity);
    
    Task Delete(Guid code);
}