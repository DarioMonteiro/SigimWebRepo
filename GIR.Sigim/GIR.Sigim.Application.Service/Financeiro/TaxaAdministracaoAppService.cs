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
using GIR.Sigim.Domain.Specification.Financeiro;
using GIR.Sigim.Application.Reports.Financeiro;
using CrystalDecisions.Shared;
using System.Data;


namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TaxaAdministracaoAppService : BaseAppService, ITaxaAdministracaoAppService
    {
        private ITaxaAdministracaoRepository taxaAdministracaoRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public TaxaAdministracaoAppService(ITaxaAdministracaoRepository taxaAdministracaoRepository, 
                                           IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                           MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.taxaAdministracaoRepository = taxaAdministracaoRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region ITaxaAdministracaoRepository Members

        public List<TaxaAdministracaoDTO> ListarPeloCentroCustoCliente(string CentroCustoId, int ClienteId)
        {
            return taxaAdministracaoRepository.ListarPeloFiltro(l => l.CentroCustoId == CentroCustoId && l.ClienteId == ClienteId ,l => l.CentroCusto, l => l.Cliente, l => l.Classe).To<List<TaxaAdministracaoDTO>>();
        }

        public bool Salvar(string CentroCustoId, int ClienteId, List<TaxaAdministracaoDTO> listaDto)
        {
            if (listaDto == null) throw new ArgumentNullException("dto");

            if (ValidaSalvar(listaDto) == false) { return false; }

            var taxaAdministracao = new TaxaAdministracao();
            var listaRemocao = ListarPeloCentroCustoCliente(CentroCustoId, ClienteId);

            foreach (var item in listaRemocao)
            {
                taxaAdministracao = new TaxaAdministracao();
                taxaAdministracao = taxaAdministracaoRepository.ObterPeloId(item.Id);
                taxaAdministracaoRepository.Remover(taxaAdministracao);
            }


            bool bolOK = true;
            foreach (var item in listaDto)
            {
                taxaAdministracao = new TaxaAdministracao();
                taxaAdministracao.Id = null;
                taxaAdministracao.CentroCustoId = CentroCustoId;
                taxaAdministracao.ClienteId = ClienteId;
                taxaAdministracao.ClasseId = item.Classe.Codigo;
                taxaAdministracao.Percentual = item.Percentual;

                if (Validator.IsValid(taxaAdministracao, out validationErrors))
                {
                    taxaAdministracaoRepository.Inserir(taxaAdministracao);
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
                taxaAdministracaoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
            else
            {
                taxaAdministracaoRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add(Resource.Sigim.ErrorMessages.GravacaoErro, TypeMessage.Error);
            }

            return bolOK;
         
        }

        public bool Deletar(string CentroCustoId, int ClienteId)
        {
            if ((CentroCustoId == null) || (ClienteId == 0))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            bool bolOK = true;
            var taxaAdministracao = new TaxaAdministracao();
            var listaRemocao = ListarPeloCentroCustoCliente(CentroCustoId, ClienteId);

            foreach (var item in listaRemocao)
            {
                taxaAdministracao = new TaxaAdministracao();
                taxaAdministracao = taxaAdministracaoRepository.ObterPeloId(item.Id);
                try
                {
                    taxaAdministracaoRepository.Remover(taxaAdministracao);
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
                taxaAdministracaoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
            }
            else
            {
                taxaAdministracaoRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add(Resource.Sigim.ErrorMessages.ExclusaoErro, TypeMessage.Error);
            }

            return bolOK;
        }

        public List<TaxaAdministracaoDTO> ListarTodos()
        {
            var lista = taxaAdministracaoRepository.ListarTodos(l => l.CentroCusto, l => l.Cliente).OrderBy(l => l.CentroCustoId).To<List<TaxaAdministracaoDTO>>();

            var vetTeste = new System.Collections.Hashtable();
            string texto;

            foreach (var item in Enumerable.Reverse(lista))
            {
                texto = item.CentroCusto.Codigo + "|" + item.ClienteId;

                if (vetTeste.ContainsKey(texto) == true) 
                {
                    lista.Remove(item);
                }
                else
                {
                    vetTeste.Add(texto, texto);
                }
            }

            return lista;

        }

        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TaxaAdministracaoGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TaxaAdministracaoDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.TaxaAdministracaoImprimir);
        }

        public FileDownloadDTO ExportarRelTaxaAdministracao(string centroCustoCodigo, int? clienteId, FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            List<TaxaAdministracao> listaTaxaAdministracao = new List<TaxaAdministracao>();
            if ((!string.IsNullOrEmpty(centroCustoCodigo)) && (clienteId.HasValue))
            {

                var specification = (Specification<TaxaAdministracao>)new TrueSpecification<TaxaAdministracao>();


                listaTaxaAdministracao = taxaAdministracaoRepository.ListarPeloFiltro(l => l.CentroCustoId == centroCustoCodigo && l.ClienteId == clienteId.Value, 
                                                                                      l => l.CentroCusto.ListaCentroCustoEmpresa, 
                                                                                      l => l.Cliente, 
                                                                                      l => l.Classe).To<List<TaxaAdministracao>>();
            }

            relTaxaAdministracao objRel = new relTaxaAdministracao();

            objRel.SetDataSource(RelTaxaAdministracaoToDataTable(listaTaxaAdministracao));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = listaTaxaAdministracao.First().CentroCusto;

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

        #region Métodos Privados

        public bool ValidaSalvar(List<TaxaAdministracaoDTO> listaDto)
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

        private DataTable RelTaxaAdministracaoToDataTable(List<TaxaAdministracao> listaTaxaAdministracao)
        {
            DataTable dta = new DataTable();
            DataColumn centroCusto = new DataColumn("centroCusto");
            DataColumn descricaoCentroCusto = new DataColumn("descricaoCentroCusto");
            DataColumn nomeCliente = new DataColumn("nomeCliente");
            DataColumn classe = new DataColumn("classe");
            DataColumn descricaoClasse = new DataColumn("descricaoClasse");
            DataColumn percentual = new DataColumn("percentual", System.Type.GetType("System.Decimal"));
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(centroCusto);
            dta.Columns.Add(descricaoCentroCusto);
            dta.Columns.Add(nomeCliente);
            dta.Columns.Add(classe);
            dta.Columns.Add(descricaoClasse);
            dta.Columns.Add(percentual);
            dta.Columns.Add(girErro);

            foreach (var taxaAdministracao in listaTaxaAdministracao)
            {
                DataRow row = dta.NewRow();

                row[centroCusto] = taxaAdministracao.CentroCusto.Codigo;
                row[descricaoCentroCusto] = taxaAdministracao.CentroCusto.Descricao;
                row[nomeCliente] = taxaAdministracao.Cliente.Nome;
                row[classe] = taxaAdministracao.Classe.Codigo;
                row[descricaoClasse] = taxaAdministracao.Classe.Descricao;
                row[percentual] = taxaAdministracao.Percentual;

                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }
        
        #endregion


    }
}