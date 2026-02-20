
namespace ROFit.Model.Common
{
    public interface IUser
    {
         Guid Id { get; set; }
         string FullName { get; set; }
         string Email { get; set; }
         string PasswordHash { get; set; }
         string Role { get; set; }
         DateTime DateCreated { get; set; } 
         DateTime? DateUpdated { get; set; }
         Guid? CreatedBy { get; set; }
         Guid? UpdatedBy { get; set; }
         bool IsActive { get; set; }
         string PinHash { get; set; }
    }
}
