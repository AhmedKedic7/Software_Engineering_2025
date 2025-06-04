using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByEmailAndPassword(string email, string password);
        Task UpdateUser(User user);
        User GetUserById(Guid userId);

        Task<User?> GetUserWithAddressesAsync(Guid userId);

        IEnumerable<User> GetAllUsers();

        Task AddUserAsync(User user);

        Task<User?> GetUserByEmailAsync(string email);
        Task<Address> AddAsync(Address address);
        Task DeleteAddressAsync(Guid userId, Guid addressId);
    }
}
