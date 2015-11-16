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
    public class ImpostoFinanceiroAppService : BaseAppService, IImpostoFinanceiroAppService
    {
        private IImpostoFinanceiroRepository impostoFinanceiroRepository;
        private IParametrosFinanceiroRepository parametrosFinanceiroRepository;

        public ImpostoFinanceiroAppService(IImpostoFinanceiroRepository impostoFinanceiroRepository, 
                                           IParametrosFinanceiroRepository parametrosFinanceiroRepository,
                                           MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.impostoFinanceiroRepository = impostoFinanceiroRepository;
            this.parametrosFinanceiroRepository = parametrosFinanceiroRepository;
        }

        #region IImpostoFinanceiroAppService Members

        public List<ImpostoFinanceiroDTO> ListarTodos()
        {
            return impostoFinanceiroRepository.ListarTodos().To<List<ImpostoFinanceiroDTO>>();
        }

        public List<ImpostoFinanceiroDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<ImpostoFinanceiro>)new TrueSpecification<ImpostoFinanceiro>();

            return impostoFinanceiroRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.TipoCompromisso,
                l => l.Cliente).To<List<ImpostoFinanceiroDTO>>();
        }

        public ImpostoFinanceiroDTO ObterPeloId(int? id)
        {
            return impostoFinanceiroRepository.ObterPeloId(id).To<ImpostoFinanceiroDTO>();
        }

        public bool Salvar(ImpostoFinanceiroDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (dto == null) throw new ArgumentNullException("dto");

            if (ValidaSalvar(dto) == false) { return false; } 

            bool novoItem = false;

            var impostoFinanceiro = impostoFinanceiroRepository.ObterPeloId(dto.Id);
            if (impostoFinanceiro == null)
            {
                impostoFinanceiro = new ImpostoFinanceiro();
                novoItem = true;
            }

            impostoFinanceiro.Sigla = dto.Sigla;
            impostoFinanceiro.Descricao = dto.Descricao;
            impostoFinanceiro.Aliquota = dto.Aliquota;
            impostoFinanceiro.ContaContabil = dto.ContaContabil;
            impostoFinanceiro.ClienteId = dto.ClienteId;
            impostoFinanceiro.TipoCompromissoId = dto.TipoCompromissoId;
            impostoFinanceiro.EhRetido = dto.EhRetido;
            impostoFinanceiro.Indireto = dto.Indireto;
            impostoFinanceiro.PagamentoEletronico = dto.PagamentoEletronico;
            if (dto.Periodicidade != null)
            {
                impostoFinanceiro.Periodicidade = (PeriodicidadeImpostoFinanceiro)dto.Periodicidade;
            }
            if (dto.FimDeSemana != null)
            {
                impostoFinanceiro.FimDeSemana = (FimDeSemanaImpostoFinanceiro)dto.FimDeSemana;
            }
            impostoFinanceiro.DiaVencimento = dto.DiaVencimento;
            if (dto.FatoGerador != null)
            {
                impostoFinanceiro.FatoGerador = (FatoGeradorImpostoFinanceiro)dto.FatoGerador;
            }
            
            if (Validator.IsValid(impostoFinanceiro, out validationErrors))
            {
                if (novoItem)
                    impostoFinanceiroRepository.Inserir(impostoFinanceiro);
                else
                    impostoFinanceiroRepository.Alterar(impostoFinanceiro);

                impostoFinanceiroRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroDeletar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return false;
            }

            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var impostoFinanceiro = impostoFinanceiroRepository.ObterPeloId(id);

            try
            {
                impostoFinanceiroRepository.Remover(impostoFinanceiro);
                impostoFinanceiroRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, impostoFinanceiro.Descricao), TypeMessage.Error);
                return false;
            }
        }

        public List<ItemListaDTO> ListarOpcoesPeriodicidade()
        {  return typeof(PeriodicidadeImpostoFinanceiro).ToItemListaDTO(); }

        public List<ItemListaDTO> ListarOpcoesFimDeSemana()
        {  return typeof(FimDeSemanaImpostoFinanceiro).ToItemListaDTO(); }

        public List<ItemListaDTO> ListarOpcoesFatoGerador()
        {  return typeof(FatoGeradorImpostoFinanceiro).ToItemListaDTO();}


        public bool EhPermitidoSalvar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroGravar);
        }

        public bool EhPermitidoDeletar()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroDeletar);
        }

        public bool EhPermitidoImprimir()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.ImpostoFinanceiroImprimir);
        }

        public FileDownloadDTO ExportarRelImpostoFinanceiro(FormatoExportacaoArquivo formato)
        {
            if (!EhPermitidoImprimir())
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return null;
            }

            var specification = (Specification<ImpostoFinanceiro>)new TrueSpecification<ImpostoFinanceiro>();

            var listaImpostoFinanceiro = impostoFinanceiroRepository.ListarPeloFiltro(specification,
                                                                                      l => l.TipoCompromisso,
                                                                                      l => l.Cliente).To<List<ImpostoFinanceiro>>();
            relImpostoFinanceiro objRel = new relImpostoFinanceiro();

            objRel.SetDataSource(RelImpostoFinanceiroToDataTable(listaImpostoFinanceiro));

            var parametros = parametrosFinanceiroRepository.Obter();
            CentroCusto centroCusto = null;

            var caminhoImagem = PrepararIconeRelatorio(centroCusto, parametros);

            var nomeEmpresa = ObterNomeEmpresa(centroCusto, parametros);
            objRel.SetParameterValue("nomeEmpresa", nomeEmpresa);
            objRel.SetParameterValue("caminhoImagem", caminhoImagem);

            FileDownloadDTO arquivo = new FileDownloadDTO("Rel. Imposto Financeiro",
                                                          objRel.ExportToStream((ExportFormatType)formato),
                                                          formato);
            if (System.IO.File.Exists(caminhoImagem))
                System.IO.File.Delete(caminhoImagem);
            return arquivo;
        }

        #endregion


        #region métodos privados de IImpostoFinanceiroAppService

        public bool ValidaSalvar(ImpostoFinanceiroDTO dto)
        {
            bool retorno = true;

            if (dto == null)
            {
                retorno = false;
                throw new ArgumentNullException("dto");
            }

            if (dto.DiaVencimento != null)
            {
                if (dto.DiaVencimento < 0)
                {
                    messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.ValorDeveSerMaiorQue, "Dia do vencimento", "0"), TypeMessage.Error);
                    retorno = false;
                }
                if (dto.DiaVencimento > 31)
                {
                    messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.ValorDeveSerMenorQue, "Dia do vencimento", "31"), TypeMessage.Error);
                    retorno = false;
                }
            }

            return retorno;
        }

        private DataTable RelImpostoFinanceiroToDataTable(List<ImpostoFinanceiro> listaImpostoFinanceiro)
        {
            DataTable dta = new DataTable();
            DataColumn codigo = new DataColumn("codigo");
            DataColumn sigla = new DataColumn("sigla");
            DataColumn descricao = new DataColumn("descricao");
            DataColumn aliquota = new DataColumn("aliquota");
            DataColumn contaContabil = new DataColumn("contaContabil");
            DataColumn descricaoRetido = new DataColumn("descricaoRetido");
            DataColumn descricaoIndireto = new DataColumn("descricaoIndireto");
            DataColumn descricaoPagamentoEletronico = new DataColumn("descricaoPagamentoEletronico");
            DataColumn descricaoTipoCompromisso = new DataColumn("descricaoTipoCompromisso");
            DataColumn nomeCliente = new DataColumn("nomeCliente");
            DataColumn descricaoPeriodicidade = new DataColumn("descricaoPeriodicidade");
            DataColumn diaVencimento = new DataColumn("diaVencimento");
            DataColumn descricaoFimDeSemana = new DataColumn("descricaoFimDeSemana");
            DataColumn descricaoFatoGerador = new DataColumn("descricaoFatoGerador");
            DataColumn girErro = new DataColumn("girErro");

            dta.Columns.Add(codigo);
            dta.Columns.Add(sigla);
            dta.Columns.Add(descricao);
            dta.Columns.Add(aliquota);
            dta.Columns.Add(contaContabil);
            dta.Columns.Add(descricaoRetido);
            dta.Columns.Add(descricaoIndireto);
            dta.Columns.Add(descricaoPagamentoEletronico);
            dta.Columns.Add(descricaoTipoCompromisso);
            dta.Columns.Add(nomeCliente);
            dta.Columns.Add(descricaoPeriodicidade);
            dta.Columns.Add(diaVencimento);
            dta.Columns.Add(descricaoFimDeSemana);
            dta.Columns.Add(descricaoFatoGerador);
            dta.Columns.Add(girErro);

            foreach (var registro in listaImpostoFinanceiro)
            {
                ImpostoFinanceiroDTO impostoFinanceiro = registro.To<ImpostoFinanceiroDTO>();
                DataRow row = dta.NewRow();

                row[codigo] = impostoFinanceiro.Id;
                row[sigla] = impostoFinanceiro.Sigla;
                row[descricao] = impostoFinanceiro.Descricao;
                row[aliquota] = impostoFinanceiro.Aliquota;
                row[contaContabil] = impostoFinanceiro.ContaContabil;

                row[descricaoRetido] = impostoFinanceiro.EhRetidoDescricao;
                row[descricaoIndireto] = impostoFinanceiro.IndiretoDescricao;
                row[descricaoPagamentoEletronico] = impostoFinanceiro.PagamentoEletronicoDescricao;
                row[descricaoTipoCompromisso] = "";
                if (impostoFinanceiro.TipoCompromisso != null)
                {
                    row[descricaoTipoCompromisso] = impostoFinanceiro.TipoCompromisso.Descricao;
                }
                row[nomeCliente] = "";
                if (impostoFinanceiro.Cliente != null)
                {
                    row[nomeCliente] = impostoFinanceiro.Cliente.Nome;
                }
                row[descricaoPeriodicidade] = impostoFinanceiro.PeriodicidadeDescricao;
                row[diaVencimento] = DBNull.Value;
                if (impostoFinanceiro.DiaVencimento != null)
                {
                    row[diaVencimento] = impostoFinanceiro.DiaVencimento.Value;
                }
                row[descricaoFimDeSemana] = impostoFinanceiro.FimDeSemanaDescricao;
                row[descricaoFatoGerador] = impostoFinanceiro.FatoGeradorDescricao;
                row[girErro] = "";
                dta.Rows.Add(row);
            }

            return dta;
        }


        #endregion


    }
}