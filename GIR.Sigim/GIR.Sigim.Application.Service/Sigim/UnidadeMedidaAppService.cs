using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Sigim;
using System.Linq.Expressions;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Sigim;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class UnidadeMedidaAppService : BaseAppService, IUnidadeMedidaAppService
    {
        private IUnidadeMedidaRepository unidadeMedidaRepository;
        private IParametrosOrdemCompraRepository parametrosOrdemCompraRepository;

        public UnidadeMedidaAppService(IUnidadeMedidaRepository unidadeMedidaRepository,
                                       IParametrosOrdemCompraRepository parametrosOrdemCompraRepository, 
                                       MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.unidadeMedidaRepository = unidadeMedidaRepository;
            this.parametrosOrdemCompraRepository = parametrosOrdemCompraRepository;
        }

        #region IUnidadeMedidaAppService Members

        public List<UnidadeMedidaDTO> ListarPeloFiltro(UnidadeMedidaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<UnidadeMedida>)new TrueSpecification<UnidadeMedida>();


            return unidadeMedidaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<UnidadeMedidaDTO>>();
        }

        public UnidadeMedidaDTO ObterPeloCodigo(string sigla)
        {
            return unidadeMedidaRepository.ObterPeloCodigo(sigla).To<UnidadeMedidaDTO>();
        }

        public List<UnidadeMedidaDTO> ListarTodos()
        {
            return unidadeMedidaRepository.ListarTodos().To<List<UnidadeMedidaDTO>>();
        }

        public bool Salvar(UnidadeMedidaDTO dto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var unidadeMedida = unidadeMedidaRepository.ObterPeloCodigo(dto.Sigla);
            if (unidadeMedida == null)
            {
                unidadeMedida = new UnidadeMedida();
                novoItem = true;
            }

            unidadeMedida.Descricao = dto.Descricao;
            unidadeMedida.Sigla = dto.Sigla;

            if (Validator.IsValid(unidadeMedida, out validationErrors))
            {
                if (novoItem)                    
                    unidadeMedidaRepository.Inserir(unidadeMedida);
                else
                    unidadeMedidaRepository.Alterar(unidadeMedida);

                unidadeMedidaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(string sigla)
        {
            if (!EhPermitidoDeletar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (sigla == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var unidadeMedida = unidadeMedidaRepository.ObterPeloCodigo(sigla);

            try
            {
                unidadeMedidaRepository.Remover(unidadeMedida);
                unidadeMedidaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, unidadeMedida.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.OrdemCompraUnidadeMedidaGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.OrdemCompraUnidadeMedidaDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.OrdemCompraUnidadeMedidaImprimir);
        }

        public FileDownloadDTO ExportarRelUnidadeMedida(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<UnidadeMedida>)new TrueSpecification<UnidadeMedida>();

            var listaUnidadeMedida = unidadeMedidaRepository.ListarPeloFiltro(specification).OrderBy(l => l.Sigla).To<List<UnidadeMedida>>();
            relUnidadeMedida objRel = new relUnidadeMedida();

            objRel.SetDataSource(RelUnidadeMedidaToDataTable(listaUnidadeMedida));

            var parametros = parametrosOrdemCompraRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeSistema", "ORDEMCOMPRA");
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Unidade medida",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region métodos privados de IUnidadeMedidaAppService

        private DataTable RelUnidadeMedidaToDataTable(List<UnidadeMedida> listaUnidadeMedida)
        {
            DataTable dta = new DataTable();
            DataColumn siglaUnidade = new DataColumn("siglaUnidade");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(siglaUnidade);
            dta.Columns.Add(descricao);
            dta.Columns.Add(girErro);

            foreach (var unidadeMedida in listaUnidadeMedida)
            {
                DataRow row = dta.NewRow();

                row[siglaUnidade] = unidadeMedida.Sigla;
                row[descricao] = unidadeMedida.Descricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion

    }
}