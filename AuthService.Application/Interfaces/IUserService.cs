using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using Common.Application.DTOs.Filter;
using Common.Application.Pagination;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserEntityDto>> GetAllAsync();

    Task<Page<UserEntityDto>> GetPagedAsync(FilterEntityDto filter, CancellationToken cancellationToken);
    
    Task<UserEntityDto> GetByIdAsync(int id);
    
    Task<UserEntityDto> GetByCodeAsync(Guid code);
    
    Task<Guid> CreateAsync(CreateUserEntityDto entity);
    
    Task<Guid> UpdateAsync(UpdateUserEntityDto entity);
    
    Task DeleteAsync(Guid code);
}