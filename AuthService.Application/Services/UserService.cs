using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AutoMapper;

namespace AuthService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IStatusRepository statusRepository,
        IMapper mapper
    )
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _statusRepository = statusRepository;
        _mapper = mapper;
    }
    
    public IEnumerable<UserEntityDto> GetAll()
    {
        IEnumerable<User> users = _userRepository.GetAll();
        
        return _mapper.Map<IEnumerable<UserEntityDto>>(users);
    }

    public async Task<IEnumerable<UserEntityDto>> GetAllAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        
        return _mapper.Map<IEnumerable<UserEntityDto>>(users);
    }

    public UserEntityDto GetById(int id)
    {
        User? user = _userRepository.Get(id);
        
        return user == null 
            ? throw new NotFoundEntityException(nameof(User), id) 
            : _mapper.Map<UserEntityDto>(user);
    }

    public async Task<UserEntityDto> GetByIdAsync(int id)
    {
        User? user = await _userRepository.GetAsync(id);
        
        return user == null
            ? throw new NotFoundEntityException(nameof(User), id)
            : _mapper.Map<UserEntityDto>(user);
    }

    public UserEntityDto GetByCode(Guid code)
    {
        User? user = _userRepository.GetByCode(code);
        
        return user == null
            ? throw new NotFoundEntityException(nameof(User), code)
            : _mapper.Map<UserEntityDto>(user);
    }

    public async Task<UserEntityDto> GetByCodeAsync(Guid code)
    {
        User? user = await _userRepository.GetByCodeAsync(code);
        
        return user == null
            ? throw new NotFoundEntityException(nameof(User), code)
            : _mapper.Map<UserEntityDto>(user);
    }

    public Guid Create(CreateUserEntityDto entity)
    {
        CheckDataValidity(entity);
        
        User user = _mapper.Map<User>(entity);

        Guid roleCode = Guid.Parse(entity.RoleCode!);
        Role? role = _roleRepository.GetByCode(roleCode);

        if (role == null)
            throw new NotFoundEntityException(nameof(Role), roleCode);
        
        Guid statusCode = Guid.Parse(entity.StatusCode!);
        Status? status = _statusRepository.GetByCode(statusCode);
        
        if (status == null)
            throw new NotFoundEntityException(nameof(Status), statusCode);
        
        user.Code = Guid.NewGuid();
        // hashear password
        user.RoleId = role.Id;
        user.StatusId = status.Id;
        
        _userRepository.Add(user);
        
        return user.Code;
    }

    public void Update(Guid code, UpdateUserEntityDto entity)
    {
        User? user = _mapper.Map<User>(entity);
        
        _userRepository.Update(code, user);
    }

    public void Delete(Guid code)
    {
        _userRepository.Delete(code);
    }

    private void CheckDataValidity(CreateUserEntityDto entity)
    {
        if (entity.RoleCode == null || !Guid.TryParse(entity.RoleCode, out _))
            throw new ArgumentException("RoleCode is null or not valid Guid");
        
        if (entity.StatusCode == null || !Guid.TryParse(entity.StatusCode, out _))
            throw new ArgumentException("StatusCode is null or not valid Guid");
    }
    
}