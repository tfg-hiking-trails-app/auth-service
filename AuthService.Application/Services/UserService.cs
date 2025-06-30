using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Interfaces;
using AutoMapper;

namespace AuthService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public IEnumerable<UserEntityDto> GetAll() => 
        _mapper.Map<IEnumerable<UserEntityDto>>(_userRepository.GetAll());

    public Task<IEnumerable<UserEntityDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public UserEntityDto? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserEntityDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public UserEntityDto? GetByCode(Guid code)
    {
        throw new NotImplementedException();
    }

    public Task<UserEntityDto?> GetByCodeAsync(Guid code)
    {
        throw new NotImplementedException();
    }

    public void Create(UserEntityDto entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Guid code, UserEntityDto entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid code)
    {
        throw new NotImplementedException();
    }
}