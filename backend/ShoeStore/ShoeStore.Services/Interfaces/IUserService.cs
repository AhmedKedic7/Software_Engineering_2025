using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Services.Interfaces
{
    public interface IUserService
    {
        LoginResponse? Login(LoginRequest request);
        User GetUserInfo(Guid userId);
        Task<UserProfileContract?> GetUserDetailsAsync(Guid userId);
        IEnumerable<UserContract> GetAllUsers();
        Task RegisterUserAsync(RegisterContract userDto);

        Task LogoutUserAsync(Guid userId);

         Task<AddressContract> AddAddressAsync(AddressContract addressDto);
        Task DeleteAddressAsync(Guid userId, Guid addressId);
    }
}
