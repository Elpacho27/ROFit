using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;

namespace ROFit.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<ClientDto> GetByIdAsync(Guid id)
        {
            return await _clientRepository.GetByIdAsync(id);
        }

    }
}
