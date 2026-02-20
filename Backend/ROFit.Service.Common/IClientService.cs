using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IClientService
    {
        Task<ClientDto> GetByIdAsync(Guid id);
    }
}
