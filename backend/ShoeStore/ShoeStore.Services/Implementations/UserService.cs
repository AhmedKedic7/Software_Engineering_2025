using AutoMapper;
using ShoeStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStore.Repository.Interfaces;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
using ShoeStore.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ShoeStore.Repository.Implementions;

namespace ShoeStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public IEnumerable<UserContract> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return _mapper.Map<IEnumerable<UserContract>>(users);
        }
        public LoginResponse? Login(LoginRequest request)
        {
            
            var user = _userRepository.GetUserByEmailAndPassword(request.Email, request.Password);

            if (user == null)
                return null;
            user.IsLogedin = 1;
            _userRepository.UpdateUser(user);
            var response = _mapper.Map<LoginResponse>(user);
            response.Token = GenerateJwtToken(user);

            return response;
        }

        public User GetUserInfo(Guid userId)
        {
            {

                var user = _userRepository.GetUserById(userId);

                return user;
            }
        }
        public void Logout(Guid userId)
        {
            
            var user = _userRepository.GetUserById(userId);

            if (user != null)
            {
                
                user.IsLogedin = 0;
                _userRepository.UpdateUser(user); 
            }
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("KediKey123456789012345678901234567890");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.IsAdmin == 1 ? "Admin" : "User")
                   }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<UserProfileContract?> GetUserDetailsAsync(Guid userId)
        {
            var user = await _userRepository.GetUserWithAddressesAsync(userId);
            return user == null ? null : _mapper.Map<UserProfileContract>(user);
        }

        public async Task RegisterUserAsync(RegisterContract userDto)
        {
            
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists");
            var user = _mapper.Map<User>(userDto);

            user.UserId = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.IsAdmin = 0;
            user.IsLogedin = 0;

            await _userRepository.AddUserAsync(user);
        }

        public async Task LogoutUserAsync(Guid userId)
        {
            var user =  _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsLogedin = 0;  
            await _userRepository.UpdateUser(user);  
        }

        public async Task<AddressContract> AddAddressAsync(AddressContract addressDto)
        {
             
            var address = _mapper.Map<Address>(addressDto);

            address.AddressId = Guid.NewGuid();
            var addedAddress = await _userRepository.AddAsync(address);

             
            return _mapper.Map<AddressContract>(addedAddress);
        }

        public async Task DeleteAddressAsync(Guid userId, Guid addressId)
        {

            try
            {
                await _userRepository.DeleteAddressAsync(userId, addressId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting address: {ex.Message}", ex);
            }
        }

    }
}
