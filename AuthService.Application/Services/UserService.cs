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

        Guid roleCode = Guid.Parse(entity.RoleCode!);
        Role? role = await _roleRepository.GetByCodeAsync(roleCode);

        if (role is null)
            throw new NotFoundEntityException(nameof(Role), roleCode);
        
        Guid statusCode = Guid.Parse(entity.StatusCode!);
        Status? status = await _statusRepository.GetByCodeAsync(statusCode);
        
        if (status is null)
            throw new NotFoundEntityException(nameof(Status), statusCode);
        
        user.Code = Guid.NewGuid();
        user.Password = _passwordHasher.HashPassword(entity.Password!);
        user.RoleId = role.Id;
        user.StatusId = status.Id;
        
        await _userRepository.AddAsync(user);
        
        return user.Code;
    }

    protected override void CheckDataValidity(CreateUserEntityDto entity)
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