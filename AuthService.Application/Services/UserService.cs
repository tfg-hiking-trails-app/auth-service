using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Common.Application.DTOs.Filter;
using Common.Application.Pagination;
using Common.Domain.Exceptions;
using Common.Domain.Filter;

namespace AuthService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IStatusRepository statusRepository,
        IMapper mapper,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _statusRepository = statusRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<IEnumerable<UserEntityDto>> GetAllAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        
        return _mapper.Map<IEnumerable<UserEntityDto>>(users);
    }
    
    public async Task<Page<UserEntityDto>> GetPagedAsync(
        FilterEntityDto filter, 
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetPagedAsync(
            _mapper.Map<FilterData>(filter),
            cancellationToken
        );
        
        return _mapper.Map<Page<UserEntityDto>>(users);
    }

    public async Task<UserEntityDto> GetByIdAsync(int id)
    {
        User? user = await _userRepository.GetAsync(id);
        
        return user is null
            ? throw new NotFoundEntityException(nameof(User), id)
            : _mapper.Map<UserEntityDto>(user);
    }

    public async Task<UserEntityDto> GetByCodeAsync(Guid code)
    {
        User? user = await _userRepository.GetByCodeAsync(code);
        
        return user is null
            ? throw new NotFoundEntityException(nameof(User), code)
            : _mapper.Map<UserEntityDto>(user);
    }

    public async Task<Guid> CreateAsync(CreateUserEntityDto entity)
    {
        CheckDataValidity(entity);
        
        User user = _mapper.Map<User>(entity);

        Guid roleCode = Guid.Parse(entity.RoleCode!);
        Role? role = _roleRepository.GetByCode(roleCode);

        if (role is null)
            throw new NotFoundEntityException(nameof(Role), roleCode);
        
        Guid statusCode = Guid.Parse(entity.StatusCode!);
        Status? status = _statusRepository.GetByCode(statusCode);
        
        if (status is null)
            throw new NotFoundEntityException(nameof(Status), statusCode);
        
        user.Password = _passwordHasher.HashPassword(entity.Password!);
        user.RoleId = role.Id;
        user.StatusId = status.Id;
        
        await _userRepository.AddAsync(user);
        
        return user.Code;
    }

    public async Task<Guid> UpdateAsync(UpdateUserEntityDto entity)
    {
        User? user = _mapper.Map<User>(entity);
        
        await _userRepository.UpdateAsync(entity.Code, user);
        
        return user.Code;
    }

    public async Task DeleteAsync(Guid code)
    {
        await _userRepository.DeleteAsync(code);
    }

    private void CheckDataValidity(CreateUserEntityDto entity)
    {
        if (entity.RoleCode is not null && !Guid.TryParse(entity.RoleCode, out _))
            throw new ArgumentException("RoleCode is null or not valid Guid");
        
        if (entity.StatusCode is not null && !Guid.TryParse(entity.StatusCode, out _))
            throw new ArgumentException("StatusCode is null or not valid Guid");

        if (string.IsNullOrWhiteSpace(entity.Password) || string.IsNullOrWhiteSpace(entity.ConfirmPassword))
            throw new ArgumentException("Password is null or empty");
        
        if (!entity.Password.Equals(entity.ConfirmPassword))
            throw new ArgumentException("Password and confirm password do not match");
    }
    
}