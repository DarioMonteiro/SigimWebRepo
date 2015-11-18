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
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.Sigim ;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Sigim;
using CrystalDecisions.Shared;
using System.Data;


namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TabelaBasicaAppService : BaseAppService, ITabelaBasicaAppService
    {
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;
        private IAssuntoContatoAppService assuntoContatoAppService;
        private IAssuntoContatoRepository assuntoContatoRepository;
        private IInteresseBairroAppService interesseBairroAppService;
        private IInteresseBairroRepository interesseBairroRepository;
        private IEstadoCivilAppService estadoCivilAppService;
        private IEstadoCivilRepository estadoCivilRepository;
        private IFonteNegocioAppService fonteNegocioAppService;
        private IFonteNegocioRepository fonteNegocioRepository;
        private IGrupoAppService grupoAppService;
        private IGrupoRepository grupoRepository;
        private INacionalidadeAppService nacionalidadeAppService;
        private INacionalidadeRepository nacionalidadeRepository;
        private IParentescoAppService parentescoAppService;
        private IParentescoRepository parentescoRepository;
        private IProfissaoAppService profissaoAppService;
        private IProfissaoRepository profissaoRepository;
        private IRamoAtividadeAppService ramoAtividadeAppService;
        private IRamoAtividadeRepository ramoAtividadeRepository;
        private IRelacionamentoAppService relacionamentoAppService;
        private IRelacionamentoRepository relacionamentoRepository;
        private ITipologiaAppService tipologiaAppService;
        private ITipologiaRepository tipologiaRepository;
        private ITratamentoAppService tratamentoAppService;
        private ITratamentoRepository tratamentoRepository;
        private ITipoAreaAppService tipoAreaAppService;
        private ITipoAreaRepository tipoAreaRepository;
        private ITipoCaracteristicaAppService tipoCaracteristicaAppService;
        private ITipoCaracteristicaRepository tipoCaracteristicaRepository;
        private ITipoEspecificacaoAppService tipoEspecificacaoAppService;
        private ITipoEspecificacaoRepository tipoEspecificacaoRepository;

        public TabelaBasicaAppService(IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                      IAssuntoContatoAppService assuntoContatoAppService,
                                      IAssuntoContatoRepository assuntoContatoRepository,
                                      IInteresseBairroAppService interesseBairroAppService,
                                      IInteresseBairroRepository interesseBairroRepository,
                                      IEstadoCivilAppService estadoCivilAppService,
                                      IEstadoCivilRepository estadoCivilRepository, 
                                      IFonteNegocioAppService fonteNegocioAppService,
                                      IFonteNegocioRepository fonteNegocioRepository, 
                                      IGrupoAppService grupoAppService,
                                      IGrupoRepository grupoRepository,
                                      INacionalidadeAppService nacionalidadeAppService,
                                      INacionalidadeRepository nacionalidadeRepository,
                                      IParentescoAppService parentescoAppService,
                                      IParentescoRepository parentescoRepository,
                                      IProfissaoAppService profissaoAppService,
                                      IProfissaoRepository profissaoRepository,
                                      IRamoAtividadeAppService ramoAtividadeAppService,
                                      IRamoAtividadeRepository ramoAtividadeRepository,
                                      IRelacionamentoAppService relacionamentoAppService,
                                      IRelacionamentoRepository relacionamentoRepository,
                                      ITipologiaAppService tipologiaAppService,
                                      ITipologiaRepository tipologiaRepository,
                                      ITratamentoAppService tratamentoAppService,
                                      ITratamentoRepository tratamentoRepository,
                                      ITipoAreaAppService tipoAreaAppService,
                                      ITipoAreaRepository tipoAreaRepository,
                                      ITipoCaracteristicaAppService tipoCaracteristicaAppService,
                                      ITipoCaracteristicaRepository tipoCaracteristicaRepository,
                                      ITipoEspecificacaoAppService tipoEspecificacaoAppService,
                                      ITipoEspecificacaoRepository tipoEspecificacaoRepository,
                                      MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
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

            this.assuntoContatoRepository = assuntoContatoRepository;
            this.interesseBairroRepository = interesseBairroRepository;
            this.estadoCivilRepository = estadoCivilRepository;
            this.fonteNegocioRepository = fonteNegocioRepository;
            this.grupoRepository = grupoRepository;
            this.nacionalidadeRepository = nacionalidadeRepository;
            this.parentescoRepository = parentescoRepository;
            this.profissaoRepository = profissaoRepository;
            this.ramoAtividadeRepository = ramoAtividadeRepository;
            this.relacionamentoRepository = relacionamentoRepository;
            this.tipologiaRepository = tipologiaRepository;
            this.tratamentoRepository = tratamentoRepository;
            this.tipoAreaRepository = tipoAreaRepository;
            this.tipoCaracteristicaRepository = tipoCaracteristicaRepository;
            this.tipoEspecificacaoRepository = tipoEspecificacaoRepository;

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

            if (ListaRetorno.Count > 0)
            {
                ListaRetorno = ListaRetorno.OrderBy(l => l.Descricao).ToList();
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
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

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
            if (!EhPermitidoDeletar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

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

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TabelaBasicaFinanceiroGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TabelaBasicaFinanceiroDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TabelaBasicaFinanceiroImprimir);
        }

        public FileDownloadDTO ExportarRelTabelaBasica(int? tipoTabelaId, FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            List<TabelaBasicaDTO> listaTabelaBasica = new List<TabelaBasicaDTO>();
            string nomeTabela = "";

            switch (tipoTabelaId)
            {
                case (int)TabelaBasicaFinanceiro.AssuntoContato:
                    var specificationAssuntoContato = (Specification<AssuntoContato>)new TrueSpecification<AssuntoContato>();
                    listaTabelaBasica = assuntoContatoRepository.ListarPeloFiltro(specificationAssuntoContato).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.AssuntoContato.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.BairroInteresse:
                    var specificationBairroInteresse = (Specification<InteresseBairro>)new TrueSpecification<InteresseBairro>();
                    listaTabelaBasica = interesseBairroRepository.ListarPeloFiltro(specificationBairroInteresse).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.BairroInteresse.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.EstadoCivil:
                    var specificationEstadoCivil = (Specification<EstadoCivil>)new TrueSpecification<EstadoCivil>();
                    listaTabelaBasica = estadoCivilRepository.ListarPeloFiltro(specificationEstadoCivil).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.EstadoCivil.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.FonteNegocio:
                    var specificationFonteNegocio = (Specification<FonteNegocio>)new TrueSpecification<FonteNegocio>();
                    listaTabelaBasica = fonteNegocioRepository.ListarPeloFiltro(specificationFonteNegocio).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.FonteNegocio.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Grupo:
                    var specificationGrupo = (Specification<Grupo>)new TrueSpecification<Grupo>();
                    listaTabelaBasica = grupoRepository.ListarPeloFiltro(specificationGrupo).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Grupo.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Nacionalidade:
                    var specificationNacionalidade = (Specification<Nacionalidade>)new TrueSpecification<Nacionalidade>();
                    listaTabelaBasica = nacionalidadeRepository.ListarPeloFiltro(specificationNacionalidade).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Nacionalidade.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Parentesco:
                    var specificationParentesco = (Specification<Parentesco>)new TrueSpecification<Parentesco>();
                    listaTabelaBasica = parentescoRepository.ListarPeloFiltro(specificationParentesco).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Parentesco.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Profissao:
                    var specificationProfissao = (Specification<Profissao>)new TrueSpecification<Profissao>();
                    listaTabelaBasica = profissaoRepository.ListarPeloFiltro(specificationProfissao).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Profissao.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.RamoAtividade:
                    var specificationRamoAtividade = (Specification<RamoAtividade>)new TrueSpecification<RamoAtividade>();
                    listaTabelaBasica = ramoAtividadeRepository.ListarPeloFiltro(specificationRamoAtividade).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.RamoAtividade.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Relacionamento:
                    var specificationRelacionamento = (Specification<Relacionamento>)new TrueSpecification<Relacionamento>();
                    listaTabelaBasica = relacionamentoRepository.ListarPeloFiltro(specificationRelacionamento).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Relacionamento.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Tipologia:
                    var specificationTipologia = (Specification<Tipologia>)new TrueSpecification<Tipologia>();
                    listaTabelaBasica = tipologiaRepository.ListarPeloFiltro(specificationTipologia).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Tipologia.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.Tratamento:
                    var specificationTratamento = (Specification<Tratamento>)new TrueSpecification<Tratamento>();
                    listaTabelaBasica = tratamentoRepository.ListarPeloFiltro(specificationTratamento).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.Tratamento.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoArea:
                    var specificationTipoArea = (Specification<TipoArea>)new TrueSpecification<TipoArea>();
                    listaTabelaBasica = tipoAreaRepository.ListarPeloFiltro(specificationTipoArea).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.TipoArea.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoCaracteristica:
                    var specificationTipoCaracteristica = (Specification<TipoCaracteristica>)new TrueSpecification<TipoCaracteristica>();
                    listaTabelaBasica = tipoCaracteristicaRepository.ListarPeloFiltro(specificationTipoCaracteristica).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.TipoCaracteristica.ObterDescricao();
                    break;
                case (int)TabelaBasicaFinanceiro.TipoEspecificacao:
                    var specificationTipoEspecificacao = (Specification<TipoEspecificacao>)new TrueSpecification<TipoEspecificacao>();
                    listaTabelaBasica = tipoEspecificacaoRepository.ListarPeloFiltro(specificationTipoEspecificacao).To<List<TabelaBasicaDTO>>();
                    nomeTabela = TabelaBasicaFinanceiro.TipoEspecificacao.ObterDescricao();
                    break;
                default:
                    break;
            }


            relTabelaBasica objRel = new relTabelaBasica();

            objRel.SetDataSource(RelRateioAutomaticoToDataTable(listaTabelaBasica));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeTabela", nomeTabela);
            objRel.SetParameterValue("nomeSistema", "FINANCEIRO");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Tabelas básicas",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region Métodos Privados ITabelaBasicaAppService

        private DataTable RelRateioAutomaticoToDataTable(List<TabelaBasicaDTO> listaTabelaBasica)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(girErro);

            foreach (var registro in listaTabelaBasica.OrderBy(l => l.Descricao))
            {
                DataRow row = dta.NewRow();

                row[codigo] = registro.Id;
                row[descricao] = registro.Descricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion
    }


}