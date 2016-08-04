using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Application.Service.Orcamento
{
    public class EmpresaAppService : BaseAppService, IEmpresaAppService
    {
        #region "Declaração"

        private IEmpresaRepository empresaRepository;

        #endregion

        #region "Construtor"
        public EmpresaAppService(IEmpresaRepository empresaRepository, 
                                 MessageQueue messageQueue)
                : base(messageQueue)
            {
                this.empresaRepository = empresaRepository;
            }
        #endregion

        #region IEmpresaAppService Members

        public List<EmpresaDTO> ListarTodos()
        {
            return empresaRepository.ListarTodos(l => l.ClienteFornecedor).To<List<EmpresaDTO>>();
        }

        public EmpresaDTO ObterEmpresaSemObraPai(int? id)
        {
            EmpresaDTO empresaDTO = new EmpresaDTO();

            if (id.HasValue)
            {
                Empresa empresa = empresaRepository.ObterPeloId(id, l => l.ListaObra);
                empresaDTO = empresa.To<EmpresaDTO>();
                Empresa empresaAux = empresa;
                if (empresa != null)
                {
                    foreach (Obra obra in empresa.ListaObra.OrderBy(l => l.Numero))
                    {
                        bool contem = false;
                        foreach (Obra obraAux in empresa.ListaObra.OrderBy(l => l.Numero))
                        {
                            if (obra.Numero == obraAux.Numero) continue;
                            if ((obraAux.Numero.Contains(".")) && (obraAux.Numero.Contains(obra.Numero)))
                            {
                                contem = true;
                                break;
                            }
                        }
                        if (!contem)
                        {
                            empresaDTO.ListaObraSemPai.Add(obra.To<ObraDTO>());
                        }
                    }
                }
            }

            return empresaDTO;
        }


        #endregion

    }
}
