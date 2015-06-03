using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.Sigim ;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TabelaBasicaAppService : BaseAppService, ITabelaBasicaAppService
    {
        private IAssuntoContatoAppService assuntoContatoAppService;
        private IInteresseBairroAppService interesseBairroAppService;
        private IEstadoCivilAppService estadoCivilAppService;
        private IFonteNegocioAppService fonteNegocioAppService;
        private IGrupoAppService grupoAppService;
        private INacionalidadeAppService nacionalidadeAppService;
        private IParentescoAppService parentescoAppService;
        private IProfissaoAppService profissaoAppService;
        private IRamoAtividadeAppService ramoAtividadeAppService;
        private IRelacionamentoAppService relacionamentoAppService;
        private ITipologiaAppService tipologiaAppService;
        private ITratamentoAppService tratamentoAppService;
        private ITipoAreaAppService tipoAreaAppService;
        private ITipoCaracteristicaAppService tipoCaracteristicaAppService;
        private ITipoEspecificacaoAppService tipoEspecificacaoAppService;

        public TabelaBasicaAppService(IAssuntoContatoAppService assuntoContatoAppService, 
                                      IInteresseBairroAppService interesseBairroAppService, 
                                      IEstadoCivilAppService estadoCivilAppService, 
                                      IFonteNegocioAppService fonteNegocioAppService, 
                                      IGrupoAppService grupoAppService, 
                                      INacionalidadeAppService nacionalidadeAppService, 
                                      IParentescoAppService parentescoAppService, 
                                      IProfissaoAppService profissaoAppService, 
                                      IRamoAtividadeAppService ramoAtividadeAppService, 
                                      IRelacionamentoAppService relacionamentoAppService,
                                      ITipologiaAppService tipologiaAppService,
                                      ITratamentoAppService tratamentoAppService,
                                      ITipoAreaAppService tipoAreaAppService,
                                      ITipoCaracteristicaAppService tipoCaracteristicaAppService,
                                      ITipoEspecificacaoAppService tipoEspecificacaoAppService,
                                      MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.assuntoContatoAppService = assuntoContatoAppService;
            this.interesseBairroAppService = interesseBairroAppService;
            this.estadoCivilAppService = estadoCivilAppService;
            this.fonteNegocioAppService = fonteNegocioAppService;
            this.grupoAppService = grupoAppService;
            this.nacionalidadeAppService = nacionalidadeAppService;
            this.parentescoAppService = parentescoAppService;
            this.profissaoAppService = profissaoAppService;
            this.ramoAtividadeAppService = ramoAtividadeAppService;
            this.relacionamentoAppService = relacionamentoAppService;
            this.tipologiaAppService = tipologiaAppService;
            this.tratamentoAppService = tratamentoAppService;
            this.tipoAreaAppService = tipoAreaAppService;
            this.tipoCaracteristicaAppService = tipoCaracteristicaAppService;
            this.tipoEspecificacaoAppService = tipoEspecificacaoAppService;
        }

        #region ITabelaBasicaAppService Members

        public List<TabelaBasicaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros, int? tipoTabela)
        {
            List<TabelaBasicaDTO> ListaRetorno = new List<TabelaBasicaDTO>();

            totalRegistros = 0;

            switch (tipoTabela)
            {
                case (int) TabelaBasicaFinanceiro.AssuntoContato:
                    ListaRetorno = assuntoContatoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.BairroInteresse:
                    ListaRetorno = interesseBairroAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.EstadoCivil:
                    ListaRetorno = estadoCivilAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.FonteNegocio:
                    ListaRetorno = fonteNegocioAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Grupo:
                    ListaRetorno = grupoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Nacionalidade:
                    ListaRetorno = nacionalidadeAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Parentesco:
                    ListaRetorno = parentescoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Profissao:
                    ListaRetorno = profissaoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.RamoAtividade:
                    ListaRetorno = ramoAtividadeAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Relacionamento:
                    ListaRetorno = relacionamentoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Tipologia:
                    ListaRetorno = tipologiaAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.Tratamento:
                    ListaRetorno = tratamentoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoArea:
                    ListaRetorno = tipoAreaAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoCaracteristica:
                    ListaRetorno = tipoCaracteristicaAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoEspecificacao:
                    ListaRetorno = tipoEspecificacaoAppService.ListarPeloFiltro(filtro, out totalRegistros).To<List<TabelaBasicaDTO>>();
                    break;

                default:
                    break;
            }
            return ListaRetorno;
        }

        public TabelaBasicaDTO ObterPeloId(int? id, int? tipoTabela)
        {
            var tabelaBasicaDTO = new TabelaBasicaDTO();
            switch (tipoTabela) {
                case (int)TabelaBasicaFinanceiro.AssuntoContato:
                    tabelaBasicaDTO = assuntoContatoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.BairroInteresse :
                    tabelaBasicaDTO = interesseBairroAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.EstadoCivil:
                    tabelaBasicaDTO = estadoCivilAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.FonteNegocio:
                    tabelaBasicaDTO = fonteNegocioAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Grupo:
                    tabelaBasicaDTO = grupoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Nacionalidade:
                    tabelaBasicaDTO = nacionalidadeAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Parentesco :
                    tabelaBasicaDTO = parentescoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Profissao:
                    tabelaBasicaDTO = profissaoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.RamoAtividade:
                    tabelaBasicaDTO = ramoAtividadeAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Relacionamento:
                    tabelaBasicaDTO = relacionamentoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Tipologia:
                    tabelaBasicaDTO = tipologiaAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.Tratamento:
                    tabelaBasicaDTO = tratamentoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoArea:
                    tabelaBasicaDTO = tipoAreaAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoCaracteristica:
                    tabelaBasicaDTO = tipoCaracteristicaAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoEspecificacao:
                    tabelaBasicaDTO = tipoEspecificacaoAppService.ObterPeloId(id).To<TabelaBasicaDTO>();
                    break;

                default:
                    break;
            }
            return tabelaBasicaDTO;
        }

        public bool Salvar(TabelaBasicaDTO dto)
        {
            if (dto == null) throw new ArgumentNullException("dto");

            if (ValidaAutomatico(dto) == false ) {return false;} 

            bool retorno = false;
          
            switch (dto.TipoTabela)
            {
                case (int)TabelaBasicaFinanceiro.AssuntoContato:
                    retorno = assuntoContatoAppService.Salvar(dto.To<AssuntoContatoDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.BairroInteresse:
                    retorno = interesseBairroAppService.Salvar(dto.To<InteresseBairroDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.EstadoCivil:
                    retorno = estadoCivilAppService.Salvar(dto.To<EstadoCivilDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.FonteNegocio:
                    retorno = fonteNegocioAppService.Salvar(dto.To<FonteNegocioDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.Grupo:
                    retorno = grupoAppService.Salvar(dto.To<GrupoDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.Nacionalidade:
                    retorno = nacionalidadeAppService.Salvar(dto.To<NacionalidadeDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.Parentesco:
                    retorno = parentescoAppService.Salvar(dto.To<ParentescoDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.Profissao:
                    retorno = profissaoAppService.Salvar(dto.To<ProfissaoDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.RamoAtividade:
                    retorno = ramoAtividadeAppService.Salvar(dto.To<RamoAtividadeDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.Relacionamento:
                    retorno = relacionamentoAppService.Salvar(dto.To<RelacionamentoDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.Tipologia:
                    retorno = tipologiaAppService.Salvar(dto.To<TipologiaDTO>());
                    break;

                case (int)TabelaBasicaFinanceiro.Tratamento:
                    retorno = tratamentoAppService.Salvar(dto.To<TratamentoDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.TipoArea:
                    retorno = tipoAreaAppService.Salvar(dto.To<TipoAreaDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.TipoCaracteristica:
                    retorno = tipoCaracteristicaAppService.Salvar(dto.To<TipoCaracteristicaDTO>());
                    break;
                case (int)TabelaBasicaFinanceiro.TipoEspecificacao:
                    retorno = tipoEspecificacaoAppService.Salvar(dto.To<TipoEspecificacaoDTO>());
                    break;
                    
                default:
                    messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo de tabela"), TypeMessage.Error);
                    break;
            }
            return retorno;
        }

        public bool Deletar(int? id, int tipoTabela)
        {
            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var dto = ObterPeloId(id, tipoTabela);
            if (ValidaAutomatico(dto) == false) { return false; } 

            bool retorno = false;

            switch (tipoTabela)
            {
                case (int)TabelaBasicaFinanceiro.AssuntoContato:
                    retorno = assuntoContatoAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.BairroInteresse:
                    retorno = interesseBairroAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.EstadoCivil:
                    retorno = estadoCivilAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.FonteNegocio:
                    retorno = fonteNegocioAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Grupo:
                    retorno = grupoAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Nacionalidade:
                    retorno = nacionalidadeAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Parentesco :
                    retorno = parentescoAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Profissao :
                    retorno = profissaoAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.RamoAtividade:
                    retorno = ramoAtividadeAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Relacionamento:
                    retorno = relacionamentoAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Tipologia:
                    retorno = tipologiaAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.Tratamento:
                    retorno = tratamentoAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.TipoArea:
                    retorno = tipoAreaAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.TipoCaracteristica:
                    retorno = tipoCaracteristicaAppService.Deletar(id);
                    break;
                case (int)TabelaBasicaFinanceiro.TipoEspecificacao:
                    retorno = tipoEspecificacaoAppService.Deletar(id);
                    break;

                default:
                    messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo de tabela"), TypeMessage.Error);
                    break;
            }
            return retorno;
        }

        public bool ValidaAutomatico(TabelaBasicaDTO dto)
        {
            bool retorno = true;

            if (dto == null) 
            {
                retorno = false;
                throw new ArgumentNullException("dto");
            }

            if (dto.Automatico == null) { dto.Automatico = false; }

            if (dto.Automatico == true)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroProtegido, dto.Descricao), TypeMessage.Error);
                retorno = false;
            }

            return retorno;
        }

        #endregion
    }
}