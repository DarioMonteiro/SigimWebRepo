using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Contrato;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoAppService : BaseAppService , IContratoAppService
    {
        #region Declaração

        private IContratoRepository contratoRepository;
        private IUsuarioAppService usuarioAppService;

        #endregion

        #region Construtor

        public ContratoAppService(IContratoRepository contratoRepository, 
                                  IUsuarioAppService usuarioAppService, 
                                  MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRepository = contratoRepository;
            this.usuarioAppService = usuarioAppService; 
        }

        #endregion

        #region Métodos IContratoAppService


        public List<ContratoDTO> ListarPeloFiltro(MedicaoContratoFiltro filtro,int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Contrato))
                specification &= ContratoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Contrato);

            if (filtro.Id.HasValue)
                specification &= ContratoSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= ContratoSpecification.PertenceAoCentroCusto(filtro.CentroCusto.Codigo);
                specification &= ContratoSpecification.PertenceAoContratante(filtro.ContratanteId);
                specification &= ContratoSpecification.PertenceAoContratado(filtro.ContratadoId);
            }

            return contratoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.CentroCusto,
                l => l.ContratoDescricao.ListaContrato,
                l => l.Contratante.ListaContratoContratante,
                l => l.Contratado.ListaContratoContratado).To<List<ContratoDTO>>();
        }

        public ContratoDTO ObterPeloId(int? id, int? idUsuario)
        {
            var specification = (Specification<Domain.Entity.Contrato.Contrato>)new TrueSpecification<Domain.Entity.Contrato.Contrato>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Contrato))
                specification &= ContratoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Contrato);

            return contratoRepository.ObterPeloId(id, 
                                                  specification, 
                                                  l => l.CentroCusto, 
                                                  l => l.Contratado.PessoaFisica, 
                                                  l => l.Contratado.PessoaJuridica , 
                                                  l => l.ContratoDescricao, 
                                                  l => l.ListaContratoRetificacao,
                                                  l => l.ListaContratoRetificacaoItem.Select(d => d.Servico),
                                                  l => l.ListaContratoRetificacaoItemMedicao).To<ContratoDTO>();
        }

        public bool EhContratoAssinado(ContratoDTO dto)
        {
            return PodeSerUmContratoAssinado(dto.Situacao);
        }

        public bool EhContratoExistente(ContratoDTO dto)
        {
            if (dto == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            if (!dto.Id.HasValue)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool EhContratoComCentroCustoAtivo(ContratoDTO dto)
        {
            if (!dto.CentroCusto.Ativo)
            {
                messageQueue.Add(Application.Resource.Financeiro.ErrorMessages.CentroCustoInativo, TypeMessage.Error);
                return false;
            }

            return true;
        }


        public List<ContratoRetificacaoProvisaoDTO> ObterListaCronograma(int contratoId, int contratoRetificacaoItemId)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId, 
                                                          l => l.ListaContratoRetificacaoItem.Select(i => i.Servico),
                                                          l => l.ListaContratoRetificacaoItem.Select(i => i.RetencaoTipoCompromisso),
                                                          l => l.ListaContratoRetificacaoProvisao,
                                                          l => l.ListaContratoRetificacaoItemCronograma,
                                                          l => l.ListaContratoRetificacaoItemMedicao);
            return contrato.ListaContratoRetificacaoProvisao
                .Where(l => l.ContratoRetificacaoItemId == contratoRetificacaoItemId)
                .To<List<ContratoRetificacaoProvisaoDTO>>();
        }

        public List<ContratoRetificacaoItemMedicaoDTO> ObtemMedicaoPorSequencialItem(int contratoId, int sequencialItem)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId,
                                                          l => l.ListaContratoRetificacaoItemMedicao.Select(i => i.TipoDocumento));

            return contrato.ListaContratoRetificacaoItemMedicao
                    .Where(l => l.SequencialItem == sequencialItem).OrderBy(l => l.DataVencimento)
                    .To<List<ContratoRetificacaoItemMedicaoDTO>>();
        }

        public ContratoRetificacaoItemMedicaoDTO ObtemMedicaoPorId(int contratoId, int contratoRetificacaoItemMedicaoId)
        {
            var contrato = contratoRepository.ObterPeloId(contratoId,
                                                          l => l.ListaContratoRetificacaoItemCronograma,
                                                          l => l.ListaContratoRetificacaoItemMedicao);

            var medicao =  contrato.ListaContratoRetificacaoItemMedicao
                            .Where(l => l.Id.Value == contratoRetificacaoItemMedicaoId).SingleOrDefault()
                            .To<ContratoRetificacaoItemMedicaoDTO>();
            return medicao;
        }

        public bool ExisteContratoRetificacaoProvisao(List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao)
        {
            if (listaContratoRetificacaoProvisao == null)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.RetificacaoItemSemProvisionamento, TypeMessage.Error);
                return false;
            }
            if (listaContratoRetificacaoProvisao.Count == 0)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.RetificacaoItemSemProvisionamento, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ExisteMedicao(ContratoRetificacaoItemMedicaoDTO dto)
        {
            if (dto == null)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.MedicaoNaoEncontrada, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool EhValidoParametrosVisualizacaoMedicao(int? contratoId,
                                                          int? tipoDocumentoId,
                                                          string numeroDocumento,
                                                          Nullable<DateTime> dataEmissao,
                                                          int? contratadoId)
        {
            if (!contratadoId.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contrato"), TypeMessage.Error);
                return false;
            }

            if (contratadoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contrato"), TypeMessage.Error);
                return false;
            }

            if (!tipoDocumentoId.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
                return false;
            }

            if (tipoDocumentoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(numeroDocumento))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Nº"), TypeMessage.Error);
                return false;
            }

            if (!dataEmissao.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data emissão"), TypeMessage.Error);
                return false;
            }

            if (!contratadoId.HasValue)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contratado"), TypeMessage.Error);
                return false;
            }

            return true;

        }

        public List<ContratoRetificacaoItemMedicaoDTO> RecuperaMedicaoPorDadosDaNota(int contratoId,
                                                                                     int tipoDocumentoId,
                                                                                     string numeroDocumento,
                                                                                     DateTime dataEmissao,
                                                                                     int? contratadoId)
        {
            List<ContratoRetificacaoItemMedicaoDTO> listaMedicao;
            var contrato = contratoRepository.ObterPeloId(contratoId,
                                                          l => l.ListaContratoRetificacaoItem.Select(i => i.Servico),
                                                          l => l.ListaContratoRetificacaoItemMedicao);
            
            if ((contratadoId.HasValue) && (contratadoId.Value > 0) && (contrato != null))
            {
                listaMedicao =
                contrato.ListaContratoRetificacaoItemMedicao.Where(i => i.TipoDocumentoId == tipoDocumentoId &&
                                                                        i.NumeroDocumento == numeroDocumento &&
                                                                        i.DataEmissao == dataEmissao &&
                                                                        ((i.MultiFornecedorId == contratadoId) ||
                                                                        (i.MultiFornecedorId == null && 
                                                                         i.Contrato.ContratadoId == contratadoId))
                                                                   ).To<List<ContratoRetificacaoItemMedicaoDTO>>();
            }
            else{
                listaMedicao =
                contrato.ListaContratoRetificacaoItemMedicao.Where(i => i.TipoDocumentoId == tipoDocumentoId &&
                                                                        i.NumeroDocumento == numeroDocumento &&
                                                                        i.DataEmissao == dataEmissao
                                                                   ).To<List<ContratoRetificacaoItemMedicaoDTO>>();
            }

            return listaMedicao;
        }

        public bool ExisteBlaBlaBla()
        {
            return contratoRepository.ListarPeloFiltro(l => l.ListaContratoRetificacaoItemMedicao
                .Any(s => s.NumeroDocumento == "51")).Any();
        }


        #endregion

        #region Métodos Privados

        private bool PodeSerUmContratoAssinado(SituacaoContrato situacao)
        {
            if (situacao != SituacaoContrato.Assinado)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.ContratoNaoAssinado, TypeMessage.Error);
                return false;
            }

            return true;
        }

        #endregion
    }
}
