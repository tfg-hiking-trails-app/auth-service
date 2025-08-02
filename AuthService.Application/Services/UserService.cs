using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Common.Application.Services;
using Common.Domain.Exceptions;

namespace AuthService.Application.Services;

public class UserService 
    : AbstractService<User, UserEntityDto, CreateUserEntityDto, UpdateUserEntityDto>, IUserService
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
        IPasswordHasher passwordHasher) : base(userRepository, mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _statusRepository = statusRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public override async Task<Guid> CreateAsync(CreateUserEntityDto entity)
    {
        CheckDataValidity(entity);
        
        User user = _mapper.Map<User>(entity);
        
        Role? role = await _roleRepository.GetByCodeAsync(entity.RoleCode);

        if (role is null)
            throw new NotFoundEntityException(nameof(Role), entity.RoleCode);
        
        Status? status = await _statusRepository.GetByCodeAsync(entity.StatusCode);
        
        if (status is null)
            throw new NotFoundEntityException(nameof(Status), entity.StatusCode);
        
        user.Code = Guid.NewGuid();
        user.Password = _passwordHasher.HashPassword(entity.Password!);
        user.RoleId = role.Id;
        user.StatusId = status.Id;
        
        await _userRepository.AddAsync(user);
        
        return user.Code;
    }

    protected override void CheckDataValidity(CreateUserEntityDto entity)
    {
        if (entity.RoleCode == Guid.Empty)
            throw new ArgumentException("RoleCode is not valid Guid");
        
        if (entity.StatusCode == Guid.Empty)
            throw new ArgumentException("StatusCode is not valid Guid");

        if (string.IsNullOrWhiteSpace(entity.Password) || string.IsNullOrWhiteSpace(entity.ConfirmPassword))
            throw new ArgumentException("Password is null or empty");
        
        if (!entity.Password.Equals(entity.ConfirmPassword))
            throw new ArgumentException("Password and confirm password do not match");
    }
    
}