using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
