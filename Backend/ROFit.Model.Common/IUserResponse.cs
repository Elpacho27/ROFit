
namespace ROFit.Model.Common
{
    public interface IUserResponse
    {
        Guid Id { get; set; }
        string FullName { get; set; }
        string Email { get; set; }
        string Role { get; set; }
    }
}
