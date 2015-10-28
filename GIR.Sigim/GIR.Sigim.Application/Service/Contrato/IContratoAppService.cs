using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoAppService : IBaseAppService
    {
        List<ContratoDTO> ListarPeloFiltro(MedicaoContratoFiltro filtro, int? idUsuario, out int totalRegistros);
        List<ContratoDTO> ListarPeloFiltro(LiberacaoContratoFiltro filtro, int? idUsuario, out int totalRegistros);
        ContratoDTO ObterPeloId(int? id, int? idUsuario);
        bool EhContratoAssinado(ContratoDTO dto);
        bool EhContratoExistente(ContratoDTO dto);
        bool EhContratoComCentroCustoAtivo(ContratoDTO dto);
        List<ContratoRetificacaoProvisaoDTO> ObterListaCronogramaPorContratoEhRetificacaoItem(int contratoId, int contratoRetificacaoItemId);
        List<ContratoRetificacaoItemMedicaoDTO> ObtemMedicaoPorSequencialItem(int contratoId, int sequencialItem);
        bool ExisteContratoRetificacaoProvisao(List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao);
        bool ExisteMedicao(ContratoRetificacaoItemMedicaoDTO dto);
        ContratoRetificacaoItemMedicaoDTO ObtemMedicaoPorId(int contratoId, int contratoRetificacaoItemMedicaoId);
        bool EhValidoParametrosVisualizacaoMedicao(int? contratoId, int? tipoDocumentoId, string numeroDocumento, Nullable<DateTime> dataEmissao, int? contratadoId);
        List<ContratoRetificacaoItemMedicaoDTO> RecuperaMedicaoPorDadosDaNota(int contratoId, int tipoDocumentoId, string numeroDocumento, DateTime dataEmissao, int? contratadoId);
        bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId);
        FileDownloadDTO ExportarMedicao(int contratoId, int? contratadoId, int tipoDocumentoId, string numeroDocumento, DateTime dataEmissao, string valorContratadoItem, FormatoExportacaoArquivo formato, OrigemChamada origemChamada);
        bool ExcluirMedicao(int? contratoId, int? contratoRetificacaoItemMedicaoId);
        bool SalvarMedicao(ContratoRetificacaoItemMedicaoDTO dto);
        bool EhPermitidoSalvarMedicao();
        bool EhPermitidoDeletarMedicao();
        bool EhPermitidoImprimirMedicao();
        bool EhPermitidoAprovarLiberarLiberacao();
        bool EhPermitidoAprovarLiberacao();
        bool EhPermitidoLiberarLiberacao();
        bool EhPermitidoCancelarLiberacao();
        bool EhPermitidoAssociarNFLiberacao();
        bool EhPermitidoAlterarvencimentoLiberacao();
        bool EhPermitidoImprimirMedicaoLiberacao();
        List<ContratoDTO> PesquisarContratosPeloFiltro(ContratoPesquisaFiltro filtro, out int totalRegistros);
        void RecuperarMedicoesALiberar(ContratoDTO contrato, ContratoRetificacaoItemDTO contratoRetificacaoItemSelecionado, ResumoLiberacaoDTO resumo, out List<ItemLiberacaoDTO> listaItemLiberacao);
        bool PodeConcluirContrato(ContratoDTO contrato);
        bool EhPermitidoHabilitarBotoes(ContratoDTO dto);
        bool EhUltimoContratoRetificacao(int? contratoId, int? contratoRetificacaoId);
        bool AtualizarSituacaoParaConcluido(int? contratoId);
        bool AprovarListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO);
        bool ValidarImpressaoMedicaoPelaLiberacao(int? contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO, out int? contratoRetificacaoItemMedicaoId);
        FileDownloadDTO ImprimirMedicaoPelaLiberacao(FormatoExportacaoArquivo formato, int? contratoId, int? contratoRetificacaoItemMedicaoId);
        bool ValidarTrocaDataVencimentoListaItemLiberacao(int contratoId,Nullable<DateTime> dataVencimento, List<ItemLiberacaoDTO> listaItemLiberacaoDTO);
        bool TrocarDataVencimentoListaItemLiberacao(int contratoId, Nullable<DateTime> dataVencimento, List<ItemLiberacaoDTO> listaItemLiberacaoDTO);
        bool ValidarAssociacaoNotaFiscalListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO, out ItemLiberacaoDTO itemLiberacao);
        bool AssociarNotaFiscalListaItemLiberacao(int? contratoId, int? contratoRetificacaoItemMedicaoId, int? tipoDocumentoId, string numeroDocumento, Nullable<DateTime> dataEmissao, Nullable<DateTime> dataVencimento);
        bool AprovarLiberarListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO, OperacaoLiberarMedicao operacao);
        bool CancelarListaItemLiberacao(int contratoId, List<ItemLiberacaoDTO> listaItemLiberacaoDTO);
        bool OrdenarListaLiberacao(string colunaSelecao, bool ordenacaoAscendente, ref List<ItemLiberacaoDTO> listaItemLiberacaoDTO);
    }
}
