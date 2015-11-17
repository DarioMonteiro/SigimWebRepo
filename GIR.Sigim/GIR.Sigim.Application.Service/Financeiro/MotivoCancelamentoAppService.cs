using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class MotivoCancelamentoAppService : BaseAppService, IMotivoCancelamentoAppService
    {
        private IMotivoCancelamentoRepository motivoCancelamentoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public MotivoCancelamentoAppService(IMotivoCancelamentoRepository motivoCancelamentoRepository, 
                                            IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.motivoCancelamentoRepository = motivoCancelamentoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region IMotivoCancelamentoAppService Members

       
        public List<MotivoCancelamentoDTO> ListarPeloFiltro(MotivoCancelamentoFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<MotivoCancelamento>)new TrueSpecification<MotivoCancelamento>();


            return motivoCancelamentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<MotivoCancelamentoDTO>>();
        }

        public MotivoCancelamentoDTO ObterPeloId(int? id)
        {
            return motivoCancelamentoRepository.ObterPeloId(id).To<MotivoCancelamentoDTO>();
        }

        public bool Salvar(MotivoCancelamentoDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.MotivoCancelamentoGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var motivoCancelamento = motivoCancelamentoRepository.ObterPeloId(dto.Id);
            if (motivoCancelamento == null)
            {
                motivoCancelamento = new MotivoCancelamento();
                novoItem = true;
            }

            motivoCancelamento.Descricao = dto.Descricao;

            if (Validator.IsValid(motivoCancelamento, out validationErrors))
            {
                if (novoItem)
                    motivoCancelamentoRepository.Inserir(motivoCancelamento);
                else
                    motivoCancelamentoRepository.Alterar(motivoCancelamento);

                motivoCancelamentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {

            if (!UsuarioLogado.IsInRole(Funcionalidade.MotivoCancelamentoDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var motivoCancelamento = motivoCancelamentoRepository.ObterPeloId(id);

            try
            {
                motivoCancelamentoRepository.Remover(motivoCancelamento);
                motivoCancelamentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, motivoCancelamento.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.MotivoCancelamentoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.MotivoCancelamentoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TipoMovimentoImprimir);
        }

        public FileDownloadDTO ExportarRelMotivoCancelamento(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<MotivoCancelamento>)new TrueSpecification<MotivoCancelamento>();

            var listaMotivoCancelamento = motivoCancelamentoRepository.ListarPeloFiltro(specification).To<List<MotivoCancelamento>>();

            relMotivoCancelamento objRel = new relMotivoCancelamento();

            objRel.SetDataSource(RelMotivoCancelamentoToDataTable(listaMotivoCancelamento));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Motivo cancelamento",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region métodos privados de IMotivoCancelamentoAppService

        private DataTable RelMotivoCancelamentoToDataTable(List<MotivoCancelamento> listaMotivoCancelamento)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(girErro);

            foreach (var motivoCancelamento in listaMotivoCancelamento)
            {
                DataRow row = dta.NewRow();

                row[codigo] = motivoCancelamento.Id;
                row[descricao] = motivoCancelamento.Descricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion
      
    }
}