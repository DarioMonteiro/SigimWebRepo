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
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class RateioAutomaticoAppService : BaseAppService, IRateioAutomaticoAppService
    {
        private IRateioAutomaticoRepository rateioAutomaticoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public RateioAutomaticoAppService(IRateioAutomaticoRepository rateioAutomaticoRepository,
                                          IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                          MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.rateioAutomaticoRepository = rateioAutomaticoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region IRateioAutomaticoRepository Members

        public List<RateioAutomaticoDTO> ListarPeloTipoRateio(int TipoRateioId)
        {
            return rateioAutomaticoRepository.ListarPeloFiltro(l => l.TipoRateioId == TipoRateioId, l => l.TipoRateio, l => l.Classe, l => l.CentroCusto).To<List<RateioAutomaticoDTO>>();
        }

        public bool Salvar(int TipoRateioId, List<RateioAutomaticoDTO> listaDto)
        {
            if (!EhPermitidoSalvar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (listaDto == null) throw new ArgumentNullException("dto");

            if (ValidaSalvar(listaDto) == false) { return false; }

            var rateioAutomatico = new RateioAutomatico();
            var listaRemocao = ListarPeloTipoRateio(TipoRateioId);

            foreach (var item in listaRemocao)
            {
                rateioAutomatico = new RateioAutomatico();
                rateioAutomatico = rateioAutomaticoRepository.ObterPeloId(item.Id);
                rateioAutomaticoRepository.Remover(rateioAutomatico);
            }

            bool bolOK = true;
            foreach (var item in listaDto)
            {
                rateioAutomatico = new RateioAutomatico();
                rateioAutomatico.Id = null;
                rateioAutomatico.TipoRateioId = TipoRateioId;
                rateioAutomatico.CentroCustoId = item.CentroCusto.Codigo;
                rateioAutomatico.ClasseId = item.Classe.Codigo;
                rateioAutomatico.Percentual = item.Percentual;

                if (Validator.IsValid(rateioAutomatico, out validationErrors))
                {
                    rateioAutomaticoRepository.Inserir(rateioAutomatico);
                    bolOK = true;
                }
                else
                {
                    bolOK = false;
                    break;
                }
            }

            if (bolOK == true) 
            {
                rateioAutomaticoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
            else
            {
                rateioAutomaticoRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add(Resource.Sigim.ErrorMessages.GravacaoErro, TypeMessage.Error);
            }

            return bolOK;
         
        }

        public bool Deletar(int TipoRateioId)
        {
            if (!EhPermitidoDeletar())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (TipoRateioId == 0)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            bool bolOK = true;
            var rateioAutomatico = new RateioAutomatico();
            var listaRemocao = ListarPeloTipoRateio(TipoRateioId);

            foreach (var item in listaRemocao)
            {
                rateioAutomatico = new RateioAutomatico();
                rateioAutomatico = rateioAutomaticoRepository.ObterPeloId(item.Id);
                try
                {
                    rateioAutomaticoRepository.Remover(rateioAutomatico);
                    bolOK = true;
                }
                catch (Exception)
                {
                    bolOK = false;
                    break;
                }
            }

            if (bolOK == true)
            {
                rateioAutomaticoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
            }
            else
            {
                rateioAutomaticoRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add(Resource.Sigim.ErrorMessages.ExclusaoErro, TypeMessage.Error);
            }

            return bolOK;
        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RateioAutomaticoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RateioAutomaticoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RateioAutomaticoImprimir);
        }

        public FileDownloadDTO ExportarRelRateioAutomatico(int? tipoRateioId, FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            List<RateioAutomatico> listaRateioAutomatico = new List<RateioAutomatico>();
            if (tipoRateioId.HasValue)
            {

                var specification = (Specification<RateioAutomatico>)new TrueSpecification<RateioAutomatico>();

                listaRateioAutomatico = rateioAutomaticoRepository.ListarPeloFiltro(l => l.TipoRateioId == tipoRateioId,
                                                                                    l => l.TipoRateio,
                                                                                    l => l.Classe,
                                                                                    l => l.CentroCusto.ListaCentroCustoEmpresa).To<List<RateioAutomatico>>();
            }

            relRateioAutomatico objRel = new relRateioAutomatico();

            objRel.SetDataSource(RelRateioAutomaticoToDataTable(listaRateioAutomatico));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = listaRateioAutomatico.Last().CentroCusto;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Rateio automático",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }


        #endregion


        #region Métodos Privados IRateioAutomaticoAppService

        public bool ValidaSalvar(List<RateioAutomaticoDTO> listaDto)
        {
            bool retorno = true;
            decimal decPercentualTotal = 0;

            foreach (var item in listaDto)
            {
                decPercentualTotal += item.Percentual;
            }

            if (decPercentualTotal > 100)
            {
                messageQueue.Add("O percentual total não pode ser maior que 100% !", TypeMessage.Error);
                retorno = false;
            }

            return retorno;
        }

        private DataTable RelRateioAutomaticoToDataTable(List<RateioAutomatico> listaRateioAutomatico)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn tipoRateio = new DataColumn("tipoRateio");
            DataColumn classe = new DataColumn("classe");
            DataColumn centroCusto = new DataColumn("centroCusto");
            DataColumn percentual = new DataColumn("percentual", System.Type.GetType("System.Decimal"));
            DataColumn descricaoTipoRateio = new DataColumn("descricaoTipoRateio");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn descricaoCentroCusto = new DataColumn("descricaoCentroCusto");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(tipoRateio);
            dta.Columns.Add(classe);
            dta.Columns.Add(centroCusto);
            dta.Columns.Add(percentual);
            dta.Columns.Add(descricaoTipoRateio);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(descricaoCentroCusto);
            dta.Columns.Add(girErro);

            foreach (var registro in listaRateioAutomatico)
            {
                RateioAutomaticoDTO rateioAutomatico = registro.To<RateioAutomaticoDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = rateioAutomatico.Id;
                row[tipoRateio] = rateioAutomatico.TipoRateioId;
                row[classe] = rateioAutomatico.ClasseId;
                row[centroCusto] = rateioAutomatico.CentroCusto;
                row[percentual] = rateioAutomatico.Percentual;
                row[descricaoTipoRateio] = rateioAutomatico.TipoRateio.Descricao;
                row[descricaoClasse] = rateioAutomatico.Classe.ClasseDescricao;
                row[descricaoCentroCusto] = rateioAutomatico.CentroCusto.CentroCustoDescricao;

                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }


        #endregion


    }
}