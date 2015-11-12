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
    public class CaixaAppService : BaseAppService, ICaixaAppService
    {
        private ICaixaRepository caixaRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public CaixaAppService(ICaixaRepository caixaRepository, 
                               IParametrosFinanceiroRepository parametrosFinanceiroRepository, 
                               MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.caixaRepository = caixaRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region ICaixaAppService Members
        
        public List<CaixaDTO> ListarPeloFiltro(CaixaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Caixa>)new TrueSpecification<Caixa>();


            return caixaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.CentroCusto ).To<List<CaixaDTO>>();
        }

        public CaixaDTO ObterPeloId(int? id)
        {
            return caixaRepository.ObterPeloId(id, l => l.CentroCusto).To<CaixaDTO>();
        }

        public bool Salvar(CaixaDTO dto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(dto.Descricao))
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Descrição"), TypeMessage.Error);
                return false;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var caixa = caixaRepository.ObterPeloId(dto.Id, l => l.CentroCusto);
            if (caixa == null)
            {
                caixa = new Caixa();
                novoItem = true;
            }

            caixa.Descricao = dto.Descricao;
            caixa.Situacao = dto.Situacao;
            caixa.CentroContabil = dto.CentroContabil;
            caixa.CodigoCentroCusto = dto.CentroCusto.Codigo;

            if (Validator.IsValid(caixa, out validationErrors))
            {
                if (novoItem)
                {
                    caixaRepository.Inserir(caixa);
                }
                else
                {
                    caixaRepository.Alterar(caixa);
                }

                caixaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
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

            var caixa = caixaRepository.ObterPeloId(id);

            try
            {
                caixaRepository.Remover(caixa);
                caixaRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, caixa.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.CaixaGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.CaixaDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.CaixaImprimir);
        }

        public FileDownloadDTO ExportarRelCaixa(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<Caixa>)new TrueSpecification<Caixa>();

            var listaCaixa = caixaRepository.ListarPeloFiltro(specification,
                                                              l => l.CentroCusto).To<List<Caixa>>();
            relCaixa objRel = new relCaixa();

            objRel.SetDataSource(RelCaixaToDataTable(listaCaixa));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Caixa",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion

        #region métodos privados de ICaixaAppService

        private DataTable RelCaixaToDataTable(List<Caixa> listaCaixa)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn descricaoSituacao = new DataColumn("descricaoSituacao");
            DataColumn centroContabil = new DataColumn("centroContabil");
            DataColumn codigoDescricaoCentroCusto = new DataColumn("codigoDescricaoCentroCusto");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(descricao);
            dta.Columns.Add(descricaoSituacao);
            dta.Columns.Add(centroContabil);
            dta.Columns.Add(codigoDescricaoCentroCusto);
            dta.Columns.Add(girErro);

            foreach (var registro in listaCaixa)
            {
                CaixaDTO caixa = registro.To<CaixaDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = caixa.Id;
                row[descricao] = caixa.Descricao;
                row[descricaoSituacao] = caixa.DescricaoSituacao;
                row[centroContabil] = caixa.CentroContabil ;
                row[codigoDescricaoCentroCusto] = caixa.CentroCustoDescricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }

        #endregion
    }
}