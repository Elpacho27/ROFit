using ROFit.Model;
using ROFit.Models;

namespace ROFit.Repository.Common
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid id);
        Task<User> CreateAsync(UserRegister userRegister, Guid? createdBy = null);
        Task<User> UpdateAsync(User user, Guid updatedBy);
        Task<bool> DeleteAsync(Guid id, Guid updatedBy);
        Task<bool> UserExistsAsync(string email);
        Task<List<User>> GetAllAsync();
        bool VerifyPassword(string password, string hash);
        Task<string> GetPinHashAsync(Guid userId);
        Task SetPinAsync(Guid userId, string pin);
        Task<bool> VerifyPinAsync(Guid userId, string pin);
        Task<bool> HasPinAsync(Guid userId);
        Task<bool> UpdateUserFcmTokens(Guid userId, string token);
        Task<List<string>> GetAllUserFcmTokens(Guid userId);
        Task<bool> DeleteUserFcmToken(Guid userId, string token);
    }
}
