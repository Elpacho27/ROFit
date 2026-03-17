using ROFit.Model;
using ROFit.Models;
using ROFit.Repository.Common;
using ROFit.Service.Common;

namespace ROFit.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(UserRegister userRegister, Guid? createdBy = null)
        {
            return await _userRepository.CreateAsync(userRegister, createdBy);
        }

        public async Task<User> UpdateAsync(User user, Guid updatedBy)
        {
            return await _userRepository.UpdateAsync(user, updatedBy);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid updatedBy)
        {
            return await _userRepository.DeleteAsync(id, updatedBy);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userRepository.UserExistsAsync(email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> RegisterAsync(UserRegister model)
        {
            if (await _userRepository.UserExistsAsync(model.Email))
                throw new Exception("User already exists");

            var userRegister = new UserRegister
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            return await _userRepository.CreateAsync(userRegister);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;
            if (!VerifyPassword(password, user.PasswordHash)) return null;
            return user;
        }

        public Task<bool> HasPinAsync(Guid userId)
       => _userRepository.HasPinAsync(userId);

        public Task SetPinAsync(Guid userId, string pin)
            => _userRepository.SetPinAsync(userId, pin);

        public Task<bool> VerifyPinAsync(Guid userId, string pin)
            => _userRepository.VerifyPinAsync(userId, pin);


        public bool VerifyPassword(string password, string hash)
        {
            return _userRepository.VerifyPassword(password, hash);
        }

        public Task<bool> UpdateUserFcmTokens(Guid userId, string token)
        {
            return _userRepository.UpdateUserFcmTokens(userId, token);
        }

        public Task<List<string>> GetAllUserFcmTokens(Guid userId)
        {
            return _userRepository.GetAllUserFcmTokens(userId);
        }

        public Task<bool> DeleteUserFcmToken(Guid userId, string token)
        {
            return _userRepository.DeleteUserFcmToken(userId, token);
        }
    }
}
