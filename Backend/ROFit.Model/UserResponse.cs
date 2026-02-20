using ROFit.Model.Common;

namespace ROFit.Model
{
    public class UserResponse : IUserResponse
    {
       public Guid Id { get; set; }
       public string FullName { get; set; }
       public string Email { get; set; }
       public string Role { get; set; }

        public UserResponse() { }

        public UserResponse(Guid id, string fullName, string email, string role)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Role = role;
        }
    }
}
