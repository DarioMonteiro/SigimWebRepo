using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class ClienteFornecedorAppService : BaseAppService, IClienteFornecedorAppService
    {
        private IClienteFornecedorRepository clienteFornecedorRepository;

        public ClienteFornecedorAppService(IClienteFornecedorRepository clienteFornecedorRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.clienteFornecedorRepository = clienteFornecedorRepository;
        }

        public List<ClienteFornecedorDTO> ListarAtivos()
        {
            return clienteFornecedorRepository.ListarAtivos().To<List<ClienteFornecedorDTO>>();
        }
    }
}