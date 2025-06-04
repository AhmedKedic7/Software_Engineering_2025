using Microsoft.EntityFrameworkCore;
using ShoeStore.Repository.Interfaces;
using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Implementions
{
    public class UserRepository:IUserRepository
        
    {
        private readonly ShoeStoreDbContext _context;

        public UserRepository(ShoeStoreDbContext context)
        {
            _context = context;
        }
        public User? GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChangesAsync();
        }
        public User GetUserById(Guid userId)
        {
            
            return _context.Users
                           .SingleOrDefault(u => u.UserId == userId); 
        }

        public async Task<User?> GetUserWithAddressesAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Addresses.Where(a => a.DeletedAt == null)) 
                .Include(u=>u.Carts)
                .Include(u=>u.Orders)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Address> AddAsync(Address address)
        {
            await _context.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task DeleteAddressAsync(Guid userId, Guid addressId)
        {
            var user = await _context.Users
            .Include(u => u.Addresses)  
            .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var address = user.Addresses.FirstOrDefault(a => a.AddressId == addressId);

            if (address != null)
            {
                
                address.DeletedAt = DateTime.UtcNow;

                 
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Address not found.");
            }
        }
    }
}
