using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Sigim; 

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IClienteFornecedorAppService
    {
        List<ClienteFornecedorDTO> ListarAtivos();
        List<ClienteFornecedorDTO> ListarAtivosDeContrato();
        ClienteFornecedorDTO ObterPeloId(int? id);
        //List<ClienteFornecedorDTO> ListarClienteFornecedor(ClassificacaoClienteFornecedor classificacaoClienteFornecedor, SituacaoClienteFornecedor situacaoClienteFornecedor, TipoPessoa tipoPessoa);
    }
}