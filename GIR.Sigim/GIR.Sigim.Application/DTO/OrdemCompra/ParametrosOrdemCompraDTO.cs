using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class ParametrosOrdemCompraDTO : BaseDTO
    {
        [Display(Name = "Empresa")]
        //public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }

        [StringLength(18, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Classe de insumo")]
        public string MascaraClasseInsumo { get; set; }

        [Display(Name = "Ícone para relatórios")]
        public byte[] IconeRelatorio { get; set; }

        public bool RemoverImagem { get; set; }

        [Display(Name = "Assunto contato para e-mail")]
        public int? AssuntoContatoId { get; set; }
        public AssuntoContatoDTO AssuntoContato { get; set; }

        [Display(Name = "Gera títulos \"Aguardando liberação\" no financeiro")]
        public bool GeraTituloAguardando { get; set; }

        [Display(Name = "Gera provisionamento dos títulos na cotação?")]
        public bool GeraProvisionamentoNaCotacao { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Nº de dias para cálculo da data mínima")]
        public short? DiasDataMinima { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Nº de dias para cálculo do prazo")]
        public short? DiasPrazo { get; set; }

        [Display(Name = "Pré requisição de material")]
        public bool EhPreRequisicaoMaterial { get; set; }

        [Display(Name = "Tipo de compromisso do frete")]
        public int? TipoCompromissoFreteId { get; set; }
        //public TipoCompromisso TipoCompromissoFrete { get; set; }

        [Display(Name = "Servidor saída SMTP")]
        public string SmtpServidorSaidaEmail { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Porta de saída SMTP")]
        public int? SmtpPortaSaidaEmail { get; set; }

        [Display(Name = "Requisição obrigatória")]
        public bool EhRequisicaoObrigatoria { get; set; }

        [Display(Name = "Interface orçamento")]
        public bool EhInterfaceOrcamento { get; set; }

        [Display(Name = "Habilita SSL")]
        public bool HabilitaSSL { get; set; }

        [Display(Name = "Inibe forma de pagamento na impressão da OC")]
        public bool InibeFormaPagamento { get; set; }

        [Display(Name = "Interface contabil")]
        public bool EhInterfaceContabil { get; set; }

        public InterfaceCotacaoDTO InterfaceCotacao { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Nº de dias para EM")]
        public int? DiasEntradaMaterial { get; set; }

        [Display(Name = "Confere NF")]
        public bool ConfereNF { get; set; }

        [Display(Name = "Grava cotação na web")]
        public bool GravaCotacaoWeb { get; set; }

        [Display(Name = "Layout SPED")]
        public int? LayoutSPEDId { get; set; }
        //public BancoLayout LayoutSPED { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Nº de dias min. para pagamento")]
        public int? DiasPagamento { get; set; }

        [Display(Name = "Dados SPED obrigatórios")]
        public bool EhObrigatorioDadosSPED { get; set; }

        [Display(Name = "Gera RM pendente (Pré RM)")]
        public bool EhGeraRequisicaoMaterialRequisitada { get; set; }

        public ParametrosOrdemCompraDTO()
        {
            this.InterfaceCotacao = new InterfaceCotacaoDTO();
        }
    }
}