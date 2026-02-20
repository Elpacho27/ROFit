using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IClientRepository
    {
        Task<ClientDto> GetByIdAsync(Guid id);
    }
}
